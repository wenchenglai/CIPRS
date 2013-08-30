using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CamperAnswerDA
/// </summary>
public class CamperAnswerDA
{
    public static DataSet GetParentCountryOfOrigin(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, string CountryID_List, int TimesReceivedGrant)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "GetParentCountry");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@FedID_List", FedID_List);
        db.AddParameter("@CampID_List", CampID_List);
        db.AddParameter("@StatusID_List", StatusID_List);
        db.AddParameter("@CountryID_List", CountryID_List);
        db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
        return db.FillDataSet("usprsCamperAnswer_Select");
    }

    public static DataSet GetSchoolTypeReport(int CampYearID, int FedID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "SchoolTypeReport");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@FedID", FedID);
        return db.FillDataSet("usprsCamperAnswer_Select");
    }
}