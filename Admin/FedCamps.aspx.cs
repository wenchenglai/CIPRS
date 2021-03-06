﻿using System;
using System.Linq;
using Model;

public partial class Admin_FedCamps : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        if (!IsPostBack)
        {
            using (var ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Where(x => x.CurrentYear).Select(x => new { id = x.ID, text = x.CampYear });
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
            var list = from row in ctx.tblFederationCamps.Include("tblCamp")
                       where row.CampYearID == campYearID - 1
                       orderby row.FederationID, row.tblCamp.ID
                       select row;

            foreach (var row in list)
            {
                if (row.tblCamp != null)
                {
                    var camp = ctx.tblCamps.Where(x => x.ID == row.tblCamp.ID + 1000).FirstOrDefault();

                    if (camp != null)
                    {
                        var p = new tblFederationCamp
                        {
                            ID = row.ID + 1000,
                            CampYearID = campYearID,
                            tblCamp = camp,
                            FederationID = row.FederationID,
                            isJDS = row.isJDS,
                            Inactive = false
                        };
                    }
                }
            }
            ctx.SaveChanges();
        }
        lblMsg.Text = "Data generated successfully.";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int campYearId = Int32.Parse(ddlCampYear.SelectedValue);

        using (var ctx = new CIPMSEntities1())
        {
            ctx.usp_FedCampDelete(campYearId);
        }
        lblMsg.Text = "Data deleted successfully.";
    }
}