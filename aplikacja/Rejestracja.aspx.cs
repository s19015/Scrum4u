using Scrum4u;
using Scrum4u.Aplikacja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Rejestracja : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["email"]!=null && Scrum4uHelper.CzyJestToEmail(Request.QueryString["email"]))
        {
            txtEmail.Text = Uzytkownik.PoprawPlusWMailu(Request.QueryString["email"].Trim(),true);
        }
    }
    protected void btnRejestruj_Click(object sender, EventArgs e)
    {
        Uzytkownik u = new Uzytkownik();
        u.UzytkownikImie = txtImie.Text;
        u.UzytkownikNazwisko = txtNazwisko.Text;
        u.UzytkownikEmail = txtEmail.Text;
        u.UzytkownikHaslo = txtHaslo.Text;

        if (!Uzytkownik.SprawdzCzyIstnieje(u))
        {


            bool zapisano = false;
            try
            {
                if (u.Dodaj().ToLower() != "false")
                    zapisano = true;

            }
            catch (Exception ex)
            {
                Zdarzenie.Loguj("RejestracjaUzytkownika", "Blad", ex);
            }

            if (zapisano)
            {
                lblInfo.ForeColor = System.Drawing.Color.Green;
                lblInfo.Text = "Użytkownik poprawnie zapisany. Na podany adres email wysłaliśmy adres z linkiem aktywującym.";
                formRejestracja.Visible = false;

                Zdarzenie.Loguj("RejestracjaUzytkownika", "Info", "Użytkownik: " + u.UzytkownikImie + " " + u.UzytkownikNazwisko + " zarejestrowany poprawnie");
            }
            else
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "Wystąpił błąd. Spróbuj ponownie później.";
            }
        }
        else
        {
            lblInfo.ForeColor = System.Drawing.Color.Red;
            lblInfo.Text = "Podany adres email jest już zarejestrowany w naszym serwisie.";
        }
    }
}