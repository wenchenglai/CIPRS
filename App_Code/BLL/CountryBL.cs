using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CountryBL
/// </summary>
public class CountryBL
{
	public static DataTable GetCountryByCampYearID(int CampYearID)
	{
		DataTable dt = CountryDA.GetCountryByCampYearID(CampYearID);
		return dt;
		//DataTable dtOut = new DataTable();
		//dtOut.Columns.Add(new DataColumn("CountryID", typeof(int)));
		//dtOut.Columns.Add(new DataColumn("CountryName", typeof(string)));

		//DataRow newrow;
		//int temp;
		//foreach(DataRow dr in dt.Rows)
		//{
		//    string name = dr["CountryName"].ToString();

		//    if (!int.TryParse(name, out temp))
		//    {
		//        if (name.Length <= 20)
		//        {
		//            newrow = dtOut.NewRow();
		//            if (dtOut.Rows.Count > 0)
		//                newrow[0] = dtOut.Rows[dtOut.Rows.Count - 1]["CountryID"];
		//            else
		//                newrow[0] = 1;
		//            newrow[1] = name;

		//            dtOut.Rows.Add(newrow);
		//        }
		//    }
		//}

		//return dtOut;
	}
}