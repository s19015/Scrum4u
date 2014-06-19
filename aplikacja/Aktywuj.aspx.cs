using Scrum4u.Aplikacja;
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
        if (Request.QueryString["gr"] != null && Request.QueryString["gr"] == "1")
        {
            AktywujWProjekcie();
        }
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

    private void AktywujWProjekcie()
    {

        bool zapisano = false;
        if (Request.QueryString["e"] != null && Request.QueryString["t"] != null && !String.IsNullOrEmpty(Request.QueryString["e"]) && !String.IsNullOrEmpty(Request.QueryString["t"]))
        {
            string email = Request.QueryString["e"].Replace("'", "''");
            string token = Request.QueryString["t"].Replace("'", "''");
            string idGrupy = Request.QueryString["id"].Replace("'", "''");
            int idGrupyRoboczej = 0;
            int.TryParse(idGrupy, out idGrupyRoboczej);
            try
            {
                zapisano = Scrum4u.GrupyRoboczeZaproszenie.Aktywuj(email, token, idGrupyRoboczej);
            }
            catch (Exception ex)
            {
                Zdarzenie.Loguj("PotwierdzenieGrupy", "Blad", ex);
            }

            if (zapisano)
            {
                litInfo.Text = "Zostałeś dodany do grupy roboczej. Możesz się zalogować <a href='/Zaloguj.aspx'>tutaj.</a>";

                Zdarzenie.Loguj("PotwierdzenieGrupy", "Info", "Email: " + email + " potiwerdzony");
            }
            else
            {
                litInfo.Text = "Niepoprawny link. Skontaktuj się z <a href='mailto:noreply@scrum4u.pl'>administratorem</a>";
            }

            //Rejestruje, jeśli użytkownik jeszcze nie jest zarejestrowany.
            bool istnieje = Scrum4u.Uzytkownik.SprawdzCzyIstnieje(new Scrum4u.Uzytkownik(email));
            if (zapisano && !istnieje)
            {
                Response.Redirect("/Rejestracja.aspx?email=" + Scrum4u.Uzytkownik.PoprawPlusWMailu(email));
            }
            if (istnieje)
            {
                Response.Redirect("/Panel/GrupaRobocza.aspx?id=" + idGrupyRoboczej);
            }
        }
        else
        {
            litInfo.Text = "Niepoprawny link aktywacyjny. Skontaktuj się z <a href='mailto:noreply@scrum4u.pl'>administratorem</a>";
        }
    }
}