using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

using Model;
using GemBox.ExcelLite;

public partial class PaymentProcessing : System.Web.UI.Page
{
    private bool isFinal = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        lblMsg2.Text = "";

        if (!IsPostBack)
        {
            ResetWarning(false);
            using (var ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.SelectedValue = Application["CampYearID"].ToString();
                ddlCampYear.DataBind();
            }
        }
    }

    protected void btnRunPayment_Click(object sender, EventArgs e)
    {
        ResetWarning(false);
        RunMainProcess();
    }

    private void ResetWarning(bool turnOn)
    {
        if (turnOn)
        {
            divWarning.Visible = true;
            lblFinal.Visible = true;
        }
        else
        {
            divWarning.Visible = false;
            lblFinal.Visible = false;
            chkFinal.Checked = false;            
        }
    }

    protected void btnRunPayment2_Click(object sender, EventArgs e)
    {
        if (!chkFinal.Checked)
        {
            ResetWarning(true);
            return;
        }

        isFinal = true;

        RunMainProcess();
    }

    private void RunMainProcess()
    {
        bool e_flag = true;
        var campIdList = new List<int>();

        foreach (ListItem li in chklistCamp.Items)
        {
            if (li.Selected)
            {
                e_flag = false;
                campIdList.Add(Int32.Parse(li.Value));
            }
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one camp";
            lblMsg2.Text = "You must select at least one camp";
            return;
        }

        DataTable dtAllCamps = PaymentProcessingDAL.GetReport(Int32.Parse(ddlCampYear.SelectedValue), Int32.Parse(ddlFed.SelectedValue), campIdList, isFinal);

        if (dtAllCamps.Rows.Count > 0)
            GenerateExcelReport(dtAllCamps, campIdList);
        else
        {
            lblMsg.Text = "There are no campers qualified under the selection criteria.";
            lblMsg2.Text = "There are no campers qualified under the selection criteria.";
        }        
    }

    protected void btnReversePayment_Click(object sender, EventArgs e)
    {

    }

    private void GenerateExcelReport(DataTable dtAllCamps ,IList<int> campIdList)
    {
        var templateFile = Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
        var workFileDir = Server.MapPath(@"~/Docs");

        // Make a excel report
        ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
        var excel = new ExcelFile();
        excel.LoadXls(templateFile);

        var summaryTable = new DataTable();
        summaryTable.Columns.Add("CampName", typeof(string));
        summaryTable.Columns.Add("1st/2nd/3rd Time", typeof(int));
        summaryTable.Columns.Add("Day School", typeof(int));
        summaryTable.Columns.Add("Total Dollars", typeof(double));


        foreach (var campId in campIdList)
        {
            IEnumerable<DataRow> enumByCamp = from camperApp in dtAllCamps.AsEnumerable()
                               where camperApp.Field<int>("CampID") == campId
                               select camperApp;

            if (enumByCamp.Any())
            {
                var sum = enumByCamp.Sum(x => x.Field<double>("GrantAmount"));
                var countDS = (from camperApp in enumByCamp
                               where camperApp.Field<string>("Day School") == "Y"
                               select camperApp).Count();
                var countAll = enumByCamp.Count();
 
                DataTable dt = enumByCamp.CopyToDataTable();

                var campName = dt.Rows[0]["CampName"].ToString();

                var dr = summaryTable.NewRow();
                dr["CampName"] = campName;
                dr["1st/2nd/3rd Time"] = countAll;
                dr["Day School"] = countDS;
                dr["Total Dollars"] = sum;
                summaryTable.Rows.Add(dr);

                if (campName.Contains(":"))
                    campName = campName.Replace(":", "-");

                //We start at first row, because for ExcelLite control, the header row is not included
                const int BEGIN_COLUMN_INDEX = 0;
                const int REPORT_HEADER_CELL_NUMBER = 5;
                const int REPORT_SUB_HEADER_CELL_NUMBER = 5;
                const int CAMP_NAME_MERGED_CELL_NUMBER = 5;

                int iRow = 1;

                var ws = excel.Worksheets.Add(campName);

                // Global artistic setting
                ws.Columns[0].Width = 20*20; // make the first column smaller

                // Create Report Header
                var styleReportHeader = new CellStyle
                {
                    Font = {Color = Color.Blue, Size = 22*20, Weight = ExcelFont.BoldWeight}
                };

                CellRange reportHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_HEADER_CELL_NUMBER);
                reportHeader.Merged = true;
                reportHeader.Style = styleReportHeader;
                reportHeader.Value = "One Happy Camper Payment Report";

                ws.Rows[iRow].Height = 25*20;

                iRow += 1;

                // Create Report SubHeader - usually it's camp year and report generation time
                var styleReportSubHeader = new CellStyle {Font = {Size = 16*20, Weight = ExcelFont.BoldWeight}};

                CellRange subHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow,
                    REPORT_SUB_HEADER_CELL_NUMBER);
                subHeader.Merged = true;
                subHeader.Style = styleReportSubHeader;
                subHeader.Value = string.Format("Camp Year: {0}  Generated on {1} {2}", ddlCampYear.SelectedItem.Text,
                    DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());

                // Create Federation Name row
                iRow += 2;

                var cs = new CellStyle {Font = {Size = 18*20, Weight = ExcelFont.BoldWeight}};
                CellRange fedNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow,
                    CAMP_NAME_MERGED_CELL_NUMBER);
                fedNameRange.Merged = true;
                fedNameRange.Style = cs;
                fedNameRange.Value = ddlFed.SelectedItem.Text;

                // admin name
                iRow += 1;
                //var cs = new CellStyle { Font = { Size = 18 * 20, Weight = ExcelFont.BoldWeight } };
                CellRange adminNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow,
                    CAMP_NAME_MERGED_CELL_NUMBER);
                adminNameRange.Merged = true;
                //fedNameRange.Style = cs;
                adminNameRange.Value = String.Format("Report Generated by: {0} {1}", Session["FirstName"],
                    Session["LastName"]);

                // report type name
                iRow += 2;
                var reportTypeStyle = new CellStyle {Font = {Size = 10*20, Weight = ExcelFont.BoldWeight}};
                var reportTypeText = "";
                if (isFinal)
                {
                    reportTypeStyle.FillPattern.SetSolid(Color.ForestGreen);
                    reportTypeStyle.Font.Color = Color.White;
                    reportTypeText = "FINAL REPORT.  ALL RECORDS HAVE BEEN UPDATED TO PAYMENT REQUESTED.";
                }
                else
                {
                    reportTypeStyle.FillPattern.SetSolid(Color.Yellow);
                    reportTypeStyle.Font.Color = Color.Red;
                    reportTypeText = "IMPORTANT:  This is a Preliminary Report.  Once data confirmed, run a FINAL report.";
                }

                CellRange reportTypeNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
                reportTypeNameRange.Merged = true;
                reportTypeNameRange.Style = reportTypeStyle;
                reportTypeNameRange.Value = reportTypeText;
                    
                iRow += 1;

                CellRange campNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
                campNameRange.Merged = true;
                campNameRange.Style = cs;
                campNameRange.Value = campName;

                iRow += 1;

                // this creats the real table
                dt.Columns.Remove("CampID");
                dt.Columns.Remove("CampName");
                dt.Columns.Remove("StatusID");
                ws.InsertDataTable(dt, iRow, BEGIN_COLUMN_INDEX, true);

                // decorate the header of content table
                // loop through each column and 1.set the width of each colum, 2. set the header style of each column
                var tableHeaderStyle = new CellStyle {Font = {Weight = ExcelFont.BoldWeight}};
                tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);

                for (int i = BEGIN_COLUMN_INDEX; i < dt.Columns.Count; i++)
                {
                    ws.Cells[iRow, i].Style = tableHeaderStyle;

                    if (i == 3)
                        ws.Columns[i].Width = 30 * 256;
                    else if (i == 4)
                        ws.Columns[i].Width = 8 * 256;
                    else if (i == 5)
                        ws.Columns[i].Width = 28 * 256;
                    else if (i == 7)
                        ws.Columns[i].Width = 16 * 256;
                    else
                        ws.Columns[i].Width = 18 * 256;

                }

                var totalGrantAmount = dt.AsEnumerable().Sum(x => x.Field<double>("GrantAmount"));

                var totalCamperCount = dt.Rows.Count;

                iRow += dt.Rows.Count + 2;
                var boldTextStyle = new CellStyle { Font = { Weight = ExcelFont.BoldWeight } };
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Value = "Total Dollars";
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Style = boldTextStyle;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 8].Value = totalGrantAmount;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 8].Style = boldTextStyle;

                iRow += 1;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Value = "# of campers";
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Style = boldTextStyle;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 8].Value = totalCamperCount;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 8].Style = boldTextStyle;
             
            }
        }

        if (dtAllCamps.Rows.Count > 0)
        {
            // ******** Summary worksheet ***************
            IEnumerable<DataRow> enumSummary = from rows in summaryTable.AsEnumerable()
                                              select rows;

            var sumTimer = enumSummary.Sum(x => x.Field<int>("1st/2nd/3rd Time"));
            var sumDS = enumSummary.Sum(x => x.Field<int>("Day School"));
            var sumTotal = enumSummary.Sum(x => x.Field<double>("Total Dollars"));

            var dr = summaryTable.NewRow();
            dr["CampName"] = "TOTAL";
            dr["1st/2nd/3rd Time"] = sumTimer;
            dr["Day School"] = sumDS;
            dr["Total Dollars"] = sumTotal;
            summaryTable.Rows.Add(dr);            

            const int BEGIN_COLUMN_INDEX = 0;
            const int REPORT_HEADER_CELL_NUMBER = 5;
            const int REPORT_SUB_HEADER_CELL_NUMBER = 5;
            const int CAMP_NAME_MERGED_CELL_NUMBER = 5;

            int iRow = 1;

            var ws = excel.Worksheets.Add("Summary");

            // Global artistic setting
            ws.Columns[0].Width = 20 * 20; // make the first column smaller

            // Create Report Header
            var styleReportHeader = new CellStyle
            {
                Font = { Color = Color.Blue, Size = 22 * 20, Weight = ExcelFont.BoldWeight }
            };

            CellRange reportHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow,
                REPORT_HEADER_CELL_NUMBER);
            reportHeader.Merged = true;
            reportHeader.Style = styleReportHeader;
            reportHeader.Value = "One Happy Camper Payment Report";

            ws.Rows[iRow].Height = 25 * 20;

            iRow += 1;

            // Create Report SubHeader - usually it's camp year and report generation time
            var styleReportSubHeader = new CellStyle { Font = { Size = 16 * 20, Weight = ExcelFont.BoldWeight } };

            CellRange subHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow,
                REPORT_SUB_HEADER_CELL_NUMBER);
            subHeader.Merged = true;
            subHeader.Style = styleReportSubHeader;
            subHeader.Value = string.Format("Camp Year: {0}  Generated on {1} {2}", ddlCampYear.SelectedItem.Text,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());

            // Create Federation Name row
            iRow += 2;

            var cs = new CellStyle { Font = { Size = 18 * 20, Weight = ExcelFont.BoldWeight } };
            CellRange fedNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow,
                CAMP_NAME_MERGED_CELL_NUMBER);
            fedNameRange.Merged = true;
            fedNameRange.Style = cs;
            fedNameRange.Value = ddlFed.SelectedItem.Text;

            // admin name
            iRow += 1;
            //var cs = new CellStyle { Font = { Size = 18 * 20, Weight = ExcelFont.BoldWeight } };
            CellRange adminNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow,
                CAMP_NAME_MERGED_CELL_NUMBER);
            adminNameRange.Merged = true;
            //fedNameRange.Style = cs;
            adminNameRange.Value = String.Format("Report Generated by: {0} {1}", Session["FirstName"],
                Session["LastName"]);

            // report type name
            iRow += 2;
            var reportTypeStyle = new CellStyle { Font = { Size = 10 * 20, Weight = ExcelFont.BoldWeight } };
            var reportTypeText = "";
            if (isFinal)
            {

                reportTypeStyle.FillPattern.SetSolid(Color.ForestGreen);
                reportTypeStyle.Font.Color = Color.White;
                reportTypeText = "FINAL REPORT.  ALL RECORDS HAVE BEEN UPDATED TO PAYMENT REQUESTED.";
            }
            else
            {
                reportTypeStyle.FillPattern.SetSolid(Color.Yellow);
                reportTypeStyle.Font.Color = Color.Red;
                reportTypeText = "IMPORTANT:  This is a Preliminary Report.  Once data confirmed, run a FINAL report.";
            }

            CellRange reportTypeNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
            reportTypeNameRange.Merged = true;
            reportTypeNameRange.Style = reportTypeStyle;
            reportTypeNameRange.Value = reportTypeText;

            iRow += 1;

            // this creats the real table
            ws.InsertDataTable(summaryTable, iRow, BEGIN_COLUMN_INDEX, true);

            // decorate the header of content table
            // loop through each column and 1.set the width of each colum, 2. set the header style of each column
            var tableHeaderStyle = new CellStyle { Font = { Weight = ExcelFont.BoldWeight }};
            tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);
            var boldTextStyle = new CellStyle { Font = { Weight = ExcelFont.BoldWeight } };

            for (int i = BEGIN_COLUMN_INDEX; i < summaryTable.Columns.Count; i++)
            {
                ws.Cells[iRow, i].Style = tableHeaderStyle;
                ws.Cells[iRow + summaryTable.Rows.Count, i].Style = boldTextStyle;

                if (i == BEGIN_COLUMN_INDEX)
                    ws.Columns[i].Width = 30 * 256;
                else if (i == BEGIN_COLUMN_INDEX + 3)
                    ws.Columns[i].Width = 20 * 256;
                else
                    ws.Columns[i].Width = 18 * 256;

            }

            // ******** End of Summary worksheet ********

            excel.Worksheets[0].Delete();
            excel.Worksheets[0].Delete();
            excel.Worksheets[0].Delete();
            excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];            
        }


        // Save to a file on the local file system
        string filename = String.Format("\\{0}{1}{2}{3}-Payment.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
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

    protected void chklistCamp_DataBound(object sender, EventArgs e)
    {
        if (chklistCamp.Items.Count == 0)
        {
            if (lblMsg.Text == "")
            {
                lblMsg.Text = "The federation has no camp in the camper applications data";
            }

            if (lblMsg2.Text == "")
            {
                lblMsg2.Text = "The federation has no camp in the camper applications data";
            }

            chkAllCamps.Enabled = false;
            btnRun.Enabled = false;
            btnRun2.Enabled = false;
        }
        else
        {
            lblMsg.Text = "";
            lblMsg2.Text = "";
            chkAllCamps.Enabled = true;
            btnRun.Enabled = true;
            btnRun2.Enabled = true;
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

    protected void ddlFed_DataBound(object sender, EventArgs e)
    {
        if (ddlFed.Items.Count == 0)
        {
            chkAllCamps.Visible = false;
            btnRun.Enabled = false;
            btnRun2.Enabled = false;
            lblMsg.Text = "Currently you are not associated with any federations, so you won't see any data in this report";
            lblMsg2.Text = "Currently you are not associated with any federations, so you won't see any data in this report";
        }
        else
        {
            chkAllCamps.Visible = true;
            btnRun.Enabled = true;
            btnRun2.Enabled = true;
            FillPaymentSummaryData();
        }
    }

    protected void ddlFed_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkAllCamps.Checked = false;

        foreach (ListItem li in chklistCamp.Items)
        {
            li.Selected = false;
        }
        FillPaymentSummaryData();
    }

    private void FillPaymentSummaryData()
    {
        DataTable dt = PaymentProcessingDAL.GetSummary(Int32.Parse(ddlCampYear.SelectedValue), Int32.Parse(ddlFed.SelectedValue));
        lblTotalCampersReady.Text = dt.Rows[0]["Count14"].ToString();
        //lblTotalCampsReady.Text = dt.Rows[0]["Camp14"].ToString();
        lblTotalCampersDone.Text = dt.Rows[0]["Count25"].ToString();
       // lblTotalCampsDone.Text = dt.Rows[0]["Camp25"].ToString();        
    }
    protected void rdoReversePayment_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoReversePayment.Checked)
        {
            pnlPaymentRun.Visible = false;
            pnlReversePayment.Visible = true;
            btnRun.Visible = false;
            btnRun2.Visible = false;
            btnReverse.Visible = true;
        }
        else
        {
            pnlPaymentRun.Visible = true;
            pnlReversePayment.Visible = false;
            btnRun.Visible = true;
            btnRun2.Visible = true;
            btnReverse.Visible = false;
        }

    }
}