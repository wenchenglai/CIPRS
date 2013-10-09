using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GemBox.ExcelLite;
using System.Drawing;

using Model;

public partial class CamperSummaryReportByCamp : System.Web.UI.Page
{
    Role UserRole;

    protected void Page_Load(object sender, EventArgs e)
    {
        UserRole = (Role)Convert.ToInt32(Session["RoleID"]);
        lblMsg.Text = "";
        chkAllCamps.Visible = true;

        MakeKeyStatusBold();

        // We have to determine if its Fed Admin
        if (!IsPostBack)
        {
            if (UserRole == Role.CampDirector || UserRole == Role.FederationAdmin)
            {
                chkAllFeds.Visible = false;
            }

            using (CIPMSEntities1 ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.SelectedValue = Application["CampYearID"].ToString();
                ddlCampYear.DataBind();
            }
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
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
            lblMsg.Text = "You must select at least one federation";
            return;
        }

        e_flag = true;
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

        if (!chk1stTimers.Checked)
            if (!chk2ndTimers.Checked)
                if (!chk3rdTimers.Checked)
                    if (!chkAllTimers.Checked)
                    {
                        lblMsg.Text = "You must at least select one option from # of years in program ";
                        return;
                    }

        DataSet ds = GenerateDataSet();

        if (chkAllTimers.Checked)
        {
            gvAllTimers.DataSource = ds.Tables["AllCampers"];
            gvAllTimers.DataBind();
            divAllTimers.Visible = true;
        }
        else
        {
            divAllTimers.Visible = false;
        }

        if (chk1stTimers.Checked)
        {
            gv1stTimers.DataSource = ds.Tables["1stYearCampers"];
            gv1stTimers.DataBind();
            div1stTimers.Visible = true;
        }
        else
        {
            div1stTimers.Visible = false;
        }

        if (chk2ndTimers.Checked)
        {
            gv2ndTimers.DataSource = ds.Tables["2ndYearCampers"];
            gv2ndTimers.DataBind();
            div2ndTimers.Visible = true;
        }
        else
        {
            div2ndTimers.Visible = false;
        }

        if (chk3rdTimers.Checked)
        {
            if (ds.Tables["3rdYearCampers"].Rows.Count > 0)
            {
                gv3rdTimers.DataSource = ds.Tables["3rdYearCampers"];
                gv3rdTimers.DataBind();
                div3rdTimers.Visible = true;
            }
            else
                div3rdTimers.Visible = false;
        }
        else
        {
            div3rdTimers.Visible = false;
        }  

        divMenu.Visible = false;
        divReport.Visible = true;
    }

    private DataSet GenerateDataSet()
    {
        string FedID_List = "";
        foreach (ListItem li in chklistFed.Items)
        {
            if (li.Selected)
            {
                if (FedID_List == "")
                    FedID_List = li.Value;
                else
                    FedID_List += ", " + li.Value;
            }
        }

        if (FedID_List == "")
        {
            FedID_List = "3,4";
        }

        string CampID_List = "";
        foreach (ListItem li in chklistCamp.Items)
        {
            if (li.Selected)
            {
                if (CampID_List == "")
                    CampID_List = li.Value;
                else
                    CampID_List += ", " + li.Value;
            }
        }

        string StatusID_List = "";
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
            {
                if (StatusID_List == "")
                    StatusID_List = String.Format("{0}", li.Value.Trim());
                else
                    StatusID_List += String.Format(", {0}", li.Value.Trim());
            }
        }

        int TimesReceivedGrant = 0;
        if (chkAllTimers.Checked)
            TimesReceivedGrant += 1;

        if (chk1stTimers.Checked)
            TimesReceivedGrant += 2;

        if (chk2ndTimers.Checked)
            TimesReceivedGrant += 4;

        if (chk3rdTimers.Checked)
            TimesReceivedGrant += 8;

        return CamperApplicationBL.GetCamperSummaryReportByCamp(Int32.Parse(ddlCampYear.SelectedValue), FedID_List, CampID_List, StatusID_List, TimesReceivedGrant);
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

    protected void chklistCamp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (chklistCamp.Items.Count == 0)
        {
            //btnReport.Enabled = false;
        }
        else
        {
            //btnReport.Enabled = true;
        }
    }

    private static DataTable GenerateDataTable(ReportParamCampersFJC param)
    {
        return CamperApplicationBL.GetCamperSummaryReport(param.CampYearID, param.FedID, param.CampID_List, param.StatusID_List);
    }

    protected void chklistCamp_DataBound(object sender, EventArgs e)
    {
        if (chklistCamp.Items.Count == 0)
        {
            lblMsg.Text = "The federation has no camp in the camper applications data OR no federation is selected";
            chkAllCamps.Enabled = false;
            //btnReport.Enabled = false;
        }
        else
        {
            lblMsg.Text = "";
            chkAllCamps.Enabled = true;
            //btnReport.Enabled = true;
        }
    }

    //protected void ddlFed_DataBound(object sender, EventArgs e)
    //{
    //    if (ddlFed.Items.Count == 0)
    //    {
    //        chkAllCamps.Visible = false;
    //        btnReport.Enabled = false;
    //        lblMsg.Text = "Currently you are not associated with any federations, so you won't see any data in this report";
    //    }
    //    else
    //    {
    //        chkAllCamps.Visible = true;
    //        btnReport.Enabled = true;

    //        // Federation admins will see only their own Federation
    //        if (UserRole == Role.FederationAdmin)
    //        {
    //            int FedID = Int32.Parse(Session["FedID"].ToString());
    //            ddlFed.SelectedValue = FedID.ToString();
    //            ddlFed.Enabled = false;
    //        }
    //    }
    //}

    //protected void ddlFed_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    chkAllCamps.Checked = false;

    //    foreach (ListItem li in chklistCamp.Items)
    //    {
    //        li.Selected = false;
    //    }

    //    foreach (ListItem li in chklistStatus.Items)
    //    {
    //        li.Selected = false;
    //    }
    //}

    protected void chklistStatus_DataBound(object sender, EventArgs e)
    {
        MakeKeyStatusBold();
    }

    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        divMenu.Visible = true;
        divReport.Visible = false;
    }

    protected void odsCamp_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["FedList"] = chklistFed.Items;
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkAllFeds.Checked = false;
        chkAllCamps.Checked = false;
    }

    protected void btnExcelExport_Click(object sender, EventArgs e)
    {
        GenerateExcelReport();
    }

    private void GenerateExcelReport()
    {
        string templateFile = Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
        string workFileDir = Server.MapPath(@"~/Docs");

        // Make a excel report
        ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
        ExcelFile excel = new ExcelFile();
        excel.LoadXls(templateFile);

        ExcelWorksheet ws = excel.Worksheets["Sheet1"];

        //We start at first row, because for ExcelLite control, the header row is not included
        int BEGIN_COLUMN_INDEX = 1;
        const int REPORT_HEADER_CELL_NUMBER = 6;
        const int REPORT_SUB_HEADER_CELL_NUMBER = 4;

        int iRow = 1;

        // Global artistic setting
        ws.Columns[0].Width = 20 * 20; // make the first column smaller

        // Create Report Header
        CellStyle styleReportHeader = new CellStyle();
        styleReportHeader.Font.Color = System.Drawing.Color.Blue;
        styleReportHeader.Font.Size = 22 * 20;
        styleReportHeader.Font.Weight = ExcelFont.BoldWeight;

        CellRange ReportHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_HEADER_CELL_NUMBER);
        ReportHeader.Merged = true;
        ReportHeader.Style = styleReportHeader;
        ReportHeader.Value = "Session Length by Camp (Online Data Only)";

        ws.Rows[iRow].Height = 25 * 20;

        iRow += 1;

        // Create Report SubHeader - usually it's camp year and report generation time
        CellStyle styleReportSubHeader = new CellStyle();
        styleReportSubHeader.Font.Size = 16 * 20;
        styleReportSubHeader.Font.Weight = ExcelFont.BoldWeight;

        CellRange SubHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
        SubHeader.Merged = true;
        SubHeader.Style = styleReportSubHeader;
        SubHeader.Value = string.Format("Camp Year: {0}.  Generated on {1} {2}", ddlCampYear.SelectedItem.Text, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());

        iRow += 4;

        CellStyle styleTableTitle = new CellStyle { HorizontalAlignment = HorizontalAlignmentStyle.Center };
        styleTableTitle.Font.Size = 16 * 20;
        styleTableTitle.FillPattern.SetSolid(Color.LightBlue);
        styleTableTitle.Font.Weight = ExcelFont.BoldWeight;

        CellStyle styleTableDataRow = new CellStyle { HorizontalAlignment = HorizontalAlignmentStyle.Center };
        CellStyle styleTableDataRowCampName = new CellStyle { HorizontalAlignment = HorizontalAlignmentStyle.Left };

        CellStyle styleTableHeaderColumns = new CellStyle();
        styleTableHeaderColumns.Font.Weight = ExcelFont.BoldWeight;
        styleTableHeaderColumns.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        styleTableHeaderColumns.FillPattern.SetSolid(Color.LightGray);

        // Data Content of report
        DataSet dsModified = GenerateDataSet();

        // Get rid of first columns for other tables except first one
        for (int i = 1; i < dsModified.Tables.Count; i++)
            dsModified.Tables[i].Columns.Remove("Camp Name");

        int table_column_count = dsModified.Tables[0].Columns.Count;
        int times = 0, current_starting_column = 0;
        foreach (DataTable dt in dsModified.Tables)
        {
            if (times == 1)
                BEGIN_COLUMN_INDEX += 1;
            // Get the header location
            current_starting_column = BEGIN_COLUMN_INDEX + times * table_column_count;

            int temp_column = 0;
            if (times == 0)
                temp_column = current_starting_column + 1;
            else
                temp_column = current_starting_column;

            // Table Title
            CellRange TableTitle = ws.Cells.GetSubrangeAbsolute(iRow - 1, temp_column, iRow - 1, current_starting_column + dt.Columns.Count - 1);
            TableTitle.Merged = true;
            TableTitle.Value = dt.TableName;
            TableTitle.Style = styleTableTitle;

            // this creats the real table
            ws.InsertDataTable(dt, iRow, current_starting_column, true);

            // loop through each column and set style accordingly
            for (int i = current_starting_column; i <= (dt.Columns.Count + current_starting_column - 1); i++)
            {
                ws.Cells[iRow, i].Style = styleTableHeaderColumns;
                ws.Cells[iRow + dt.Rows.Count, i].Style = styleTableHeaderColumns;

                ws.Columns[i].Width = 11 * 256;

                // first column of first table, e.g. camp naem
                if (times == 0)
                {
                    if (i == current_starting_column)
                        ws.Columns[i].Width = 55 * 256; // camp/program name
                    else if (i == current_starting_column + 1)
                        ws.Columns[i].Width = 15 * 256;
                }
                else if (i == current_starting_column)
                {
                    ws.Columns[i].Width = 15 * 256;
                }   
            }

            // Set the data row style
            for (int j = iRow + 1; j < iRow + dt.Rows.Count; j++)
            {
                ws.Rows[j].Style = styleTableDataRow;
            }

            // left justify the camp names
            if (times == 0)
            {
                for (int j = iRow + 1; j < iRow + dt.Rows.Count; j++)
                {
                    ws.Cells[j, current_starting_column].Style = styleTableDataRowCampName;
                }
            }

            times++;
        }

        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

        // Save to a file on the local file system
        string filename = String.Format("\\{0}{1}{2}{3}CamperSummaryReportByCamp.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
        string newFile = workFileDir + filename;
        excel.SaveXls(newFile);


        string[] strFileParts = newFile.Split(new string[] { "\\" }, StringSplitOptions.None);

        //Display excel spreadsheet
        this.Response.Clear();
        this.Response.Buffer = true;
        this.Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileParts[strFileParts.Length - 1]);
        this.Response.ContentType = "application/vnd.ms-excel";
        this.Response.Charset = "";

        if (newFile.Length == 0)
            this.Response.Write("Error encountered - no spreadsheet to display");
        else
            this.Response.WriteFile(newFile);

        Response.End();
    }
}