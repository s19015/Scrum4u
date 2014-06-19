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
        Scrum4uMasterPage master = this.Page.Master as Scrum4uMasterPage;
        master.typStrony = Scrum4uHelper.TypStrony.lista;
        
        h4Usunieto.Visible = false;
        litInfo.Text = "";
        if (!IsPostBack)
        {
            if (Request.QueryString["dodaj"] != null && Request.QueryString["dodaj"] == "1")
            {
                btnPokazDodajGrupe_Click(sender, e);
            }
            if (Request.QueryString["usun"] != null && Request.QueryString["usun"] == "1" && Request.QueryString["id"] != null)
            {
                int idGrupy = 0;
                if (int.TryParse(Request.QueryString["id"], out idGrupy))
                {
                    bool usunieto = false;
                    GrupaRobocza gr = GrupaRobocza.PobierzGrupe(idGrupy, false);
                    if (gr != null)
                    {
                        usunieto = gr.Usun();
                    }
                    h4Usunieto.Visible = true;
                    if (usunieto)
                    {
                        h4Usunieto.InnerText = "Grupa robocza usunięta poprawnie";
                        h4Usunieto.Attributes["class"] = "widgettitle title-success";
                    }
                    else
                    {
                        h4Usunieto.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
                        h4Usunieto.Attributes["class"] = "widgettitle title-danger";
                    }
                    litInfo.Text = @"<script>
var newurl = window.location.protocol + '//' + window.location.host + window.location.pathname

window.history.pushState({path:newurl},'',newurl);
</script>";
                }
            }
        }

    }
 
    protected void btnPokazDodajGrupe_Click(object sender, EventArgs e)
    {
        h4Usunieto.Visible = false;
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
        }

        if (dodano)
        {
            panelDodajGrupe.Visible = false;
            h4TytulDodajGrupe.InnerText = "Grupa robocza dodana poprawnie";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-success";
            GrupyRobocze1.ZaladujDane();
        }
        else
        {
            h4TytulDodajGrupe.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajGrupe.Attributes["class"] = "widgettitle title-danger";
        }
    }
}