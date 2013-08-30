using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using GemBox.ExcelLite;
using System.Drawing;

public partial class FJCAllocationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
        foreach (ListItem li in chklistFed.Items)
        {
            if (li.Selected)
                e_flag = false;
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one federation";
            return;
        }

        gv.DataSource = GenerateDataTable();
        gv.DataBind();

        divReport.Visible = true;
        divMenu.Visible = false;
    }

    private DataTable GenerateDataTable()
    {
        DataSet ds = CamperApplicationDA.GetFJCAllocationData(Int32.Parse(ddlYear.SelectedValue));
        DataTable dtMain = ds.Tables[0];
        DataTable dtOverage = ds.Tables[1];
        DataTable dtFirstSecondTimers = ds.Tables[2];

        dtMain.Columns.Add(new DataColumn("FJC Standard", System.Type.GetType("System.Int32")));
        dtMain.Columns.Add(new DataColumn("FJC Overage", System.Type.GetType("System.Int32")));
        dtMain.Columns.Add(new DataColumn("Partner Allocated", System.Type.GetType("System.Int32"))); // need this for the correct column order in excel report
        dtMain.Columns.Add(new DataColumn("Total Campers", System.Type.GetType("System.Int32")));
        dtMain.Columns.Add(new DataColumn("First Time", System.Type.GetType("System.Int32")));
        dtMain.Columns.Add(new DataColumn("Second Time", System.Type.GetType("System.Int32")));

        foreach (DataRow dr in dtMain.Rows)
        {
            // This line of code is purely for excel report, because we have to make sure excel report's table columns follow the same order as in html gridview
            dr["Partner Allocated"] = dr["PartnerAllocated"];

            // first step, filter out those data that has not been selected
            bool isSelected = false;
            foreach (ListItem li in chklistFed.Items)
            {
                if (li.Selected)
                {
                    if (li.Value == dr["FederationID"].ToString())
                        isSelected = true;
                }
            }

            if (!isSelected)
                dr.Delete();
            else
            {
                // this federation is selected, so we must process the data
                DataRow[] drows = dtOverage.Select("FederationID = " + dr["FederationID"]);
                if (drows.Length > 0)
                {
                    dr["FJC Overage"] = drows[0]["Overage"];
                    int FJCStandard = Convert.ToInt32(dr["FJCMatch"]) - Convert.ToInt32(dr["FJC Overage"]);
                    if (FJCStandard != 0)
                        dr["FJC Standard"] = FJCStandard;
                }
                else
                {
                    dr["FJC Standard"] = dr["FJCMatch"];
                }

                // First timer and Second timer calculation
                int firstTimer = 0;
                int secondTimer = 0;

                drows = dtFirstSecondTimers.Select("FederationID = " + dr["FederationID"]);
                if (drows.Length > 0)
                {
                    foreach (DataRow mydr in drows)
                    {
                        // 2010/01/07 Now there is an easier way to access first/seconder timer data in CamperApplications table
                        if (mydr["TimeInCamp"] is int)
                        {
                            int TimeInCamp = Convert.ToInt32(mydr["TimeInCamp"]);

                            if (TimeInCamp == 1)
                            {
                                firstTimer = Convert.ToInt32(mydr["CamperCount"]);
                                dr["First Time"] = firstTimer;
                            }
                            else if (TimeInCamp == 2)
                            {
                                secondTimer = Convert.ToInt32(mydr["CamperCount"]);
                                dr["Second Time"] = secondTimer;
                            }

                        }
                    }
                }
                dr["Total Campers"] = firstTimer + secondTimer;
            }
        }

        DataRow newrow = dtMain.NewRow();

        newrow[0] = "Total";

        int total = 0;
        for (int i = 1; i < dtMain.Columns.Count; i++)
        {
            try
            {
                total = Int32.Parse(dtMain.Compute(String.Format("SUM([{0}])", dtMain.Columns[i].ColumnName), "").ToString());
            }
            catch
            {
                total = 0;
            }
            newrow[i] = total;
        }
        dtMain.Rows.Add(newrow);
        
        return dtMain;
    }
    
    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        divMenu.Visible = true;
        divReport.Visible = false;
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
        ReportHeader.Value = "FJC Allocation Report";

        //
        iRow += 1;

        // Create Report SubHeader - usually it's camp year
        CellStyle styleSubHeader = new CellStyle();
        styleSubHeader.Font.Size = 16 * 20;
        styleSubHeader.Font.Weight = ExcelFont.BoldWeight;

        CellRange SubHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
        SubHeader.Merged = true;
        SubHeader.Value = "Camp Year: " + ddlYear.SelectedItem.Text;
        SubHeader.Style = styleSubHeader;

        iRow += 2;

        // Data Content of report
        DataTable dt = GenerateDataTable();
        dt.Columns.RemoveAt(1); // delete the federation ID
        dt.Columns.RemoveAt(2); // delete the FCJ Allocated (because it's alreay divided into FJC Standard and Overage
        dt.Columns.RemoveAt(2); // delete the PartnerAllocated (because it has another duplicate row that located in the right position in column index

        CellStyle cs = new CellStyle();
        cs.Font.Size = 18 * 20;
        cs.Font.Weight = ExcelFont.BoldWeight;


        CellRange cr = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
        cr.Merged = true;
        cr.Value = "";
        cr.Style = cs;

        ws.Rows[iRow].Height = 25 * 20;

        iRow += 1;

        dt.AcceptChanges();
        ws.InsertDataTable(dt, iRow, BEGIN_COLUMN_INDEX, true);

        CellStyle tableHeaderStyle = new CellStyle();
        tableHeaderStyle.Font.Weight = ExcelFont.BoldWeight;
        tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);

        for (int i = BEGIN_COLUMN_INDEX; i <= dt.Columns.Count; i++)
        {
            ws.Cells[iRow, i].Style = tableHeaderStyle;
            ws.Cells[iRow + dt.Rows.Count, i].Style = tableHeaderStyle;
            if (i == 1)
                ws.Columns[i].Width = 35 * 256;
            else
                ws.Columns[i].Width = 20 * 256;
        }

        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

        // Save to a file on the local file system
        string filename = "\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Millisecond.ToString() + "CamperCountByProgramAndStatus.xls";
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

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkAllFeds.Checked = false;
    }
}
