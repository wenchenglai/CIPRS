using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for DuplicateCheckingBL
/// </summary>
public class DuplicateCheckingBL
{
    public static DataTable GetDuplicateCampers(Role myUserRole, int CampYearID, string StatusID_List)
    {
        DataTable dt = null;

        if (myUserRole == Role.FJCAdmin)
        {
            dt = CamperApplicationBL.GetDuplicateCampers(CampYearID, StatusID_List);
        }

        return dt;
    }
}