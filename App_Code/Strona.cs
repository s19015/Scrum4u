using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrum4u
{
    /// <summary>
    /// Summary description for Strona
    /// </summary>
    public class Strona : System.Web.UI.Page
    {
        public class Uzytkownik
        {
            public int UzytkownikID { get; set; }
            public string UzytkownikImie { get; set; }
            public string UzytkownikNazwisko { get; set; }
            public string UzytkownikEmail { get; set; }
            public string UzytkownikHaslo { get; set; }
        }
    }
}