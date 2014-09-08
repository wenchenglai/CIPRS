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
            using (var ctx = new CIPMSEntities1())
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

        using (var ctx = new CIPMSEntities1())
        {
            var list = from camp in ctx.tblCamps
                       where camp.CampYearID == campYearID - 1
                       select camp;

            foreach (var newCamp in list)
            {
                ctx.AddTotblCamps(new tblCamp
                {
                    CampYearID = campYearID,
                    ID = newCamp.ID + 1000,
                    Inactive = false,
                    Name = newCamp.Name,
                    IsManual = false,
                    State = newCamp.State,
                    JDataID = newCamp.JDataID,
                    IsWestCamp = newCamp.IsWestCamp,
                    IsAdamahCamp = newCamp.IsAdamahCamp,
                    IsURJCamp = newCamp.IsURJCamp
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
            ctx.uspCampsDelete(campYearID);
        }
        lblMsg.Text = "Data deleted successfully.";
    }
}