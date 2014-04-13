using Scrum4u;
using Scrum4u.Aplikacja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Aktywuj : System.Web.UI.Page
{
    private string email = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["e"] != null && Request.QueryString["t"] != null && Request.QueryString["h"] != null && !String.IsNullOrEmpty(Request.QueryString["e"]) && !String.IsNullOrEmpty(Request.QueryString["t"]) && !String.IsNullOrEmpty(Request.QueryString["h"]))
        {
            email = Request.QueryString["e"].Replace("'","''");
            string token = Request.QueryString["t"].Replace("'","''");
            string hash_sprawdzajacy = Request.QueryString["h"].Replace("'", "''");

            string hash = Scrum4uHelper.PobierzHashMD5(email + token + "!");

            if (hash!=hash_sprawdzajacy)
            {
                formResetHasla.Visible = false;
                lblInfo.Text = "Niepoprawny link. Skontaktuj się z <a href='mailto:noreply@scrum4u.pl'>administratorem</a>";
            }

        }
        else
        {
            formResetHasla.Visible = false;
            lblInfo.Text = "Niepoprawny link. Skontaktuj się z <a href='mailto:noreply@scrum4u.pl'>administratorem</a>";
        }
    }
    protected void btnResetujHaslo_Click(object sender, EventArgs e)
    {
        if (Uzytkownik.ZmienHaslo(email,txtHaslo.Text))
        {
            lblInfo.Text = "Hasło zmienione poprawnie. Możesz się teraz <a href='/Zaloguj.aspx'>zalogować </a>";
            lblInfo.ForeColor = System.Drawing.Color.Green;
            formResetHasla.Visible = false;
        }
        else
        {
            lblInfo.ForeColor = System.Drawing.Color.Red;
            lblInfo.Text = "Wystąpił błąd. Spróbuj ponownie później.";
        }
    }
}