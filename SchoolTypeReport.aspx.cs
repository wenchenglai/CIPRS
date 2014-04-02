using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class SchoolTypeReport : System.Web.UI.Page
{
    Role UserRole;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserRole = (Role)Convert.ToInt32(Session["RoleID"]);
            using (CIPMSEntities1 ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.SelectedValue = Application["CampYearID"].ToString();
                ddlCampYear.DataBind();
            }

            if (UserRole == Role.CampDirector)
            {
                ddlFed.Visible = false;
                ddlFed.DataSourceID = null;
            }
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        divReport.Visible = true;

        gv.DataSourceID = "ods";
        gv.DataBind();
    }
}