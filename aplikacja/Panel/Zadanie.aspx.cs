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

        if (Request.QueryString["id"] != null)
        {
            if (!IsPostBack)
            {
                foreach (Scrum4u.Status t in Enum.GetValues(typeof(Scrum4u.Status)))
                {
                    ListItem item = new ListItem(Scrum4uHelper.PobierzStatus(t), t.ToString());
                    ddStatus.Items.Add(item);
                }



                int idZadania = 0;
                int.TryParse(Request.QueryString["id"], out idZadania);
                Zadanie z = Zadanie.Pobierz(idZadania);
                if (z != null)
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

                    lblTypZadania.Text = Enum.GetName(typeof(TypZadania), z.ZadanieTypZadania);
                    txtOpisZadania.Text = z.ZadanieOpis;
                    ddPriorytet.SelectedValue = z.ZadaniePriorytet.ToString();
                    txtDataZakonczenia.Text = z.ZadanieDeadline.ToShortDateString();

                    ddPrzypisaneDO.SelectedValue = z.ZadaniePrzypisaneDo;
                    ddStatus.SelectedValue = z.ZadanieStatus.ToString();

                    lblPrzypisujacy.Text = z.ZadanieDodajacy;
                    lblDataUtworzenia.Text = z.ZadanieDataUtworzenia.ToShortDateString() + " " + z.ZadanieDataUtworzenia.ToShortTimeString();

                    UstawReadOnly(true);
                }
            }
        }
    }

    private void UstawReadOnly(bool readOnly)
    {
        txtNazwaZadania.ReadOnly = readOnly;

        txtOpisZadania.ReadOnly = readOnly;
        ddPriorytet.Enabled = !readOnly;
        txtDataZakonczenia.ReadOnly = readOnly;

        ddPrzypisaneDO.Enabled = !readOnly;
        ddStatus.Enabled = !readOnly;


    }
    protected void btnEdytuj_Click(object sender, EventArgs e)
    {
        UstawReadOnly(false);

        btnEdytuj.Visible = false;
        btnZapisz.Visible = true;
    }
    protected void btnZapisz_Click(object sender, EventArgs e)
    {
        int idZadania = 0;
        int.TryParse(Request.QueryString["id"], out idZadania);

        if (idZadania <= 0) return;
        bool dodano = false;
        Zadanie z = Zadanie.Pobierz(idZadania);
        if (z != null)
        {
            z.ZadanieNazwa = txtNazwaZadania.Text;
            z.ZadanieOpis = txtOpisZadania.Text;
            z.ZadaniePriorytet = int.Parse(ddPriorytet.SelectedValue);
            DateTime dZak = DateTime.MinValue;
            DateTime.TryParse(txtDataZakonczenia.Text, out dZak);
            z.ZadanieDeadline = dZak;
            z.ZadaniePrzypisaneDo = ddPrzypisaneDO.SelectedValue;
            Status s = Status.DOWYKONANIA;
            Enum.TryParse<Status>(ddStatus.SelectedValue, out s);
            z.ZadanieStatus = s;


            dodano = z.Aktualizuj();

        }

        if (dodano)
        {
            Response.Redirect("/Panel/Zadanie.aspx?id=" + Request.QueryString["id"]);
        }
        else
        {
            lblInfo.ForeColor = System.Drawing.Color.Red;
            lblInfo.Text = "Wystąpił błąd. Spróbuj ponownie póżniej.";
        }
    }
}