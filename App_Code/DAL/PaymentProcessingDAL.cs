using System.Collections.Generic;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for PaymentProcessingDA
/// </summary>
public class PaymentProcessingDAL
{
    public static DataTable GetReport(int campYearId, int fedId, IList<int> campIdList, bool isFinal)
    {
        var db = new SQLDBAccess("CIPMS");

        var spName = "usp_PaymentProcessingSelfFuncingPreliminary";
        if (isFinal)
        {
            spName = "usp_PaymentProcessingSelfFuncingFinalMode";
            db.AddParameter("@UserID", HttpContext.Current.Session["UserID"]);
        }

        db.AddParameter("@CampYearID", campYearId);
        db.AddParameter("@FedID", fedId);

        var dt = new DataTable();
        dt.Columns.Add("CampID");

        foreach (var id in campIdList)
        {
            var row = dt.NewRow();
            row["CampID"] = id;
            dt.Rows.Add(row);
        }

        db.AddParameterWithValue("@CampIDList", dt);
 
        return db.FillDataTable(spName);
    }

    public static DataTable GetSummary(int campYearId, int fedId)
    {
        var db = new SQLDBAccess("CIPMS");

        db.AddParameter("@CampYearID", campYearId);
        db.AddParameter("@FedID", fedId);
        return db.FillDataTable("usp_PaymentProcessingSummary");
    }
}