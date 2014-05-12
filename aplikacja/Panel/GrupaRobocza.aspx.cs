using Scrum4u;
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
        if (Request.QueryString["id"]!=null && int.TryParse(Request.QueryString["id"],out idGrupy))
        {
            grupa = GrupaRobocza.PobierzGrupe(idGrupy);
            if (grupa!=null)
            {
                litTytul.Text = "- " + grupa.GrupaRoboczaNazwa;
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

    }
}