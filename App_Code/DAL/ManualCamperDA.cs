using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for ManualCamperDA
/// </summary>
public class ManualCamperDA
{
	public static DataTable GetAll()
	{
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        return db.FillDataTable("usprsManualCampers_Select");
	}
}
