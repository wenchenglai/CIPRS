using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for UserDA
/// </summary>
public class UserDA
{
	public static bool Authenticate(string EmailID, string Password)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@EmailID", EmailID);
		db.AddParameter("@Password", Password);
		IDataReader dr = db.ExecuteReader("usp_ValidateLogin");
        return dr.Read();
	}

	public static bool AuthenticateByUserID(string UserID, out DataTable UserInfo)
	{
		SQLDBAccess db = new SQLDBAccess("CIPMS");
		db.AddParameter("@Action", "GetUserDetailByUserID");
		db.AddParameter("@UserID", UserID);
        UserInfo = db.FillDataTable("usprsUser_Select");
        return UserInfo.Rows.Count > 0;
	}

    public static bool AuthenticateAndGetUserProfile(string EmailID, string Password, out DataSet dsRetLogin)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");

        db.AddParameter("@EmailID", EmailID);
        db.AddParameter("@Password", Password);

        DataSet dsLogin = db.FillDataSet("usp_ValidateLogin");

        dsRetLogin = dsLogin;

        return dsLogin.Tables[0].Rows.Count > 0 ? true : false;
    }
}
