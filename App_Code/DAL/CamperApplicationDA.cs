using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

/// <summary>
/// Summary description for CamperApplicationDA
/// </summary>
public class CamperApplicationDA
{
	/// <summary>
	/// Overloaded function - used for Fed admin, so FedID is required here
	/// </summary>
	/// <param name="CampYearID"></param>
	/// <param name="CampID_List"></param>
	/// <param name="StatusID_List"></param>
	/// <param name="FedID"></param>
	/// <returns></returns>
	public static DataTable GetCampsListThatHaveData(int CampYearID, ProgramType Program, string CampID_List, string StatusID_List, int FedID, int TimesReceivedGrant)
	{
		return GetCampsListThatHaveDataDA(CampYearID, Program, CampID_List, StatusID_List, FedID, TimesReceivedGrant);
	}

	public static DataTable GetSynagsListThatHaveData(int CampYearID, string SynagID_List, string StatusID_List, int FedID)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetSynagsThatHaveData");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID_List", SynagID_List);
		db.AddParameter("@StatusID_List", StatusID_List);
		db.AddParameter("@FedID", FedID);

		return db.FillDataTable("usprsCamperApplications_Select");
	}

	/// <summary>
	/// Get Camps that have data using the input parameters as filters
	/// There are two versions of this method
	/// </summary>
	/// <param name="CampYearID"></param>
	/// <param name="CampID_List"></param>
	/// <param name="StatusID_List"></param>
	/// <param name="FedID"></param>
	/// <returns></returns>
	private static DataTable GetCampsListThatHaveDataDA(int CampYearID, ProgramType Program, string CampID_List, string StatusID_List, int FedID, int TimesReceivedGrant)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetCampsThatHaveData");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusID_List", StatusID_List);

		if (Program != ProgramType.NoUse)
			db.AddParameter("@Program", Program);

		if (FedID > 0)
			db.AddParameter("@FedID", FedID);

		if (TimesReceivedGrant > 0)
			db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);

		return db.FillDataTable("usprsCamperApplications_Select");
	}

	public static DataTable GetCampsListThatHaveDataDAForCamperContactInfo(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, int TimesReceivedGrant)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetCampsThatHaveDataForCamperContactInfo");
		db.AddParameter("@CampYearID", CampYearID);        
		db.AddParameter("@FedID_List", FedID_List);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusID_List", StatusID_List);
		db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);

		return db.FillDataTable("usprsCamperApplications_Select");
	}

	public static DataTable GetFJCCamperReport(CamperOrgType CamperOrg, ProgramType Program, int FedID, int CampYearID, int CampID, string StatusID_List)
	{
		var db = new SQLDBAccess("CIPMS");

		// Camp and Syang are different 
		if (CamperOrg == CamperOrgType.EnrollmentConfirmationFJC || CamperOrg == CamperOrgType.EnrollmentConfirmationPartner)
			db.AddParameter("@Action", "GetFJCCamperReportPerCamp");
		else if (CamperOrg == CamperOrgType.Synagogue)
			db.AddParameter("@Action", "GetFJCCamperReportPerSynag");

		if (Program != ProgramType.NoUse)
			db.AddParameter("@Program", Program);

		if (FedID >= 0)
			db.AddParameter("@FedID", FedID);

		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID", CampID);
		db.AddParameter("@StatusID_List", StatusID_List);
		return db.FillDataTable("usprsCamperApplications_Select");
	}

	public static DataTable GetCamperContactInfoData(Role userRole, int FedID, string FedID_List, int CampYearID, int CampID, string StatusID_List, int TimesReceivedGrant)
	{
		var db = new SQLDBAccess("CIPMS");

		db.AddParameter("@Action", "GetCamperContactInfo");
		db.AddParameter("@FedID_List", FedID_List);
		db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID", CampID);
		db.AddParameter("@StatusID_List", StatusID_List);
		return db.FillDataTable("usprsCamperApplications_Select");
	}

    public static DataTable GetCamperDetailReport(Role userRole, int FedID, string FedID_List, int CampYearID, int CampID, string StatusID_List, int TimesReceivedGrant)
    {
        var db = new SQLDBAccess("CIPMS");

        db.AddParameter("@Action", "GetCamperDetailReport");
        db.AddParameter("@FedID_List", FedID_List);
        db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@CampID", CampID);
        db.AddParameter("@StatusID_List", StatusID_List);
        return db.FillDataTable("usprsCamperApplications_Select");
    }

	public static DataTable GetDuplicateCampers(int CampYearID, string StatusID_List)
	{
		var db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetDuplicateCampers");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@StatusID_List", StatusID_List);
		return db.FillDataTable("usprsCamperApplications_Select");
	}

	/// <summary>
	/// Used for excel reporting data query
	/// </summary>
	/// <param name="program"></param>
	/// <param name="FedID"></param>
	/// <param name="CampYearID"></param>
	/// <param name="CampID_List"></param>
	/// <param name="StatusID_List"></param>
	/// <returns></returns>
	public static DataTable GetFJCCamperReportInBatch(CamperOrgType CamperOrg, ProgramType program, int FedID, int CampYearID, string CampID_List, string StatusID_List)
	{
		var db = new SQLDBAccess("CIPMS");

		if (CamperOrg == CamperOrgType.EnrollmentConfirmationFJC || CamperOrg == CamperOrgType.EnrollmentConfirmationPartner)
			db.AddParameter("@Action", "GetFJCCamperReportCampInBatch");
		else if (CamperOrg == CamperOrgType.Synagogue)
			db.AddParameter("@Action", "GetFJCCamperReportSynagInBatch");

		if (program != ProgramType.NoUse)
			db.AddParameter("@Program", program);

		if (FedID >= 0)
			db.AddParameter("@FedID", FedID);

		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusID_List", StatusID_List);
		return db.FillDataTable("usprsCamperApplications_Select");
	}

	public static DataTable GetCamperContactInfoReportInBatch(Role userRole, int FedID, string FedID_List, int CampYearID, string CampID_List, string StatusID_List, int TimesReceivedGrant)
	{
		var db = new SQLDBAccess("CIPMS");

		db.AddParameter("@Action", "GetCamperContactInfoReportInBatch");
		db.AddParameter("@FedID_List", FedID_List);
		db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusID_List", StatusID_List);
		return db.FillDataTable("usprsCamperApplications_Select");
	}

    public static DataTable GetCamperDetailReportInBatch(Role userRole, int FedID, string FedID_List, int CampYearID, string CampID_List, string StatusID_List, int TimesReceivedGrant)
    {
        var db = new SQLDBAccess("CIPMS");

        db.AddParameter("@Action", "GetCamperDetailReportInBatch");
        db.AddParameter("@FedID_List", FedID_List);
        db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@CampID_List", CampID_List);
        db.AddParameter("@StatusID_List", StatusID_List);
        return db.FillDataTable("usprsCamperApplications_Select");
    }

	public static DataSet GetCamperCountByFed(int CampYearID, string FedID_List, string StatusName_List, int TimesReceivedGrant)
	{
		var db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "CamperCountByFed");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@FedID_List", FedID_List);
		db.AddParameter("@StatusName_List", StatusName_List);
		db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
		return db.FillDataSet("usprsCamperApplications_Select");
	}

	public static DataSet GetCamperSummaryReportByCamp(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, int TimesReceivedGrant)
	{
		var db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetCamperSummaryReportByCamp");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@FedID_List", FedID_List);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusID_List", StatusID_List);
		db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
		return db.FillDataSet("usprsCamperApplications_Select");
	}

	public static DataSet GetCamperByState(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, int TimesReceivedGrant)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetCamperByState");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@FedID_List", FedID_List);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusID_List", StatusID_List);
		db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
		return db.FillDataSet("usprsCamperApplications_Select");
	}

	public static DataSet GetCamperTimesInCampCountByState(int CampYearID, string FedID_List, string StatusName_List, int TimesReceivedGrant)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "CamperTimesInCampCountByState");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@FedID_List", FedID_List);
		db.AddParameter("@StatusName_List", StatusName_List);
		db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
		return db.FillDataSet("usprsCamperApplications_Select");
	}

	public static DataTable GetCamperCountByCamp(int CampYearID, ProgramType Program, string CampID_List, string StatusName_List, int UserID)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "CamperCountByCampList");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusName_List", StatusName_List);

		//if (Program == ProgramType.JWest)
			db.AddParameter("@Program", Program);

		if (UserID >= 0)
			db.AddParameter("@UserID", UserID);

		return db.FillDataTable("usprsCamperApplications_Select");
	}

	public static DataSet GetFJCAllocationData(int CampYearID)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetFJCAllocation");
		db.AddParameter("@CampYearID", CampYearID);
		return db.FillDataSet("usprsCamperApplications_Select");
	}

	public static DataTable GetCamperSummaryReport(int CampYearID, int FedID, string CampID_List, string StatusID_List)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetCamperSummaryReport");
		db.AddParameter("@CampYearID", CampYearID);
		db.AddParameter("@CampID_List", CampID_List);
		db.AddParameter("@StatusID_List", StatusID_List);
		db.AddParameter("@FedID", FedID);

		return db.FillDataTable("usprsCamperApplications_Select");
	}

	public static DataTable GetCamperApplicationsWithSearchCriteria(int CampYearID, SearchKeys SearchKey, string SearchText)
	{   
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "SearchCamperApps");
		db.AddParameter("@CampYearID", CampYearID);

		string SearchQuery = "";
		if (SearchKey == SearchKeys.Name)
		{
			if (SearchText.Contains(" "))
			{
				string[] names = SearchText.Split(new char[] { });
				SearchQuery = "LastName LIKE '%" + names[1] + "%' AND FirstName LIKE '%" + names[0] + "%'";
			}
			else
				SearchQuery = "LastName LIKE '%" + SearchText + "%' OR FirstName LIKE '%" + SearchText + "%'";
		}
		else if (SearchKey == SearchKeys.Address)
		{
			SearchQuery = "ADDRESS LIKE '%" + SearchText + "%'";
		}
		else if (SearchKey == SearchKeys.Email)
		{
			SearchQuery = "Email LIKE '%" + SearchText + "%'";
		}
		else if (SearchKey == SearchKeys.BirthDate)
		{
			DateTime myDate;
			try
			{
				myDate = DateTime.Parse(SearchText);
			}
			catch
			{
				myDate = DateTime.Now;
			}
			SearchQuery = "DateOfBirth = '" + myDate.ToShortDateString() + "'";
		}

		db.AddParameter("@SearchQueryText", SearchQuery);
		return db.FillDataTable("usprsCamperApplications_Select");
	}
}
