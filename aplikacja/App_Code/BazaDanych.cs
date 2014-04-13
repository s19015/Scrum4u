using Scrum4u;
using Scrum4u.Aplikacja;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

/// <summary>
/// Summary description for BazaDanych
/// </summary>
public static class BazaDanych
{
    private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Scrum4u"].ConnectionString;
    private static string kluczHasla = System.Configuration.ConfigurationManager.AppSettings["Salt"];

    private static string Truncate(string value, int maxLength)
        {
            value = value.Replace("'", "''");
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    public static class UzytkownikProvider
    {
 
        internal static string  HashujHasloSHA256(string tresc)
        {
            if (tresc.Length == 64 || String.IsNullOrEmpty(tresc)) return tresc;
            tresc = tresc + kluczHasla;
            byte[] hash;
            byte[] m = System.Text.Encoding.UTF8.GetBytes(tresc);
            SHA256Managed hashString = new SHA256Managed();
            string encoded = Convert.ToBase64String(m);
            string hex = "";
            hash = hashString.ComputeHash(System.Text.Encoding.UTF8.GetBytes(encoded));
            foreach (byte x in hash)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        internal static bool SprawdzCzyIstnieje(string email)
        {
            bool dodano = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("czyUzytkownikIstnieje", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("inEmailUzytkownika", email);
                    cmd.Connection.Open();
                    dodano = cmd.ExecuteScalar() is string;
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 93", ex.StackTrace));
                }
            }
            return dodano;
        }

        internal static string DodajNowego(Scrum4u.Uzytkownik u)
        {
            string token = "false";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("rejestracjaUzytkownika", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("uzytkownikEmail", u.UzytkownikEmail);
                    cmd.Parameters.AddWithValue("imie", Truncate(u.UzytkownikImie, 50));
                    cmd.Parameters.AddWithValue("nazwisko", Truncate(u.UzytkownikNazwisko, 50));
                    cmd.Parameters.AddWithValue("haslo", HashujHasloSHA256(u.UzytkownikHaslo));
                    cmd.Parameters.AddWithValue("dataRejestracji", DateTime.Now);

                    cmd.Connection.Open();
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!String.IsNullOrEmpty(reader[0].ToString()))
                                token = reader[0].ToString();
                        }
                    }
                    cmd.Connection.Close();
                }
                catch(Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 60", ex.StackTrace));
                }
            }
            return token;
        }

        internal static bool AktywujUzytkownika(string email, string token)
        {
            bool dodano = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("aktywacjaKonta", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("inUzytkownikEmail", email);
                    cmd.Parameters.AddWithValue("token", token);

                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!String.IsNullOrEmpty(reader[0].ToString()))
                                dodano = reader.GetBoolean(0);
                        }
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 120", ex.StackTrace));
                }
            }
            return dodano;
        }

        internal static bool ZalogujUzytkownika(string email, string haslo)
        {
            bool dodano = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("logowanie", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("inUzytkownikEmail", email);
                    cmd.Parameters.AddWithValue("inHaslo", HashujHasloSHA256(haslo));
                    cmd.Connection.Open();
                    dodano = cmd.ExecuteScalar() is string;
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 149", ex.StackTrace));
                }
            }
            return dodano;
        }

        internal static string PobierzTokenRejestracyjny(string email)
        {
            string token = "false";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("rejestracjaUzytkownika", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("uzytkownikEmail", email);

                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!String.IsNullOrEmpty(reader[0].ToString()))
                                token = reader.GetString(0);
                        }
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 182", ex.StackTrace));
                }
            }
            return token;
        }

        internal static Scrum4u.Uzytkownik PobierzUzytkownika(string email)
        {
            Uzytkownik u = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pobierzProfil", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("inUzytkownikEmail", email);

                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) u = new Uzytkownik();
                        while (reader.Read())
                        {
                            if (!String.IsNullOrEmpty(reader[0].ToString()))
                            {
                                u.UzytkownikEmail = email;
                                u.UzytkownikImie = reader["imie"].ToString(); //imie
                                u.UzytkownikNazwisko = reader["nazwisko"].ToString(); // nazwisko
                            }
                        }
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 220", ex.StackTrace));
                }
            }
            return u;
        }

        internal static bool Aktualizuj(Uzytkownik u)
        {
            throw new NotImplementedException();
        }
    }
    public class DziennikProvider
    {

        internal static bool Loguj(Zdarzenie z)
        {
            int dodano = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dodajWpisDoDziennikaWydarzen", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("inDziennikZrodlo", Truncate(z.ZdarzenieZrodlo,200));
                    cmd.Parameters.AddWithValue("inDziennikOpis", z.ZdarzenieOpis);
                    cmd.Parameters.AddWithValue("inDziennikStackTrace", z.ZdarzenieStackTrace);

                    cmd.Connection.Open();
                    
                    dodano = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 93", ex.StackTrace));
                }
            }
            if (dodano > 0) return true;
            return false;
        }
    }

    public class EmailProvider
    {

        internal static bool DodajEmailaDoKolejki(Email e)
        {
            int dodano = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dodajDoKolejkiMaili", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("inEmailOd", Truncate((String.IsNullOrEmpty(e.EmailOd)?"noreply@scrum4u.pl":e.EmailOd), 50));
                    cmd.Parameters.AddWithValue("inEmailDo", Truncate(e.EmailDo,50));
                    cmd.Parameters.AddWithValue("inEmailTemat", Truncate(e.EmailTemat,150));
                    cmd.Parameters.AddWithValue("inEmailTresc", HttpContext.Current.Server.HtmlEncode(e.EmailTresc));
                    cmd.Parameters.AddWithValue("inEmailWersja", Truncate(e.EmailWersja, 10));
                    cmd.Parameters.AddWithValue("inEmailDataKolejki", DateTime.Now);
                    cmd.Connection.Open();

                    dodano = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 132", ex.StackTrace));
                }
            }
            if (dodano > 0) return true;
            return false;
        }

        internal static bool UsunZKolejki(Email email)
        {
            throw new NotImplementedException();
        }

        internal static List<Email> PobierzWszystkieNieWyslaneEmaile(DateTime odKiedy)
        {
            List<Email> listaEmaili = null;
            Email e = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pobierzNiewyslaneMaileZKolejki", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("inDataOd", odKiedy);

                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) listaEmaili = new List<Email>();
                        while (reader.Read())
                        {
                            e = new Email();

                            e.EmailID = int.Parse(reader["idKolejkaEmaili"].ToString());
                            e.EmailOd = reader["emailOd"].ToString();
                            e.EmailDo = reader["emailDo"].ToString();
                            e.EmailTemat = reader["emailTemat"].ToString();
                            e.EmailTresc = HttpContext.Current.Server.HtmlDecode(reader["emailTresc"].ToString());
                            e.EmailWersja = reader["emailWersja"].ToString();
                            e.EmailWyslany = bool.Parse(reader["emailWyslany"].ToString());
                            e.EmailDataKolejki = DateTime.Parse(reader["emailDataKolejki"].ToString());

                            listaEmaili.Add(e);
                        }
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 235", ex.StackTrace));
                }
            }
            return listaEmaili;
        }

        internal static int ZapiszWyslaneEmaileDoKolejki(List<Email> listaWyslanych)
        {
            int zapisano = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                foreach (Email e in listaWyslanych)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("potwierdzWyslanieEmailaZKolejki", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("inIdKolejkaEmaili", e.EmailID);

                        cmd.Connection.Open();

                        zapisano += cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    catch (Exception ex)
                    {
                        BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 262", ex.StackTrace));
                    }
                }
            }

            return zapisano;
        }
    }
}