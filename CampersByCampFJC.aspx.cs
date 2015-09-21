using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Model;

public partial class CampersByCampFJC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
        return true;
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (!ValidateInputData())
            return;

        ReportParamCampersFJC param = new ReportParamCampersFJC() 
        { 
            CampYearID = Int32.Parse(ddlCampYear.SelectedValue), 
            CampYear = Int32.Parse(ddlCampYear.SelectedItem.Text), 
            ProgramTypeID = (ProgramType)Int32.Parse(ddlProgram.SelectedValue), 
            FedID = -1, /* flag value*/ 
            CamperOrg = CamperOrgType.Camp
        };

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

        Session.Add("ReportParamCampersFJC", param);

        Response.Redirect("ReportFJCCampers.aspx");
    }

    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
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
}
