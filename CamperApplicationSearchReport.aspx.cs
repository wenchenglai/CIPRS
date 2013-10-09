using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model;

public partial class CamperApplicationSearchReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (CIPMSEntities1 ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.SelectedValue = Application["CampYearID"].ToString();
                ddlCampYear.DataBind();
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gv.DataSourceID = "ods";
        gv.DataBind();
        //gv.DataSource = CamperApplicationDA.GetCamperApplicationsWithSearchCriteria(Int32.Parse(ddlCampYear.SelectedValue), (SearchKeys)Int32.Parse(ddlSearchKey.SelectedValue), txtSearchText.Text);
        //gv.DataBind();
    }
}
