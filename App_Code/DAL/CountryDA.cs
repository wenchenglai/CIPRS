using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CountryDA
/// </summary>
public class CountryDA
{
    public static DataTable GetCountryByCampYearID(int CampYearID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "SelectAvailableCountry");
        db.AddParameter("@CampYearID", CampYearID);
        return db.FillDataTable("usprsCamperAnswer_Select");
    }
}