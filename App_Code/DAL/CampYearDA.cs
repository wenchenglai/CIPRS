using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

/// <summary>
/// Summary description for CampYearDA
/// </summary>
public class CampYearDA
{
    public static DataTable GetAllYearsWithoutCurrentYear()
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "GetAllYearsWithoutCurrentYear");

        return db.FillDataTable("usprsCampYear_Select");
    }
}