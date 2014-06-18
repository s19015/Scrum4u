using Scrum4u;
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

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                return;
            }
            litImieNazwisko.Text = HttpContext.Current.User.Identity.Name;

            if (!IsPostBack)
                ZaladujMenu();
        
    }

    private void ZaladujMenu()
    {
        List<GrupaRobocza> listaGrupRoboczych = GrupaRobocza.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true);
        if (listaGrupRoboczych!=null && listaGrupRoboczych.Count>0)
        {
            repNawigacja.DataSource = listaGrupRoboczych;
            repNawigacja.DataBind();
        }
    }
    protected void btnWyloguj_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Session.Abandon();
        System.Web.Security.FormsAuthentication.SignOut();

        Response.Redirect("/Zaloguj.aspx");
    }
    protected void repNawigacja_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem item = e.Item;
        if (item.ItemType==ListItemType.Item || item.ItemType==ListItemType.AlternatingItem)
        {
            Repeater projekty = (Repeater)item.FindControl("repNawigacja1");
            List<Projekt> listaProjektow = Projekt.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true, ((GrupaRobocza)item.DataItem).GrupaRoboczaID);
            projekty.DataSource = listaProjektow;
            projekty.DataBind();
        }
    }
}
