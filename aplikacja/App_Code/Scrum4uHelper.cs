using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrum4u
{
    /// <summary>
    /// Summary description for Scrum4uHelper
    /// </summary>
    public class Scrum4uHelper
    {
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
        public static Uzytkownik ObecnyUzytkownik = (Uzytkownik)HttpContext.Current.Session["uzytkownik"];

    }
}