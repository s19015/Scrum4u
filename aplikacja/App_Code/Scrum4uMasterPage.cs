using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Scrum4uMasterPage
/// </summary>
public class Scrum4uMasterPage : System.Web.UI.MasterPage
{
	public Scrum4uMasterPage()
	{
        typStrony = Scrum4u.Scrum4uHelper.TypStrony.domyslna;
	}

    public Scrum4u.Scrum4uHelper.TypStrony typStrony { get; set; }
    public Scrum4u.Scrum4uHelper.RodzajStrony rodzajStrony { get; set; }
}