using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.ComponentModel;
/// <summary>
/// Summary description for FedManager
/// </summary>
public class CIPMSManager
{
    public static DataSet getAllFederations()
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        return db.FillDataSet("usp_GetAllFederations");
    }

    public static DataSet GetFederationAndQuestionnaireDetails(string strFederationIds)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@FederationIds", strFederationIds);
        return db.FillDataSet("usp_GetFederationAndQuestionnaireDetails");
    }

    public static string GetGradeEligibilityRange(int FederationID)
    {
        SQLDBAccess db = new SQLDBAccess("CIPMS");
        db.AddParameter("@FederationIds", FederationID.ToString());
        DataSet ds = db.FillDataSet("usp_GetFederationAndQuestionnaireDetails");
        return ds.Tables[0].Rows[0][0].ToString();
    }

    public static String GetEnumDescription(Enum e)
    {
        FieldInfo fieldInfo = e.GetType().GetField(e.ToString());

        DescriptionAttribute[] enumAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (enumAttributes.Length > 0)
        {
            return enumAttributes[0].Description;
        }
        return e.ToString();
    }
}