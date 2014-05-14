using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebParts_GrupyRobocze : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int idGrupy=0;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out idGrupy))
            {
                List<GrupyRoboczeZaproszenie> grupy = GrupyRoboczeZaproszenie.PobierzWszystkie(idGrupy);
                if (grupy != null && grupy.Count() > 0)
                {
                    grupyRobocze.Visible = true;
                    grupyRobocze.DataSource = grupy;
                    grupyRobocze.DataBind();
                }
            }
        }
    }
}