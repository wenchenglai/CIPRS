using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using GemBox.ExcelLite;
using System.IO;

public partial class ReportFJCCampers : System.Web.UI.Page
{
	ReportParamCampersFJC param;
	Role UserRole;


	protected void Page_Load(object sender, EventArgs e)
	{
		lblMsg.Text = "";

		// We use Session variable for passing the composite object for reporting
		param = (ReportParamCampersFJC)Session["ReportParamCampersFJC"];
		UserRole = (Role)Convert.ToInt32(Session["RoleID"]);

		DeteleOldReportFiles();

		if (!IsPostBack)
		{
			// 2011-11-01 Camper Contact Info report is totally different from other reports here, but I need to re-use the paginig mechanism.  Refactor is needed, else this page
			// will be bloated
			if (param.CamperOrg == CamperOrgType.CamperContactInfo)
			{
				CreateCampThatHaveDataDictionaryObject();

				int CampCount = param.CampsThatHaveDataDict.Count;

				if (CampCount > 0)
				{
					CreateMainReport();
					// show this message when actual camp with data didn't match with selected camps by user
					if (param.Camp_Dict.Count != param.CampsThatHaveDataDict.Count)
						lblMsg.Text = String.Format("Only {0} of selected {1} have data", CampCount, param.Camp_Dict.Count);

					spanPagerTop.Visible = true;
					spanPagerBottom.Visible = true;
				}
				else
				{
					// no camps that have data with selected status, we would display message and show no data
					lblMsg.Text = String.Format("There is no data from the selected {0} and Status", "programs and camps");
					spanPagerTop.Visible = false;
					spanPagerBottom.Visible = false;
				}
			}
			else
			{
				// We need to return only camps that have data (selected camp with no data won't be displayed at all)
				// so before we create the real report, we need to query the db to see how many camps actually have data
				int CampCount = BuildCampListThatHasData(param, UserRole);

				if (CampCount > 0)
				{
					// we have at least one camp that has data, so we can create the main reports with paging
					CreateMainReport();

					// show this message when actual camp with data didn't match with selected camps by user
					if (param.Camp_Dict.Count != param.CampsThatHaveDataDict.Count)
						lblMsg.Text = String.Format("Only {0} of selected {1} have data", CampCount, param.Camp_Dict.Count);

					spanPagerTop.Visible = true;
					spanPagerBottom.Visible = true;
				}
				else
				{
					// no camps that have data with selected status, we would display message and show no data
					lblMsg.Text = String.Format("There is no data from the selected {0} and Status", param.CamperOrg);
					spanPagerTop.Visible = false;
					spanPagerBottom.Visible = false;
				}
			}
		}
	}

	private int BuildCampListThatHasData(ReportParamCampersFJC param, Role userRole)
	{
		if (UserRole == Role.CampDirector)
		{
			param.ProgramTypeID = ProgramType.JWest; // Camp director 's data must be limited with JWest
			param.CampsThatHaveDataDict.Add(param.Camp_Dict.First().Key, param.Camp_Dict.First().Value);
			param.CampID_HaveData_List = param.Camp_Dict.First().Key.ToString();
			// Remember to save the changes back to session
			Session["ReportParamCampersFJC"] = param;
			return 1;  // by definition, camper director is guaranteed to have one and ONLY on camp association
		}
		else
		{
			// For other roles that can see more than 1 camp
			// The very first thing we have to do is to find out how many camps have actual data with selected Status
			return CreateCampThatHaveDataDictionaryObject();
		}
	}

	private void DeteleOldReportFiles()
	{
		string workFileDir = Server.MapPath(@"~/Docs");

		// Delete old work files
		// need to check and see if any old files are left behind
		DirectoryInfo myDir = new DirectoryInfo(workFileDir);

		foreach (FileSystemInfo myFile in myDir.GetFileSystemInfos("*.xls"))
		{
			// Delete any files that are 1 min old
			//if ((DateTime.Now - myFile.CreationTime).Minutes > 1)
			myFile.Delete();
		}
	}
	/// <summary>
	/// The function is created to filter out the selected camps that don't have selected status.  Users could get annoyed by seeing
	/// so man empty camps that has no matching status, so we do the extra step to filter out.
	/// </summary>
	/// <returns></returns>
	private int CreateCampThatHaveDataDictionaryObject()
	{
		// Get all the camps that have data with selected status
		DataTable dtCampHaveData = null;
		
		// 2010-01-23 switch explosion.  Strategy pattern is needed if one more switch is added
		if (param.CamperOrg == CamperOrgType.Synagogue)
		{
			dtCampHaveData = CamperApplicationDA.GetSynagsListThatHaveData(param.CampYearID, param.CampID_List, param.StatusID_List, param.FedID);
		}
		else if (param.CamperOrg == CamperOrgType.Camp)
		{
			//
			if (UserRole == Role.FJCAdmin)
				dtCampHaveData = CamperApplicationDA.GetCampsListThatHaveData(param.CampYearID, param.ProgramTypeID, param.CampID_List, param.StatusID_List, param.FedID, param.TimesReceivedGrant);
			else if (UserRole == Role.FederationAdmin)
				dtCampHaveData = CamperApplicationDA.GetCampsListThatHaveData(param.CampYearID, param.ProgramTypeID, param.CampID_List, param.StatusID_List, param.FedID, param.TimesReceivedGrant);
			else
			{
				// 2010-01-20 Haven't tested other roles yet, used the most stringent condition, that is, with FedID
				dtCampHaveData = CamperApplicationDA.GetCampsListThatHaveData(param.CampYearID, param.ProgramTypeID, param.CampID_List, param.StatusID_List, param.FedID, param.TimesReceivedGrant);
			}
		}
		else if (param.CamperOrg == CamperOrgType.CamperContactInfo)
		{
			dtCampHaveData = CamperApplicationDA.GetCampsListThatHaveDataDAForCamperContactInfo(param.CampYearID, param.FedID_List, param.CampID_List, param.StatusID_List, param.TimesReceivedGrant);
		}

		string campIDHaveData_list = "";
		foreach (DataRow dr in dtCampHaveData.Rows)
		{
			param.CampsThatHaveDataDict.Add(Convert.ToInt32(dr["CampID"]), dr["Name"].ToString());
			campIDHaveData_list += dr["CampID"].ToString();
			if (dtCampHaveData.Rows.IndexOf(dr) != dtCampHaveData.Rows.Count - 1)
				campIDHaveData_list += ", ";
		}
		param.CampID_HaveData_List = campIDHaveData_list;

		// Remember to save the changes back to session
		Session["ReportParamCampersFJC"] = param;

		return param.CampsThatHaveDataDict.Count;
	}

	/// <summary>
	/// This creates each data table + total table for each camp.  It's designed to fit the paging mechanism
	/// </summary>
	private void CreateMainReport()
	{
		lblCampName.Text = param.CampsThatHaveDataDict.ElementAt(Int32.Parse(txtPage.Text) - 1).Value;
		lblPage.Text = param.CampsThatHaveDataDict.Count.ToString();
		lblPage2.Text = lblPage.Text;
		DataTable dtTotal;

		if (param.CamperOrg == CamperOrgType.Camp)
		{
			DataTable dt = CamperApplicationDA.GetFJCCamperReport(param.CamperOrg, param.ProgramTypeID, param.FedID, param.CampYearID, param.CampsThatHaveDataDict.ElementAt(Int32.Parse(txtPage.Text) - 1).Key, param.StatusID_List);

			dtTotal = CreateTotalTablePerCamp(dt);

			gvPerCamp.DataSource = dt;
			gvPerCamp.DataBind();

			gvTotalPerCamp.DataSource = dtTotal;
			gvTotalPerCamp.DataBind();

			divPerCamp.Visible = true;
		}
		else if (param.CamperOrg == CamperOrgType.Synagogue)
		{
			DataTable dt = CamperApplicationDA.GetFJCCamperReport(param.CamperOrg, param.ProgramTypeID, param.FedID, param.CampYearID, param.CampsThatHaveDataDict.ElementAt(Int32.Parse(txtPage.Text) - 1).Key, param.StatusID_List);

			dtTotal = CreateTotalTablePerSynag(dt);

			gvPerSynag.DataSource = dt;
			gvPerSynag.DataBind();

			gvTotalPerSynag.DataSource = dtTotal;
			gvTotalPerSynag.DataBind();

			divPerSynag.Visible = true;
		}
		else if (param.CamperOrg == CamperOrgType.CamperContactInfo)
		{
			DataTable dt = CamperApplicationDA.GetCamperContactInfoData(UserRole, param.FedID, param.FedID_List, param.CampYearID, param.CampsThatHaveDataDict.ElementAt(Int32.Parse(txtPage.Text) - 1).Key, param.StatusID_List, param.TimesReceivedGrant);
			gvCamperContactInfo.DataSource = dt;
			gvCamperContactInfo.DataBind();

			divCamperContactInfo.Visible = true;
		}
	}

    protected void gvCamperContactInfo_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[14].Text == "No")
            {
                e.Row.Cells[14].ForeColor = Color.Red;
                e.Row.Cells[15].ForeColor = Color.Red;
                e.Row.Cells[16].ForeColor = Color.Red;                
            }
        }
    }

    protected void gvPerCamp_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[13].Text == "No")
            {
                e.Row.Cells[13].ForeColor = Color.Red;
                e.Row.Cells[14].ForeColor = Color.Red;
            }
        }
    }

	private static DataTable CreateTotalTablePerSynag(DataTable dtIn)
	{
		int TotalCampers = 0;
		float TotalAmount = 0;

		foreach (DataRow dr in dtIn.Rows)
		{
			if (dr["Grant Amount"] != DBNull.Value) //it's still possible the data in DB is corrupted
			{
				TotalCampers += 1;
				TotalAmount += Convert.ToSingle(dr["Grant Amount"]);
			}
		}

		DataTable dt = new DataTable();

		dt.Columns.Add("Total Campers", TotalCampers.GetType());
		dt.Columns.Add("Total Amount", TotalAmount.GetType());

		DataRow newdr = dt.NewRow();
		newdr[0] = TotalCampers;
		newdr[1] = TotalAmount;

		dt.Rows.Add(newdr);

		return dt;
	}

	private static DataTable CreateTotalTablePerCamp(DataTable dtIn)
	{
		int TotalCampers = 0, TotalFirstTimer = 0, TotalSecondTimer = 0, totalThirdTimer = 0;
		float TotalAmount = 0, TotalFirstAmount = 0, TotalSecondAmount = 0, totalThirdAmount = 0;

		foreach (DataRow dr in dtIn.Rows)
		{
			if (dr["Grant Amount"] != DBNull.Value) //it's still possible the data in DB is corrupted
			{
				TotalCampers += 1;
				// Column 7 is First OR Second Timer.  1 is first timer, 2 is second timer
				int timer = (int)dr["1st/2nd/3rd Timer"];
				if ( timer == 1)
				{
					TotalFirstTimer += 1;
					TotalFirstAmount += Convert.ToSingle(dr["Grant Amount"]);
				}
				else if (timer == 2)
				{
					TotalSecondTimer += 1;
					TotalSecondAmount += Convert.ToSingle(dr["Grant Amount"]);
				}
				else
				{
					totalThirdTimer += 1;
					totalThirdAmount += Convert.ToSingle(dr["Grant Amount"]);
				}

				TotalAmount += Convert.ToSingle(dr["Grant Amount"]);
			}
		}

		DataTable dt = new DataTable();

		dt.Columns.Add("Total Campers", TotalCampers.GetType());
		dt.Columns.Add("Total Amount", TotalAmount.GetType());
		dt.Columns.Add("Total First Timer", TotalFirstTimer.GetType());
		dt.Columns.Add("First Amount", TotalFirstAmount.GetType());
		dt.Columns.Add("Total Second Timer", TotalSecondTimer.GetType());
		dt.Columns.Add("Second Amount", TotalSecondAmount.GetType());
		dt.Columns.Add("Total Third Timer", TotalSecondTimer.GetType());
		dt.Columns.Add("Third Amount", TotalSecondAmount.GetType());


		DataRow newdr = dt.NewRow();
		newdr[0] = TotalCampers;
		newdr[1] = TotalAmount;
		newdr[2] = TotalFirstTimer;
		newdr[3] = TotalFirstAmount;
		newdr[4] = TotalSecondTimer;
		newdr[5] = TotalSecondAmount;
		newdr[6] = totalThirdTimer;
		newdr[7] = totalThirdAmount;

		dt.Rows.Add(newdr);

		return dt;
	}

	protected void txtPage_TextChanged(object sender, EventArgs e)
	{
		if (((TextBox)sender).ID == "txtPage")
			txtPage2.Text = txtPage.Text;
		else
			txtPage.Text = txtPage2.Text;

		int currentPage = PageIndexCheck();

		if (currentPage > 0)
		{
			CreateMainReport();
		}
	}
	
	protected void lnkbtnPrevious_Click(object sender, EventArgs e)
	{
		int currentPage = PageIndexCheck();

		if (currentPage > 1)
		{
			txtPage.Text = (currentPage -= 1).ToString();
			txtPage2.Text = txtPage.Text;
			CreateMainReport();
		}
	}

	protected void lnkbtnNext_Click(object sender, EventArgs e)
	{
		int currentPage = PageIndexCheck();
		int maxPageIndex = Int32.Parse(lblPage.Text);

		if (currentPage < maxPageIndex)
		{
			txtPage.Text = (currentPage += 1).ToString();
			txtPage2.Text = txtPage.Text;
			CreateMainReport();
		}
	}

	private int PageIndexCheck()
	{
		int currentPage = 0;
		if (!Int32.TryParse(txtPage.Text, out currentPage))
		{
			lblMsg.Text = "Error!  Page number must be integer";
			return -1;
		}

		if (currentPage < 1)
		{
			lblMsg.Text = "Error!  Page number must be greater than 0";
			return -1;
		}

		int maxPageIndex = Int32.Parse(lblPage.Text);
		if (currentPage > maxPageIndex)
		{
			lblMsg.Text = "Error!  Page number cannot exceed the max page number";
			return -1;
		}

		return currentPage;
	}

	protected void btnReport_Click(object sender, EventArgs e)
	{
		DataSet ds;
		
		// Work sheet #1 - Data grouped by camp
		// the data content section of the report
		if (param.CamperOrg == CamperOrgType.CamperContactInfo)
			ds = CamperApplicationBL.GetCamperContactInfoReportInBatch(UserRole, param.FedID, param.FedID_List, param.CampYearID, param.CampID_List, param.StatusID_List, param.TimesReceivedGrant);
		else
			ds = CamperApplicationBL.GetFJCCamperReportInBatch(param.CamperOrg, param.ProgramTypeID, param.FedID, param.CampYearID, param.CampID_HaveData_List, param.StatusID_List, param.TimesReceivedGrant);


		string templateFile = Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
		string workFileDir = Server.MapPath(@"~/Docs");

		// Make a excel report
		ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
		ExcelFile excel = new ExcelFile();
		excel.LoadXls(templateFile);

		ExcelWorksheet ws = excel.Worksheets["Sheet1"];
		ws.Name = "Camper Detail";

		//We start at first row, because for ExcelLite control, the header row is not included
		int BEGIN_COLUMN_INDEX;

		if (param.CamperOrg == CamperOrgType.CamperContactInfo)
			BEGIN_COLUMN_INDEX = 0;
		else
			BEGIN_COLUMN_INDEX = 1;

		const int CAMP_NAME_MERGED_CELL_NUMBER = 6;
		const int REPORT_HEADER_CELL_NUMBER = 6;
		const int REPORT_SUB_HEADER_CELL_NUMBER = 4;

		int iRow = 1;

		// Global artistic seeting
		ws.Columns[0].Width = 20 * 20;

		// Create Report Header
		CellStyle styleReportHeader = new CellStyle();
		styleReportHeader.Font.Color = System.Drawing.Color.Blue;
		styleReportHeader.Font.Size = 22 * 20;
		styleReportHeader.Font.Weight = ExcelFont.BoldWeight;

		CellRange ReportHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_HEADER_CELL_NUMBER);
		ReportHeader.Merged = true;
		ReportHeader.Style = styleReportHeader;

		if (param.CamperOrg == CamperOrgType.Camp)
		{
			if (UserRole == Role.FJCAdmin)
				ReportHeader.Value = "Camper Detail Report By Camp";
			else
				ReportHeader.Value = "Camper Detail Report By Camp";
		}
		else if (param.CamperOrg == CamperOrgType.CamperContactInfo)
			ReportHeader.Value = "Camper Contact Info";
		else
			ReportHeader.Value = "Camper Detail Report By Synagogue";

		//
		iRow += 1;

		// Create Report SubHeader
		CellStyle styleSubHeader = new CellStyle();
		styleSubHeader.Font.Size = 16 * 20;
		styleSubHeader.Font.Weight = ExcelFont.BoldWeight;

		CellRange SubHeader = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
		SubHeader.Merged = true;
		SubHeader.Value = String.Format("Camp Year: {0}  Generated on {1}", param.CampYear, DateTime.Now);
		SubHeader.Style = styleSubHeader;

		iRow += 2;

		CellStyle cs = new CellStyle();
		//cs.Font.Color = System.Drawing.Color.Red;
		cs.Font.Size = 18 * 20;
		cs.Font.Weight = ExcelFont.BoldWeight;

		CellStyle csFirst = new CellStyle();
		csFirst.FillPattern.SetSolid(Color.Yellow);
		csFirst.Font.Weight = ExcelFont.BoldWeight;
		csFirst.WrapText = true;

		ws.Columns[0].Width = 20 * 256;

		CellStyle tableHeaderStyle = new CellStyle();
		tableHeaderStyle.Font.Weight = ExcelFont.BoldWeight;
		tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);

        CellStyle redText = new CellStyle();
        redText.Font.Color = Color.Red;

		foreach (DataTable dt in ds.Tables)
		{
			CellRange cr = ws.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, CAMP_NAME_MERGED_CELL_NUMBER);
			cr.Merged = true;

			cr.Value = dt.Rows[0]["Camp Name"].ToString();

			//if (param.CamperOrg == CamperOrgType.CamperContactInfo)
			//{
			//    cr.Value = dt.Rows[0]["Camp Name"].ToString();
			//}
			//else
			//{
			//    cr.Value = param.CampsThatHaveDataDict.ElementAt(ds.Tables.IndexOf(dt)).Value;
			//}
			
			cr.Style = cs;

			ws.Rows[iRow].Height = 25 * 20;

			iRow += 1;

			ws.Rows[iRow].Height = 25 * 20;

			CellRange crFirst = ws.Cells.GetSubrangeAbsolute(iRow, 0, iRow, 0);

			if (param.CamperOrg != CamperOrgType.CamperContactInfo)
			{
				crFirst.Value = "Registered at Camp: Enter a Y / N here";
				crFirst.Style = csFirst;
			}

			ws.InsertDataTable(dt, iRow, BEGIN_COLUMN_INDEX, true);

            // 2012-10-28 Need to make allow contact emails stand out so admins won't contact campers who wish not to be contacted
            if (param.CamperOrg == CamperOrgType.CamperContactInfo)
            {
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    if (ws.Cells[i + iRow, 14].Value.ToString() == "No")
                    {
                        ws.Cells[i + iRow, 14].Style = redText;
                        ws.Cells[i + iRow, 15].Style = redText;
                        ws.Cells[i + iRow, 16].Style = redText;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    //if (ws.Cells[i + iRow, 14].Value.ToString() == "No")
                    //{
                    //    ws.Cells[i + iRow, 14].Style = redText;
                    //    ws.Cells[i + iRow, 15].Style = redText;
                    //}
                }
            }

            // loop through each column and 1.set the width of each colum, 2. set the header style of each column
            for (int i = BEGIN_COLUMN_INDEX; i <= dt.Columns.Count; i++)
			{
				if (param.CamperOrg == CamperOrgType.CamperContactInfo)
				{
					ws.Cells[iRow, i].Style = tableHeaderStyle;
					if (i== 2)
						ws.Columns[i].Width = 22 * 256;
					else if (i == 3)
						ws.Columns[i].Width = 16 * 256;
					else if (i == 4)
						ws.Columns[i].Width = 16 * 256;
					else if (i == 5)
						ws.Columns[i].Width = 16 * 256;
					else if (i == 7)
						ws.Columns[i].Width = 23 * 256;
					else if (i == 5)
						ws.Columns[i].Width = 15 * 256;
					else if (i == 11)
						ws.Columns[i].Width = 26 * 256;
					else if (i == 12)
						ws.Columns[i].Width = 15 * 256;
                    else if (i == 13)
                        ws.Columns[i].Width = 10 * 256;
                    else if (i == 14)
                    {
                        ws.Columns[i].Width = 10 * 256;
                    }
                    else
                        ws.Columns[i].Width = 20 * 256;
				}
				else
				{
					ws.Cells[iRow, i].Style = tableHeaderStyle;
					if (i == 3)
						ws.Columns[i].Width = 30 * 256;
					else if (i == 4)
						ws.Columns[i].Width = 16 * 256;
					else if (i == 5)
						ws.Columns[i].Width = 16 * 256;
					else if (i == 7)
						ws.Columns[i].Width = 23 * 256;
					else if (i == 5)
						ws.Columns[i].Width = 15 * 256;
					else if (i == 11)
						ws.Columns[i].Width = 26 * 256;
					else if (i == 12)
						ws.Columns[i].Width = 15 * 256;
					else
						ws.Columns[i].Width = 20 * 256;
				}
			}

			iRow += dt.Rows.Count + 2;

			DataTable dtTotal;

			if (param.CamperOrg == CamperOrgType.Camp)
			{
				dtTotal = CreateTotalTablePerCamp(dt);
			}
			else if (param.CamperOrg == CamperOrgType.Synagogue)
			{
				dtTotal = CreateTotalTablePerSynag(dt);
			}
			else
				dtTotal = null;

			if (param.CamperOrg != CamperOrgType.CamperContactInfo)
			{
				ws.InsertDataTable(dtTotal, iRow, BEGIN_COLUMN_INDEX, true);

				for (int i = BEGIN_COLUMN_INDEX; i <= dtTotal.Columns.Count; i++)
				{
					ws.Cells[iRow, i].Style = tableHeaderStyle;
				}

				iRow += dtTotal.Rows.Count + 3;
			}
		}

		// Second alternate sheet that list all rows as table format
		ExcelWorksheet ws2 = excel.Worksheets["Sheet2"];
		ws2.Name = "Alternate Format";
		DataTable dtAlternate;
		if (param.CamperOrg == CamperOrgType.CamperContactInfo)
			dtAlternate = CamperApplicationDA.GetCamperContactInfoReportInBatch(UserRole, param.FedID, param.FedID_List, param.CampYearID, param.CampID_List, param.StatusID_List, param.TimesReceivedGrant);
		else
			dtAlternate = CamperApplicationDA.GetFJCCamperReportInBatch(param.CamperOrg, param.ProgramTypeID, param.FedID, param.CampYearID, param.CampID_HaveData_List, param.StatusID_List);

		dtAlternate.Columns.RemoveAt(0); // We don't want the first column, which is CampID

		iRow = 1;
		ReportHeader = ws2.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_HEADER_CELL_NUMBER);
		ReportHeader.Merged = true;
		ReportHeader.Style = styleReportHeader;

		if (param.CamperOrg == CamperOrgType.Camp)
		{
			ReportHeader.Value = "Camper Detail Report By Camp";
		}
		else if (param.CamperOrg == CamperOrgType.CamperContactInfo)
			ReportHeader.Value = "Camper Contact Info";
		else
			ReportHeader.Value = "Camper Detail Report By Synagogue";

		ws2.Rows[iRow].Height = 25 * 20;

		iRow += 1;

		ws2.Rows[iRow].Height = 25 * 20;
		// Create Report SubHeader
		SubHeader = ws2.Cells.GetSubrangeAbsolute(iRow, BEGIN_COLUMN_INDEX, iRow, REPORT_SUB_HEADER_CELL_NUMBER);
		SubHeader.Merged = true;
		SubHeader.Value = "Camp Year: " + param.CampYear;
		SubHeader.Style = styleSubHeader;

		iRow += 2;

		ws2.InsertDataTable(dtAlternate, iRow, BEGIN_COLUMN_INDEX, true);

        // 2012-10-28 Need to make allow contact emails stand out so admins won't contact campers who wish not to be contacted
        if (param.CamperOrg == CamperOrgType.CamperContactInfo)
        {
            for (int i = 0; i <= dtAlternate.Rows.Count; i++)
            {
                if (ws2.Cells[i + iRow, 14].Value.ToString() == "No")
                {
                    ws2.Cells[i + iRow, 14].Style = redText;
                    ws2.Cells[i + iRow, 15].Style = redText;
                    ws2.Cells[i + iRow, 16].Style = redText;
                }
            }
        }
        else
        {
            //for (int i = 0; i <= dtAlternate.Rows.Count; i++)
            //{
            //    if (ws2.Cells[i + iRow, 14].Value.ToString() == "No")
            //    {
            //        ws2.Cells[i + iRow, 14].Style = redText;
            //        ws2.Cells[i + iRow, 15].Style = redText;
            //    }
            //}
        }

		ws2.Rows[iRow].Style = tableHeaderStyle;
		if (param.CamperOrg == CamperOrgType.CamperContactInfo)
		{
			ws2.Columns[BEGIN_COLUMN_INDEX].Width = 16 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 1].Width = 28 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 2].Width = 28 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 3].Width = 14 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 4].Width = 16 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 5].Width = 23 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 10].Width = 26 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 11].Width = 15 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 14].Width = 25 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 15].Width = 25 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 16].Width = 25 * 256;
		}
		else
		{
			// Camper Detail Report
			ws2.Columns[BEGIN_COLUMN_INDEX].Width = 15 * 256; // CampID
			ws2.Columns[BEGIN_COLUMN_INDEX + 1].Width = 30 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 2].Width = 25 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 3].Width = 25 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 4].Width = 30 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 5].Width = 18 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 6].Width = 30 * 256; // Camp Name
			ws2.Columns[BEGIN_COLUMN_INDEX + 9].Width = 22 * 256; // Session Datea
			ws2.Columns[BEGIN_COLUMN_INDEX + 10].Width = 18 * 256; // Timer
			ws2.Columns[BEGIN_COLUMN_INDEX + 12].Width = 25 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 13].Width = 25 * 256;
			ws2.Columns[BEGIN_COLUMN_INDEX + 14].Width = 25 * 256;
		}

		excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];
		
		// Save to a file on the local file system
		string filename = String.Format("\\{0}{1}{2}{3}CamperDetailReport.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
		string newFile = workFileDir + filename;
		excel.SaveXls(newFile);

		
		string[] strFileParts = newFile.Split(new string[]{"\\"}, StringSplitOptions.None);

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

	protected void lnkbtnBack_Click(object sender, EventArgs e)
	{
		// Set the return page link to appropriate page, becuase more than 1 page share this reporting page
		string retURL = "";

		if (param.CamperOrg == CamperOrgType.Synagogue)
		{
			retURL = "CampersBySynagogue.aspx";
		}
		else if (param.CamperOrg == CamperOrgType.CamperContactInfo)
		{
			retURL = "CamperContactInfo.aspx";
		}
		else
		{
			if (UserRole == Role.FJCAdmin)
				if (param.ProgramTypeID == ProgramType.NoUse)
					retURL = "CampersByCampFed.aspx";
				else
					retURL = "CampersByCampFJC.aspx";
			else
				retURL = "CampersByCampFed.aspx";
		}

		Session["ReportParamCampersFJC"] = null;
		Response.Redirect(retURL);
	}


}
