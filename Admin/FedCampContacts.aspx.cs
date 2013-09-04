using System;
using System.Collections.Generic;
using System.Linq;
using Model;

public partial class Admin_FedCampContacts : System.Web.UI.Page
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
            var list = from row in ctx.tblFederationCampsContactDetails.Include("tblCamp")
                       where row.CampYearID == campYearID - 1
                       orderby row.FederationID, row.tblCamp.ID
                       select row;

            foreach (var row in list)
            {
                // create new detail rows.  EF will insert entities automatically because referecing to the existing camps
                var p = new tblFederationCampsContactDetail
                {
                    CampYearID = campYearID,
                    tblCamp = ctx.tblCamps.Where(x => x.ID == row.tblCamp.ID + 1000).First(),
                    FederationID = row.FederationID,
                    Contact = row.Contact,
                    Phone = row.Phone,
                    Email = row.Email,
                    NavigationURL = row.NavigationURL,
                    ParentInfoPreviousClickURL = row.ParentInfoPreviousClickURL
                };
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
            ctx.uspFedCampsContactsDelete(campYearID);
        }
        lblMsg.Text = "Data deleted successfully.";
    }
}