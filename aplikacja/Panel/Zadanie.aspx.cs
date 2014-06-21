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
            int idZadania = 0;
            int.TryParse(Request.QueryString["id"], out idZadania);
            Zadanie z = Zadanie.Pobierz(idZadania);
            if (z!=null)
            {
                litTytulZadania.Text = z.ZadanieNazwa;
            }
        }
    }
}