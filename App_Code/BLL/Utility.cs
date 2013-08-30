using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{
    public static void AssignTableNames(DataSet ds, int TimesReceivedGrant)
    {
        var nameList = new List<string>();

        if ((TimesReceivedGrant & 1) > 0)
            nameList.Add("AllCampers");

        if ((TimesReceivedGrant & 2) > 0)
            nameList.Add("1stYearCampers");

        if ((TimesReceivedGrant & 4) > 0)
            nameList.Add("2ndYearCampers");

        if ((TimesReceivedGrant & 8) > 0)
            nameList.Add("3rdYearCampers");

        int i = 0;
        foreach (DataTable dt in ds.Tables)
        {
            dt.TableName = nameList[i];
            i++;
        }
    }

    public static void CreateTotalColumnAndRow(DataTable dt, string TotalRowName)
    {
        CreateTotalRow(dt, TotalRowName);
        CreateTotalColumn(dt);
    }

    public static void CreateTotalRow(DataTable dt, string TotalRowName)
    {
        // Add new Total row
        DataRow newrow = dt.NewRow();

        newrow[0] = TotalRowName;

        int total = 0;
        for (int i = 1; i < dt.Columns.Count; i++)
        {
            try
            {
                total = Int32.Parse(dt.Compute(String.Format("SUM([{0}])", dt.Columns[i].ColumnName), "").ToString());
            }
            catch
            {
                total = 0;
            }
            newrow[i] = total;
        }
        dt.Rows.Add(newrow);
    }

    public static void CreateTotalColumn(DataTable dt)
    {
        int total = 0;
        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                total += (int)dr[i];
            }
            dr[dt.Columns.Count - 1] = total;
            total = 0;
        }
    }
}