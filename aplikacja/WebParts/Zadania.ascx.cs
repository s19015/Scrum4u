using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebParts_Zadania : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void ZaladujDane(int idSprintu, int idProjektu)
    {
        if (idSprintu > 0)
        {
            if (idProjektu <= 0)
            {
                Sprint s = Sprint.Pobierz(idSprintu);
                if (s != null)
                    idProjektu = s.SprintProjektID;
            }

            List<Zadanie> listaZadan = Zadanie.PobierzWszystkie(idProjektu, idSprintu);
            if (listaZadan != null && listaZadan.Count > 0)
            {
                ZadaniaListView.Visible = true;
                ZadaniaListView.DataSource = listaZadan;
                ZadaniaListView.DataBind();
            }

        }
    }
    public string PobierzStatus (object Status)
    {
        string wynik = "";
        if (Status!=null)
        {
            string sStatus = Status.ToString();

            Scrum4u.Status s = (Scrum4u.Status)Enum.Parse(typeof(Scrum4u.Status), sStatus, true);

            switch(s)
            {
                case Scrum4u.Status.DOWYKONANIA:
                    wynik = "Do wykonania";
                    break;
                case Scrum4u.Status.WTRAKCIE:
                    wynik = "W trakcie";
                    break;
                case Scrum4u.Status.ODLOZONE:
                    wynik = "Odłożone";
                    break;
                case Scrum4u.Status.WYKONANE:
                    wynik = "Wykonane";
                    break;
            }
        }

        return wynik;
    }
}