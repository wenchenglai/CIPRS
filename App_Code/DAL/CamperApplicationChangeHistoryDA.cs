using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

/// <summary>
/// Summary description for CamperApplicationChangeHistoryDA
/// </summary>
public class CamperApplicationChangeHistoryDA
{
    public static DataSet GetSummparyByProgramReports(string campYearIDs, DateTime cutoffDate, string fedIDs, string statusNames, bool hasManualData)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "GetFJCCamperReportPerCamp");
        db.AddParameter("@CampYearID_List", campYearIDs);
        db.AddParameter("@CutoffDate", cutoffDate.ToString("MM/dd/yyyy"));
        db.AddParameter("@FedID_List", fedIDs);
        db.AddParameter("@StatusName_List", statusNames);
        db.AddParameter("@hasManualStatus", hasManualData);
        return db.FillDataSet("usprsApplicationChangeHistory_Select");
    }
}