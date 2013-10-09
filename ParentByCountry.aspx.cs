using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemBox.ExcelLite;
using System.Data;
using System.Drawing;

using Model;

public partial class ParentByCountry : System.Web.UI.Page
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
                chkByProgram.Visible = false;
            }

            using (CIPMSEntities1 ctx = new CIPMSEntities1())
            {
                ddlCampYear.DataSource = ctx.tblCampYears.Select(x => new { id = x.ID, text = x.CampYear });
                ddlCampYear.SelectedValue = Application["CampYearID"].ToString();
                ddlCampYear.DataBind();
            }

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    chklistCountryPopular.Items[i].Attributes.Add("title", "Russia, Belarus, Estonia, Kazakhstan, Lithuania, Ukraine, Armenia, Azerbaijan, Georgia, Latvia, Moldova, Uzbekistan, Kyrgyzstan, Turkmenistan, Tajikistan");
                    chklistCountryPopular.Items[i].Attributes.Add("id", "pretty");
                }
                else
                {
                    chklistCountryPopular.Items[i].Attributes.Add("title", "Mexico, Belize, Costa Rica, El Salvador, Guatemala, Honduras, Panama, Argentina, Bolivia, Brazil, Chile, Colombia, Ecuador, French Guiana, Guyana, Paraguay, Peru, Suriname, Uruguay, Venezuela, Falkland Islands");
                    chklistCountryPopular.Items[i].Attributes.Add("id", "pretty2");

                }
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

        e_flag = true;
        if (rdoGroupCountry.Checked)
        {
            foreach (ListItem li in chklistCountryPopular.Items)
            {
                if (li.Selected)
                {
                    e_flag = false;
                    break;
                }
            }            
        }
        else
        {
            foreach (ListItem li in chklistCountry.Items)
            {
                if (li.Selected)
                {
                    e_flag = false;
                    break;
                }
            } 
        }

        if (e_flag)
        {
            lblMsg.Text = "You must select at least one country";
            return;
        }

        if (!chkByCamp.Checked)
            if (!chkByProgram.Checked)
            {
                lblMsg.Text = "You must select at least one status";
                return;            
            }

        DataSet[] dsArray = GenerateDataSet();

        DataSet ds = dsArray[0];
        DataSet ds2 = dsArray[1];

        if (chkByProgram.Checked)
        {
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

            divRep1.Visible = true;
        }
        else
            divRep1.Visible = false;

        if (chkByCamp.Checked)
        {
            if (chkAllTimers.Checked)
            {
                gvAllTimers2.DataSource = ds2.Tables["AllCampers"];
                gvAllTimers2.DataBind();
                divAllTimers2.Visible = true;
            }
            else
            {
                divAllTimers2.Visible = false;
            }

            if (chk1stTimers.Checked)
            {
                gv1stTimers2.DataSource = ds2.Tables["1stYearCampers"];
                gv1stTimers2.DataBind();
                div1stTimers2.Visible = true;
            }
            else
            {
                div1stTimers2.Visible = false;
            }

            if (chk2ndTimers.Checked)
            {
                gv2ndTimers2.DataSource = ds2.Tables["2ndYearCampers"];
                gv2ndTimers2.DataBind();
                div2ndTimers2.Visible = true;
            }
            else
            {
                div2ndTimers2.Visible = false;
            }

            if (chk3rdTimers.Checked)
            {
                if (ds2.Tables["3rdYearCampers"].Rows.Count > 0)
                {
                    gv3rdTimers2.DataSource = ds2.Tables["3rdYearCampers"];
                    gv3rdTimers2.DataBind();
                    div3rdTimers2.Visible = true;
                }
                else
                    div3rdTimers2.Visible = false;
            }
            else
            {
                div3rdTimers2.Visible = false;
            }

            divRep2.Visible = true;
        }
        else
            divRep2.Visible = false;

        divMenu.Visible = false;
        divReport.Visible = true;

        //gv.DataSource = GenerateDataTable(param);
        //gv.DataBind();
    }

    private DataSet[] GenerateDataSet()
    {
        string FedID_List = "";
        foreach (ListItem li in chklistFed.Items)
        {
            if (li.Selected)
            {
                if (FedID_List == "")
                    FedID_List = li.Value;
                else
                    FedID_List += "," + li.Value;
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
                    CampID_List += "," + li.Value;
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
                    StatusID_List += String.Format(",{0}", li.Value.Trim());
            }
        }

        string CountryID_List = "";
        if (rdoIndividualCountry.Checked)
        {
            foreach (ListItem li in chklistCountry.Items)
            {
                if (li.Selected)
                {
                    if (CountryID_List == "")
                        CountryID_List = String.Format("{0}", li.Value.Trim());
                    else
                        CountryID_List += String.Format(",{0}", li.Value.Trim());
                }
            }        
        }
        else
        {
            // select group countries
            foreach (ListItem li in chklistCountryPopular.Items)
            {
                if (li.Selected)
                {
                    if (li.Value.Trim() == "1000")
                        CountryID_List = "12,16,21,65,75,105,109,111,117,134,173,206,216,220,224";
                    else if (li.Value.Trim() == "1001")
                    {
                        if (CountryID_List == "")
                            CountryID_List = "11,23,27,31,42,46,51,60,62,67,72,84,87,90,132,160,162,163,199,223,226";
                        else
                            CountryID_List += ",11,23,27,31,42,46,51,60,62,67,72,84,87,90,132,160,162,163,199,223,226";
                    }
                    else if (li.Value.Trim() == "1002")
                    {
                        CountryID_List = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232";
                    }
                    else if (li.Value.Trim() == "1003")
                    {
                        CountryID_List = "3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,232";
                    }
                    else
                    {
                        if (CountryID_List == "")
                            CountryID_List = String.Format("{0}", li.Value.Trim());
                        else
                            CountryID_List += String.Format(",{0}", li.Value.Trim());
                    }
                }
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
        
        return CamperAnswerBL.GetParentCountryOfOrigin(Int32.Parse(ddlCampYear.SelectedValue), FedID_List, CampID_List, StatusID_List, CountryID_List, TimesReceivedGrant);
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
            if (IsPostBack)
                if (chklistFed.SelectedIndex != -1)
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

    protected void btnDataExtract_Click(object sender, EventArgs e)
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
        const int BEGIN_COLUMN_INDEX = 1;
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
        ReportHeader.Value = "Parent Country of Origin (Online Data Only)";

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

        iRow += 2;

        CellStyle styleReportSubHeaderNote = new CellStyle();
        //styleReportSubHeader.Font.Size = 16 * 20;
        //styleReportSubHeader.Font.Weight = ExcelFont.BoldWeight;
        //styleReportSubHeaderNote.

        CellRange SubHeaderNote = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, 11);
        SubHeaderNote.Merged = true;
        //SubHeaderNote.Style = styleReportSubHeaderNote;
        SubHeaderNote.Value = "This report offers two different pieces of data.  “Number Of Campers” who have at least one parent from the country(s) selected and the list of countries with the number of corresponding parents.  Note: the";
        //ws.Rows[iRow].Height = 25 * 20;

        iRow += 1;

        CellRange SubHeaderNote2 = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, 11);
        SubHeaderNote2.Merged = true;
        //SubHeaderNote.Style = styleReportSubHeaderNote;
        SubHeaderNote2.Value = "number of campers will typically be less than the total number of parents in this report.";
        //ws.Rows[iRow].Height = 25 * 20;


        iRow += 2;

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
        DataSet[] dsArray = GenerateDataSet();

        //DataSet dsModified = dsArray[0];
        //DataSet ds2 = dsArray[1];

        int indexDataSet = 0; // serve to see which dataset we are running (either ByProgram or ByCamp
        bool isRun = false;
        string EntityName = "", ReportName = "";
        foreach (DataSet dsModified in dsArray)
        {
            isRun = false;

            if (chkByProgram.Checked && indexDataSet == 0)
            {
                EntityName = "Program";
                ReportName = "By Program, By Parent Birth Country";
                isRun = true;
            }

            if (chkByCamp.Checked && indexDataSet == 1)
            {
                EntityName = "CampName";
                ReportName = "By Camp, By Parent Birth Country";
                isRun = true;
            }

            if (isRun)
            {
                // Get rid of first columns for other tables except first one
                for (int i = 1; i < dsModified.Tables.Count; i++)
                    dsModified.Tables[i].Columns.Remove(EntityName);

                int table_column_count = dsModified.Tables[0].Columns.Count;
                int times = 0, current_starting_column = 0;

                // Table Title
                CellStyle styleSubReportTitle = new CellStyle { HorizontalAlignment = HorizontalAlignmentStyle.Center };
                styleSubReportTitle.Font.Size = 16 * 20;
                styleSubReportTitle.FillPattern.SetSolid(Color.Transparent);
                styleSubReportTitle.Font.Weight = ExcelFont.BoldWeight;

                CellRange SubReportTitle = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
                SubReportTitle.Merged = true;
                SubReportTitle.Value = ReportName;
                SubReportTitle.Style = styleSubReportTitle;

                iRow += 3;

                foreach (DataTable dt in dsModified.Tables)
                {
                    if (times == 1)
                    {
                        // Get the header location
                        current_starting_column = BEGIN_COLUMN_INDEX + 1 + times * table_column_count;
                    }
                    else
                    {
                        // Get the header location
                        current_starting_column = BEGIN_COLUMN_INDEX + times * table_column_count;
                    }

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

                iRow += dsModified.Tables[0].Rows.Count + 5;
            }
            indexDataSet += 1;
        }




        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

        // Save to a file on the local file system
        string filename = String.Format("\\{0}{1}{2}{3}ParentCountryOfOrigin.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
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

    protected void rdoGroupCountry_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoGroupCountry.Checked)
        {
            chklistCountryPopular.Visible = true;
            chklistCountry.Visible = false;
        }           
    }

    protected void rdoIndividualCountry_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoIndividualCountry.Checked)
        {
            chklistCountry.Visible = true;
            chklistCountryPopular.Visible = false;
        }
    }
}