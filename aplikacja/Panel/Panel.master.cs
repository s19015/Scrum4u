using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Panel : Scrum4uMasterPage
{
    public int iloscGrupRoboczych = 0;
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

            ObsluzOkruszki();
    }

    private void ObsluzOkruszki()
    {
        string wynik = "";
        if (typStrony == Scrum4uHelper.TypStrony.lista)
        {
            wynik = @"<span class='separator'></span>
                        </li>";
            wynik += "<li>" + this.Page.Title.Substring(0, this.Page.Title.IndexOf('-') - 1) + "</li>";
        }
        if (typStrony==Scrum4uHelper.TypStrony.element)
        {
            if (rodzajStrony== Scrum4uHelper.RodzajStrony.GrupaRobocza)
            {
                wynik = @"<span class='separator'></span>
                        </li>";

                wynik+=@"<li>
                            <a href='/Panel/GrupyRobocze.aspx'>Grupy robocze</a>
                            <span class='separator'></span>
                        </li>
                        <li>Grupa robocza</li>";
            }
        }

        if (String.IsNullOrEmpty(wynik))
        {
            wynik = "</li>";
        }

        litOkruszki.Text = wynik;
    }

    private void ZaladujMenu()
    {
        List<GrupaRobocza> listaGrupRoboczych = GrupaRobocza.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true);
        if (listaGrupRoboczych!=null && listaGrupRoboczych.Count>0)
        {
            iloscGrupRoboczych = listaGrupRoboczych.Count;
            repNawigacja.Visible = true;
            litRepNawigacjaPusta.Visible = false;
            repNawigacja.DataSource = listaGrupRoboczych;
            repNawigacja.DataBind();
        }
        else
        {
            repNawigacja.Visible = false;
            litRepNawigacjaPusta.Visible = true;
            litRepNawigacjaPusta.Text = @"<ul>
                                            <li>
                                                <a href='/Panel/GrupyRobocze.aspx?dodaj=1'>Dodaj grupę roboczą</a>
                                            </li>
                                        </ul>";
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
