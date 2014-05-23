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
        if (!IsPostBack)
        {
            //laduje grupy robocze
            ddGrupaRobocza.DataSource=Scrum4u.GrupaRobocza.PobierzWszystkie(HttpContext.Current.User.Identity.Name, false);
            ddGrupaRobocza.DataValueField = "GrupaRoboczaID";
            ddGrupaRobocza.DataTextField = "GrupaRoboczaNazwa";
            ddGrupaRobocza.DataBind();
        }
    }
    protected void btnPokazDodajProjekt_Click(object sender, EventArgs e)
    {
        formDodaJprojekt.Visible = true;
        btnPokazDodajProjekt.Visible = false;
    }
    protected void btnZapiszNowyProjekt_ServerClick(object sender, EventArgs e)
    {
        int idGrupy =0;
        int.TryParse(ddGrupaRobocza.SelectedValue,out idGrupy);
        Scrum4u.Projekt p = new Scrum4u.Projekt() { ProjektDataUtworzenia=DateTime.Now, ProjektGrupaRoboczaID= idGrupy, ProjektScrumMasterID=HttpContext.Current.User.Identity.Name,ProjektManagerProjektuID=HttpContext.Current.User.Identity.Name,  ProjektNazwa=txtNazwaProjektu.Text, ProjektAktywny=true};
        
        bool dodano = false;
        if (p!=null)
            dodano = p.Dodaj();

        if (dodano)
        {
            panelDodajProjekt.Visible = false;
            h4TytulDodajProjekt.InnerText = "Projekt dodany poprawnie";
            h4TytulDodajProjekt.Attributes["class"] = "widgettitle title-success";
        }
        else
        {
            h4TytulDodajProjekt.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajProjekt.Attributes["class"] = "widgettitle title-danger";
        }

            
    }
}