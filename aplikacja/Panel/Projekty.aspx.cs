using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Projekty : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Scrum4uMasterPage master = this.Page.Master as Scrum4uMasterPage;
        master.typStrony = Scrum4u.Scrum4uHelper.TypStrony.lista;
        master.rodzajStrony = Scrum4u.Scrum4uHelper.RodzajStrony.Projekty;

        //if (!IsPostBack)
        //{
        //    //laduje grupy robocze
        //    ddGrupaRobocza.DataSource=Scrum4u.GrupaRobocza.PobierzWszystkie(HttpContext.Current.User.Identity.Name, false);
        //    ddGrupaRobocza.DataValueField = "GrupaRoboczaID";
        //    ddGrupaRobocza.DataTextField = "GrupaRoboczaNazwa";
        //    ddGrupaRobocza.DataBind();
        //}

        if (!IsPostBack)
        {
            if (Request.QueryString["id_gr"]!=null)
            {
                int idGrupyRoboczej = 0;
                int.TryParse(Request.QueryString["id_gr"], out idGrupyRoboczej);
                Projekty1.ZaladujDane(idGrupyRoboczej);
                if (idGrupyRoboczej <= 0) return;

                if (Request.QueryString["dodaj"]!=null && Request.QueryString["dodaj"]=="1")
                {
                    btnPokazDodajProjekt_Click(sender, e);
                }

                Scrum4u.GrupaRobocza gr = Scrum4u.GrupaRobocza.PobierzGrupe(idGrupyRoboczej, true);
                if (gr!=null)
                {
                    lblNazwaGrupy.Text = gr.GrupaRoboczaNazwa;
                }
                else
                {
                    NiepoprawnaGrupa();
                }
            }
        }
    }

    private void NiepoprawnaGrupa()
    {
        formDodaJprojekt.Visible = false;
        btnPokazDodajProjekt.Visible = false;
        Projekty1.Visible = false;
        lblInfo.Text = "Nie znaleziono grupy roboczej";
        lblInfo.Visible = true;
    }
    protected void btnPokazDodajProjekt_Click(object sender, EventArgs e)
    {
        formDodaJprojekt.Visible = true;
        btnPokazDodajProjekt.Visible = false;
    }
    protected void btnZapiszNowyProjekt_ServerClick(object sender, EventArgs e)
    {
        int idGrupy =0;
        int.TryParse(Request.QueryString["id_gr"],out idGrupy);
        Scrum4u.Projekt p = new Scrum4u.Projekt() { ProjektOpis=txtOpisProjektu.Text, ProjektDataUtworzenia=DateTime.Now, ProjektGrupaRoboczaID= idGrupy, ProjektScrumMasterID=HttpContext.Current.User.Identity.Name,ProjektManagerProjektuID=HttpContext.Current.User.Identity.Name,  ProjektNazwa=txtNazwaProjektu.Text, ProjektAktywny=true};
        
        bool dodano = false;
        if (p!=null)
            dodano = p.Dodaj();

        if (dodano)
        {
            panelDodajProjekt.Visible = false;
            h4TytulDodajProjekt.InnerText = "Projekt dodany poprawnie";
            h4TytulDodajProjekt.Attributes["class"] = "widgettitle title-success";

            Projekty1.ZaladujDane(idGrupy);
        }
        else
        {
            h4TytulDodajProjekt.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajProjekt.Attributes["class"] = "widgettitle title-danger";
        }

            
    }
}