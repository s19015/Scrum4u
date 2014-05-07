﻿using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Panel : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
        {
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }

        u= (Uzytkownik)Session["uzytkownik"];
        if (u!=null)
        {
            litImieNazwisko.Text = u.UzytkownikImie + " " + u.UzytkownikNazwisko;
            litEmail.Text = u.UzytkownikEmail;
        }
    }
    protected void btnWyloguj_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Session.Abandon();
        System.Web.Security.FormsAuthentication.SignOut();
    }
}
