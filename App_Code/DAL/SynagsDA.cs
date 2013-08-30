using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for SynagsDA
/// </summary>
public class SynagsDA
{
    public static DataTable GetAllSynagByFedID(int CampYearID, int FedID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByFedIDCampYearID");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@FedID", FedID);
        return db.FillDataTable("usprsSynagogues_Select");
    }
}
