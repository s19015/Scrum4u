using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_GrupyRobocze : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"]!=null)
        {

        }
    }
    protected void btnPokazDodajGrupe_Click(object sender, EventArgs e)
    {
        formDodajGrupe.Visible = true;
        btnPokazDodajGrupe.Visible = false;
    }
    protected void btnZapiszNowaGrupe_ServerClick(object sender, EventArgs e)
    {

        GrupaRobocza gr = new GrupaRobocza { GrupaRoboczaUzytkownikID = HttpContext.Current.User.Identity.Name, GrupaRoboczaAktywna = false, GrupaRoboczaNazwa = txtNazwaGrupy.Text, GrupaRoboczaData = DateTime.Now };

        bool dodano = false;
        if (gr!=null)
        {
            dodano=gr.Dodaj();
            dodano = true;
        }

        if (dodano)
        {
            panelDodajGrupe.Visible = false;
            h4TytulDodajGrupe.InnerText = "Grupa robocza dodana poprawnie";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-success";
        }
        else
        {
            h4TytulDodajGrupe.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-danger";
        }
    }
}