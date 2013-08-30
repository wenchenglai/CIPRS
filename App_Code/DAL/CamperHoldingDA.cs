using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CamperHoldingDA
/// </summary>
public class CamperHoldingDA
{
    public static DataTable GetData(int CampYear)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByDateRange");
        db.AddParameter("@CampYear", CampYear);
        return db.FillDataTable("usp_CamperHolding_Select");
    }
}