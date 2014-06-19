using Scrum4u;
using Scrum4u.Aplikacja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_GrupaRobocza : System.Web.UI.Page
{
    public Scrum4u.GrupaRobocza grupa;
    protected void Page_Load(object sender, EventArgs e)
    {
        int idGrupy = 0;
        lblInfo.Visible = false;
        if (Request.QueryString["id"]!=null && int.TryParse(Request.QueryString["id"],out idGrupy))
        {
            grupa = GrupaRobocza.PobierzGrupe(idGrupy,true);
            if (grupa!=null)
            {
                litTytul.Text = "- " + grupa.GrupaRoboczaNazwa;

                if (Request.QueryString["usunOsobe"]!=null && Scrum4uHelper.CzyJestToEmail(Request.QueryString["usunOsobe"]))
                {
                    GrupyRoboczeZaproszenie zapro = GrupyRoboczeZaproszenie.Pobierz(idGrupy, Request.QueryString["usunOsobe"]);
                    if (zapro!=null)
                    {
                        bool usunieto = zapro.Usun();

                        if (usunieto)
                            Server.Transfer("/Panel/GrupaRobocza.aspx?id=" + idGrupy);
                    }
                }
            }
            else
            {
                lblInfo.Text = "Grupa nie znaleziona.";
                lblInfo.Visible = true;
                btnPokazDodajGrupe.Visible = false;
            }
        }
        else
        {
            lblInfo.Text = "Grupa nie znaleziona.";
            lblInfo.Visible = true;
            btnPokazDodajGrupe.Visible = false;
        }


    }
    protected void btnPokazDodajGrupe_Click(object sender, EventArgs e)
    {
        listaOsobWGrupie.Visible = true;
        btnPokazDodajGrupe.Visible = false;
    }
    protected void btnZapiszNowaGrupe_ServerClick(object sender, EventArgs e)
    {
        int idGrupy = 0;
        int.TryParse(Request.QueryString["id"],out idGrupy);
        if (idGrupy <= 0) {
            h4TytulDodajGrupe.InnerText = "Wystąpił błąd. Spróbuj ponownie później. Sprawdź grupę.";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-danger";
            return;
        }

        GrupyRoboczeZaproszenie noweZaproszenie = new GrupyRoboczeZaproszenie()
        {
            GrupyRoboczeZaproszenieData = DateTime.Now,
            GrupyRoboczeZaproszenieIDZapraszajacego = HttpContext.Current.User.Identity.Name,
            GrupyRoboczeZaproszenieIDZapraszanego = txtNazwaGrupy.Text.Trim().Replace("'", "''"),
            GrupyRoboczeZaproszenieAktywne = true,
            GrupyRoboczeGrupaRoboczaID = idGrupy
        };

        bool dodano = false;
        try
        {
            dodano = noweZaproszenie.Dodaj();
        }
        catch(Exception ex)
        {
            Zdarzenie.Loguj("GrupaRoboczaZapraszanie", "Blad", ex);
        }

        if (dodano)
        {
            panelDodajGrupe.Visible = false;
            h4TytulDodajGrupe.InnerText = "Zaproszenie zostało wysłane.";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-success";
        }
        else
        {
            h4TytulDodajGrupe.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-danger";
        }
    }
}