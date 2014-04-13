using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Zaloguj : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnZaloguj_Click(object sender, EventArgs e)
    {
        string email=txtEmail.Text.Replace("'", "''").Trim();
        if(Uzytkownik.Zaloguj(email, txtHaslo.Text))
        {

            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                FormsAuthentication.SetAuthCookie(email, false);
                Response.Redirect("~/Panel/");
            }
            else
                FormsAuthentication.RedirectFromLoginPage(email, false);

        }
        else
        {
            lblInfo.ForeColor = System.Drawing.Color.Red;
            lblInfo.Text = "Użytkownik nie jest aktywny lub nieprawidłowe dane logowania.";
        }
    }
    protected void lnkNiePamietamHasla_Click(object sender, EventArgs e)
    {
        panelHasla.Visible = !panelHasla.Visible;
    }
    protected void btnWyslijPrzypomnienie_Click(object sender, EventArgs e)
    {
        if (Uzytkownik.WyslijPrzypomnienie(txtEmailPrzypomnienie.Text.Replace("'", "''")))
        {
            lblInfo.ForeColor = System.Drawing.Color.Green;
            lblInfo.Text = "Email z linkiem do zmiany hasła został wysłany.";
        }
        else
        {
            lblInfo.ForeColor = System.Drawing.Color.Red;
            lblInfo.Text = "Nieznany adres email";
        }
    }
}