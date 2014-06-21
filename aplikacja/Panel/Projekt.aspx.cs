using Scrum4u;
using Scrum4u.Aplikacja;
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
        Scrum4uMasterPage master = this.Page.Master as Scrum4uMasterPage;
        master.typStrony = Scrum4uHelper.TypStrony.element;
        master.rodzajStrony = Scrum4uHelper.RodzajStrony.Projekt;
            int idProjektu = 0;
            if (Request.QueryString["id"]!=null && int.TryParse(Request.QueryString["id"],out idProjektu))
            {
                projekt = Projekt.Pobierz(idProjektu, true);
                if (projekt != null)
                {
                    litProjektNazwa.Text = projekt.ProjektNazwa;
                }
            }


            //if (!IsPostBack)
            //{
            //    foreach (Scrum4u.TypZadania t in Enum.GetValues(typeof(Scrum4u.TypZadania)))
            //    {
            //        ListItem item = new ListItem(Enum.GetName(typeof(Scrum4u.TypZadania), t), t.ToString());
            //        ddTypZadania.Items.Add(item);
            //    }


            //    List<Uzytkownik> uzytkownicy = GrupaRobocza.PobierzWszystkichUzytkownikow(projekt.ProjektGrupaRoboczaID);
            //    if (uzytkownicy != null)
            //    {
            //        ddPrzypisaneDO.DataSource = uzytkownicy;
            //        ddPrzypisaneDO.DataTextField = "UzytkownikEmail";
            //        ddPrzypisaneDO.DataValueField = "UzytkownikEmail";
            //        ddPrzypisaneDO.DataBind();
            //    }
            //}
    }
    //protected void btnDodajPokazZadanie_Click(object sender, EventArgs e)
    //{
    //    formDodajPokazZadanie.Visible = true;
    //    btnDodajPokazZadanie.Visible = false;
    //}
    //protected void btnZapiszZadanie_ServerClick(object sender, EventArgs e)
    //{
    //    Zadanie z = new Zadanie()
    //    {
    //        ZadanieNazwa = txtNazwaZadania.Text,
    //        ZadanieOpis = txtOpisZadania.Text,
    //        ZadaniePriorytet = int.Parse(ddPriorytet.SelectedValue),
    //        ZadanieProjektID = projekt.ProjektID,
    //        ZadanieTypZadania = (TypZadania)Enum.Parse(typeof(TypZadania), ddTypZadania.SelectedValue),
    //        ZadanieStatus = Status.do_wykonania,
    //        ZadanieDataUtworzenia = DateTime.Now,
    //        ZadaniePrzypisaneDo = ddPrzypisaneDO.SelectedValue

    //    };

    //    DateTime dZak = DateTime.MinValue;
    //    DateTime.TryParse(txtDataZakonczenia.Text, out dZak);
    //    z.ZadanieDataUkonczenia = dZak;
    //    z.ZadanieDeadline = dZak;

    //    bool dodano = false;
    //    try
    //    {
    //        dodano = z.Dodaj();
    //    }
    //    catch (Exception ex)
    //    {
    //        Zdarzenie.Loguj("ProjektZadanie", "Blad", ex);
    //    }

    //    if (dodano)
    //    {
    //        panelDodajZadanie.Visible = false;
    //        h4TytulDodajZadanie.InnerText = "Zadanie dodane poprawnie";
    //        h4TytulDodajZadanie.Attributes["class"] = "widgettitle title-success";
    //    }
    //    else
    //    {
    //        h4TytulDodajZadanie.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
    //        h4TytulDodajZadanie.Attributes["class"] = "widgettitle title-danger";
    //    }

    //}
    protected void btnDodajSprint_ServerClick(object sender, EventArgs e)
    {
        int idProjektu = 0;
        int.TryParse(Request.QueryString["id"],out idProjektu);
        DateTime deadline = DateTime.Now;
        DateTime.TryParse(txtDeadLine.Text, out deadline);

        Sprint s = new Sprint() { 
       SprintProjektID =idProjektu, 
       SprintNazwa = txtNazwaSprintu.Text,
       SprintOpis = txtOpisSprintu.Text,
       SprintTerminWykonania = deadline
        };

        bool dodano = false;
        try
        {
            dodano = s.Dodaj();
        }
        catch (Exception ex)
        {
            Zdarzenie.Loguj("ProjektSprint", "Blad", ex);
        }

        if (dodano)
        {
            panelDodajSprint.Visible = false;
            h4TytulDodajSprint.InnerText = "Sprint dodany poprawnie";
            h4TytulDodajSprint.Attributes["class"] = "widgettitle title-success";
        }
        else
        {
            h4TytulDodajSprint.InnerText = "Wystąpił błąd. Spróbuj ponownie później.";
            h4TytulDodajSprint.Attributes["class"] = "widgettitle title-danger";
        }
    }
    protected void btnPokazDodajSprint_Click(object sender, EventArgs e)
    {
        formDodajSprint.Visible = true;
        btnPokazDodajSprint.Visible = false;
    }
}