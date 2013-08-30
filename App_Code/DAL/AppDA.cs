using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

public class AppData
{
    public int CampYearID { get; set; }
    public int CampYear { get; set; }
}
/// <summary>
/// Summary description for AppDA
/// </summary>
public class AppDA
{
    public static AppData GetAppLevelData()
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "GetAppData");

        IDataReader dr = db.ExecuteReader("usprsCampYear_Select");

        if (dr.Read())
        {
            var data = new AppData() { CampYear = Convert.ToInt32(dr["CampYear"]), CampYearID = Convert.ToInt32(dr["ID"]) };

            return data;
        }

        return null;
    }
}