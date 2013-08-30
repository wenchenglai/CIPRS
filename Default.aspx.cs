using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((Role)(Convert.ToInt32(Session["RoleID"])) == Role.FJCAdmin)
            {
                divFJCAdminContent.Visible = true;
                divOtherContent.Visible = false;
            }
            else
            {
                divFJCAdminContent.Visible = false;
                divOtherContent.Visible = true;
            }
        }
    }
}
