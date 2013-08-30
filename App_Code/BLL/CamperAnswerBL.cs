using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CamperAnswerBL
/// </summary>
public class CamperAnswerBL
{
    public static DataSet[] GetParentCountryOfOrigin(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, string CountryID_List, int TimesReceivedGrant)
    {
        DataSet ds = CamperAnswerDA.GetParentCountryOfOrigin(CampYearID, FedID_List, CampID_List, StatusID_List, CountryID_List, TimesReceivedGrant);

        // Make all the tables have the same number of rows (camps)
        DataSet dsOutput1 = Transform(ds, true);
        DataSet dsOutput2 = Transform(ds, false);

        Utility.AssignTableNames(dsOutput1, TimesReceivedGrant);
        Utility.AssignTableNames(dsOutput2, TimesReceivedGrant);

        foreach (DataTable dt in dsOutput1.Tables)
        {
            Utility.CreateTotalRow(dt, "Total");
        }

        foreach (DataTable dt in dsOutput2.Tables)
        {
            Utility.CreateTotalRow(dt, "Total");
        }

        return new DataSet[] { dsOutput1, dsOutput2 };
    }

    private static DataSet Transform(DataSet dsIn, bool isFedOrCamp)
    {
        string ColumnName = "", EntityIDName = "";
        int index = 0;
        DataSet dsOut = new DataSet();
        DataTable dtAllSelectedEntity;
        if (isFedOrCamp)
        {
            dtAllSelectedEntity = dsIn.Tables[0]; // first table is always all programs
            ColumnName = "Program";
            EntityIDName = "FedID";
            index = 2;
        }
        else
        {
            dtAllSelectedEntity = dsIn.Tables[1]; // second table is always all camps
            ColumnName = "CampName";
            EntityIDName = "CampID";
            index = 4;
        }
        DataTable dtRaw;

        // Start with table#3
        for (int i = index; i < dsIn.Tables.Count; i+=4)
        {
            DataTable dt = dsIn.Tables[i].Clone();

            dtRaw = dsIn.Tables[i];

            long EntityID;
            DataRow[] tmprows;
            foreach (DataRow dr in dtAllSelectedEntity.Rows)
            {
                EntityID = (int)dr[0];

                DataRow newrow = dt.NewRow();

                newrow[ColumnName] = dr[1].ToString();

                tmprows = dsIn.Tables[i + 1].Select(String.Format("{0} = {1}", EntityIDName, EntityID));
                if (tmprows.Length == 1)
                {
                    newrow["NoOfCamper"] = tmprows[0][1];

                }
                else
                    newrow["NoOfCamper"] = 0;

                tmprows = dtRaw.Select(String.Format("{0} = {1}", EntityIDName, EntityID));

                for (int j = 3; j < dtRaw.Columns.Count; j++)
                {
                    if (tmprows.Length == 1)
                    {
                        newrow[j] = tmprows[0][j];
                    }
                    else
                    {
                        newrow[j] = 0;
                    }
                }

                dt.Rows.Add(newrow);
            }

            dt.Columns.Remove(EntityIDName);
            dsOut.Tables.Add(dt);
        }

        return dsOut;
    }
}