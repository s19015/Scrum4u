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

        internal static string HashujHasloSHA256(string tresc)
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
            bool czy_istnieje = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT imie FROM Uzytkownicy WHERE uzytkownicy_email = @email_uzytkownika", con);


                    cmd.Parameters.AddWithValue("@email_uzytkownika", email);
                    cmd.Connection.Open();

                    czy_istnieje = cmd.ExecuteScalar() is string;
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 93", ex.StackTrace));
                }
            }
            return czy_istnieje;
        }

        internal static string DodajNowego(Scrum4u.Uzytkownik u)
        {
            string token = "false";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Uzytkownicy (uzytkownicy_email, imie, nazwisko, haslo) 
                    VALUES ( @email_uzytkownika, @imie, @nazwisko, @haslo );", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@email_uzytkownika", u.UzytkownikEmail);
                    cmd.Parameters.AddWithValue("@imie", Truncate(u.UzytkownikImie, 50));
                    cmd.Parameters.AddWithValue("@nazwisko", Truncate(u.UzytkownikNazwisko, 50));
                    cmd.Parameters.AddWithValue("@haslo", HashujHasloSHA256(u.UzytkownikHaslo));
                    //cmd.Parameters.AddWithValue("dataRejestracji", DateTime.Now);

                    cmd.Connection.Open();
                    int dodano = cmd.ExecuteNonQuery();
                    
                    if (dodano > 0) {

                        SqlCommand cmd2 = new SqlCommand(@"INSERT INTO TokenyRejestracyjne (uzytkownicy_email, token)
VALUES ( @email_uzytkownika, CONVERT(VARCHAR(32), HashBytes('MD5', cast(rand() as char(10))), 2));

SELECT token FROM TokenyRejestracyjne
WHERE uzytkownicy_email = @email_uzytkownika and is_aktywny = 1;", con);
                        cmd2.CommandType = System.Data.CommandType.Text;
                        cmd2.Parameters.AddWithValue("@email_uzytkownika", u.UzytkownikEmail);
                        //int dodano_token = cmd2.ExecuteNonQuery();

                        using (SqlDataReader reader = cmd2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!String.IsNullOrEmpty(reader["token"].ToString()))
                                    token = reader["token"].ToString();
                            }
                        }
                                                           
                    }
                    else { token = "false"; }

                    cmd.Connection.Close();
                    
                    
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 97", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand(@"UPDATE Uzytkownicy SET is_konto_aktywne = 1
WHERE uzytkownicy_email in 
	(SELECT uzytkownicy_email 
	FROM TokenyRejestracyjne 
	WHERE uzytkownicy_email = @email_uzytkownika 
	AND token = @token 
	AND is_aktywny = 1);", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("email_uzytkownika", email);
                    cmd.Parameters.AddWithValue("token", token);

                    cmd.Connection.Open();
                    dodano = cmd.ExecuteNonQuery() > 0;

                    /*using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!String.IsNullOrEmpty(reader[0].ToString()))
                                dodano = reader.GetBoolean(0);
                        }
                    }*/
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
                    SqlCommand cmd = new SqlCommand(@"SELECT count(*) FROM Uzytkownicy
WHERE uzytkownicy_email = @email_uzytkownika and haslo = @haslo and is_konto_aktywne = 1;", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("email_uzytkownika", email);
                    cmd.Parameters.AddWithValue("haslo", HashujHasloSHA256(haslo));
                    cmd.Connection.Open();
                    dodano = cmd.ExecuteScalar() is int;
                    
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
                    SqlCommand cmd = new SqlCommand(@"SELECT uzytkownicy_email, imie, nazwisko
FROM Uzytkownicy
WHERE uzytkownicy_email = @email_uzytkownika;", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("email_uzytkownika", email);


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
                    SqlCommand cmd = new SqlCommand("insert into DziennikWydarzen (zrodlo, opis, stack_trace ) VALUES (@zrodlo, @opis, @stacktrace);", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@zrodlo", Truncate(z.ZdarzenieZrodlo, 200));
                    cmd.Parameters.AddWithValue("@opis", z.ZdarzenieOpis);
                    cmd.Parameters.AddWithValue("@stacktrace", z.ZdarzenieStackTrace);

                    cmd.Connection.Open();

                    dodano = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    //BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 93", ex.StackTrace));
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
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO [Kolejka_Emaili]
           ([od]
           ,[do]
           ,[temat]
           ,[tresc]
           ,[wersja]
           ,[wyslany]
           ,[data_kolejki]
           ,[data_wyslania])
     VALUES
           (@od
           ,@do
           ,@temat
           ,@tresc
           ,@wersja
           ,@wyslany
           ,@data_kolejki
           ,@data_wyslania)", con);
                    

                    cmd.Parameters.AddWithValue("@od", Truncate((String.IsNullOrEmpty(e.EmailOd) ? "noreply@scrum4u.pl" : e.EmailOd), 50));
                    cmd.Parameters.AddWithValue("@do", Truncate(e.EmailDo, 50));
                    cmd.Parameters.AddWithValue("@temat", Truncate(e.EmailTemat, 150));
                    cmd.Parameters.AddWithValue("@tresc", HttpContext.Current.Server.HtmlEncode(e.EmailTresc));
                    cmd.Parameters.AddWithValue("@wersja", Truncate(e.EmailWersja, 10));
                    cmd.Parameters.AddWithValue("@wyslany", false);
                    cmd.Parameters.AddWithValue("@data_kolejki", e.EmailDataKolejki);
                    cmd.Parameters.AddWithValue("@data_wyslania", new DateTime(2000,1,1));
                    cmd.Connection.Open();

                    dodano = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    Email.WyslijEmaile();
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

        internal static List<Email> PobierzWszystkieNieWyslaneEmaile()
        {
            List<Email> listaEmaili = null;
            Email e = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT [id_kolejka_emaili]
      ,[od]
      ,[do]
      ,[temat]
      ,[tresc]
      ,[wersja]
      ,[wyslany]
      ,[data_kolejki]
      ,[data_wyslania]
  FROM [Kolejka_Emaili]
  Where wyslany=0", con);


                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) listaEmaili = new List<Email>();
                        while (reader.Read())
                        {
                            e = new Email();

                            e.EmailID = int.Parse(reader["id_kolejka_emaili"].ToString());
                            e.EmailOd = reader["od"].ToString();
                            e.EmailDo = reader["do"].ToString();
                            e.EmailTemat = reader["temat"].ToString();
                            e.EmailTresc = HttpContext.Current.Server.HtmlDecode(reader["tresc"].ToString());
                            e.EmailWersja = reader["wersja"].ToString();
                            e.EmailWyslany = bool.Parse(reader["wyslany"].ToString());
                            e.EmailDataKolejki = DateTime.Parse(reader["data_kolejki"].ToString());

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
                        SqlCommand cmd = new SqlCommand(@"UPDATE [Kolejka_Emaili]
   SET [wyslany] = 1

      ,[data_wyslania] = @data_wyslania
 WHERE id_kolejka_emaili = @id", con);


                        cmd.Parameters.AddWithValue("@data_wyslania", DateTime.Now);
                        cmd.Parameters.AddWithValue("@id", e.EmailID);

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

    public class GrupaRoboczaProvider
    {
        internal static bool DodajNowa(GrupaRobocza g)
        {
            int dodano = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    // Bartek
                    // sql dodawanie grupy, wszystkie dane masz w obiekcie g


//                    SqlCommand cmd = new SqlCommand(@"INSERT INTO [Kolejka_Emaili]
//           ([od]
//           ,[do]
//           ,[temat]
//           ,[tresc]
//           ,[wersja]
//           ,[wyslany]
//           ,[data_kolejki]
//           ,[data_wyslania])
//     VALUES
//           (@od
//           ,@do
//           ,@temat
//           ,@tresc
//           ,@wersja
//           ,@wyslany
//           ,@data_kolejki
//           ,@data_wyslania)", con);


//                    cmd.Parameters.AddWithValue("@od", Truncate((String.IsNullOrEmpty(e.EmailOd) ? "noreply@scrum4u.pl" : e.EmailOd), 50));
//                    cmd.Parameters.AddWithValue("@do", Truncate(e.EmailDo, 50));
//                    cmd.Parameters.AddWithValue("@temat", Truncate(e.EmailTemat, 150));
//                    cmd.Parameters.AddWithValue("@tresc", HttpContext.Current.Server.HtmlEncode(e.EmailTresc));
//                    cmd.Parameters.AddWithValue("@wersja", Truncate(e.EmailWersja, 10));
//                    cmd.Parameters.AddWithValue("@wyslany", false);
//                    cmd.Parameters.AddWithValue("@data_kolejki", e.EmailDataKolejki);
//                    cmd.Parameters.AddWithValue("@data_wyslania", new DateTime(2000, 1, 1));
                    //cmd.Connection.Open();

                    //dodano = cmd.ExecuteNonQuery();
                    //cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 493", ex.StackTrace));
                }
            }
            if (dodano > 0) return true;
            return false;
        }

        internal static List<GrupaRobocza> PobierzWszystkie(string email)
        {
            throw new NotImplementedException();
        }
    }
}