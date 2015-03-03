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
    Role UserRole;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserRole = (Role)Convert.ToInt32(Session["RoleID"]);

        lblMsg.Text = "";
        chkAllCamps.Enabled = true;
        //btnReport123.Enabled = true;

        if (!IsPostBack)
        {
            using (var ctx = new CIPMSEntities1())
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

    protected void btnRunPayment_Click(object sender, EventArgs e)
    {
        bool e_flag = true;
        var campIdList = new List<int>();
        var campIds = "";

        foreach (ListItem li in chklistCamp.Items)
        {
            if (li.Selected)
            {
                e_flag = false;
                campIdList.Add(Int32.Parse(li.Value));

                if (campIds == "")
                    campIds = li.Value;
                else
                    campIds += ", " + li.Value;
            }
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one camp";
            return;
        }

        GenerateExcelReport(campIdList, campIds);
    }

    protected void btnReversePayment_Click(object sender, EventArgs e)
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

        var param = new ReportParamCampersFJC()
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

        param.BuildStrings();

        Session.Add("ReportParamCampersFJC", param);

        Response.Redirect("ReportFJCCAmpers.aspx");
    }

    private void GenerateExcelReport(IEnumerable<int> campIdList, string campIds)
    {
        var templateFile = Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
        var workFileDir = Server.MapPath(@"~/Docs");

        // Make a excel report
        ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
        var excel = new ExcelFile();
        excel.LoadXls(templateFile);


        // Data Content of report
        DataTable dtAllCamps = PaymentProcessingDAL.GetPreliminaryReport(Int32.Parse(ddlCampYear.SelectedValue), Int32.Parse(ddlFed.SelectedValue), campIds);

        foreach (var campId in campIdList)
        {
            IEnumerable<DataRow> enumByCamp = from camperApp in dtAllCamps.AsEnumerable()
                               where camperApp.Field<int>("CampID") == campId
                               select camperApp;

            if (enumByCamp.Any())
            {
                DataTable dt = enumByCamp.CopyToDataTable();

                var campName = dt.Rows[0]["CampName"].ToString();

                if (campName.Contains(":"))
                    campName = campName.Replace(":", "-");

                //We start at first row, because for ExcelLite control, the header row is not included
                const int BEGIN_COLUMN_INDEX = 0;
                const int REPORT_HEADER_CELL_NUMBER = 12;
                const int REPORT_SUB_HEADER_CELL_NUMBER = 8;
                const int CAMP_NAME_MERGED_CELL_NUMBER = 6;

                int iRow = 1;

                var ws = excel.Worksheets.Add(campName);

                // Global artistic setting
                ws.Columns[0].Width = 20 * 20; // make the first column smaller

                // Create Report Header
                var styleReportHeader = new CellStyle
                {
                    Font = {Color = Color.Blue, Size = 22*20, Weight = ExcelFont.BoldWeight}
                };

                CellRange reportHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_HEADER_CELL_NUMBER);
                reportHeader.Merged = true;
                reportHeader.Style = styleReportHeader;
                reportHeader.Value = "One Happy Camper Payment Report";

                ws.Rows[iRow].Height = 25 * 20;

                iRow += 1;

                // Create Report SubHeader - usually it's camp year and report generation time
                var styleReportSubHeader = new CellStyle {Font = {Size = 16*20, Weight = ExcelFont.BoldWeight}};

                CellRange subHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
                subHeader.Merged = true;
                subHeader.Style = styleReportSubHeader;
                subHeader.Value = string.Format("Camp Year: {0}  Generated on {1} {2}", ddlCampYear.SelectedItem.Text, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());

                // Create Federation Name row
                iRow += 2;

                var cs = new CellStyle {Font = {Size = 18*20, Weight = ExcelFont.BoldWeight}};
                CellRange fedNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
                fedNameRange.Merged = true;
                fedNameRange.Style = cs;
                fedNameRange.Value = ddlFed.SelectedItem.Text;
                    
                iRow += 4;

                CellRange campNameRange = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
                campNameRange.Merged = true;
                campNameRange.Style = cs;
                campNameRange.Value = campName;

                iRow += 1;

                // this creats the real table
                dt.Columns.Remove("CampID");
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
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 6].Value = "Total Dollars";
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 6].Style = boldTextStyle;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Value = totalGrantAmount;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Style = boldTextStyle;

                iRow += 1;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 6].Value = "# of campers";
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 6].Style = boldTextStyle;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Value = totalCamperCount;
                ws.Cells[iRow, BEGIN_COLUMN_INDEX + 7].Style = boldTextStyle;
             
            }
        }

        excel.Worksheets[0].Delete();
        excel.Worksheets[0].Delete();
        excel.Worksheets[0].Delete();
        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

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
            btnReport.Enabled = false;
            lblMsg.Text = "Currently you are not associated with any federations, so you won't see any data in this report";
        }
        else
        {
            chkAllCamps.Visible = true;
            btnReport.Enabled = true;
        }
    }

    protected void ddlFed_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkAllCamps.Checked = false;

        foreach (ListItem li in chklistCamp.Items)
        {
            li.Selected = false;
        }
    }
}