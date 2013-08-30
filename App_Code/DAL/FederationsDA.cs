using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for FederationsDA
/// </summary>
public class FederationsDA
{
    public static DataTable GetAllFederations(int CampYearID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "All");
        db.AddParameter("@CampYearID", CampYearID);
        return db.FillDataTable("usprsFederations_Select");
    }

    public static DataTable GetAllFederationsByUserRole(int CampYearID, Role UserRole, int FedID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "All");
        db.AddParameter("@CampYearID", CampYearID);
        if (UserRole != Role.FJCAdmin)
            db.AddParameter("@FedID", FedID);
        return db.FillDataTable("usprsFederations_Select");
    }

    public static DataTable GetAllFederationsByMultipleCampYearsAndUserRole(string CampYearID_String, Role UserRole, int FedID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByMultipleYears");
        db.AddParameter("@CampYearID_String", CampYearID_String);
        if (UserRole != Role.FJCAdmin)
            db.AddParameter("@FedID", FedID);
        return db.FillDataTable("usprsFederations_Select");
    }
}
