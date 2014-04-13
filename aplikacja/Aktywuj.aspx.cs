﻿using Scrum4u.Aplikacja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Aktywuj : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool zapisano = false;
        if (Request.QueryString["e"] != null && Request.QueryString["t"] != null && !String.IsNullOrEmpty(Request.QueryString["e"]) && !String.IsNullOrEmpty(Request.QueryString["t"]))
        {
            string email = Request.QueryString["e"].Replace("'","''");
            string token = Request.QueryString["t"].Replace("'","''");

            try 
            {
               zapisano = Scrum4u.Uzytkownik.Aktywuj(email, token);
            }
            catch(Exception ex)
            {
                Zdarzenie.Loguj("AktywacjaKonta", "Blad", ex);
            }

            if (zapisano)
            {
                litInfo.Text = "Konto aktywowane. Możesz się zalogować <a href='/Zaloguj.aspx'>tutaj.</a>";

                Zdarzenie.Loguj("AktywacjaUzytkownika", "Info", "Email: " + email+ " aktywowany poprawnie");
            }
            else
            {
                litInfo.Text = "Niepoprawny link aktywacyjny. Skontaktuj się z <a href='mailto:noreply@scrum4u.pl'>administratorem</a>";
            }
        }
        else
        {
            litInfo.Text = "Niepoprawny link aktywacyjny. Skontaktuj się z <a href='mailto:noreply@scrum4u.pl'>administratorem</a>";
        }
    }
}