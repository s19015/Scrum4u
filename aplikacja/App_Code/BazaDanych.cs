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
        /// <summary>
        /// Zwraca prawde jezli uzytkownik istnieje
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

                    if (dodano > 0)
                    {

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
                    SqlCommand cmd = new SqlCommand(@"SELECT uzytkownicy_email FROM Uzytkownicy
WHERE uzytkownicy_email = @email_uzytkownika and haslo = @haslo and is_konto_aktywne = 1;", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("email_uzytkownika", email);
                    cmd.Parameters.AddWithValue("haslo", HashujHasloSHA256(haslo));
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

        internal static bool ZmienHaslo(Uzytkownik uzytkownik, string haslo)
        {
            bool zaktualizowano = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE Uzytkownicy
SET haslo = @noweHaslo
WHERE uzytkownicy_email = @emailUzytkownika);", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("emailUzytkownika", uzytkownik.UzytkownikEmail);
                    cmd.Parameters.AddWithValue("haslo", HashujHasloSHA256(haslo));

                    cmd.Connection.Open();
                    zaktualizowano = cmd.ExecuteNonQuery() > 0;

                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 281", ex.StackTrace));
                }
            }
            return zaktualizowano;

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
                    cmd.Parameters.AddWithValue("@data_wyslania", new DateTime(2000, 1, 1));
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

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO GrupyRobocze (uzytkownicy_email, nazwa) VALUES ( @nazwa_uzytkownika, @nazwa );", con);

                    cmd.Parameters.AddWithValue("@nazwa_uzytkownika", g.GrupaRoboczaUzytkownikID);
                    cmd.Parameters.AddWithValue("@nazwa", g.GrupaRoboczaNazwa);

                    cmd.Connection.Open();
                    dodano = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 493", ex.StackTrace));
                }
            }
            if (dodano > 0) return true;
            return false;
        }

        internal static List<GrupaRobocza> PobierzWszystkie(string email, bool doKtorychNaleze)
        {
            List<GrupaRobocza> grupyRobocze = null;
            GrupaRobocza g = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();

                    if (doKtorychNaleze == false)
                    {
                        cmd = new SqlCommand(@"SELECT id_grupy_robocze, nazwa, uzytkownicy_email
                                                    FROM GrupyRobocze
                                                    WHERE uzytkownicy_email = @email_uzytkownika
                                                    and is_aktywna = 1;", con);
                    }
                    else
                    {


                        cmd = new SqlCommand(@"SELECT GrupyRobocze.id_grupy_robocze, nazwa, uzytkownicy_email
FROM GrupyRobocze
WHERE uzytkownicy_email = @email_uzytkownika
and is_aktywna = 1
UNION
select Grupy.id_grupy_robocze, Grupy.nazwa , Grupy.uzytkownicy_email
from GrupyRoboczeZaproszenia as Zaproszenia
inner join GrupyRobocze as Grupy on Zaproszenia.id_grupy_robocze = Grupy.id_grupy_robocze
where id_zapraszanego = @email_uzytkownika
and is_zaproszenie_przyjete = 1", con);
                    }

                    cmd.Parameters.AddWithValue("@email_uzytkownika", email);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            grupyRobocze = new List<GrupaRobocza>();
                            while (reader.Read())
                            {
                                g = new GrupaRobocza();
                                g.GrupaRoboczaID = int.Parse(reader["id_grupy_robocze"].ToString());
                                g.GrupaRoboczaNazwa = reader["nazwa"].ToString();
                                if (!String.IsNullOrEmpty(reader["uzytkownicy_email"].ToString()))
                                {
                                    g.GrupaRoboczaUzytkownikID = reader["uzytkownicy_email"].ToString();
                                }

                                grupyRobocze.Add(g);
                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 480", ex.StackTrace));
                }
            }

            return grupyRobocze;

        }


        internal static GrupaRobocza Pobierz(int idGrupy, bool doKtorejNaleze = true)
        {
            GrupaRobocza g = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = null;

                    if (doKtorejNaleze == true)
                    {

                        cmd = new SqlCommand(@"SELECT id_grupy_robocze, nazwa, uzytkownicy_email
FROM GrupyRobocze
WHERE id_grupy_robocze = @idGrupy
and uzytkownicy_email = @email_uzytkownika
and is_aktywna = 1
UNION
SELECT Grupy.id_grupy_robocze, nazwa, uzytkownicy_email
FROM GrupyRoboczeZaproszenia as Zaproszenia
INNER JOIN GrupyRobocze as Grupy on Zaproszenia.id_grupy_robocze = Grupy.id_grupy_robocze
and Grupy.is_aktywna = 1 and Grupy.id_grupy_robocze = @idGrupy
where id_zapraszanego = @email_uzytkownika
and is_zaproszenie_przyjete = 1", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"SELECT id_grupy_robocze, nazwa, uzytkownicy_email
FROM GrupyRobocze
WHERE id_grupy_robocze = @idGrupy", con);
                    }

                    cmd.Parameters.AddWithValue("@idGrupy", idGrupy);
                    cmd.Parameters.AddWithValue("@email_uzytkownika", HttpContext.Current.User.Identity.Name);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                g = new GrupaRobocza();
                                g.GrupaRoboczaID = int.Parse(reader["id_grupy_robocze"].ToString());
                                g.GrupaRoboczaNazwa = reader["nazwa"].ToString();
                                g.GrupaRoboczaUzytkownikID = reader["uzytkownicy_email"].ToString();

                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 557", ex.StackTrace));
                }
            }

            return g;
        }

        internal static List<Uzytkownik> PobierzUzytkownikowGrupy(int idGrupy, bool tylkoAktywni = false)
        {
            List<Uzytkownik> listaUzytkownikow = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = null;
                    if (tylkoAktywni == false)
                    {
                        cmd = new SqlCommand(@"SELECT uzytkownicy_email, 1 as is_zaproszenie_przyjete
FROM GrupyRobocze Grupy 
WHERE id_grupy_robocze = @idGrupy
and is_aktywna = 1
UNION
SELECT id_zapraszanego as uzytkownicy_email, is_zaproszenie_przyjete FROM GrupyRoboczeZaproszenia
where id_grupy_robocze = @idGrupy", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"SELECT uzytkownicy_email, 1 as is_zaproszenie_przyjete
FROM GrupyRobocze Grupy 
WHERE id_grupy_robocze = @idGrupy
and is_aktywna = 1
UNION
SELECT id_zapraszanego as uzytkownicy_email, is_zaproszenie_przyjete FROM GrupyRoboczeZaproszenia
where id_grupy_robocze = @idGrupy and is_zaproszenie_przyjete = 1", con);

                    }

                    cmd.Parameters.AddWithValue("@idGrupy", idGrupy);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listaUzytkownikow = new List<Uzytkownik>();
                            while (reader.Read())
                            {
                                Uzytkownik u = new Uzytkownik();
                                u.UzytkownikEmail = reader["uzytkownicy_email"].ToString();
                                listaUzytkownikow.Add(u);
                            }
                        }

                    }

                    cmd.Connection.Close();


                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 604", ex.StackTrace));
                }
            }


            return listaUzytkownikow;
        }

        internal static bool UsunGrupe(GrupaRobocza grupaRobocza)
        {
            bool usunieto = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"  UPDATE GrupyRobocze
                      SET is_aktywna = 0
                      WHERE id_grupy_robocze = @id_grupy_robocze and uzytkownicy_email=@email", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_grupy_robocze", grupaRobocza.GrupaRoboczaID);
                    cmd.Parameters.AddWithValue("@email", HttpContext.Current.User.Identity.Name);

                    cmd.Connection.Open();
                    usunieto = cmd.ExecuteNonQuery() > 0;

                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 705", ex.StackTrace));
                }
            }

            return usunieto;
        }
    }

    public class GrupyRoboczeZaproszenieProvider
    {

        internal static string Dodaj(Scrum4u.GrupyRoboczeZaproszenie zaproszenie)
        {
            string token = "false";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(@"insert into GrupyRoboczeZaproszenia
                    ( id_grupy_robocze, id_zapraszajacego, id_zapraszanego, token )
                    values (
                        @id_grupy_roboczej, 
                        @id_zapraszajacego, 
                        @id_zapraszanego, 
                        CONVERT(VARCHAR(32), HashBytes('MD5', cast(rand() as char(10))), 2));", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_grupy_roboczej", zaproszenie.GrupyRoboczeGrupaRoboczaID);
                    cmd.Parameters.AddWithValue("@id_zapraszajacego", zaproszenie.GrupyRoboczeZaproszenieIDZapraszajacego);
                    cmd.Parameters.AddWithValue("@id_zapraszanego", zaproszenie.GrupyRoboczeZaproszenieIDZapraszanego);

                    cmd.Connection.Open();
                    int dodano = cmd.ExecuteNonQuery();

                    if (dodano > 0)
                    {

                        SqlCommand cmd2 = new SqlCommand(@"select token from GrupyRoboczeZaproszenia
                            where id_grupy_robocze = @id_grupy_roboczej
                            and id_zapraszanego = @id_zapraszanego
                            and is_aktywne_zaproszenie = 1;", con);
                        cmd2.CommandType = System.Data.CommandType.Text;
                        cmd2.Parameters.AddWithValue("@id_grupy_roboczej", zaproszenie.GrupyRoboczeGrupaRoboczaID);
                        cmd2.Parameters.AddWithValue("@id_zapraszanego", zaproszenie.GrupyRoboczeZaproszenieIDZapraszanego);


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
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 618", ex.StackTrace));
                }
            }
            return token;
        }

        internal static bool PotwierdzZaproszenie(string email, string token, int idGrupyRoboczej)
        {
            bool potwierdzenie = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(@"update GrupyRoboczeZaproszenia
                            set is_aktywne_zaproszenie = 0,
                            is_zaproszenie_przyjete = 1,
                            data_przyjecia_zaproszenia = CURRENT_TIMESTAMP
                            where id_grupy_robocze = @id_grupy_roboczej
                            and id_zapraszanego = @id_zapraszanego
                            and token = @token
                            and is_aktywne_zaproszenie = 1", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@id_grupy_roboczej", idGrupyRoboczej);
                    cmd.Parameters.AddWithValue("@id_zapraszanego", email);
                    cmd.Parameters.AddWithValue("@token", token);

                    cmd.Connection.Open();
                    potwierdzenie = cmd.ExecuteNonQuery() > 0;


                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 120", ex.StackTrace));
                }
            }
            return potwierdzenie;
        }

        internal static List<Scrum4u.GrupyRoboczeZaproszenie> PobierzWszystkie(int idGrupy)
        {
            List<GrupyRoboczeZaproszenie> grupyRobocze = null;
            GrupyRoboczeZaproszenie g = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(@"select id_zapraszajacego, id_zapraszanego, row_date,is_aktywne_zaproszenie 
                        from GrupyRoboczeZaproszenia
                        where id_grupy_robocze = @id_grupy_roboczej
                        ", con);

                    cmd.Parameters.AddWithValue("@id_grupy_roboczej", idGrupy);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            grupyRobocze = new List<GrupyRoboczeZaproszenie>();
                            while (reader.Read())
                            {
                                g = new GrupyRoboczeZaproszenie();
                                g.GrupyRoboczeGrupaRoboczaID = idGrupy;
                                g.GrupyRoboczeZaproszenieIDZapraszajacego = reader["id_zapraszajacego"].ToString();
                                g.GrupyRoboczeZaproszenieIDZapraszanego = reader["id_zapraszanego"].ToString();
                                g.GrupyRoboczeZaproszenieData = DateTime.Parse(reader["row_date"].ToString());
                                g.GrupyRoboczeZaproszenieAktywne = bool.Parse(reader["is_aktywne_zaproszenie"].ToString());
                                grupyRobocze.Add(g);
                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 480", ex.StackTrace));
                }
            }

            return grupyRobocze;
        }

        internal static Scrum4u.GrupyRoboczeZaproszenie Pobierz(int idGrupy, string email)
        {

            GrupyRoboczeZaproszenie g = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"select id_grupy_robocze, id_zapraszajacego, id_zapraszanego, token, row_date, is_aktywne_zaproszenie
                        from GrupyRoboczeZaproszenia
                        where id_grupy_robocze = @id_grupy_roboczej", con);
                    if (!String.IsNullOrEmpty(email))
                    {
                        cmd.CommandText += " and id_zapraszanego = @id_zapraszanego";
                        cmd.Parameters.AddWithValue("@id_zapraszanego", email);
                    }

                    cmd.Parameters.AddWithValue("@id_grupy_roboczej", idGrupy);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                g = new GrupyRoboczeZaproszenie();
                                g.GrupyRoboczeGrupaRoboczaID = (int)reader["id_grupy_robocze"];
                                g.GrupyRoboczeZaproszenieIDZapraszajacego = reader["id_zapraszajacego"].ToString();
                                g.GrupyRoboczeZaproszenieIDZapraszanego = reader["id_zapraszanego"].ToString();
                                g.GrupyRoboczeZaproszenieData = DateTime.Parse(reader["row_date"].ToString());


                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 557", ex.StackTrace));
                }
            }

            return g;
        }

        internal static bool Usun(GrupyRoboczeZaproszenie zaproszenie)
        {
            bool usunieto = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"delete from GrupyRoboczeZaproszenia
where id_grupy_robocze = @id_grupy_roboczej
and id_zapraszanego = @id_zapraszanego;", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("id_grupy_roboczej", zaproszenie.GrupyRoboczeGrupaRoboczaID);
                    cmd.Parameters.AddWithValue("id_zapraszanego", zaproszenie.GrupyRoboczeZaproszenieIDZapraszanego);

                    cmd.Connection.Open();
                    usunieto = cmd.ExecuteNonQuery() > 0;

                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 120", ex.StackTrace));
                }
            }
            return usunieto;
        }
    }

    public class ProjektProvider
    {

        internal static bool DodajNowy(Projekt projekt)
        {
            int dodano = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Projekty (id_menager_projektu,id_grupy_robocze,nazwa_projektu,id_scrum_master)
VALUES( @email_uzytkownika, @idGrupy, @nazwaProjektu, @scrumMaster );", con);

                    cmd.Parameters.AddWithValue("@email_uzytkownika", projekt.ProjektManagerProjektuID);
                    cmd.Parameters.AddWithValue("@idGrupy", projekt.ProjektGrupaRoboczaID);
                    cmd.Parameters.AddWithValue("@nazwaProjektu", projekt.ProjektNazwa);
                    cmd.Parameters.AddWithValue("@scrumMaster", projekt.ProjektScrumMasterID);

                    cmd.Connection.Open();
                    dodano = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 797", ex.StackTrace));
                }
            }
            if (dodano > 0) return true;
            return false;
        }


        internal static List<Projekt> PobierzWszystkie(string email, bool doKtorychNaleze, int idGrupy)
        {
            List<Projekt> projekty = null;
            GrupaRobocza g = null;
            string DoKtorychNalezeBezGrupy = @"SELECT Projekty.id_projekty ,Projekty.id_grupy_robocze, Projekty.nazwa_projektu
FROM Projekty
WHERE id_menager_projektu = @email_uzytkownika
and is_aktywny = 1
UNION
SELECT 0 as id_projekty, Projekty.id_grupy_robocze, Projekty.nazwa_projektu
FROM GrupyRoboczeZaproszenia Zaproszenia
INNER JOIN GrupyRobocze on Zaproszenia.id_grupy_robocze = GrupyRobocze.id_grupy_robocze
and GrupyRobocze.is_aktywna = 1
INNER JOIN Projekty on GrupyRobocze.id_grupy_robocze = Projekty.id_grupy_robocze
WHERE id_zapraszanego = @email_uzytkownika
and is_zaproszenie_przyjete = 1";
            string DoKtorychNalezeGrupa = @"select * from (
SELECT Projekty.id_projekty ,Projekty.id_grupy_robocze, Projekty.nazwa_projektu
FROM Projekty
WHERE id_menager_projektu = @email_uzytkownika 
and is_aktywny = 1
UNION
SELECT Projekty.id_projekty, Projekty.id_grupy_robocze, Projekty.nazwa_projektu
FROM GrupyRoboczeZaproszenia Zaproszenia
INNER JOIN GrupyRobocze on Zaproszenia.id_grupy_robocze = GrupyRobocze.id_grupy_robocze
and GrupyRobocze.is_aktywna = 1
INNER JOIN Projekty on GrupyRobocze.id_grupy_robocze = Projekty.id_grupy_robocze
WHERE id_zapraszanego = @email_uzytkownika
and is_zaproszenie_przyjete = 1
) as wynik
where id_grupy_robocze=@grupa";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand();

                    if (doKtorychNaleze == false)
                    {
                        cmd = new SqlCommand(@"SELECT id_projekty ,id_grupy_robocze, nazwa
                                                    FROM projekty
                                                    WHERE id_menager_projektu = @email_uzytkownika
                                                    and is_aktywna = 1", con);
                        if (idGrupy > 0)
                        {
                            cmd.CommandText += " and [id_grupy_robocze]=" + idGrupy;
                        }
                    }
                    else
                    {
                        if (idGrupy > 0)
                        {
                            cmd = new SqlCommand(DoKtorychNalezeGrupa, con);

                            cmd.Parameters.AddWithValue("@grupa", idGrupy);
                        }
                        else
                        {
                            cmd = new SqlCommand(DoKtorychNalezeBezGrupy, con);
                        }
                    }

                    cmd.Parameters.AddWithValue("@email_uzytkownika", email);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            projekty = new List<Projekt>();
                            while (reader.Read())
                            {
                                Projekt p = new Projekt();
                                p.ProjektID = (int)reader["id_projekty"];
                                p.ProjektNazwa = reader["nazwa_projektu"].ToString();
                                p.ProjektGrupaRoboczaID = (int)reader["id_grupy_robocze"];
                                //p.ProjektAktywny = bool.Parse(reader["is_aktywny"].ToString());
                                p.ProjektAktywny = true;
                                projekty.Add(p);

                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 845", ex.StackTrace));
                }
            }

            return projekty;
        }

        internal static Projekt Pobierz(int idProjektu, bool tylkoPowiazaneZeMna)
        {

            Projekt projekt = new Projekt();

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand cmd;

                    if (tylkoPowiazaneZeMna == false)
                    {
                        cmd = new SqlCommand(@"SELECT id_projekty
      ,id_menager_projektu
      ,id_grupy_robocze
      ,nazwa_projektu
      ,id_scrum_master
      ,is_aktywny
      ,row_date
  FROM Projekty
  WHERE id_projekty = @idProjektu
  and id_menager_projektu = @emailUzytkownika
  and is_aktywny = 1", con);
                    }
                    else
                    {
                        cmd = new SqlCommand(@"/* projekty użytkownika */
SELECT id_projekty
      ,id_menager_projektu
      ,id_grupy_robocze
      ,nazwa_projektu
      ,id_scrum_master
      ,is_aktywny
      ,row_date
  FROM Projekty
  WHERE id_projekty = @idProjektu
  and id_menager_projektu = @emailUzytkownika
  and is_aktywny = 1
  UNION
  /*Powiązane projekty*/
  select Projekty.id_projekty
      ,Projekty.id_menager_projektu
      ,Projekty.id_grupy_robocze
      ,Projekty.nazwa_projektu
      ,Projekty.id_scrum_master
      ,Projekty.is_aktywny
      ,Projekty.row_date 
from GrupyRoboczeZaproszenia Zaproszenia
inner join Projekty on Zaproszenia.id_grupy_robocze = Projekty.id_grupy_robocze and Projekty.is_aktywny = 1
where Zaproszenia.id_zapraszanego = @emailUzytkownika
and Zaproszenia.is_zaproszenie_przyjete = 1;", con);
                    }

                    cmd.Parameters.AddWithValue("@idProjektu", idProjektu);
                    cmd.Parameters.AddWithValue("@emailUzytkownika", HttpContext.Current.User.Identity.Name);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {

                                projekt.ProjektID = (int)reader["id_grupy_robocze"];
                                projekt.ProjektNazwa = reader["nazwa_projektu"].ToString();
                                projekt.ProjektManagerProjektuID = reader["id_menager_projektu"].ToString();
                                projekt.ProjektGrupaRoboczaID = Int32.Parse(reader["id_grupy_robocze"].ToString());
                                projekt.ProjektScrumMasterID = reader["id_scrum_master"].ToString();
                                projekt.ProjektAktywny = (reader["is_aktywny"] != null && reader["is_aktywny"].ToString() == "1") ? true : false;
                                projekt.ProjektDataUtworzenia = DateTime.Parse(reader["row_date"].ToString());

                            }
                        }

                    }

                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1043", ex.StackTrace));
                }
            }

            return projekt;

        }

        internal static List<Sprint> PobierzSprinty(int idProjektu)
        {
            List<Sprint> listaSprintow = null;
            //do zrobienia pobieranie listy sprintów nalezacych dla danego projektu

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT [id_sprinty]
                                  ,[id_projekty]
                                  ,[data_zakonczenia]
                                  ,[data_deadline]
                                  ,[row_date]
                                  ,[email_dodajacego]
                                  ,[nazwa]
                                  ,[opis]
                              FROM Sprinty
                              WHERE id_projekty = @idProjektu
                              and is_usuniety = 0
                            ", con);

                    cmd.Parameters.AddWithValue("@idProjektu", idProjektu);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            listaSprintow = new List<Sprint>();

                            while (reader.Read())
                            {

                                Sprint s = new Sprint();
                                s.SprintID = Int32.Parse(reader["id_sprinty"].ToString());
                                s.SprintProjektID = Int32.Parse(reader["id_projekty"].ToString());
                                //s.SprintDataZakonczenia = DateTime.Parse(reader["data_zakonczenia"].ToString());
                                s.SprintTerminWykonania = DateTime.Parse(reader["data_deadline"].ToString());
                                s.SprintDataUtworzenia = DateTime.Parse(reader["row_date"].ToString());
                                s.SprintIdTworzacego = reader["email_dodajacego"].ToString();
                                s.SprintNazwa = reader["nazwa"].ToString();
                                s.SprintOpis = reader["opis"].ToString();

                                listaSprintow.Add(s);

                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1114", ex.StackTrace));
                }
            }

            return listaSprintow;
        }
    }

    public class ZadanieProvider
    {

        internal static bool Dodaj(Zadanie zadanie)
        {


            bool dodano = false;



            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(@"
INSERT INTO [Zadania]
           ([id_sprinty]
           ,[id_zadania_typy]
           ,[tytul]
           ,[opis]
           ,[is_usuniety]
           ,[email_dodajacego]
           ,[row_date]
           ,[priorytet]
           ,[email_przydzielony_uzytkownik]
           ,[data_zakonczenia]
           ,[deadline]
           ,[id_projekty]
           ,[id_zadania_statusy])
     VALUES
           (@id_sprinty
           ,@id_zadania_typ
           ,@tytul
           ,@opis
           ,@usuniety
           ,@email_dod
           ,@row_date
           ,@priorytet
           ,@przydzielone_do
           ,@data_zak
           ,@data_dead
           ,@id_projekty
           ,@id_zadania_statusy)", con);
                    cmd.CommandType = System.Data.CommandType.Text;


                    cmd.Parameters.AddWithValue("@id_sprinty", zadanie.ZadanieSprintID);
                    cmd.Parameters.AddWithValue("@id_zadania_typ", Enum.GetName(typeof(TypZadania), zadanie.ZadanieTypZadania));
                    cmd.Parameters.AddWithValue("@tytul", zadanie.ZadanieNazwa);
                    cmd.Parameters.AddWithValue("@usuniety", false);
                    cmd.Parameters.AddWithValue("@opis", zadanie.ZadanieOpis);
                    cmd.Parameters.AddWithValue("@email_dod", HttpContext.Current.User.Identity.Name);
                    cmd.Parameters.AddWithValue("@id_projekty", zadanie.ZadanieProjektID);
                    cmd.Parameters.AddWithValue("@row_date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@priorytet", zadanie.ZadaniePriorytet);
                    cmd.Parameters.AddWithValue("@przydzielone_do", zadanie.ZadaniePrzypisaneDo);
                   // cmd.Parameters.AddWithValue("@id_zad_nadrzednego", (zadanie.ZadanieNadrzedneID>0?zadanie.ZadanieNadrzedneID.ToString():null));
                    cmd.Parameters.AddWithValue("@data_zak", zadanie.ZadanieDataUkonczenia);
                    cmd.Parameters.AddWithValue("@data_dead", zadanie.ZadanieDeadline);

                    cmd.Parameters.AddWithValue("@id_zadania_statusy", Enum.GetName(typeof(Status), zadanie.ZadanieStatus));


                    cmd.Connection.Open();
                    int ileDodano = cmd.ExecuteNonQuery();

                    if (ileDodano > 0)
                    {
                        dodano = true;
                    }


                    cmd.Connection.Close();


                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1328", ex.StackTrace));
                }

                return dodano;
            }
        }

        internal static bool Usun(Zadanie zadanie)
        {
            bool usunieto = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"  UPDATE Projekty
                      SET is_usuniete = 1
                      WHERE id_zadania = @idZadania", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@idZadania", zadanie.ZadanieID);

                    cmd.Connection.Open();
                    usunieto = cmd.ExecuteNonQuery() > 0;

                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1158", ex.StackTrace));
                }
            }
            return usunieto;
        }


        internal static Zadanie Pobierz(int idZadania)
        {
            Zadanie z = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand(@"
SELECT [id_zadania]
      ,[id_sprinty]
      ,[id_zadania_typy]
      ,[tytul]
      ,[opis]
      ,[is_usuniety]
      ,[email_dodajacego]
      ,[row_date]
      ,[priorytet]
      ,[email_przydzielony_uzytkownik]
      ,[id_przydzielonej_grupy]
      ,[zadanie_nadrzedne]
      ,[data_zakonczenia]
      ,[deadline]
      ,[id_projekty]
      ,[id_zadania_statusy]
  FROM [Zadania]
WHERE id_zadania = @idZadania", con);



                    cmd.Parameters.AddWithValue("@idZadania", idZadania);

                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            z = new Zadanie();

                            while (reader.Read())
                            {

                                z.ZadanieID = int.Parse(reader["id_zadania"].ToString());
                                z.ZadanieNazwa = reader["tytul"].ToString();
                                z.ZadanieProjektID = int.Parse(reader["id_projekty"].ToString());
                                z.ZadanieSprintID = int.Parse(reader["id_sprinty"].ToString());
                                TypZadania t = TypZadania.ZADANIE;
                                Enum.TryParse(reader["id_zadania_typy"].ToString(), out t);
                                z.ZadanieTypZadania = t;

                                Status s = Status.DOWYKONANIA;
                                Enum.TryParse(reader["id_zadania_statusy"].ToString(), out s);
                                z.ZadanieStatus = s;
                                z.ZadanieOpis = reader["opis"].ToString();
                                z.ZadanieDodajacy = reader["email_dodajacego"].ToString();
                                z.ZadanieDataUtworzenia = DateTime.Parse(reader["row_date"].ToString());
                                z.ZadaniePriorytet = int.Parse(reader["priorytet"].ToString());
                                z.ZadaniePrzypisaneDo = reader["email_przydzielony_uzytkownik"].ToString();

                                // z.ZadanieNadrzedneID = int.Parse((reader["zadanie_nadrzedne"]!=null?reader["zadanie_nadrzedne"].ToString():"0"));
                                z.ZadanieDataUkonczenia = DateTime.Parse(reader["data_zakonczenia"].ToString());
                                z.ZadanieDeadline = DateTime.Parse(reader["deadline"].ToString());

                            }
                        }

                    }

                    cmd.Connection.Close();
                    return z;
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1043", ex.StackTrace));
                }
            }


            return z;
        }

        internal static List<Zadanie> PobierzWszystkie(string email)
        {
            List<Zadanie> listaZadan = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT Zadania.id_zadania, Zadania.id_sprinty, Zadania.id_zadania_typy, Zadania.tytul, Zadania.opis, Zadania.email_dodajacego, Zadania.row_date, Zadania.priorytet, Zadania.zadanie_nadrzedne, Zadania.data_zakonczenia, Zadania.deadline, Zadania.id_projekty
FROM Zadania
INNER JOIN Projekty on Zadania.id_projekty=Projekty.id_projekty and Projekty.is_aktywny = 1
INNER JOIN GrupyRobocze on Projekty.id_grupy_robocze = GrupyRobocze.id_grupy_robocze and GrupyRobocze.is_aktywna = 1
INNER JOIN GrupyRoboczeZaproszenia Zaproszenia on GrupyRobocze.id_grupy_robocze = Zaproszenia.id_grupy_robocze and Zaproszenia.id_zapraszanego = @emailUzytkownika and Zaproszenia.is_zaproszenie_przyjete = 1
WHERE Zadania.email_przydzielony_uzytkownik = @emailUzytkownika
and Zadania.is_usuniety = 0
order by Zadania.priorytet desc, Zadania.deadline asc, Zadania.row_date asc", con);

                    cmd.Parameters.AddWithValue("@emailUzytkownika", email);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listaZadan = new List<Zadanie>();
                            while (reader.Read())
                            {
                                Zadanie z = new Zadanie();

                                z.ZadanieID = int.Parse(reader["id_zadania"].ToString());
                                z.ZadanieProjektID = int.Parse(reader["id_projekty"].ToString());
                                z.ZadanieSprintID = int.Parse(reader["id_sprinty"].ToString());
                                TypZadania t = TypZadania.ZADANIE;
                                Enum.TryParse(reader["id_zadania_typy"].ToString(), out t);
                                z.ZadanieTypZadania = t;
                                z.ZadanieOpis = reader["opis"].ToString();
                                z.ZadanieDodajacy = HttpContext.Current.User.Identity.Name;
                                z.ZadanieDataUtworzenia = DateTime.Parse(reader["row_date"].ToString());
                                z.ZadaniePriorytet = int.Parse(reader["priorytet"].ToString());
                                z.ZadaniePrzypisaneDo = reader["email_przydzielony_uzytkownik"].ToString();

                                z.ZadanieNadrzedneID = int.Parse(reader["zadanie_nadrzedne"].ToString());
                                z.ZadanieDataUkonczenia = DateTime.Parse(reader["data_zakonczenia"].ToString());
                                z.ZadanieDeadline = DateTime.Parse(reader["deadline"].ToString());

                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1257", ex.StackTrace));
                }
            }

            return listaZadan;
        }

        internal static List<Zadanie> PobierzWszystkie(int idProjektu, int idSprintu)
        {
            List<Zadanie> listaZadan = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd ;
                    if (idSprintu == 0)
                    {

                        cmd = new SqlCommand(@"SELECT Zadania.id_zadania, Zadania.id_sprinty, Zadania.id_zadania_typy, Zadania.tytul, Zadania.opis, Zadania.email_dodajacego, Zadania.row_date, Zadania.priorytet, Zadania.zadanie_nadrzedne, Zadania.data_zakonczenia, Zadania.deadline, Zadania.id_projekty, Zadania.email_przydzielony_uzytkownik, Zadania.id_zadania_statusy
FROM Zadania
INNER JOIN Projekty on Zadania.id_projekty=Projekty.id_projekty and Projekty.is_aktywny = 1 and Projekty.id_projekty = @idProjektu
WHERE Zadania.is_usuniety = 0
order by Zadania.priorytet desc, Zadania.deadline asc, Zadania.row_date asc", con);
                    }
                    else {

                        cmd = new SqlCommand(@"SELECT Zadania.id_zadania, Zadania.id_sprinty, Zadania.id_zadania_typy, Zadania.tytul, Zadania.opis, Zadania.email_dodajacego, Zadania.row_date, Zadania.priorytet, Zadania.zadanie_nadrzedne, Zadania.data_zakonczenia, Zadania.deadline, Zadania.id_projekty, Zadania.email_przydzielony_uzytkownik, Zadania.id_zadania_statusy
FROM Zadania
INNER JOIN Projekty on Zadania.id_projekty=Projekty.id_projekty and Projekty.is_aktywny = 1 and Projekty.id_projekty = @idProjektu
WHERE Zadania.is_usuniety = 0
AND Zadania.id_sprinty = @idSprintu
order by Zadania.priorytet desc, Zadania.deadline asc, Zadania.row_date asc", con);

                        cmd.Parameters.AddWithValue("@idSprintu", idSprintu);

                    }

                    cmd.Parameters.AddWithValue("@idProjektu", idProjektu);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listaZadan = new List<Zadanie>();
                            while (reader.Read())
                            {
                                Zadanie z = new Zadanie();

                                z.ZadanieID = int.Parse(reader["id_zadania"].ToString());
                                z.ZadanieNazwa = reader["tytul"].ToString();
                                z.ZadanieProjektID = int.Parse(reader["id_projekty"].ToString());
                                z.ZadanieSprintID = int.Parse(reader["id_sprinty"].ToString());
                                TypZadania t = TypZadania.ZADANIE;
                                Enum.TryParse(reader["id_zadania_typy"].ToString(), out t);
                                z.ZadanieTypZadania = t;

                                Status s = Status.DOWYKONANIA;
                                Enum.TryParse(reader["id_zadania_statusy"].ToString(), out s);
                                z.ZadanieStatus = s;
                                z.ZadanieOpis = reader["opis"].ToString();
                                z.ZadanieDodajacy = reader["email_dodajacego"].ToString();
                                z.ZadanieDataUtworzenia = DateTime.Parse(reader["row_date"].ToString());
                                z.ZadaniePriorytet = int.Parse(reader["priorytet"].ToString());
                                z.ZadaniePrzypisaneDo = reader["email_przydzielony_uzytkownik"].ToString();

                               // z.ZadanieNadrzedneID = int.Parse((reader["zadanie_nadrzedne"]!=null?reader["zadanie_nadrzedne"].ToString():"0"));
                                z.ZadanieDataUkonczenia = DateTime.Parse(reader["data_zakonczenia"].ToString());
                                z.ZadanieDeadline = DateTime.Parse(reader["deadline"].ToString());

                                listaZadan.Add(z);
                            }
                        }

                    }

                    cmd.Connection.Close();



                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1456", ex.StackTrace));
                }
            }

            return listaZadan;
        }
    }

    public class SprintProvider
    {

        internal static bool Dodaj(Sprint sprint)
        {

            bool dodano = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Sprinty(id_projekty, data_deadline, email_dodajacego, nazwa, opis )
VALUES (@idProjektu, @deadline, @emailDodajacego, @nazwa, @opis );", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@idProjektu", sprint.SprintProjektID);
                    cmd.Parameters.AddWithValue("@deadline", sprint.SprintTerminWykonania);
                    cmd.Parameters.AddWithValue("@emailDodajacego", HttpContext.Current.User.Identity.Name);
                    cmd.Parameters.AddWithValue("@nazwa", sprint.SprintNazwa);
                    cmd.Parameters.AddWithValue("@opis", sprint.SprintOpis);


                    cmd.Connection.Open();
                    int ileDodano = cmd.ExecuteNonQuery();

                    if (ileDodano > 0)
                    {
                        dodano = true;
                    }



                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1501", ex.StackTrace));
                }

                return dodano;
            }

        }

        internal static bool Usun(Sprint sprint)
        {
            bool usunieto = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE Sprinty
                      SET is_usuniety = 1
                      WHERE id_sprinty= @idSprintu", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@idSprintu", sprint.SprintID);

                    cmd.Connection.Open();
                    usunieto = cmd.ExecuteNonQuery() > 0;

                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1531", ex.StackTrace));
                }
            }
            return usunieto;
        }

        internal static bool Aktualizuj(Sprint sprint)
        {


            bool zaktualizowano = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE Sprinty
SET data_zakonczenia = @dataZakonczenia,
    data_deadline = @dataDeadline,
    nazwa = @nazwa,
    opis = @opis
WHERE id_sprinty = @idSprintu", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@idSprintu", sprint.SprintID);
                    cmd.Parameters.AddWithValue("@dataZakonczenia", sprint.SprintDataZakonczenia);
                    cmd.Parameters.AddWithValue("@dataDeadline", sprint.SprintTerminWykonania);
                    cmd.Parameters.AddWithValue("@nazwa", sprint.SprintNazwa);
                    cmd.Parameters.AddWithValue("@opis", sprint.SprintOpis);

                    cmd.Connection.Open();
                    zaktualizowano = cmd.ExecuteNonQuery() > 0;

                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1531", ex.StackTrace));
                }
            }
            return zaktualizowano;


        }
        internal static List<Zadanie> PobierzZadania(int idSprintu)
        {
            List<Zadanie> zadaniaSprintu = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(@"SELECT [id_zadania]
      ,[id_sprinty]
      ,[id_zadania_typy]
      ,[tytul]
      ,[opis]
      ,[email_dodajacego]
      ,[row_date]
      ,[priorytet]
      ,[email_przydzielony_uzytkownik]
      ,[zadanie_nadrzedne]
      ,[data_zakonczenia]
      ,[deadline]
      ,[id_projekty]
  FROM Zadania
  WHERE id_sprinty = @idSprintu
  and is_usuniety = 0", con);

                    cmd.Parameters.AddWithValue("@idSprintu", idSprintu);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            zadaniaSprintu = new List<Zadanie>();
                            while (reader.Read())
                            {
                                Zadanie z = new Zadanie();

                                z.ZadanieID = int.Parse(reader["id_zadania"].ToString());
                                z.ZadanieProjektID = int.Parse(reader["id_projekty"].ToString());
                                z.ZadanieSprintID = int.Parse(reader["id_sprinty"].ToString());
                                TypZadania t = TypZadania.ZADANIE;
                                Enum.TryParse(reader["id_zadania_typy"].ToString(), out t);
                                z.ZadanieTypZadania = t;
                                z.ZadanieOpis = reader["opis"].ToString();
                                z.ZadanieDodajacy = HttpContext.Current.User.Identity.Name;
                                z.ZadanieDataUtworzenia = DateTime.Parse(reader["row_date"].ToString());
                                z.ZadaniePriorytet = int.Parse(reader["priorytet"].ToString());
                                z.ZadaniePrzypisaneDo = reader["email_przydzielony_uzytkownik"].ToString();

                                z.ZadanieNadrzedneID = int.Parse(reader["zadanie_nadrzedne"].ToString());
                                z.ZadanieDataUkonczenia = DateTime.Parse(reader["data_zakonczenia"].ToString());
                                z.ZadanieDeadline = DateTime.Parse(reader["deadline"].ToString());

                                zadaniaSprintu.Add(z);

                            }
                        }

                    }

                    cmd.Connection.Close();



                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1456", ex.StackTrace));
                }
            }
            return zadaniaSprintu;
        }

        internal static Sprint PobierzSprint(int idSprintu)
        {
            Sprint s = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT top 1 [id_sprinty]
                                  ,[id_projekty]
                                  ,[data_zakonczenia]
                                  ,[data_deadline]
                                  ,[row_date]
                                  ,[email_dodajacego]
                                  ,[nazwa]
                                  ,[opis]
                              FROM Sprinty
                              WHERE id_sprinty = @id_sprinty
                              and is_usuniety = 0
                            ", con);

                    cmd.Parameters.AddWithValue("@id_sprinty", idSprintu);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            s = new Sprint();
                            while (reader.Read())
                            {
                                s.SprintID = Int32.Parse(reader["id_sprinty"].ToString());
                                s.SprintProjektID = Int32.Parse(reader["id_projekty"].ToString());
                                //s.SprintDataZakonczenia = DateTime.Parse(reader["data_zakonczenia"].ToString());
                                s.SprintTerminWykonania = DateTime.Parse(reader["data_deadline"].ToString());
                                s.SprintDataUtworzenia = DateTime.Parse(reader["row_date"].ToString());
                                s.SprintIdTworzacego = reader["email_dodajacego"].ToString();
                                s.SprintNazwa = reader["nazwa"].ToString();
                                s.SprintOpis = reader["opis"].ToString();

                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1778", ex.StackTrace));
                }
            }

            return s;
        }

        internal static decimal procentWykonania( int idSprintu ) {

            decimal procentWykonania = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT CAST((SELECT CAST(
(SELECT COUNT(*) FROM Zadania
WHERE is_usuniety = 0
and id_sprinty = @idSprintu
and id_zadania_statusy = 'WYKONANE') AS DECIMAL(5, 2)))/
(SELECT CAST((
SELECT COUNT(*) FROM Zadania
WHERE is_usuniety = 0
and id_sprinty = @idSprintu) AS DECIMAL(5,2))) AS DECIMAL (5,2)) as procent_wykonania", con);

                    cmd.Parameters.AddWithValue("@idSprintu", idSprintu);
                    cmd.Connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            
                            while (reader.Read())
                            {
                                procentWykonania = Decimal.Parse(reader["procent_wykonania"].ToString());
                            }
                        }

                    }

                    cmd.Connection.Close();

                }
                catch (Exception ex)
                {
                    BazaDanych.DziennikProvider.Loguj(new Zdarzenie(ex.Message, "BazaDanych line 1888", ex.StackTrace));
                }
            }


            return procentWykonania;

        }
    }
}