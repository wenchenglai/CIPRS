using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

public partial class SummaryByCamp : System.Web.UI.Page
{
    Role UserRole;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserRole = (Role)Convert.ToInt32(Session["RoleID"]);

        lblMsg.Text = "";
        MakeKeyStatusBold();

        if (!IsPostBack)
        {
            if (UserRole != Role.FJCAdmin)
            {
                ddlProgram.Enabled = false;

                if (UserRole == Role.CampDirector)
                {
                    ddlProgram.Visible = false;
                    chkAllCamps.Visible = false;
                    ddlProgram.SelectedIndex = 1;
                }
            }

            txtOtherDate.Text = DateTime.Now.ToShortDateString();

            DataTable dt = CampYearDA.GetAllYearsWithoutCurrentYear();
            var dv = new DataView(dt) { Sort = "CampYear desc" };
            cblYearsToday.DataSource = dv;
            cblYearsToday.DataBind();
        }

        // Delete old work files
        string workFileDir = Server.MapPath(@"~/Docs");

        // need to check and see if any old files are left behind
        DirectoryInfo myDir = new DirectoryInfo(workFileDir);

        foreach (FileSystemInfo myFile in myDir.GetFileSystemInfos("*.xls"))
        {
            // Delete any files that are 1 min old
            //if ((DateTime.Now - myFile.CreationTime).Minutes > 1)
            myFile.Delete();
        }
    }

    protected void chkAllStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllStatus.Checked)
            foreach (ListItem li in chklistStatus.Items)
            {
                li.Selected = true;
            }
        else
            foreach (ListItem li in chklistStatus.Items)
            {
                li.Selected = false;
            }
    }

    protected void rdoToday_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoOtherDate.Checked)
        {
            divCalendar.Visible = true;
        }
        else
        {
            divCalendar.Visible = false;
        }
    }

    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkAllCamps.Checked = false;

        foreach (ListItem li in chklistCamp2013.Items)
        {
            li.Selected = false;
        }

        foreach (ListItem li in chklistStatus.Items)
        {
            li.Selected = false;
        }
    }

    protected void chkAllCamps_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCamps.Checked)
            foreach (ListItem li in chklistCamp2013.Items)
            {
                li.Selected = true;
            }
        else
            foreach (ListItem li in chklistCamp2013.Items)
            {
                li.Selected = false;
            }
    }

    protected void chklistCamp_DataBound(object sender, EventArgs e)
    {
        if (chklistCamp2013.Items.Count < 1)
        {
            btnReport.Enabled = false;
            chkAllCamps.Enabled = false;
            lblMsg.Text = "Currently you are not associated with any camps, so you cannot see any data from this report";
        }
        else
        {
            lblMsg.Text = "";
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

    protected void btnReport_Click(object sender, EventArgs e)
    {


    }
    protected void cblYearsToday_SelectedIndexChanged(object sender, EventArgs e)
    {
        div2012CampList.Visible = false;        
        div2011CampList.Visible = false;
        div2010CampList.Visible = false;
        div2009CampList.Visible = false;

        var selectedYears = new List<string>();
        foreach (var item in cblYearsToday.Items.Cast<ListItem>().Where(s => s.Selected))
        {
            selectedYears.Add(item.Text);
        }

        if (selectedYears.Contains("2009"))
        {
            chklistCamp2009.DataSourceID = "odsCamps2009";
            chklistCamp2009.DataBind();
            div2009CampList.Visible = true;
        }
        
        if (selectedYears.Contains("2010"))
        {
            chklistCamp2010.DataSourceID = "odsCamps2010";
            chklistCamp2010.DataBind();
            div2010CampList.Visible = true;
        }
        
        if (selectedYears.Contains("2011"))
        {
            chklistCamp2011.DataSourceID = "odsCamps2011";
            chklistCamp2011.DataBind();
            div2011CampList.Visible = true;
        }
        
        if (selectedYears.Contains("2012"))
        {
            chklistCamp2012.DataSourceID = "odsCamps2012";
            chklistCamp2012.DataBind();
            div2012CampList.Visible = true;
        }
    }
}