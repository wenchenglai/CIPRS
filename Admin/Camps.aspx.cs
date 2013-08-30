using System;
using System.Collections.Generic;
using System.Linq;
using Model;

public partial class Admin_Camps : System.Web.UI.Page
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
            var list = from camp in ctx.tblCamps
                       where camp.CampYearID == campYearID - 1
                       select camp;

            long idMax = ctx.tblCamps.Select(x => x).Max(x => x.ID);

            foreach (var newCamp in ctx.tblMetaCamps)
            {
                var oldCamp = (from camp in list
                               where camp.Name == newCamp.CampName
                               select camp).FirstOrDefault();

                if (oldCamp == null)
                {
                    // this means we have new camp for this camp year
                    idMax += 1;

                    ctx.AddTotblCamps(new tblCamp
                    {
                        CampYearID = campYearID,
                        ID = idMax.ToString()[0] == (campYearID - 1).ToString()[0] ? idMax : idMax + 1000,
                        Inactive = false,
                        Name = newCamp.CampName,
                        IsManual = false
                    });
                }
                else
                {
                    // we have exiting camp that needs to be migrated to this year
                    ctx.AddTotblCamps(new tblCamp
                    {
                        CampYearID = campYearID,
                        ID = oldCamp.ID + 1000,
                        Inactive = false,
                        Name = newCamp.NewCampName ?? newCamp.CampName,
                        IsManual = false,
                        State = oldCamp.State
                    });
                }
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
            ctx.uspCampsDelete(campYearID);
        }
        lblMsg.Text = "Data deleted successfully.";
    }
}