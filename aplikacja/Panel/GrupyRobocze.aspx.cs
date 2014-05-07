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

    }
    protected void btnPokazDodajGrupe_Click(object sender, EventArgs e)
    {
        formDodajGrupe.Visible = true;
        btnPokazDodajGrupe.Visible = false;
    }
    protected void btnZapiszNowaGrupe_ServerClick(object sender, EventArgs e)
    {
        GrupaRobocza gr = new GrupaRobocza() { GrupaRoboczaUzytkownikID = Scrum4uHelper.ObecnyUzytkownik.UzytkownikEmail, GrupaRoboczaAktywna = false, GrupaRoboczaNazwa = txtNazwaGrupy.Text, GrupaRoboczaData = DateTime.Now };

        if (gr!=null)
        {
            gr.Dodaj();
        }
    }
}