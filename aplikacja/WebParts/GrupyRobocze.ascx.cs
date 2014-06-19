using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebParts_GrupyRobocze : System.Web.UI.UserControl
{
    public bool PokazUsun { get; set; }
    public void ZaladujDane()
    {
        List<GrupaRobocza> grupy = GrupaRobocza.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true);

        if (grupy != null && grupy.Count() > 0)
        {
            grupyRobocze.Visible = true;
            grupyRobocze.DataSource = grupy;
            grupyRobocze.DataBind();
        }
        grupy = null;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ZaladujDane();
        }

    }
    public string WyswietlPokazUsun(string idGrupy, string email)
    {
        string wynik = "";
        if (PokazUsun && !String.IsNullOrEmpty(email))
        {
            if (email.Trim().ToLower() == HttpContext.Current.User.Identity.Name.Trim().ToLower())
            {
                wynik= "<a style=\"float:right;display:inline-block;\" href=\"/Panel/GrupyRobocze.aspx?usun=1&amp;id="+idGrupy +"\" onclick=\"return confirm('Czy na pewno chcesz usunąć grupę?');\">Usuń</a>";
            }
        }
        else
            wynik= "";

        return wynik;
    }
}