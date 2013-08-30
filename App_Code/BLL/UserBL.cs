using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for UserBL
/// </summary>
public class UserBL
{
    public static bool SetUserDetailsInSession(string UserID)
    {
        DataTable dt;
        bool isAuth = UserDA.AuthenticateByUserID(UserID, out dt);

        if (isAuth)
        {
            DataRow dr = dt.Rows[0];
            SetSessionVariables(dr, "UserID");
            SetSessionVariables(dr, "RoleID");
            SetSessionVariables(dr, "RoleName");
            SetSessionVariables(dr, "FirstName");
            SetSessionVariables(dr, "LastName");
            SetSessionVariables(dr, "FedID");
            SetSessionVariables(dr, "FedName");
            //HttpContext.Current.Session["UserID"] = dr["ID"].ToString();
            //HttpContext.Current.Session["RoleID"] = dr["UserRole"].ToString();
            //HttpContext.Current.Session["RoleName"] = dr["RoleName"].ToString();
            //HttpContext.Current.Session["FirstName"] = dr["FirstName"].ToString();
            //HttpContext.Current.Session["LastName"] = dr["LastName"].ToString();
            //HttpContext.Current.Session["FedID"] = dr["Federation"].ToString();
            //HttpContext.Current.Session["FedName"] = dr["FedName"].ToString();
        }

        return true;
    }

    private static void SetSessionVariables(DataRow dr, string name)
    {
        HttpContext.Current.Session[name] = dr[name].ToString();
    }
}
