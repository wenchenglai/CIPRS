using System;
using System.Collections.Generic;
using System.Linq;
using Model;

public partial class Admin_NationalCamps : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        if (!IsPostBack)
        {
            using (CIPMSEntities1 ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Where(x => x.CurrentYear == true).Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.DataValueField = "id";
                ddlCampYear.DataTextField = "text";
                ddlCampYear.DataBind();
            }
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        int campYearID = Int32.Parse(ddlCampYear.SelectedValue);

        using (CIPMSEntities1 ctx = new CIPMSEntities1())
        {
            var list = from row in ctx.tblNationalCamps
                       where row.CampYearID == 4
                       orderby row.FederationID, row.CampID
                       select row;

            foreach (var row in list)
            {
                // create new FedCampGrant
                ctx.AddTotblNationalCamps(new tblNationalCamp
                {
                    CampYearID = campYearID,
                    CampID = row.CampID + 1000,
                    FederationID = row.FederationID
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
            ctx.uspNationalCampsDelete(campYearID);
        }
        lblMsg.Text = "Data deleted successfully.";
    }
}