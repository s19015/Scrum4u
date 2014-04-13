using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Panel : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*if (!HttpContext.Current.User.Identity.IsAuthenticated)
        {
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }*/
    }
}
