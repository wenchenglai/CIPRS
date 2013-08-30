using System;
using System.Collections.Generic;
using System.Linq;
using Model;

public partial class Admin_UserCamps : System.Web.UI.Page
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
            var list = from row in ctx.tblUserCamps
                       where row.CampYearid == campYearID - 1
                       orderby row.UserId, row.CampId
                       select row;

            foreach (var row in list)
            {
                // create new detail rows.  EF will insert entities automatically because referecing to the existing camps
                ctx.AddTotblUserCamps(new tblUserCamp
                {
                    CampYearid = campYearID,
                    UserId = row.UserId,
                    CampId = row.CampId + 1000
                });
            }
            ctx.SaveChanges();
        }
        lblMsg.Text = "Data generated successfully.";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //int campYearID = Int32.Parse(ddlCampYear.SelectedValue);
        //using (CIPMSEntities1 ctx = new CIPMSEntities1())
        //{
        //    ctx.uspFedCampsContactsDelete(campYearID);
        //}
        //lblMsg.Text = "Data deleted successfully.";
    }
}