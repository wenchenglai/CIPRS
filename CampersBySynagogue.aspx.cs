using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model;

public partial class CampersBySynagogue : System.Web.UI.Page
{
    Role UserRole;

    protected void Page_Load(object sender, EventArgs e)
    {
        UserRole = (Role)Convert.ToInt32(Session["RoleID"]);

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

    protected void ddlFed_DataBound(object sender, EventArgs e)
    {
        if (ddlFed.Items.Count == 0)
        {
            chkAllSynags.Visible = false;
            btnReport.Enabled = false;
            lblMsg.Text = "Currently you are not associated with any federations, so you won't see any data in this report";
        }
        else
        {
            chkAllSynags.Visible = true;
            btnReport.Enabled = true;

            // Federation admins will see only their own Federation
            if (UserRole == Role.FederationAdmin)
            {
                //int FedID = Int32.Parse(Session["FedID"].ToString());
                //ddlFed.SelectedValue = FedID.ToString();
                ddlFed.Enabled = true;
            }
            else if (UserRole == Role.FJCAdmin || UserRole == Role.MovementAdmin)
            {
                ddlFed.Enabled = true;
            }
            else if (UserRole == Role.CampDirector)
            {
                ddlFed.SelectedValue = "3";
                ddlFed.Enabled = false;
            }
         }
    }

    protected void ddlFed_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkAllSynags.Checked = false;

        foreach (ListItem li in chklistSynag.Items)
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

    private void MakeKeyStatusBold()
    {
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Text.Trim() == "Eligible" || li.Text.Trim() == "Eligible by staff" || li.Text.Trim() == "Campership approved; payment pending" || li.Text.Trim() == "Payment requested" || li.Text.Trim() == "Camper Attended Camp")
                li.Attributes.CssStyle.Add("font-weight", "bold");
        }
    }

    protected void chkAllSynags_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllSynags.Checked)
            foreach (ListItem li in chklistSynag.Items)
            {
                li.Selected = true;
            }
        else
            foreach (ListItem li in chklistSynag.Items)
            {
                li.Selected = false;
            } 
    }
    
    protected void chklistSynag_DataBound(object sender, EventArgs e)
    {
        if (chklistSynag.Items.Count == 0)
        {
            lblMsg.Text = "The federation has no synagogue in the camper applications data";
            chkAllSynags.Enabled = false;
            btnReport.Enabled = false;
        }
        else
        {
            lblMsg.Text = "";
            chkAllSynags.Enabled = true;
            btnReport.Enabled = true;
        }
    }
    
    protected void btnReport_Click(object sender, EventArgs e)
    {
        bool e_flag = true;
        foreach (ListItem li in chklistSynag.Items)
        {
            if (li.Selected)
                e_flag = false;

        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one synagogue";
            return;
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
            return;
        }

        ReportParamCampersFJC param = new ReportParamCampersFJC() 
        { 
            CampYearID = Int32.Parse(ddlCampYear.SelectedValue), 
            CampYear = Int32.Parse(ddlCampYear.SelectedItem.Text), 
            ProgramTypeID = (ProgramType)(-1), 
            FedID = Int32.Parse(ddlFed.SelectedValue), 
            CamperOrg = CamperOrgType.Synagogue 
        };

        foreach (ListItem li in chklistSynag.Items)
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

        Response.Redirect("ReportFJCCAmpers.aspx");
    }
}
