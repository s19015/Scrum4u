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
    public enum Status { do_wykonania=0, w_trakcie=1, wykonane=2, odlozone=3 }
    public enum TypZadania 
    { 
        ZADANIE, 
        BLAD, 
        POMYSL 
    }

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
        sb.AppendLine("<a href='http://www.scrum4u.pl/Aktywuj.aspx?e=" + PoprawPlusWMailu(this.UzytkownikEmail) + "&t=" + token + "'>Aktywuj</a></p>");
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

        public static string PoprawPlusWMailu(string e)
        {
            return e.Replace("+", "%2B");
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
        public static Uzytkownik Pobierz(string email)
        {
            return BazaDanych.UzytkownikProvider.PobierzUzytkownika(email);
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

        public bool ZmienHaslo(string haslo)
        {
            return BazaDanych.UzytkownikProvider.ZmienHaslo(this, haslo);
        }

        public static bool ZmienHaslo(string email, string haslo)
        {
            Uzytkownik u = Uzytkownik.Pobierz(email);
            if (u != null)
                return u.ZmienHaslo(haslo);
            return false;
        }
    }

    public class GrupaRobocza
    {
        public int GrupaRoboczaID { get; set; }
        public string GrupaRoboczaUzytkownikID { get; set; }
        public string GrupaRoboczaNazwa { get; set; }
        public bool GrupaRoboczaAktywna { get; set; }
        public DateTime GrupaRoboczaData { get; set; }


        public bool Dodaj()
        {
            return BazaDanych.GrupaRoboczaProvider.DodajNowa(this);
        }

        public static List<GrupaRobocza> PobierzWszystkie(string email, bool doKtorychNaleze)
        {
            return BazaDanych.GrupaRoboczaProvider.PobierzWszystkie(email, doKtorychNaleze);
        }

        public static GrupaRobocza PobierzGrupe(int idGrupy, bool doKtorejNaleze)
        {
            return BazaDanych.GrupaRoboczaProvider.Pobierz(idGrupy, doKtorejNaleze);
        }

        public static List<Uzytkownik> PobierzWszystkichUzytkownikow(int idGrupy)
        {
            return BazaDanych.GrupaRoboczaProvider.PobierzUzytkownikowGrupy(idGrupy);
        }
    }

    public class GrupyRoboczeZaproszenie
{
        public int GrupyRoboczeZaproszenieID { get; set; }
        public int GrupyRoboczeGrupaRoboczaID { get; set; }
        public string GrupyRoboczeZaproszenieIDZapraszajacego { get; set; }
        public string GrupyRoboczeZaproszenieIDZapraszanego { get; set; }
        public string GrupyRoboczeZaproszenieToken { get; set; }
        public bool GrupyRoboczeZaproszenieAktywne { get; set; }
        public DateTime GrupyRoboczeZaproszenieData { get; set; }

        public bool Dodaj()
        {
            string token = BazaDanych.GrupyRoboczeZaproszenieProvider.Dodaj(this);
            bool dodano = false;
            if (token.ToLower() != "false")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("<h3>Witaj</h3>");
                sb.AppendLine();
                sb.AppendLine("<p>Ten email został wysłany z <a href='http://www.scrum4u.pl'>http://www.scrum4u.pl</a>.</p>");
                sb.AppendLine();
                sb.AppendLine("<p>Dostałeś tego emaila, ponieważ użytkownik "+this.GrupyRoboczeZaproszenieIDZapraszajacego+" zaprosił Cie do swojej grupy roboczej <strong>"+GrupaRobocza.PobierzGrupe(this.GrupyRoboczeGrupaRoboczaID,false).GrupaRoboczaNazwa+"</strong>.<br/>");
                sb.AppendLine("Jeśli nie rejestrowałeś się jeszcze w serwisie, zapraszamy do rejestracji aby wspólnie z "+this.GrupyRoboczeZaproszenieIDZapraszajacego+" tworzyć projekty.</p>");
                sb.AppendLine();
                sb.AppendLine("<p>Jeśli wyrazasz zgodę na dołączenie do grupy roboczej kliknij w poniższy link:<br/>");
                sb.AppendLine("<a href='http://www.scrum4u.pl/Aktywuj.aspx?gr=1&e=" + Uzytkownik.PoprawPlusWMailu(this.GrupyRoboczeZaproszenieIDZapraszanego) + "&t=" + token + "&id="+this.GrupyRoboczeGrupaRoboczaID+"'>Potwierdzam</a></p>");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("<p>Pozdrawiamy<br/>");
                sb.AppendLine("<a href='mailto:noreply@scrum4u.pl'>Zespół Scrum4u</a></p>");
                Aplikacja.Email e = new Aplikacja.Email();
                e.EmailOd = "noreply@scrum4u.pl";
                e.EmailDo = this.GrupyRoboczeZaproszenieIDZapraszanego;
                e.EmailTemat = "Zaproszenie do grupy roboczej Scrum4u.pl";
                e.EmailTresc = sb.ToString();
                e.EmailWersja = Aplikacja.Email.Wersja.HTML.ToString();
                e.EmailDataKolejki = DateTime.Now;

                try
                {
                   dodano= BazaDanych.EmailProvider.DodajEmailaDoKolejki(e);
                }
                catch (Exception ex)
                {
                    Aplikacja.Zdarzenie.Loguj("EmailZaproszenie", "Blad", ex);
                }
            }
            return dodano;
        }

        public static bool Aktywuj(string email, string token, int idGrupyRoboczej)
        {
            return BazaDanych.GrupyRoboczeZaproszenieProvider.PotwierdzZaproszenie(email, token, idGrupyRoboczej);
        }

        public static List<GrupyRoboczeZaproszenie> PobierzWszystkie(int idGrupy)
        {
            return BazaDanych.GrupyRoboczeZaproszenieProvider.PobierzWszystkie(idGrupy);
        }

        public static GrupyRoboczeZaproszenie Pobierz(int idGrupy, string email)
        {
            return BazaDanych.GrupyRoboczeZaproszenieProvider.Pobierz(idGrupy, email);
        }

        public bool Usun()
        {
            return BazaDanych.GrupyRoboczeZaproszenieProvider.Usun(this);
        }
}
    public class TokenRejestracji
    {
        public int TokenRejestracjiID { get; set; }
        public Guid TokenRejestracjiGuid { get; set; }
        public string TokenRejestracjiUzytkownikEmail { get; set; }
        public DateTime TokenRejestracjiDataWaznosci { get; set; }
    }

    public class Projekt
    {
        public int ProjektID { get; set; }
        public int ProjektGrupaRoboczaID { get; set; }
        public string ProjektScrumMasterID { get; set; }
        public string ProjektManagerProjektuID { get; set; }
        public bool ProjektAktywny { get; set; }
        public string ProjektNazwa { get; set; }
        public DateTime ProjektDataUtworzenia { get; set; }
        public string ProjektOpis { get; set; }

        public bool Dodaj()
        {
            return BazaDanych.ProjektProvider.DodajNowy(this);
;
        }

        public static List<Projekt> PobierzWszystkie(string email, bool doKtorychNaleze)
        {
            return BazaDanych.ProjektProvider.PobierzWszystkie(email, doKtorychNaleze,-1);
        }
        /// <summary>
        /// Pobiera dane projektu
        /// </summary>
        /// <param name="idProjektu">idProjektu</param>
        /// <param name="tylkoPowiazaneZeMna">Czy pobrac projekt do ktorego naleze</param>
        /// <returns></returns>
        public static Projekt Pobierz(int idProjektu, bool tylkoPowiazaneZeMna)
        {
            return BazaDanych.ProjektProvider.Pobierz(idProjektu, tylkoPowiazaneZeMna);
        }

        public static List<Projekt> PobierzWszystkie(string email, bool doKtorychNaleze, int idGrupy)
        {
            return BazaDanych.ProjektProvider.PobierzWszystkie(email, doKtorychNaleze, idGrupy);
        }
    }
    public class Sprint
    {
        public int SprintID { get; set; }
        public int SprintProjektID { get; set; }
        public string SprintNazwa { get; set; }
        public string SprintOpis { get; set; }
        public DateTime SprintTerminWykonania { get; set; }
        public DateTime SprintDataZakonczenia { get; set; }
        public DateTime SprintDataUtworzenia { get; set; }
        public int SprintIdTworzacego { get; set; }

        public bool Dodaj()
        {
            return BazaDanych.SprintProvider.Dodaj(this);
        }

        public bool Usun()
        {
            return BazaDanych.SprintProvider.Usun(this);
        }

        public bool Aktualizuj()
        {
            return BazaDanych.SprintProvider.Aktualizuj(this);
        }
        public static List<Sprint> PobierzWszystkieDoProjektu(int idProjektu)
        {
            return BazaDanych.ProjektProvider.PobierzSprinty(idProjektu);
        }
    }

    public class Zadanie
    {
        public int ZadanieID { get; set; }
        public int ZadanieProjektID { get; set; }
        public int ZadanieSprintID { get; set; }
        public TypZadania ZadanieTypZadania { get; set; }
        public string ZadanieNazwa { get; set; }
        public string ZadanieOpis { get; set; }
        public Status ZadanieStatus { get; set; }
        public int ZadaniePriorytet { get; set; }
        public DateTime ZadanieDataUtworzenia { get; set; }
        //public DateTime ZadanieDataRozpoczecia { get; set; }
        public DateTime ZadanieDataUkonczenia { get; set; }
        public string ZadaniePrzypisaneDo { get; set; }
        public string ZadanieDodajacy { get; set; }
        public int ZadanieNadrzedneID { get; set; }
        public DateTime ZadanieDeadline { get; set; }

        public bool Dodaj()
        {
            return BazaDanych.ZadanieProvider.Dodaj(this);
        }
        public bool Usun()
        {
            return BazaDanych.ZadanieProvider.Usun(this);
        }
        public bool Usun(int idZadania)
        {
            bool usunieto = false;
           Zadanie z = Pobierz(idZadania);
           if (z != null)
               usunieto=z.Usun();

           return usunieto;
        }

        public Zadanie Pobierz(int idZadania)
        {
            return BazaDanych.ZadanieProvider.Pobierz(idZadania);
        }

        public List<Zadanie> PobierzWszystkie(string email)
        {
            return BazaDanych.ZadanieProvider.PobierzWszystkie(email);
        }

        public List<Zadanie> PobierzWszystkie(int idProjektu)
        {
            return BazaDanych.ZadanieProvider.PobierzWszystkie(idProjektu);
        }
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
                    List<Email> lista = BazaDanych.EmailProvider.PobierzWszystkieNieWyslaneEmaile();
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