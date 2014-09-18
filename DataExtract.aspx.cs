using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GemBox.ExcelLite;
using System.Data;
using System.Drawing;

using Model;

public partial class DataExtract : System.Web.UI.Page
{
	Role UserRole;

	protected void Page_Load(object sender, EventArgs e)
	{
		UserRole = (Role)Convert.ToInt32(Session["RoleID"]);
		lblMsg.Text = "";
		chkAllCamps.Visible = true;

		if (UserRole == Role.CampDirector || UserRole == Role.FederationAdmin)
		{
			chkAllFeds.Visible = false;
		}

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
		bool e_flag = true;
		foreach (ListItem li in chklistFed.Items)
			if (li.Selected)
			{
				e_flag = false;
				break;
			}

		if (e_flag && UserRole != Role.CampDirector)
		{
			lblMsg.Text = "You must select at least one federation/camp";
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

		GenerateExcelReport();

        // 2013-12-31 The code below is for testing purpose

        //DataTable dt = GenerateDataSet();
        //gv.DataSource = dt;
        //gv.DataBind();

        //divMenu.Visible = false;
        //divReport.Visible = true;
	}

	private DataTable GenerateDataSet()
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
			FedID_List = "Select ID From tblFederations";
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
		if (rdoAllTimers.Checked)
			TimesReceivedGrant = 0;
		else if (rdo1stTimers.Checked)
			TimesReceivedGrant = 1;
		else if (rdo2ndTimers.Checked)
			TimesReceivedGrant = 2;

		return ViewDumpForAllYearsDA.GetDataExtract(Int32.Parse(ddlCampYear.SelectedValue), FedID_List, CampID_List, StatusID_List, TimesReceivedGrant);
	}

	#region "Criterial selection control events"

	protected void chklistStatus_DataBound(object sender, EventArgs e)
	{
		MakeKeyStatusBold();
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

	protected void chklistCamp_DataBound(object sender, EventArgs e)
	{
		if (chklistCamp.Items.Count == 0)
		{
			lblMsg.Text = "The federation has no camp in the camper applications data OR no federation is selected";
			chkAllCamps.Enabled = false;
		}
		else
		{
			lblMsg.Text = "";
			chkAllCamps.Enabled = true;
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

	#endregion

	#region "Report section control events"

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
		string templateFile = Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
		string workFileDir = Server.MapPath(@"~/Docs");

		// Make a excel report
		ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
		ExcelFile excel = new ExcelFile();
		excel.LoadXls(templateFile);

		ExcelWorksheet ws = excel.Worksheets["Sheet1"];

		// Data Content of report
		DataTable dt = GenerateDataSet();
		
		// The first row is decoration headers
		ws.Rows[1].Height = 2 * 256;
	    int columnPointer = 9;

		CellStyle cs = new CellStyle();
		cs.FillPattern.SetSolid(Color.FromArgb(204, 153, 255));
		cs.Font.Weight = ExcelFont.BoldWeight;
		cs.WrapText = true;
		cs.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr = ws.Cells.GetSubrangeAbsolute(0, 0, 0, columnPointer);
		cr.Merged = true;
		cr.Value = "Program Information";
		cr.Style = cs;

		CellStyle cs2 = new CellStyle();
		cs2.FillPattern.SetSolid(Color.FromArgb(102, 255, 255));
		cs2.Font.Weight = ExcelFont.BoldWeight;
		cs2.WrapText = true;
		cs2.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr2 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 1, 0, columnPointer += 5);
		cr2.Merged = true;
		cr2.Value = "Camp Information";
		cr2.Style = cs2;

		CellStyle cs3 = new CellStyle();
		cs3.FillPattern.SetSolid(Color.FromArgb(255, 153, 204));
		cs3.Font.Weight = ExcelFont.BoldWeight;
		cs3.WrapText = true;
		cs3.HorizontalAlignment = HorizontalAlignmentStyle.Center;

		CellRange cr3 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 1, 0, columnPointer += 8);
		cr3.Merged = true;
		cr3.Value = "Basic Camper Information";
		cr3.Style = cs3;

		CellStyle cs4 = new CellStyle();
		cs4.FillPattern.SetSolid(Color.FromArgb(204, 255, 204));
		cs4.Font.Weight = ExcelFont.BoldWeight;
		cs4.WrapText = true;
		cs4.HorizontalAlignment = HorizontalAlignmentStyle.Center;

		CellRange cr4 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 1, 0, columnPointer += 5);
		cr4.Merged = true;
		cr4.Value = "Camper Contact Information";
		cr4.Style = cs4;

		CellStyle cs5 = new CellStyle();
		cs5.FillPattern.SetSolid(Color.FromArgb(255, 255, 153));
		cs5.Font.Weight = ExcelFont.BoldWeight;
		cs5.WrapText = true;
		cs5.HorizontalAlignment = HorizontalAlignmentStyle.Center;

		CellRange cr5 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 1, 0, columnPointer += 14);
		cr5.Merged = true;
		cr5.Value = "Parent Contact Information";
		cr5.Style = cs5;

		CellStyle cs6 = new CellStyle();
		cs6.FillPattern.SetSolid(Color.FromArgb(150, 150, 150));
		cs6.Font.Weight = ExcelFont.BoldWeight;
		cs6.WrapText = true;
		cs6.HorizontalAlignment = HorizontalAlignmentStyle.Center;

		CellRange cr6 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 1, 0, columnPointer += 3);
		cr6.Merged = true;
		cr6.Value = "Application Information";
		cr6.Style = cs6;

		CellStyle cs7 = new CellStyle();
		cs7.FillPattern.SetSolid(Color.FromArgb(255, 204, 153));
		cs7.Font.Weight = ExcelFont.BoldWeight;
		cs7.WrapText = true;
		cs7.HorizontalAlignment = HorizontalAlignmentStyle.Center;

		CellRange cr7 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 1, 0, columnPointer += 8);
		cr7.Merged = true;
		cr7.Value = "Marketing Source";
		cr7.Style = cs7;

		CellStyle cs8 = new CellStyle();
		cs8.FillPattern.SetSolid(Color.FromArgb(153, 204, 255));
		cs8.Font.Weight = ExcelFont.BoldWeight;
		cs8.WrapText = true;
		cs8.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr8 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 1, 0, columnPointer += 16);
		cr8.Merged = true;
		cr8.Value = "Demographic Information";
		cr8.Style = cs8;

        CellStyle cs9 = new CellStyle();
        cs9.FillPattern.SetSolid(Color.FromArgb(233, 19, 210));
        cs9.Font.Weight = ExcelFont.BoldWeight;
        cs9.WrapText = true;
        cs9.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr9 = ws.Cells.GetSubrangeAbsolute(0, columnPointer + 2, 0, columnPointer += 6);
        cr9.Merged = true;
        cr9.Value = "FJC ONLY";
        cr9.Style = cs8;

		// this creats the real table
		ws.InsertDataTable(dt, 1, 0, true);

		CellStyle styleTableHeaderColumns = new CellStyle();
		styleTableHeaderColumns.Font.Weight = ExcelFont.BoldWeight;
		styleTableHeaderColumns.HorizontalAlignment = HorizontalAlignmentStyle.Center;
		styleTableHeaderColumns.VerticalAlignment = VerticalAlignmentStyle.Center;
		styleTableHeaderColumns.FillPattern.SetSolid(Color.LightGray);
		styleTableHeaderColumns.WrapText = true;

		ws.Columns[0].Width = 15 * 256;
		ws.Columns[1].Width = 35 * 256;
		ws.Columns[2].Width = 30 * 256;

		ws.Columns[11].Width = 35 * 256; // Camp Name
		ws.Columns[12].Width = 27 * 256;
		
		ws.Columns[16].Width = 30 * 256; // Last Name
		ws.Columns[17].Width = 30 * 256;

		ws.Rows[1].Style = styleTableHeaderColumns;
		ws.Rows[1].Height = 3 * 256;

		CellStyle styleContent = new CellStyle();
		styleContent.HorizontalAlignment = HorizontalAlignmentStyle.Left;
		styleContent.VerticalAlignment = VerticalAlignmentStyle.Center;

		for (int i = 2; i < dt.Rows.Count; i++)
		{
			ws.Rows[i].Style = styleContent;
		}

		styleContent.WrapText = true;

		excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

		// Save to a file on the local file system
		string filename = String.Format("\\{0}{1}{2}{3}DataExtract.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
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

	#endregion
}