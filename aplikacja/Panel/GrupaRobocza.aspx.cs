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
        Scrum4uMasterPage master = this.Page.Master as Scrum4uMasterPage;
        master.typStrony = Scrum4uHelper.TypStrony.element;
        master.rodzajStrony = Scrum4uHelper.RodzajStrony.GrupaRobocza;

        int idGrupy = 0;
        lblInfo.Visible = false;
        h4Usunieto.Visible = false;
        if (Request.QueryString["id"]!=null && int.TryParse(Request.QueryString["id"],out idGrupy))
        {
            grupa = GrupaRobocza.PobierzGrupe(idGrupy,true);
            if (grupa!=null)
            {
                litTytul.Text =  grupa.GrupaRoboczaNazwa;
                this.Page.Title = grupa.GrupaRoboczaNazwa + " - Grupa robocza - Scrum4u.pl";
                if (Request.QueryString["usunOsobe"]!=null && Scrum4uHelper.CzyJestToEmail(Request.QueryString["usunOsobe"]))
                {
                    GrupyRoboczeZaproszenie zapro = GrupyRoboczeZaproszenie.Pobierz(idGrupy, Request.QueryString["usunOsobe"]);
                    if (zapro!=null)
                    {
                        bool usunieto = zapro.Usun();

                        h4Usunieto.Visible = true;
                        if (usunieto)
                        {
                            h4Usunieto.InnerText = "Użytkownik usunięty poprawnie";
                            h4Usunieto.Attributes["class"] = "widgettitle title-success";
                        }
                        else
                        {
                            h4Usunieto.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
                            h4Usunieto.Attributes["class"] = "widgettitle title-danger";
                        }
                        
                    }
                    // nie moze to tak tu dzailac, poniewaz usunie id z query stringa
//                    lblInfo.Visible = true;
//                    lblInfo.Text = @"<script>
//var newurl = window.location.protocol + '//' + window.location.host + window.location.pathname
//
//window.history.pushState({path:newurl},'',newurl);
//</script>";
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

            GrupyRoboczeZaproszenia.ZaladujDane();
        }
        else
        {
            h4TytulDodajGrupe.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-danger";
        }
    }
}