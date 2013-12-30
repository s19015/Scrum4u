using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Zapisz()
        {

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
        //klasy do obslugi aplikacji
        public class Zdarzenie
        {
            public int ZdarzenieID { get; set; }
            public DateTime ZdarzenieData { get; set; }
            public string ZdarzenieNazwa { get; set; }
            public string ZdarzenieOpis { get; set; }
            public string ZdarzenieZrodlo { get; set; }
        }
    }
}