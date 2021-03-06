﻿using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Panel : Scrum4uMasterPage
{
    public int iloscGrupRoboczych = 0;
    public int iloscWszystkichProjektow = 0;
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
            if (rodzajStrony==Scrum4uHelper.RodzajStrony.Projekty)
            {
                wynik = @"<span class='separator'></span>
                        </li>";

                string projekt = "";
                if (!String.IsNullOrEmpty(Request.QueryString["id_gr"]))
                    projekt=@"<li>
                            <a href='/Panel/GrupaRobocza.aspx?id="+Request.QueryString["id_gr"]+@"'>Grupa robocza</a>
                            <span class='separator'></span>
                        </li>";

                wynik += @"<li>
                            <a href='/Panel/GrupyRobocze.aspx'>Grupy robocze</a>
                            <span class='separator'></span>
                        </li>
                        "+projekt+"<li>Projekty</li>";
            }
        }
        if (typStrony == Scrum4uHelper.TypStrony.element)
        {
            if (rodzajStrony == Scrum4uHelper.RodzajStrony.GrupaRobocza)
            {
                wynik = @"<span class='separator'></span>
                        </li>";

                wynik += @"<li>
                            <a href='/Panel/GrupyRobocze.aspx'>Grupy robocze</a>
                            <span class='separator'></span>
                        </li>
                        <li>Grupa robocza</li>";
            }
            if (rodzajStrony == Scrum4uHelper.RodzajStrony.Projekt)
            {
                wynik = @"<span class='separator'></span>
                        </li>";

                Projekt p = Projekt.Pobierz(int.Parse(Request.QueryString["id"]), false);

                wynik += @"<li>
                            <a href='/Panel/GrupyRobocze.aspx'>Grupy robocze</a>
                            <span class='separator'></span>
                        </li>
                        <li>
                            <a href='/Panel/GrupaRobocza.aspx?id=" + p.ProjektGrupaRoboczaID + @"'>Grupa robocza</a>
                            <span class='separator'></span>
                        </li>
                        <li>Projekt</li>";

            }
            if (rodzajStrony == Scrum4uHelper.RodzajStrony.Zadanie)
            {
                wynik = @"<span class='separator'></span>
                        </li>";

                Zadanie z = Zadanie.Pobierz(int.Parse(Request.QueryString["id"]));
                Projekt p = Projekt.Pobierz(z.ZadanieProjektID, false);

                wynik += @"<li>
                            <a href='/Panel/GrupyRobocze.aspx'>Grupy robocze</a>
                            <span class='separator'></span>
                        </li>
                        <li>
                            <a href='/Panel/GrupaRobocza.aspx?id=" + p.ProjektGrupaRoboczaID + @"'>Grupa robocza</a>
                            <span class='separator'></span>
                        </li>
<li>
                            <a href='/Panel/Projekt.aspx?id=" + z.ZadanieProjektID + @"'>Projekt</a>
                            <span class='separator'></span>
                        </li>
                        <li>Zadanie</li>";
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
        if (listaGrupRoboczych != null && listaGrupRoboczych.Count > 0)
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
        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater projekty = (Repeater)item.FindControl("repNawigacja1");
            List<Projekt> listaProjektow = Projekt.PobierzWszystkie(HttpContext.Current.User.Identity.Name, true, ((GrupaRobocza)item.DataItem).GrupaRoboczaID);

            if (listaProjektow != null)
                iloscWszystkichProjektow += listaProjektow.Count;
            
            Literal nowyProjekt = (Literal)item.FindControl("nowyProjekt");
            string wzor = "<li><a href=\"/Panel/Projekty.aspx?dodaj=1&amp;id_gr=" + ((GrupaRobocza)item.DataItem).GrupaRoboczaID + "\">Dodaj projekt</a></li>";
            
            projekty.DataSource = listaProjektow;
            projekty.DataBind();

            if  ((((GrupaRobocza)item.DataItem).GrupaRoboczaUzytkownikID.Trim().ToLower()==HttpContext.Current.User.Identity.Name.Trim().ToLower()) && (listaProjektow == null || listaProjektow.Count == 0))
            {
                nowyProjekt.Text = "<ul>" + wzor + "</ul>";
            }
        }
    }
}
