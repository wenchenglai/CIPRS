using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

/// <summary>
/// Summary description for ViewDumpForAllYearsDA
/// </summary>
public class ViewDumpForAllYearsDA
{
    public static DataTable GetDataExtract(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, int TimesReceivedGrant)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "DataExtract");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@FedID_List", FedID_List);
        db.AddParameter("@CampID_List", CampID_List);
        db.AddParameter("@StatusID_List", StatusID_List);
        db.AddParameter("@TimesReceivedGrant", TimesReceivedGrant);
        return db.FillDataTable("ViewDumpForAllYears_Select");
    }
}