﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Model;

public partial class CamperSummaryByProgram : System.Web.UI.Page
{
    Role UserRole;

    protected void Page_Load(object sender, EventArgs e)
    {
        UserRole = (Role)Convert.ToInt32(Session["RoleID"]);

        lblMsg.Text = "";
        chkAllCamps.Enabled = true;

        MakeKeyStatusBold();

        // We have to determine if its Fed Admin
        if (!IsPostBack)
        {
            if (UserRole == Role.CampDirector)
            {
                ddlFed.Visible = false;
                ddlFed.DataSourceID = null;

                chklistCamp.DataSourceID = null;
                chklistCamp.DataSource = CampsDA.GetCampByJWestCampDirector(Int32.Parse(ddlCampYear.SelectedValue), Int32.Parse(Session["UserID"].ToString()));
                chklistCamp.DataBind();

                chkAllCamps.Visible = false;
            }

            using (CIPMSEntities1 ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.SelectedValue = Application["CampYearID"].ToString();
                ddlCampYear.DataBind();
            }
        }
    }

    private void MakeKeyStatusBold()
    {
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Value == "1" || li.Value == "7" || li.Value == "14" || li.Value == "25" || li.Value == "28") 
                li.Attributes.CssStyle.Add("font-weight", "bold");
        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCamps.Checked)
            foreach (ListItem li in chklistCamp.Items)
            {
                li.Selected = true;
            }
        else
            foreach (ListItem li in chklistCamp.Items)
            {
                li.Selected = false;
            }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        bool e_flag = true;
        foreach (ListItem li in chklistCamp.Items)
        {
            if (li.Selected)
            {
                e_flag = false;
                break;
            }
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one camp";
            return;
        }

        e_flag = true;
        int status_count = 0;
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
            {
                e_flag = false;
                status_count++;
            }
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one status";
            return;
        }

        if (status_count != 1)
        {
            lblMsg.Text = "You can only select one status at a time";
            return;
        }

        ReportParamCampersFJC param = new ReportParamCampersFJC();

        param.CampYearID = Int32.Parse(ddlCampYear.SelectedValue);
        param.ProgramTypeID = ProgramType.NoUse;
        param.CampYear = Int32.Parse(ddlCampYear.SelectedItem.Text);

        if (UserRole == Role.CampDirector)
        {
            param.FedID = -1;  // for camp director, Fed are JWest Federations only
        }
        else
            param.FedID = Int32.Parse(ddlFed.SelectedValue);

        foreach (ListItem li in chklistCamp.Items)
        {
            if (li.Selected)
            {
                param.Camp_Dict[Int32.Parse(li.Value)] = li.Text;
            }
        }

        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
                param.Status_Dict[Int32.Parse(li.Value)] = li.Text;
        }

        param.BuildStrings();

        divMenu.Visible = false;
        divReport.Visible = true;

        gv.DataSource = GenerateDataTable(param);
        gv.DataBind();
    }

    private DataTable GenerateDataTable(ReportParamCampersFJC param)
    {
        return CamperApplicationBL.GetCamperSummaryReport(param.CampYearID, param.FedID, param.CampID_List, param.StatusID_List);
    }

    protected void chklistCamp_DataBound(object sender, EventArgs e)
    {
        if (chklistCamp.Items.Count == 0)
        {
            lblMsg.Text = "The federation has no camp in the camper applications data";
            chkAllCamps.Enabled = false;
            btnReport.Enabled = false;
        }
        else
        {
            lblMsg.Text = "";
            chkAllCamps.Enabled = true;
            btnReport.Enabled = true;
        }
    }

    protected void ddlFed_DataBound(object sender, EventArgs e)
    {
        if (ddlFed.Items.Count == 0)
        {
            chkAllCamps.Visible = false;
            btnReport.Enabled = false;
            lblMsg.Text = "Currently you are not associated with any federations, so you won't see any data in this report";
        }
        else
        {
            chkAllCamps.Visible = true;
            btnReport.Enabled = true;

            // Federation admins will see only their own Federation
            if (UserRole == Role.FederationAdmin)
            {
                int FedID = Int32.Parse(Session["FedID"].ToString());
                ddlFed.SelectedValue = FedID.ToString();
                ddlFed.Enabled = false;
            }
        }
    }

    protected void ddlFed_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkAllCamps.Checked = false;

        foreach (ListItem li in chklistCamp.Items)
        {
            li.Selected = false;
        }

        foreach (ListItem li in chklistStatus.Items)
        {
            li.Selected = false;
        }
    }

    protected void chklistStatus_DataBound(object sender, EventArgs e)
    {
        MakeKeyStatusBold();
    }

    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        divMenu.Visible = true;
        divReport.Visible = false;
    }
}
