using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebParts_Projekty : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<Projekt> projekty = Projekt.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true);
            if (projekty!=null && projekty.Count>0)
            {
                projektyListView.Visible = true;
                projektyListView.DataSource = projekty;
                projektyListView.DataBind();
            }
        }
    }
}