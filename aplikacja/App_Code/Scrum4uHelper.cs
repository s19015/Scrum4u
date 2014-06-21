using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Scrum4u
{
    /// <summary>
    /// Summary description for Scrum4uHelper
    /// </summary>
    public class Scrum4uHelper
    {
        public Scrum4uHelper()
        {
            this.ObecnyUzytkownik = (Uzytkownik)HttpContext.Current.Session["uzytkownik"];
        }
        public static string PobierzHashMD5(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
        /// <summary>
        /// Pobiera obecnie zalogowanego uzytkownika
        /// </summary>
        /// <returns>Obecnie zalogowany uzytkownik</returns>
        public Uzytkownik ObecnyUzytkownik = (Uzytkownik)HttpContext.Current.Session["uzytkownik"];

        public static bool SprawdzDataViewJestNullLubPusty(System.Data.DataView dv)
        {
            if (dv == null) return true;
            if (dv.Table.Rows.Count == 0) return true;
            return false;
        }
        /// <summary>
        /// Sprawdza czy dany tekst jest adresem email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CzyJestToEmail(string email)
        {
            Regex r = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return r.IsMatch(email);
        }

        public enum TypStrony { domyslna, lista, element }
        public enum RodzajStrony { GrupaRobocza, Projekt, Projekty, Zadanie }

        public static string PobierzStatus(object Status)
        {
            string wynik = "";
            if (Status != null)
            {
                string sStatus = Status.ToString();

                Scrum4u.Status s = (Scrum4u.Status)Enum.Parse(typeof(Scrum4u.Status), sStatus, true);

                switch (s)
                {
                    case Scrum4u.Status.DOWYKONANIA:
                        wynik = "Do wykonania";
                        break;
                    case Scrum4u.Status.WTRAKCIE:
                        wynik = "W trakcie";
                        break;
                    case Scrum4u.Status.ODLOZONE:
                        wynik = "Odłożone";
                        break;
                    case Scrum4u.Status.WYKONANE:
                        wynik = "Wykonane";
                        break;
                }
            }

            return wynik;
        }
    }
}