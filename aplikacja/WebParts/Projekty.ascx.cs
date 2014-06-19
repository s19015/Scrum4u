using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebParts_Projekty : System.Web.UI.UserControl
{
    public bool Aktywny { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int idg = -1;
            if (Request.QueryString["id_gr"]!=null)
            {
                int.TryParse(Request.QueryString["id_gr"], out idg);
            }
            ZaladujDane(idg);
        }
    }
    public void ZaladujDane(int idGrupy)
    {
        List<Projekt> projekty = null;
        if (idGrupy>0)
            projekty = Projekt.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true,idGrupy);
        else
            projekty = Projekt.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true); 

        if (projekty != null && projekty.Count > 0)
        {
            if (Aktywny) projekty = projekty.Where(p => p.ProjektAktywny == true).ToList();
            projektyListView.Visible = true;
            projektyListView.DataSource = projekty;
            projektyListView.DataBind();
        }
    }
}