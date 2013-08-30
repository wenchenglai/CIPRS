using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for StatusDA
/// </summary>
public class StatusDA
{
    public static DataTable GetAllStatusUsedInCamperApplications(bool ExcludeIncompleteStatus)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        if (ExcludeIncompleteStatus)
            db.AddParameter("@Action", "UsedInCamperApplicationsExcludesIncomplete");
        else
            db.AddParameter("@Action", "UsedInCamperApplications");

        return db.FillDataTable("usprsStatus_Select");
    }

    public static DataTable GetMostUsedStatus()
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "GetMostUsedStatus");
        return db.FillDataTable("usprsStatus_Select");
    }

    public static DataTable GetStatusUsedByFedID(int FedID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByFedID");
        db.AddParameter("@FederationID", FedID);

        return db.FillDataTable("usprsStatus_Select");
    }
}
