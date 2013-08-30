using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CampsDA
/// </summary>
public class CampsDA
{
    public static DataTable GetAllCampsByFedID(int CampYearID, int FedID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByFedID");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@FedID", FedID);
        return db.FillDataTable("usprsCamps_Select");
    }

    public static DataTable GetAllCampsByYearIDAndFedIDList(int CampYearID, string FedIDList)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByFedIDAndFedIDList");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@FedIDList", FedIDList);
        return db.FillDataTable("usprsCamps_Select");
    }

    public static DataTable GetCampByCampID(int CampYearID, int UserID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByCampID");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@UserID", UserID);
        return db.FillDataTable("usprsCamps_Select");
    }

    /// <summary>
    /// FJC Admin would use this method to see all the data
    /// </summary>
    /// <param name="CampYearID"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static DataTable GetAllCampsByProgram(int CampYearID, ProgramType type)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByProgram");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@Program", type);
        return db.FillDataTable("usprsCamps_Select");
    }

    /// <summary>
    /// People who are not FJC admin (such as Federation Admin) will need the filter of Fed ID
    /// </summary>
    /// <param name="CampYearID"></param>
    /// <param name="type"></param>
    /// <param name="RoleID"></param>
    /// <param name="UserID"></param>
    /// <returns></returns>
    public static DataTable GetAllCampsByRoleID(int CampYearID, ProgramType type, Role RoleID, int UserID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");

        if (RoleID == Role.FederationAdmin)
        {
            db.AddParameter("@Action", "ByUserID");
            db.AddParameter("@CampYearID", CampYearID);
            db.AddParameter("@UserID", UserID);
        }
        else if (RoleID == Role.CampDirector)
        {
            db.AddParameter("@Action", "ByCampDirector");
            db.AddParameter("@UserID", UserID);
            db.AddParameter("@CampYearID", CampYearID);
        }
        else
        {
            db.AddParameter("@Action", "ByProgram");
            db.AddParameter("@Program", type);
            db.AddParameter("@CampYearID", CampYearID);
        }

        return db.FillDataTable("usprsCamps_Select");
    }

    public static DataTable GetCampByJWestCampDirector(int CampYearID, int UserID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@Action", "ByCampDirector");
        db.AddParameter("@CampYearID", CampYearID);
        db.AddParameter("@UserID", UserID);
        return db.FillDataTable("usprsCamps_Select");
    }
}
