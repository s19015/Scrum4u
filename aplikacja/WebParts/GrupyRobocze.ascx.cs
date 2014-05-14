﻿using Scrum4u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebParts_GrupyRobocze : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<GrupaRobocza> grupy = GrupaRobocza.PobierzWszystkie(HttpContext.Current.User.Identity.Name);
            if (grupy!=null && grupy.Count()>0)
            {
                grupyRobocze.Visible = false;
                grupyRobocze.DataSource = grupy;
                grupyRobocze.DataBind();
            }
        }
    }
}