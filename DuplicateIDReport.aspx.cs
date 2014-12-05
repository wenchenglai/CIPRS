using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using GemBox.ExcelLite;
using System.Data;
using System.Drawing;

using Model;

public partial class DuplicateIDReport : System.Web.UI.Page
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
       
    protected void btnReport_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        bool e_flag = true;

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

        int count = 0;
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
            {
                count++;
                if (count > 5)
                {
                    lblMsg.Text = "Please select 5 or less Status";
                    return;
                }
            }
        }

        toYear.Text = ddlCampYear.SelectedItem.Text;
        fromYear.Text = (Int32.Parse(ddlCampYear.SelectedItem.Text) - 1).ToString();
        

        gv.DataSource = GenerateDataTable();
        gv.DataBind();

        divMenu.Visible = false;
        divReport.Visible = true;
    }

    private DataTable GenerateDataTable()
    {
        string StatusID_List = "";
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
            {
                if (StatusID_List == "")
                    StatusID_List = String.Format("{0}", li.Value.Trim());
                else
                    StatusID_List += String.Format(",{0}", li.Value.Trim());
            }
        }

        return DuplicateCheckingBL.GetDuplicateCampers(UserRole, Int32.Parse(ddlCampYear.SelectedValue), StatusID_List);
    }
    
    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        divMenu.Visible = true;
        divReport.Visible = false;
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
        const int BEGIN_COLUMN_INDEX = 1;
        const int CAMP_NAME_MERGED_CELL_NUMBER = 6;
        const int REPORT_HEADER_CELL_NUMBER = 6;
        const int REPORT_SUB_HEADER_CELL_NUMBER = 4;

        int iRow = 1;

        // Global artistic setting
        ws.Columns[0].Width = 20 * 20;

        // Create Report Header
        CellStyle styleReportHeader = new CellStyle();
        styleReportHeader.Font.Color = System.Drawing.Color.Blue;
        styleReportHeader.Font.Size = 22 * 20;
        styleReportHeader.Font.Weight = ExcelFont.BoldWeight;

        CellRange ReportHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_HEADER_CELL_NUMBER);
        ReportHeader.Merged = true;
        ReportHeader.Style = styleReportHeader;
        ReportHeader.Value = "Number of Possible Duplicate Campers";

        //
        iRow += 1;

        // Create Report SubHeader - usually it's camp year
        CellStyle styleSubHeader = new CellStyle();
        styleSubHeader.Font.Size = 16 * 20;
        styleSubHeader.Font.Weight = ExcelFont.BoldWeight;

        CellRange SubHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
        SubHeader.Merged = true;
        SubHeader.Value = "Camp Year: " + ddlCampYear.SelectedItem.Text;
        SubHeader.Style = styleSubHeader;

        iRow += 2;

        // Data Content of report
        DataTable dt = GenerateDataTable();

        CellStyle cs = new CellStyle();
        cs.Font.Size = 18 * 20;
        cs.Font.Weight = ExcelFont.BoldWeight;


        CellRange cr = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
        cr.Merged = true;
        cr.Value = "";
        cr.Style = cs;

        ws.Rows[iRow].Height = 25 * 20;

        iRow += 1;

        ws.InsertDataTable(dt, iRow, BEGIN_COLUMN_INDEX, true);

        CellStyle tableHeaderStyle = new CellStyle();
        tableHeaderStyle.Font.Weight = ExcelFont.BoldWeight;
        tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);

        for (int i = BEGIN_COLUMN_INDEX; i <= dt.Columns.Count; i++)
        {
            ws.Cells[iRow, i].Style = tableHeaderStyle;
            ws.Cells[iRow + dt.Rows.Count, i].Style = tableHeaderStyle;
            if (i == 1)
                ws.Columns[i].Width = 30 * 256;
            else
                ws.Columns[i].Width = 20 * 256;
        }

        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

        // Save to a file on the local file system
        string filename = "\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Millisecond.ToString() + "CamperCountByCampAndStatus.xls";
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

    protected void btnExcelExport_Click(object sender, EventArgs e)
    {
        GenerateExcelReport();
    }
}
