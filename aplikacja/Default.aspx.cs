using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Scrum4u;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnZaloguj_ServerClick(object sender, EventArgs e)
    {

        string email = txtEmail.Text.Replace("'", "''").Trim();
        if (Uzytkownik.Zaloguj(email, txtHaslo.Text))
        {
            FormsAuthentication.SetAuthCookie(email, false);
            Session.Add("uzytkownik", Uzytkownik.Pobierz(email));

            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                Response.Redirect("~/Panel/");
            }
            else
                FormsAuthentication.RedirectFromLoginPage(email, false);

        }
        else
        {
            FormsAuthentication.RedirectToLoginPage("e=" + email + "&s=0");
        }
    }
}