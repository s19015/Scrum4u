﻿using Scrum4u;
using Scrum4u.Aplikacja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebParts_Sprinty : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            int idg = -1;
            if (Request.QueryString["id"] != null)
            {
                int.TryParse(Request.QueryString["id"], out idg);
            }
            ZaladujDane(idg);
        }
    }
    public void ZaladujDane(int idProjektu)
    {
        if (idProjektu > 0)
        {
            List<Sprint> listaSprintow = Sprint.PobierzWszystkieDoProjektu(idProjektu);
            if (listaSprintow != null && listaSprintow.Count > 0)
            {
                listaSprintowListView.Visible = true;
                listaSprintowListView.DataSource = listaSprintow;
                listaSprintowListView.DataBind();
            }
        }
    }

    protected void listaSprintowListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int idSprintu = 0;
        if (e.CommandArgument == null) return;

        int.TryParse(e.CommandArgument.ToString(), out idSprintu);
        if (idSprintu > 0 && e.CommandName == "PokazDodajZadanie")
        {
            Sprint sp = Sprint.Pobierz(idSprintu);
            Panel panel = (Panel)e.Item.FindControl("formDodajPokazZadanie");
            panel.Visible = true;

            DropDownList ddTypZadania = (DropDownList)e.Item.FindControl("ddTypZadania");
            foreach (Scrum4u.TypZadania t in Enum.GetValues(typeof(Scrum4u.TypZadania)))
            {
                ListItem item = new ListItem(Enum.GetName(typeof(Scrum4u.TypZadania), t), t.ToString());
                ddTypZadania.Items.Add(item);
            }

            Projekt p = Projekt.Pobierz(sp.SprintProjektID, false);
            if (p != null)
            {


                DropDownList ddPrzypisaneDO = (DropDownList)e.Item.FindControl("ddPrzypisaneDO");
                List<Uzytkownik> uzytkownicy = GrupaRobocza.PobierzWszystkichUzytkownikow(p.ProjektGrupaRoboczaID);
                if (uzytkownicy != null)
                {
                    ddPrzypisaneDO.DataSource = uzytkownicy;
                    ddPrzypisaneDO.DataTextField = "UzytkownikEmail";
                    ddPrzypisaneDO.DataValueField = "UzytkownikEmail";
                    ddPrzypisaneDO.DataBind();
                }
            }
        }

        if (idSprintu>0 && e.CommandName=="DodajZadanie")
        {
            Sprint sp = Sprint.Pobierz(idSprintu);
            Zadanie z = new Zadanie()
        {
            ZadanieNazwa = ((TextBox)e.Item.FindControl("txtNazwaZadania")).Text,
            ZadanieOpis =((TextBox)e.Item.FindControl("txtOpisZadania")).Text,
            ZadaniePriorytet = int.Parse(((DropDownList)e.Item.FindControl("ddPriorytet")).SelectedValue),
            ZadanieProjektID = sp.SprintProjektID,
            ZadanieTypZadania = (TypZadania)Enum.Parse(typeof(TypZadania), ((DropDownList)e.Item.FindControl("ddTypZadania")).SelectedValue),
            ZadanieStatus = Status.DOWYKONANIA,
            ZadanieDataUtworzenia = DateTime.Now,
            ZadaniePrzypisaneDo = ((DropDownList)e.Item.FindControl("ddPrzypisaneDO")).SelectedValue,
            ZadanieSprintID = sp.SprintID
        };

            DateTime dZak = DateTime.MinValue;
            DateTime.TryParse(((TextBox)e.Item.FindControl("txtDataZakonczenia")).Text, out dZak);
            z.ZadanieDataUkonczenia = dZak;
            z.ZadanieDeadline = dZak;

            bool dodano = false;
            try
            {
                dodano = z.Dodaj();
            }
            catch (Exception ex)
            {
                Zdarzenie.Loguj("ProjektZadanie", "Blad", ex);
            }

            HtmlGenericControl h4 = (HtmlGenericControl)e.Item.FindControl("h4TytulDodajZadanie");

            if (dodano)
            {
                ((Panel)e.Item.FindControl("formDodajPokazZadanie")).Visible = false;
                h4.InnerText = "Zadanie dodane poprawnie";
                h4.Attributes["class"] = "widgettitle title-success";
                ZaladujDane(sp.SprintProjektID);
            }
            else
            {
                h4.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
                h4.Attributes["class"] = "widgettitle title-danger";
            }
        }
    }

    public string DodajOpis(object SprintOpis)
    {
        string wynik = "";
        if (SprintOpis!=null)
        {
            if (!String.IsNullOrEmpty(SprintOpis.ToString()))
            {
                wynik = @"<div class='DaneSprintu'>

                <div class='col-sm-2 ColumnName'>
                    Opis sprintu:
                </div>
                <div class='col-sm-10 ColumnValue'>
                    <p style='height: 100%;'>"+SprintOpis.ToString()+@"</p>
                </div>
            </div>";
            }
        }

        return wynik;
    }
    protected void listaSprintowListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Sprint s = (Sprint)e.Item.DataItem;

            WebParts_Zadania z = (WebParts_Zadania)e.Item.FindControl("Zadania1");
            z.ZaladujDane(s.SprintID, s.SprintProjektID);
        }
    }
}