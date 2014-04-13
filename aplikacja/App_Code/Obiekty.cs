using Scrum4u.Aplikacja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;


namespace Scrum4u
{
    //enumeratory
    public enum Status { do_wykonania, w_trakcie, wykonane, odlozone }
    public enum TypZadania { zadanie, blad, pomysl }

    //klasy
    public class Uzytkownik
    {
        public int UzytkownikID { get; set; }
        public string UzytkownikImie { get; set; }
        public string UzytkownikNazwisko { get; set; }
        public string UzytkownikEmail { get; set; }
        public string UzytkownikHaslo { get; set; }
        public bool UzytkownikAktywny { get; set; }
        public DateTime UzytkownikDataUtworzenia { get; set; }
        public DateTime UzytkownikDataModyfikacja { get; set; }

        public Uzytkownik()
        {

        }

        public Uzytkownik(string email)
        {
            this.UzytkownikEmail = email;
             Uzytkownik u =  BazaDanych.UzytkownikProvider.PobierzUzytkownika(email);
            if (u!=null)
            {
                this.UzytkownikDataUtworzenia = u.UzytkownikDataUtworzenia;
                this.UzytkownikAktywny = u.UzytkownikAktywny;
                this.UzytkownikDataModyfikacja = u.UzytkownikDataModyfikacja;
                this.UzytkownikImie = u.UzytkownikImie;
                this.UzytkownikNazwisko = u.UzytkownikNazwisko;
            }
        }
        public string Dodaj()
        {
           string token = BazaDanych.UzytkownikProvider.DodajNowego(this);
            if (token.ToLower()!="false")
            {
                        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("<h3>Witaj <strong>"+this.UzytkownikImie+" "+this.UzytkownikNazwisko+"</strong></h3>");
        sb.AppendLine();
        sb.AppendLine("<p>Ten email został wysłany z <a href='http://www.scrum4u.pl'>http://www.scrum4u.pl</a>.</p>");
        sb.AppendLine();
        sb.AppendLine("<p>Dostałeś tego emaila, ponieważ ten adres e-mail został użyty podczas rejestracji konta w naszym serwisie.<br/>");
        sb.AppendLine("Jeśli nie Ty rejestrowałeś się w serwisie, to zignoruj tę wiadomość. Nie musisz się wypisywać, ani podejmować dodatkowych akcji.</p>");
        sb.AppendLine();
        sb.AppendLine("<p>Aby dokończyć proces rejestracji kliknij w poniższy link:<br/>");
        sb.AppendLine("<a href='http://www.scrum4u.pl/Aktywuj.aspx?e="+this.UzytkownikEmail+"&t="+token+"'>Aktywuj</a></p>");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("<p>Pozdrawiamy<br/>");
        sb.AppendLine("<a href='mailto:noreply@scrum4u.pl'>Zespół Scrum4u</a></p>");
                Aplikacja.Email e = new Aplikacja.Email();
                e.EmailOd = "noreply@scrum4u.pl";
                e.EmailDo = this.UzytkownikEmail;
                e.EmailTemat = "Rejestracja w serwisie Scrum4u.pl - Link aktywacyjny";
                e.EmailTresc = sb.ToString();
                e.EmailWersja = Aplikacja.Email.Wersja.HTML.ToString();
                e.EmailDataKolejki = DateTime.Now;

                try
                {
                    BazaDanych.EmailProvider.DodajEmailaDoKolejki(e);
                }
                catch (Exception ex)
                {
                    Aplikacja.Zdarzenie.Loguj("EmailRejestracyjny", "Blad", ex);
                }
            }

           return token;
        }

        public static bool SprawdzCzyIstnieje(Uzytkownik u)
        {
            return BazaDanych.UzytkownikProvider.SprawdzCzyIstnieje(u.UzytkownikEmail);
        }

        public static bool Aktywuj(string email, string token)
        {
            return BazaDanych.UzytkownikProvider.AktywujUzytkownika(email, token);
        }

        public static bool Zaloguj(string email, string haslo)
        {
            return BazaDanych.UzytkownikProvider.ZalogujUzytkownika(email, haslo);
        }

        public static bool WyslijPrzypomnienie(string email)
        {
            bool wyslano = false;
            string token = BazaDanych.UzytkownikProvider.PobierzTokenRejestracyjny(email);
            if (token.ToLower() != "false")
            {
                string hash = Scrum4uHelper.PobierzHashMD5(email+token+"!");
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("<h3>Witamy serdecznie</h3>");
                sb.AppendLine();
                sb.AppendLine();sb.AppendLine();
                sb.AppendLine("<p>Aby zresetować swoje hasło w serwisie Scrum4u, kliknij poniższy link i wpisz nowe hasło:<br/>");
                sb.AppendLine("<a href='http://www.scrum4u.pl/ResetHasla.aspx?e=" + email + "&t=" + token + "&h="+hash+"'>Reset hasła</a></p>");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("<p>Pozdrawiamy<br/>");
                sb.AppendLine("<a href='mailto:noreply@scrum4u.pl'>Zespół Scrum4u</a></p>");

                Email e = new Email();
                e.EmailDo = email;
                e.EmailTemat = "Prośba o zmianę hasła";
                e.EmailTresc = sb.ToString();
                e.EmailWersja = Aplikacja.Email.Wersja.HTML.ToString();
                e.EmailDataKolejki = DateTime.Now;

                try
                {
                    wyslano = BazaDanych.EmailProvider.DodajEmailaDoKolejki(e);
                    Email.WyslijEmaile();
                }
                catch (Exception ex)
                {
                    Aplikacja.Zdarzenie.Loguj("EmailResetHasla", "Blad", ex);
                }

               
            }
            return wyslano;
        }

        public static bool ZmienHaslo(string email, string haslo)
        {
            Uzytkownik u = new Uzytkownik(email);
            if (u!=null)
            {
                u.UzytkownikHaslo = haslo;
                return u.Aktualizuj();
            }
            return false;
        }

        private bool Aktualizuj()
        {
            return BazaDanych.UzytkownikProvider.Aktualizuj(this);
        }
    }
    public class Tag
    {
        public int TagID { get; set; }
        public string TagNazwa { get; set; }
    }

    public class GrupaRobocza
    {
        public int GrupaRoboczaID { get; set; }
        public int GrupaRoboczaUzytkownikID { get; set; }

    }
    public class TokenRejestracji
    {
        public int TokenRejestracjiID { get; set; }
        public Guid TokenRejestracjiGuid { get; set; }
        public string TokenRejestracjiUzytkownikEmail { get; set; }
        public DateTime TokenRejestracjiDataWaznosci { get; set; }
    }

    public class ProjektZaproszenie
    {
        public int ProjektZaproszenieID { get; set; }
        public int ProjektZaproszenieProjektID { get; set; }
        public string ProjektZaproszenieUzytkownikEmail { get; set; }
    }

    public class Projekt
    {
        public int ProjektID { get; set; }
        public int ProjektGrupaRoboczaID { get; set; }
        public int ProjektScrumMasterID { get; set; }
        public string ProjektNazwa { get; set; }
        public DateTime ProjektDataUtworzenia { get; set; }
    }
    public class Sprint
    {
        public int SprintID { get; set; }
        public int SprintProjektID { get; set; }
        public string SprintNazwa { get; set; }
        public string SprintOpis { get; set; }
        public string SprintTerminWykonania { get; set; }
    }

    public class Zadanie
    {
        public int ZadanieID { get; set; }
        public int ZadanieProjektID { get; set; }
        public int ZadanieSprintID { get; set; }
        public TypZadania ZadanieTypZadania { get; set; }
        public string ZadanieNazwa { get; set; }
        public Status ZadanieStatus { get; set; }
        public bool ZadaniePriorytet { get; set; }
        public DateTime ZadanieDataUtworzenia { get; set; }
        public DateTime ZadanieDataRozpoczecia { get; set; }
        public DateTime ZadanieDataUkonczenia { get; set; }
    }

    public class Blad : Zadanie
    {

    }

    public class Pomysl : Zadanie
    {

    }


    namespace Aplikacja
    {
        public class Email
        {
            public enum Wersja { HTML, TEXT }
            public int EmailID { get; set; }
            public string EmailOd { get; set; }
            public string EmailDo { get; set; }
            public string EmailTemat { get; set; }
            public string EmailTresc { get; set; }
            public string EmailWersja { get; set; }
            public bool EmailWyslany { get; set; }
            public DateTime EmailDataKolejki { get; set; }
            public DateTime EmailDataWyslania { get; set; }
            public bool DodajDoKolejki()
            {
                return BazaDanych.EmailProvider.DodajEmailaDoKolejki(this);
            }
            public bool Usun()
            {
                return BazaDanych.EmailProvider.UsunZKolejki(this);
            }

            public static string WyslijEmaile()
            {
                string wynik ="Nie było żadnych emaili do wysłania.";

                int wyslane = 0, niewyslane = 0;
                try
                {
                    List<Email> listaWyslanych = new List<Email>();
                    List<Email> lista = BazaDanych.EmailProvider.PobierzWszystkieNieWyslaneEmaile(DateTime.MinValue);
                    if (lista != null)
                    {
                        MailMessage wiadomosc = new MailMessage();
                        SmtpClient mailClient = new SmtpClient();
                        mailClient.EnableSsl = false;
                        if (lista.Count > 0)
                        {
                            foreach (Email e in lista)
                            {
                                bool bWyslane = true;

                                if (e.EmailWersja.Trim() == Email.Wersja.HTML.ToString().Trim())
                                {
                                    wiadomosc = new MailMessage(e.EmailOd, e.EmailDo, e.EmailTemat, e.EmailTresc);
                                    wiadomosc.IsBodyHtml = true;
                                }
                                else
                                {
                                    wiadomosc = new MailMessage(e.EmailOd, e.EmailDo, e.EmailTemat, e.EmailTresc);
                                    wiadomosc.IsBodyHtml = false;
                                }
                                wiadomosc.BodyEncoding = System.Text.Encoding.UTF8;
                                wiadomosc.SubjectEncoding = System.Text.Encoding.UTF8;

                                try
                                {
                                    mailClient.Send(wiadomosc);
                                }
                                catch (Exception ex)
                                {
                                    niewyslane++;
                                    bWyslane = false;
                                    Zdarzenie.Loguj("KolejkaEmaili", "Blad", ex);
                                }
                                if (bWyslane) listaWyslanych.Add(e);
                                wiadomosc.Dispose();
                                wiadomosc = null;
                            }
                        }
                        if (listaWyslanych != null)
                        {
                            if (listaWyslanych.Count > 0) BazaDanych.EmailProvider.ZapiszWyslaneEmaileDoKolejki(listaWyslanych);
                        }
                        wyslane = lista.Count - niewyslane;
                        Zdarzenie.Loguj("KolejkaEmaili", "Info", "Wyslano: " + wyslane + "; Niewyslane: " + niewyslane + "; Emaile: " + lista.ToArray().ToString());
                        wynik = "Wyslano: " + wyslane + "; Niewyslane: " + niewyslane + ";";
                    }
                }
                catch (Exception ex)
                {
                    Zdarzenie.Loguj("KolejkaEmaili", "Blad", ex);
                }

                return wynik;
            }
        }
        //klasy do obslugi aplikacji
        public class Zdarzenie
        {
            public Zdarzenie()
            {

            }
            public Zdarzenie(string opis, string zrodlo, string stackTrace)
            {
                // TODO: Complete member initialization
                this.ZdarzenieOpis = opis;
                this.ZdarzenieZrodlo = zrodlo;
                this.ZdarzenieStackTrace = stackTrace;
            }
            public int ZdarzenieID { get; set; }
            public DateTime ZdarzenieData { get; set; }
            public string ZdarzenieOpis { get; set; }
            public string ZdarzenieZrodlo { get; set; }
            public string ZdarzenieStackTrace { get; set; }

            public static bool Loguj(string zrodlo, string opis, Exception ex)
            {
                return BazaDanych.DziennikProvider.Loguj(new Zdarzenie() { ZdarzenieZrodlo = opis + "|" + zrodlo, ZdarzenieOpis = ex.Message, ZdarzenieStackTrace = ex.StackTrace });
            }

            public static bool Loguj(string zrodlo, string opis, string wiadomosc)
            {
                return BazaDanych.DziennikProvider.Loguj(new Zdarzenie() { ZdarzenieZrodlo = opis + "|" + zrodlo, ZdarzenieOpis = opis, ZdarzenieStackTrace = wiadomosc });
            }
        }
    }
}