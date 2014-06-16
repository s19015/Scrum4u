using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Ustawienia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnZmienHaslo_ServerClick(object sender, EventArgs e)
    {
        bool zmieniono = false;
        Uzytkownik u = Uzytkownik.Pobierz(HttpContext.Current.User.Identity.Name);
        if (u != null)
            zmieniono = u.ZmienHaslo(txtHaslo.Text);

        if (zmieniono)
        {
            panelZmienHaslo.Visible = false;
            h4TytulDodajZadanie.InnerText = "Hasło zmienione poprawnie.";
            h4TytulDodajZadanie.Attributes["class"] = "widgettitle title-success";
        }
        else
        {
            h4TytulDodajZadanie.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajZadanie.Attributes["class"] = "widgettitle title-danger";
        }
    }
}