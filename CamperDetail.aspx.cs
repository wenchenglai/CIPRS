﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class CamperDetail : System.Web.UI.Page
{
    Role UserRole;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserRole = (Role)Convert.ToInt32(Session["RoleID"]);
        lblMsg.Text = "";
        MakeKeyStatusBold();

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

    protected void chkAllCamps_CheckedChanged(object sender, EventArgs e)
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

    private bool ValidateInputData()
    {
        bool e_flag = true;
        foreach (ListItem li in chklistFed.Items)
            if (li.Selected)
            {
                e_flag = false;
                break;
            }

        if (e_flag && UserRole != Role.CampDirector)
        {
            lblMsg.Text = "You must select at least one federation/camp";
            return false;
        }

        e_flag = true;
        foreach (ListItem li in chklistCamp.Items)
        {
            if (li.Selected)
                e_flag = false;
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one camp";
            return false;
        }

        e_flag = true;
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
                e_flag = false;
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one status";
            return false;
        }

        if (!chk1stTimers.Checked)
            if (!chk2ndTimers.Checked)
                if (!chk3rdTimers.Checked)
                {
                    lblMsg.Text = "You must at least select one option from # of years in program ";
                    return false;
                }

        return true;
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (!ValidateInputData())
            return;

        var param = new ReportParamCampersFJC()
        {
            CampYearID = Int32.Parse(ddlCampYear.SelectedValue),
            CampYear = Int32.Parse(ddlCampYear.SelectedItem.Text),
            FedID = -1, /* flag value*/
            CamperOrg = CamperOrgType.CamperDetailReport
        };

        foreach (ListItem li in chklistFed.Items)
        {
            if (li.Selected)
            {
                param.Fed_Dict[Int32.Parse(li.Value)] = li.Text;
            }
        }

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

        int timesReceivedGrant = 0;

        if (chk1stTimers.Checked)
            timesReceivedGrant += 2;

        if (chk2ndTimers.Checked)
            timesReceivedGrant += 4;

        if (chk3rdTimers.Checked)
            timesReceivedGrant += 8;

        param.TimesReceivedGrant = timesReceivedGrant;

        Session.Add("ReportParamCampersFJC", param);

        Response.Redirect("ReportFJCCampers.aspx");
    }

    protected void chklistCamp_DataBound(object sender, EventArgs e)
    {
        if (chklistCamp.Items.Count < 1)
        {
            btnReport.Enabled = false;
            chkAllCamps.Enabled = false;
            lblMsg.Text = "Currently you are not associated with any camps, so you cannot see any data from this report";
        }
        else
        {
            btnReport.Enabled = true;
            chkAllCamps.Enabled = true;
        }
    }

    protected void chklistStatus_DataBound(object sender, EventArgs e)
    {
        MakeKeyStatusBold();
    }

    private void MakeKeyStatusBold()
    {
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Value == "1" || li.Value == "7" || li.Value == "14" || li.Value == "25" || li.Value == "28") 
                li.Attributes.CssStyle.Add("font-weight", "bold");
        }
    }

    protected void chkAllFeds_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllFeds.Checked)
            foreach (ListItem li in chklistFed.Items)
            {
                li.Selected = true;
            }
        else
            foreach (ListItem li in chklistFed.Items)
            {
                li.Selected = false;
            }

        chklistCamp.DataBind();
        chkAllCamps.Checked = false;
    }

    protected void chklistFed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        chklistCamp.DataBind();
    }

    protected void odsCamp_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["FedList"] = chklistFed.Items;
    }

    protected void chklistCamp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (chklistCamp.Items.Count == 0)
        {
            btnReport.Enabled = false;
        }
        else
        {
            btnReport.Enabled = true;
        }
    }
}