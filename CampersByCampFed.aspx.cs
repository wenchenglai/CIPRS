using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Model;

public partial class CampersByCampFed : System.Web.UI.Page
{
    Role UserRole;

    protected void Page_Load(object sender, EventArgs e)
    {
        UserRole = (Role)Convert.ToInt32(Session["RoleID"]);

        lblMsg.Text = "";
        chkAllCamps.Enabled = true;
        //btnReport123.Enabled = true;

        MakeKeyStatusBold();

        if (!IsPostBack)
        {
            using (CIPMSEntities1 ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.SelectedValue = Application["CampYearID"].ToString();
                ddlCampYear.DataBind();
            }

            if (UserRole == Role.CampDirector)
            {
                ddlFed.Visible = false;
                ddlFed.DataSourceID = null;

                chklistCamp.DataSourceID = null;
                chklistCamp.DataSource = CampsDA.GetCampByJWestCampDirector(Int32.Parse(ddlCampYear.SelectedValue), Int32.Parse(Session["UserID"].ToString()));
                chklistCamp.DataBind();

                chkAllCamps.Visible = false;
            }
        }
    }

    private void MakeKeyStatusBold()
    {
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Text.Trim() == "Eligible" || li.Text.Trim() == "Eligible by staff" || li.Text.Trim() == "Campership approved; payment pending" || li.Text.Trim() == "Payment requested" || li.Text.Trim() == "Camper Attended Camp")
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
                e_flag = false;

        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one camp";
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
            ProgramTypeID = ProgramType.NoUse, 
            CampYear = Int32.Parse(ddlCampYear.SelectedItem.Text) 
        };

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

        Session.Add("ReportParamCampersFJC", param);

        Response.Redirect("ReportFJCCAmpers.aspx");
    }

    protected void chklistCamp_DataBound(object sender, EventArgs e)
    {
        if (chklistCamp.Items.Count == 0)
        {
            if (lblMsg.Text == "")
                lblMsgCamps.Text = "The federation has no camp in the camper applications data";
            chkAllCamps.Enabled = false;
            btnReport.Enabled = false;
        }
        else
        {
            lblMsgCamps.Text = "";
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
                if (ddlFed.Items.FindByValue(FedID.ToString()) == null)
                {
                    lblMsg.Text = "Currently there are no camper applications for this program in the system";
                    ddlFed.SelectedItem.Text = "No camper application data";
                }
                else                    
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
}
