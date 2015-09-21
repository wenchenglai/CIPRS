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

public partial class NumberOfCampersByProgram : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
    
    protected void btnReport_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
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

        if (!chk1stTimers.Checked)
            if (!chk2ndTimers.Checked)
                if (!chk3rdTimers.Checked)
                    if (!chkAllTimers.Checked)
                    {
                        lblMsg.Text = "You must at least select one option from # of years in program ";
                        return;
                    }

        DataSet dsRaw = GenerateDataTable();
        DataSet ds = NormalizeTables(dsRaw);

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
            gv3rdTimers.DataSource = ds.Tables["3rdYearCampers"];
            gv3rdTimers.DataBind();
            div3rdTimers.Visible = true;
        }
        else
        {
            div3rdTimers.Visible = false;
        }      

        divMenu.Visible = false;
        divReport.Visible = true;

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

    private DataSet GenerateDataTable()
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

        string StatusName_List = "";
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
            {
                if (StatusName_List == "")
                    StatusName_List = String.Format("[{0}]", li.Text.Trim());
                else
                    StatusName_List += String.Format(", [{0}]", li.Text.Trim());
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

        return CamperApplicationBL.GetCamperCountByFed(Int32.Parse(ddlCampYear.SelectedValue), FedID_List, StatusName_List, TimesReceivedGrant);
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
            if (li.Value == "1" || li.Value == "7" || li.Value == "14" || li.Value == "25" || li.Value == "28") 
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
        int BEGIN_COLUMN_INDEX = 1;
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
        ReportHeader.Value = "Number of Campers By Program, Status and Summer";

        iRow += 1;

        // Create Report SubHeader - usually it's camp year
        CellStyle styleSubHeader = new CellStyle();
        styleSubHeader.Font.Size = 16 * 20;
        styleSubHeader.Font.Weight = ExcelFont.BoldWeight;

        CellRange SubHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
        SubHeader.Merged = true;
        SubHeader.Value = string.Format("Camp Year: {0}.  Generated on {1} {2}", ddlCampYear.SelectedItem.Text, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
        SubHeader.Style = styleSubHeader;

        iRow += 2;

        // 2010-11-26 no longer needed
        //AddSubTotalRowsTable(dsNormalized);

        CellStyle cs = new CellStyle();
        cs.Font.Size = 18 * 20;
        cs.Font.Weight = ExcelFont.BoldWeight;

        CellRange cr = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
        cr.Merged = true;
        cr.Value = "";
        cr.Style = cs;

        ws.Rows[iRow].Height = 25 * 20;

        iRow += 2;

        // Data Content of report
        DataSet ds = GenerateDataTable();
        DataSet dsNormalized = NormalizeTables(ds);

        for (int i = 1; i < ds.Tables.Count; i++)
            dsNormalized.Tables[i].Columns.Remove("Name");

        int times = 0;
        int table_column_count = ds.Tables[0].Columns.Count;
        int current_column = 0;
        foreach (DataTable dt in dsNormalized.Tables)
        {
            if (times == 1)
                BEGIN_COLUMN_INDEX += 1;
            // Get the header location
            current_column = BEGIN_COLUMN_INDEX + times * table_column_count;

            CellRange TableHeader = ws.Cells.GetSubrangeAbsolute(iRow - 1, current_column, iRow - 1, current_column);
            TableHeader.Value = dt.TableName;
            TableHeader.Style = styleSubHeader;

            // this creats the real table
            ws.InsertDataTable(dt, iRow, current_column, true);

            CellStyle tableHeaderStyle = new CellStyle();
            tableHeaderStyle.Font.Weight = ExcelFont.BoldWeight;
            tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);

            for (int i = current_column; i <= (dt.Columns.Count + current_column - 1); i++)
            {
                ws.Cells[iRow, i].Style = tableHeaderStyle;
                ws.Cells[iRow + dt.Rows.Count, i].Style = tableHeaderStyle;
                if (i == current_column)
                {
                    // first column of every table
                    if (times >= 1)
                        ws.Columns[i].Width = 20 * 256;
                    else
                        ws.Columns[i].Width = 35 * 256;
                }    
                else
                    ws.Columns[i].Width = 20 * 256; // non-first column
            }

            //if (times == 1)
            //    BEGIN_COLUMN_INDEX -= 1;

            times++;
        }

        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

        // Save to a file on the local file system
        string filename = String.Format("\\{0}{1}{2}{3}CamperCountByProgramAndStatus.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
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

    /// <summary>
    /// This function will make all tables having the same number of rows, each row is a program/camp
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    private static DataSet NormalizeTables(DataSet ds)
    {
        DataTable dtModel = ds.Tables[0].Clone();

        string rowname;
        foreach (DataTable dt in ds.Tables)
        {
            foreach (DataRow dr in dt.Rows)
            {
                rowname = dr["Name"].ToString();
                string[] a = rowname.Split(new char[] { '\'' });
                if (a.Length > 1)
                {
                    if (a.Length == 2)
                        rowname = String.Format("{0}''{1}", a[0], a[1]);
                    else if (a.Length == 3)
                        rowname = String.Format("{0}''{1}''{2}", a[0], a[1], a[2]);
                    else
                        rowname = String.Format("{0}''{1}''{2}''{3}", a[0], a[1], a[2], a[3]);
                }
                DataRow[] drs = dtModel.Select(String.Format("Name = '{0}'", rowname));

                if (drs.Length == 0)
                {
                    DataRow newrow = dtModel.NewRow();
                    newrow["Name"] = dr["Name"].ToString(); ;
                    dtModel.Rows.Add(newrow);
                }
            }
        }

        DataSet dsOut = new DataSet();

        // create whole new data tables again
        foreach (DataTable dt in ds.Tables)
        {
            DataTable dtNew = dtModel.Copy();
            dtNew.TableName = dt.TableName;

            foreach (DataRow dr in dt.Rows)
            {
                rowname = dr["Name"].ToString();
                string[] a = rowname.Split(new char[] { '\'' });
                if (a.Length > 1)
                {
                    if (a.Length == 2)
                        rowname = String.Format("{0}''{1}", a[0], a[1]);
                    else if (a.Length == 3)
                        rowname = String.Format("{0}''{1}''{2}", a[0], a[1], a[2]);
                    else
                        rowname = String.Format("{0}''{1}''{2}''{3}", a[0], a[1], a[2], a[3]);
                }
                DataRow[] drs = dtNew.Select(String.Format("Name = '{0}'", rowname));

                if (drs.Length == 1)
                {
                    int numCol = dtNew.Columns.Count;

                    for (int i = 1; i < numCol; i++)
                    {
                        if ((int)dr[i] >= 1)
                            drs[0][i] = dr[i];
                        else
                            drs[0][i] = 0;
                    }
                }
                else
                {
                    //something is wrong, should always have one row
                }
            }

            // Make all other empty cell zero
            foreach (DataRow dr in dtNew.Rows)
            {
                for (int i = 1; i < dtNew.Columns.Count; i++)
                {
                    if (DBNull.Value == dr[i])
                    {
                        dr[i] = 0;
                    }
                }
            }

            dsOut.Tables.Add(dtNew);
        }

        return dsOut;
    }

    protected void btnExcelExport_Click(object sender, EventArgs e)
    {
        GenerateExcelReport();
    }
}
