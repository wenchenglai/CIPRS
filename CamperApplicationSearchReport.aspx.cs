using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CamperApplicationSearchReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        gv.DataSourceID = "ods";
        gv.DataBind();
        //gv.DataSource = CamperApplicationDA.GetCamperApplicationsWithSearchCriteria(Int32.Parse(ddlYear.SelectedValue), (SearchKeys)Int32.Parse(ddlSearchKey.SelectedValue), txtSearchText.Text);
        //gv.DataBind();
    }
}
