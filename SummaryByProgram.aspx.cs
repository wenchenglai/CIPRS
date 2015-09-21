using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using GemBox.ExcelLite;
using System.Drawing;

public partial class SummaryByProgram : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtOtherDate.Text = DateTime.Now.ToShortDateString();

            DataTable dt = CampYearDA.GetAllYearsWithoutCurrentYear();
            var dv = new DataView(dt) { Sort = "CampYear desc" };
            cblYearsToday.DataSource = dv;
            cblYearsToday.DataBind();
            
            DataTable ds = FederationsDA.GetAllFederationsByMultipleCampYearsAndUserRole(Application["CampYearID"].ToString(), (Role)Convert.ToInt32(Session["RoleID"]), Convert.ToInt32(Session["FedID"]));
            chklistFed.DataSource = ds;
            chklistFed.DataBind();
        }
        MakeKeyStatusBold();
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        if (!chk1stTimers.Checked)
            if (!chk2ndTimers.Checked)
                if (!chk3rdTimers.Checked)
                    if (!chkAllTimers.Checked)
                    {
                        lblMsg.Text = "You must at least select one option from # of years in program ";
                        return;
                    }

        DateTime cutoffDate = GetCutoffDate();
        List<int> yearList = GetYearList();

        using (DataSet ds = GenerateDataTables(cutoffDate))
        {
            // Current Year must be shown, no need year checking
            if (chkAllTimers.Checked)
            {
                gvCYAllTimers.DataSource = ds.Tables["CurrentYearAll"];
                gvCYAllTimers.DataBind();
                divCYAllTimers.Visible = true;
            }
            else
            {
                divCYAllTimers.Visible = false;
            }
            if (chk1stTimers.Checked)
            {
                gvCY1stTimers.DataSource = ds.Tables["CurrentYear1st"];
                gvCY1stTimers.DataBind();
                divCY1stTimers.Visible = true;
            }
            else
            {
                divCY1stTimers.Visible = false;
            }
            if (chk2ndTimers.Checked)
            {
                gvCY2ndTimers.DataSource = ds.Tables["CurrentYear2nd"];
                gvCY2ndTimers.DataBind();
                divCY2ndTimers.Visible = true;
            }
            else
            {
                divCY2ndTimers.Visible = false;
            }
            if (chk3rdTimers.Checked)
            {
                gvCY3rdTimers.DataSource = ds.Tables["CurrentYear3rd"];
                gvCY3rdTimers.DataBind();
                divCY3rdTimers.Visible = true;
            }
            else
            {
                divCY3rdTimers.Visible = false;
            }
            // Previous years musch check if users want to see it
            if (yearList.Any(y => y == 2012))
            {
                div2012.Visible = true;
                if (chkAllTimers.Checked)
                {
                    gv2012AllTimers.DataSource = ds.Tables["2012All"];
                    gv2012AllTimers.DataBind();
                    div2012AllTimers.Visible = true;
                }
                else
                {
                    div2012AllTimers.Visible = false;
                }
                if (chk1stTimers.Checked)
                {
                    gv20121stTimers.DataSource = ds.Tables["20121st"];
                    gv20121stTimers.DataBind();
                    div20121stTimers.Visible = true;
                }
                else
                {
                    div20121stTimers.Visible = false;
                }
                if (chk2ndTimers.Checked)
                {
                    gv20122ndTimers.DataSource = ds.Tables["20122nd"];
                    gv20122ndTimers.DataBind();
                    div20122ndTimers.Visible = true;
                }
                else
                {
                    div20122ndTimers.Visible = false;
                }
                if (chk3rdTimers.Checked)
                {
                    gv20123rdTimers.DataSource = ds.Tables["20123rd"];
                    gv20123rdTimers.DataBind();
                    div20123rdTimers.Visible = true;
                }
                else
                {
                    div20123rdTimers.Visible = false;
                }
            }
            else
            {
                div2012.Visible = false;
            }
            if (yearList.Any(y => y == 2011))
            {
                div2011.Visible = true;
                if (chkAllTimers.Checked)
                {
                    gv2011AllTimers.DataSource = ds.Tables["2011All"];
                    gv2011AllTimers.DataBind();
                    div2011AllTimers.Visible = true;
                }
                else
                {
                    div2011AllTimers.Visible = false;
                }
                if (chk1stTimers.Checked)
                {
                    gv20111stTimers.DataSource = ds.Tables["20111st"];
                    gv20111stTimers.DataBind();
                    div20111stTimers.Visible = true;
                }
                else
                {
                    div20111stTimers.Visible = false;
                }
                if (chk2ndTimers.Checked)
                {
                    gv20112ndTimers.DataSource = ds.Tables["20112nd"];
                    gv20112ndTimers.DataBind();
                    div20112ndTimers.Visible = true;
                }
                else
                {
                    div20112ndTimers.Visible = false;
                }
                if (chk3rdTimers.Checked)
                {
                    gv20113rdTimers.DataSource = ds.Tables["20113rd"];
                    gv20113rdTimers.DataBind();
                    div20113rdTimers.Visible = true;
                }
                else
                {
                    div20113rdTimers.Visible = false;
                }
            }
            else
            {
                div2011.Visible = false;
            }
            if (yearList.Any(y => y == 2010))
            {
                div2010.Visible = true;
                if (chkAllTimers.Checked)
                {
                    gv2010AllTimers.DataSource = ds.Tables["2010All"];
                    gv2010AllTimers.DataBind();
                    div2010AllTimers.Visible = true;
                }
                else
                {
                    div2010AllTimers.Visible = false;
                }
                if (chk1stTimers.Checked)
                {
                    gv20101stTimers.DataSource = ds.Tables["20101st"];
                    gv20101stTimers.DataBind();
                    div20101stTimers.Visible = true;
                }
                else
                {
                    div20101stTimers.Visible = false;
                }
                if (chk2ndTimers.Checked)
                {
                    gv20102ndTimers.DataSource = ds.Tables["20102nd"];
                    gv20102ndTimers.DataBind();
                    div20102ndTimers.Visible = true;
                }
                else
                {
                    div20102ndTimers.Visible = false;
                }
                if (chk3rdTimers.Checked)
                {
                    gv20103rdTimers.DataSource = ds.Tables["20103rd"];
                    gv20103rdTimers.DataBind();
                    div20103rdTimers.Visible = true;
                }
                else
                {
                    div20103rdTimers.Visible = false;
                }
            }
            else
            {
                div2010.Visible = false;
            }
            if (yearList.Any(y => y == 2009))
            {
                div2009.Visible = true;
                if (chkAllTimers.Checked)
                {
                    gv2009AllTimers.DataSource = ds.Tables["2009All"];
                    gv2009AllTimers.DataBind();
                    div2009AllTimers.Visible = true;
                }
                else
                {
                    div2009AllTimers.Visible = false;
                }
                if (chk1stTimers.Checked)
                {
                    gv20091stTimers.DataSource = ds.Tables["20091st"];
                    gv20091stTimers.DataBind();
                    div20091stTimers.Visible = true;
                }
                else
                {
                    div20091stTimers.Visible = false;
                }
                if (chk2ndTimers.Checked)
                {
                    gv20092ndTimers.DataSource = ds.Tables["20092nd"];
                    gv20092ndTimers.DataBind();
                    div20092ndTimers.Visible = true;
                }
                else
                {
                    div20092ndTimers.Visible = false;
                }
                if (chk3rdTimers.Checked)
                {
                    gv20093rdTimers.DataSource = ds.Tables["20093rd"];
                    gv20093rdTimers.DataBind();
                    div20093rdTimers.Visible = true;
                }
                else
                {
                    div20093rdTimers.Visible = false;
                }
            }
            else
            {
                div2009.Visible = false;
            }
        }

        divMenu.Visible = false;
        divReport.Visible = true;

        lblSelectedDate.Text = cutoffDate.ToString(" MMM d, yyyy");
    }

    private DateTime GetCutoffDate()
    {
        DateTime cutoffDate = DateTime.Now;
        if (rdoOtherDate.Checked)
        {
            cutoffDate = Convert.ToDateTime(txtOtherDate.Text);
        }
        return cutoffDate;
    }

    private List<int> GetYearList()
    {
        return (from cb in cblYearsToday.Items.Cast<ListItem>()
                  where cb.Selected == true
                  select Int32.Parse(cb.Text)).ToList();
    }

    private DataSet GenerateDataTables(DateTime cutoffDate)
    {
        StringBuilder campYearID_string = new StringBuilder(Application["CampYearID"].ToString());
        var yearList = new List<int>();
        foreach (ListItem li in cblYearsToday.Items)
            if (li.Selected)
            {
                campYearID_string.Append(string.Format(",{0} ", li.Value));
                yearList.Add(Int32.Parse(li.Text));
            }

        int year2008or2009;

        if (cutoffDate.Month >= 10)
            year2008or2009 = 2008;
        else
            year2008or2009 = 2009;

        string FedID_List = "";
        var feds = new Dictionary<int, string>();
        foreach (ListItem li in chklistFed.Items)
        {
            if (li.Selected)
            {
                feds.Add(Convert.ToInt32(li.Value), li.Text);

                if (FedID_List == "")
                    FedID_List = li.Value;
                else
                    FedID_List += ", " + li.Value;
            }
        }

        var StatusName_List = new StringBuilder();
        var stati = new Dictionary<int, string>();
        foreach (ListItem li in chklistStatus.Items)
        {
            if (li.Selected)
            {
                stati.Add(Convert.ToInt32(li.Value), li.Text.Trim());

                if (StatusName_List.Length == 0)
                    StatusName_List.Append(String.Format("'{0}'", li.Text.Trim()));
                else
                    StatusName_List.Append(String.Format(", '{0}'", li.Text.Trim()));
            }
        }

        // 2013-01-29 StatusName must also contain these 4 eligible stati because past years we only have those 4.  StatusName_List is used by database query, so it contains full list
        if (!stati.Any(s => s.Value == "Campership approved; payment pending"))
            StatusName_List.Append(", 'Campership approved; payment pending'");

        if (!stati.Any(s => s.Value == "Eligible"))
            StatusName_List.Append(", 'Eligible'");

        if (!stati.Any(s => s.Value == "Eligible by staff"))
            StatusName_List.Append(", 'Eligible by staff'");

        if (!stati.Any(s => s.Value == "Payment requested"))
            StatusName_List.Append(", 'Payment requested'");

        int TimesReceivedGrant = 0;
        if (chkAllTimers.Checked)
            TimesReceivedGrant += 1;

        if (chk1stTimers.Checked)
            TimesReceivedGrant += 2;

        if (chk2ndTimers.Checked)
            TimesReceivedGrant += 4;

        if (chk3rdTimers.Checked)
            TimesReceivedGrant += 8;

        return CamperApplicationChangeHistoryBL.GetSummaryByProgramReports(campYearID_string.ToString(), new DateTime(year2008or2009, cutoffDate.Month, cutoffDate.Day), FedID_List, StatusName_List.ToString(), TimesReceivedGrant, feds, stati, yearList);
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
    }

    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        divMenu.Visible = true;
        divReport.Visible = false;
    }

    protected void btnExcelExport_Click(object sender, EventArgs e)
    {
        GenerateExcelReport();
    }

    private void GenerateExcelReport()
    {
        // Data Content of report
        DateTime cutoffDate = GetCutoffDate();
        List<int> yearList = GetYearList();
        DataSet dsNormalized = GenerateDataTables(cutoffDate);

        string templateFile = Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
        string workFileDir = Server.MapPath(@"~/Docs");

        // Make a excel report
        ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
        ExcelFile excel = new ExcelFile();
        excel.LoadXls(templateFile);

        ExcelWorksheet ws = excel.Worksheets["Sheet1"];

        //We start at first row, because for ExcelLite control, the header row is not included
        int BEGIN_COLUMN_INDEX = 0;
        const int CAMP_NAME_MERGED_CELL_NUMBER = 6;
        const int REPORT_HEADER_CELL_NUMBER = 6;
        const int REPORT_SUB_HEADER_CELL_NUMBER = 4;

        int iRow = 1;

        // Global artistic setting

        // first column
        ws.Columns[0].Width = 40 * 250;

        // Create Report Header
        CellStyle styleReportHeader = new CellStyle();
        styleReportHeader.Font.Color = Color.Blue;
        styleReportHeader.Font.Size = 22 * 20;
        styleReportHeader.Font.Weight = ExcelFont.BoldWeight;

        CellRange ReportHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_HEADER_CELL_NUMBER);
        ReportHeader.Merged = true;
        ReportHeader.Style = styleReportHeader;
        ReportHeader.Value = "Summary by Program";

        iRow += 1;

        // Create Report SubHeader - usually it's camp year
        CellStyle styleSubHeader = new CellStyle();
        styleSubHeader.Font.Size = 16 * 20;
        styleSubHeader.Font.Weight = ExcelFont.BoldWeight;
        styleSubHeader.Font.Color = Color.Red;

        CellRange SubHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
        SubHeader.Merged = true;
        SubHeader.Value = cutoffDate.ToString(" MMM d, yyyy");
        SubHeader.Style = styleSubHeader;

        iRow += 2;

        CellStyle cs = new CellStyle();
        cs.Font.Size = 18 * 20;
        cs.Font.Weight = ExcelFont.BoldWeight;

        CellRange cr = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
        cr.Merged = true;
        cr.Value = "";
        cr.Style = cs;

        ws.Rows[iRow].Height = 25 * 20;

        iRow += 3;

        int iColumn = 0;
        int sameYearCount = 0;
        if (chkAllTimers.Checked)
            sameYearCount += 1;

        if (chk1stTimers.Checked)
            sameYearCount += 1;

        if (chk2ndTimers.Checked)
            sameYearCount += 1;

        if (chk3rdTimers.Checked)
            sameYearCount += 1;

        int sameYearOffset = 1;
        bool tempFlag = true;
        foreach (DataTable dt in dsNormalized.Tables)
        {
            CellStyle styleTableHeader = new CellStyle();
            styleTableHeader.Font.Size = 16 * 16;
            styleTableHeader.Font.Weight = ExcelFont.BoldWeight;

            string originalName = dt.TableName;
            int year = ChangeTableNameForExcelReport(dt);

            // only the first table shows the program name, the rest on the same row will have no first column
            int currentColumn = iColumn;
            if (sameYearOffset != 1)
            {
                dt.Columns.Remove("Name");
            }
            else
            {
                currentColumn = iColumn + 1;

                // This is special code for if select only current year, then the tables layout are vertical, and just need to show year at once
                if (yearList.Count > 0 || tempFlag)
                {
                    tempFlag = false;
                    CellRange TableHeaderYear = ws.Cells.GetSubrangeAbsolute(iRow - 2, currentColumn, iRow - 2, currentColumn);
                    TableHeaderYear.Value = year.ToString();
                    styleTableHeader.Font.Color = Color.Black;
                    TableHeaderYear.Style = styleTableHeader;
                }
            }

            styleTableHeader.Font.Color = Color.Red;
            CellRange TableHeader = ws.Cells.GetSubrangeAbsolute(iRow - 1, currentColumn, iRow - 1, currentColumn);
            TableHeader.Style = styleTableHeader;            

            TableHeader.Value = dt.TableName;
            dt.TableName = originalName;



            // this creats the real table
            ws.InsertDataTable(dt, iRow, iColumn, true);

            CellStyle tableHeaderStyle = new CellStyle();
            tableHeaderStyle.Font.Weight = ExcelFont.BoldWeight;
            tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);

            CellStyle tableHeaderStyle2 = new CellStyle();
            tableHeaderStyle2.Font.Weight = ExcelFont.BoldWeight;
            tableHeaderStyle2.FillPattern.SetSolid(Color.Orange);

            //Bold Style
            CellStyle boldStyle = new CellStyle();
            boldStyle.Font.Weight = ExcelFont.BoldWeight;

            // adjust each column's width and header style
            for (int i = iColumn; i <= (dt.Columns.Count + iColumn - 1); i++)
            {
                // make sure total row is bold
                ws.Cells[iRow + dt.Rows.Count, i].Style = boldStyle;

                // make column wide enough to see the column header
                if (iColumn == 0)
                {
                    // first table has program name, so has different i offset
                    if (i == 0 + iColumn)
                        ws.Columns[i].Width = 35 * 256;
                    else if (i == 1 + iColumn)
                        ws.Columns[i].Width = 10 * 256;
                    else if (i == 2 + iColumn)
                        ws.Columns[i].Width = 30 * 256;
                    else if (i == 4 + iColumn)
                        ws.Columns[i].Width = 22 * 256;
                    else
                        ws.Columns[i].Width = 17 * 256;

                    if (i == iColumn + 1)
                    {
                        // but Seth wants total column to be color of pink
                        ws.Cells[iRow, i].Style = tableHeaderStyle2;
                    }
                    else
                    {
                        // make sure header row has bgcolor and is bold
                        ws.Cells[iRow, i].Style = tableHeaderStyle;
                    }
                }
                else
                {
                    if (i == iColumn)
                        ws.Columns[i].Width = 10 * 256;
                    else if (i == 1 + iColumn)
                        ws.Columns[i].Width = 30 * 256;
                    else if (i == 3 + iColumn)
                        ws.Columns[i].Width = 22 * 256;
                    else
                        ws.Columns[i].Width = 17 * 256;

                    if (iColumn == i)
                    {
                        // but Seth wants total column to be color of pink
                        ws.Cells[iRow, i].Style = tableHeaderStyle2;
                    }
                    else
                    {
                        // make sure header row has bgcolor and is bold
                        ws.Cells[iRow, i].Style = tableHeaderStyle;
                    }
                }

            }

            // Make the same year tables on the same row
            if (sameYearOffset < sameYearCount && yearList.Count > 0)
            {
                iColumn += dt.Columns.Count + 1;
                sameYearOffset += 1;
            }
            else
            {
                iRow += dt.Rows.Count + 4;
                iColumn = 0;
                sameYearOffset = 1;
            }
        }

        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

        // Save to a file on the local file system
        string filename = String.Format("\\{0}{1}{2}{3}CamperCountByProgramAndStatus.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
        string newFile = workFileDir + filename;
        excel.SaveXls(newFile);


        string[] strFileParts = newFile.Split(new string[] { "\\" }, StringSplitOptions.None);

        //Display excel spreadsheet
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileParts[strFileParts.Length - 1]);
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "";

        if (newFile.Length == 0)
            Response.Write("Error encountered - no spreadsheet to display");
        else
            Response.WriteFile(newFile);

        Response.End();
    }

    private int ChangeTableNameForExcelReport(DataTable dt)
    {
        int year = (int)Application["CampYear"];
        if (dt.TableName == SummaryByProgramTables.CURRENT_YEAR_All.ToString())
        {
            dt.TableName = "All Campers";
        }
        else if (dt.TableName == SummaryByProgramTables.CURRENT_YEAR_3rd.ToString())
        {
            dt.TableName = "3rd year campers";
        }
        else if (dt.TableName == SummaryByProgramTables.CURRENT_YEAR_2nd.ToString())
        {
            dt.TableName = "2nd year campers";
        }
        else if (dt.TableName == SummaryByProgramTables.CURRENT_YEAR_1st.ToString())
        {
            dt.TableName = "1st year campers";
        }
        else if (dt.TableName == SummaryByProgramTables.Y2012_All.ToString())
        {
            dt.TableName = "All Campers";
            year = 2012;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2012_3rd.ToString())
        {
            dt.TableName = "3rd year campers";
            year = 2012;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2012_2nd.ToString())
        {
            dt.TableName = "2nd year campers";
            year = 2012;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2012_1st.ToString())
        {
            dt.TableName = "1st year campers";
            year = 2012;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2011_All.ToString())
        {
            dt.TableName = "All Campers";
            year = 2011;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2011_3rd.ToString())
        {
            dt.TableName = "3rd year campers";
            year = 2011;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2011_2nd.ToString())
        {
            dt.TableName = "2nd year campers";
            year = 2011;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2011_1st.ToString())
        {
            dt.TableName = "1st year campers";
            year = 2011;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2010_All.ToString())
        {
            dt.TableName = "All Campers";
            year = 2010;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2010_3rd.ToString())
        {
            dt.TableName = "3rd year campers";
            year = 2010;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2010_2nd.ToString())
        {
            dt.TableName = "2nd year campers";
            year = 2010;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2010_1st.ToString())
        {
            dt.TableName = "1st year campers";
            year = 2010;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2009_All.ToString())
        {
            dt.TableName = "All Campers";
            year = 2009;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2009_3rd.ToString())
        {
            dt.TableName = "3rd year campers";
            year = 2009;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2009_2nd.ToString())
        {
            dt.TableName = "2nd year campers";
            year = 2009;
        }
        else if (dt.TableName == SummaryByProgramTables.Y2009_1st.ToString())
        {
            dt.TableName = "1st year campers";
            year = 2009;
        }
        return year;
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
    
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DecorateDaySchoolCells(true, e);
    }

    protected void gvCurrentYear_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DecorateDaySchoolCells(false, e);
    }

    private void DecorateDaySchoolCells(bool isPast, GridViewRowEventArgs e)
    {
        // Set the border of School Columns
        int index = e.Row.Cells.Count;
        e.Row.Cells[index - 4].Attributes["style"] = "border-left:3px solid black";
        e.Row.Cells[index - 1].Attributes["style"] = "border-left:3px solid black";
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    //e.Row.Cells[index - 1].Attributes["style"] = "border-right:3px solid black;border-top:3px solid black";
        //    //e.Row.Cells[index - 2].Attributes["style"] = "border-left:3px solid black; border-top:3px solid black";
        //    e.Row.Cells[index - 3].Attributes["style"] = "border-left:3px solid black";
        //}
        //else if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[index - 3].Attributes["style"] = "border-left:3px solid black";
        //    //if (isPast)
        //    //{
        //    //    foreach (TableCell cell in e.Row.Cells)
        //    //    {
        //    //        cell.Attributes["style"] = "background-color: #80a4e6; filter:alpha(opacity=50); opacity: 0.5; -moz-opacity:0.50;";
        //    //    }

        //    //    e.Row.Cells[0].Attributes["style"] = "";
        //    //}

        //    //if (e.Row.Cells[0].Text == "Total By Status")
        //    //{
        //    //    e.Row.Cells[index - 1].Attributes["style"] = "border-right:3px solid black; border-bottom:3px solid black;";
        //    //    e.Row.Cells[index - 2].Attributes["style"] = "border-left:3px solid black; border-bottom:3px solid black;";
        //    //}
        //    //else
        //    //{
        //    //    e.Row.Cells[index - 1].Attributes["style"] = "border-right:3px solid black;";
        //    //    e.Row.Cells[index - 2].Attributes["style"] = "border-left:3px solid black;";
        //    //}
        //}
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

    protected void gv_DataBound(object sender, EventArgs e)
    {
        // make the total row bold
        GridView gv = (GridView)sender;
        gv.Rows[gv.Rows.Count - 1].Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");
    }
}