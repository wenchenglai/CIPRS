using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for CampsBL
/// </summary>
public class CampsBL
{
    public static DataTable GetAllCampsByYearIDAndFedList(int CampYearID, object FedList)
    {
        var myFedList = (ListItemCollection)FedList;

        string FedIDList = "";
        foreach (ListItem li in myFedList)
        {
            if (li.Selected)
            {
                FedIDList += li.Value;
                FedIDList += ", ";
            }
        }

        // in case there is no fed selected, we must have some data in FedIDList else the T-SQL will return error
        if (FedIDList != "")
            FedIDList = FedIDList.Substring(0, FedIDList.Length - 2);
        else
        {
            Role userRole = (Role)(Int32.Parse(HttpContext.Current.Session["RoleID"].ToString()));
            if (userRole == Role.CampDirector)
            {
                return CampsDA.GetCampByCampID(CampYearID, (int)Int32.Parse(HttpContext.Current.Session["UserID"].ToString()));
            }
            else
                FedIDList = "-987654";
        }


        return CampsDA.GetAllCampsByYearIDAndFedIDList(CampYearID, FedIDList);
    }
}