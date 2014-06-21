using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Panel_Zadanie : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Scrum4uMasterPage master = this.Page.Master as Scrum4uMasterPage;
        master.typStrony = Scrum4u.Scrum4uHelper.TypStrony.element;
        master.rodzajStrony = Scrum4u.Scrum4uHelper.RodzajStrony.Zadanie;

        if (Request.QueryString["id"]!=null)
        {
            foreach (Scrum4u.TypZadania t in Enum.GetValues(typeof(Scrum4u.TypZadania)))
            {
                ListItem item = new ListItem(Enum.GetName(typeof(Scrum4u.TypZadania), t), t.ToString());
                ddTypZadania.Items.Add(item);
            }


            int idZadania = 0;
            int.TryParse(Request.QueryString["id"], out idZadania);
            Zadanie z = Zadanie.Pobierz(idZadania);
            if (z!=null)
            {
                Projekt p = Projekt.Pobierz(z.ZadanieProjektID, false);
                if (p != null)
                {
                    List<Uzytkownik> uzytkownicy = GrupaRobocza.PobierzWszystkichUzytkownikow(p.ProjektGrupaRoboczaID);
                    if (uzytkownicy != null)
                    {
                        ddPrzypisaneDO.DataSource = uzytkownicy;
                        ddPrzypisaneDO.DataTextField = "UzytkownikEmail";
                        ddPrzypisaneDO.DataValueField = "UzytkownikEmail";
                        ddPrzypisaneDO.DataBind();
                    }
                }

                litTytulZadania.Text = z.ZadanieNazwa;

                formDodajPokazZadanie.Visible = true;
                //pola we formularzu
                txtNazwaZadania.Text = z.ZadanieNazwa;
                ddTypZadania.SelectedValue = Enum.GetName(typeof(TypZadania), z.ZadanieTypZadania);
                txtOpisZadania.Text = z.ZadanieOpis;
                ddPriorytet.SelectedValue = z.ZadaniePriorytet.ToString();
                txtDataZakonczenia.Text = z.ZadanieDeadline.ToShortDateString();
                txtPrzypisaneDO.Text = z.ZadaniePrzypisaneDo;


            }
        }
    }
}