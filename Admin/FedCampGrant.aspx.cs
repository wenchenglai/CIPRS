using System;
using System.Collections.Generic;
using System.Linq;
using Model;

public partial class Admin_FedCampGrant : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";

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

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        int campYearID = Int32.Parse(ddlCampYear.SelectedValue);

        using (CIPMSEntities1 ctx = new CIPMSEntities1())
        {
            var list = from grantrow in ctx.tblFedCampGrants
                       where grantrow.CampYearID == campYearID - 1
                       orderby grantrow.FederationID, grantrow.CampID
                       select grantrow;

            foreach (var row in list)
            {
                // create new FedCampGrant
                ctx.AddTotblFedCampGrants(new tblFedCampGrant
                {
                    CampYearID = campYearID,
                    CampID = row.CampID == 0 ? 0 : row.CampID + 1000,
                    FederationID = row.FederationID,
                    TimeInCamp = row.TimeInCamp,
                    DaysAtLeast = row.DaysAtLeast,
                    GrantAmount = row.GrantAmount
                });
            }

            ctx.SaveChanges();
        }
        lblMsg.Text = "Data generated successfully.";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int campYearID = Int32.Parse(ddlCampYear.SelectedValue);
        using (CIPMSEntities1 ctx = new CIPMSEntities1())
        {
            ctx.uspFedCampGrantDelete(campYearID);
        }
        lblMsg.Text = "Data deleted successfully.";
    }
}