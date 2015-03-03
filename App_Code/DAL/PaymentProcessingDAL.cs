using System.Data;

/// <summary>
/// Summary description for PaymentProcessingDA
/// </summary>
public class PaymentProcessingDAL
{
    public static DataTable GetPreliminaryReport(int campYearId, int fedId, string campIdList)
    {
        var db = new SQLDBAccess("CIPMS");
        db.AddParameter("@CampYearID", campYearId);
        db.AddParameter("@FedID", fedId);
        db.AddParameter("@CampID_List", campIdList);
        return db.FillDataTable("usp_PaymentProcessingCore");
    }
}