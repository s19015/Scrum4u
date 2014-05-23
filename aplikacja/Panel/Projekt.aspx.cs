using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Projekt : System.Web.UI.Page
{
    public Projekt projekt;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            int idProjektu = 0;
            if (Request.QueryString["id"]!=null && int.TryParse(Request.QueryString["id"],out idProjektu))
            {
                Projekt p = Projekt.Pobierz(idProjektu, true);
                if (p!=null)
                {
                    litProjektNazwa.Text = p.ProjektNazwa;
                }
            }
        }
    }
}