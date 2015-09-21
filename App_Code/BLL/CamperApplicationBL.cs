using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CamperApplicationBL
/// </summary>
public class CamperApplicationBL
{
    public static DataSet GetCamperCountByFed(int CampYearID, string FedID_List, string StatusName_List, int TimesReceivedGrant)
    {
        StatusName_List += ", [Total By Program]";
        DataSet ds = CamperApplicationDA.GetCamperCountByFed(CampYearID, FedID_List, StatusName_List, TimesReceivedGrant);

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
        foreach (string name in nameList)
        {
            ds.Tables[i].TableName = name;
            i++;
        }

        foreach (DataTable dt in ds.Tables)
        {
            Utility.CreateTotalColumnAndRow(dt, "Total By Status");        
        }

        return ds;
    }

    public static DataSet GetCamperSummaryReportByCamp(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, int TimesReceivedGrant)
    {
        DataSet ds = CamperApplicationDA.GetCamperSummaryReportByCamp(CampYearID, FedID_List, CampID_List, StatusID_List, TimesReceivedGrant);

        // Make all the tables have the same number of rows (camps)
        DataSet dsOutput = TransformTablesForCamperSummaryReportByCamp(ds);

        Utility.AssignTableNames(dsOutput, TimesReceivedGrant);

        // Create the total row for each table
        foreach (DataTable dt in dsOutput.Tables)
        {
            Utility.CreateTotalRow(dt, "Total");
        }

        return dsOutput;
    }

    public static DataSet GetCamperByState(int CampYearID, string FedID_List, string CampID_List, string StatusID_List, int TimesReceivedGrant)
    {
        using (DataSet ds = CamperApplicationDA.GetCamperByState(CampYearID, FedID_List, CampID_List, StatusID_List, TimesReceivedGrant))
        {
            DataSet dsOutput = TransformTablesForCamperSummaryByState(ds);
            Utility.AssignTableNames(dsOutput, TimesReceivedGrant);
            foreach (DataTable dt in dsOutput.Tables)
            {
                Utility.CreateTotalColumnAndRow(dt, "Total");
            }
            return dsOutput;
        }
    }

    private static DataSet TransformTablesForCamperSummaryByState(DataSet dsIn)
    {
        const string CampName = "CampName";

        DataSet dsOut = new DataSet();
        DataTable dtAllSelectedCamps = dsIn.Tables[0];
        DataTable dtRaw;

        for (int i = 1; i < dsIn.Tables.Count; i++)
        {
            DataTable dt = dsIn.Tables[i].Clone();

            dtRaw = dsIn.Tables[i];

            long CampID;
            DataRow[] tmprows;
            foreach (DataRow dr in dtAllSelectedCamps.Rows)
            {
                CampID = (long)dr[0];

                DataRow newrow = dt.NewRow();

                newrow[CampName] = dr[1].ToString();

                tmprows = dtRaw.Select("CampID = " + CampID.ToString());

                for (int j = 2; j < dtRaw.Columns.Count; j++)
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

            dt.Columns.Remove("CampID");
            dsOut.Tables.Add(dt);
        }

        return dsOut;
    }

    /// <summary>
    /// Make all the tables have the same number of rows (camps)
    /// Calculate the appropriate columns (Days in camp)
    /// </summary>
    /// <param name="dsIn"></param>
    /// <returns></returns>
    private static DataSet TransformTablesForCamperSummaryReportByCamp(DataSet dsIn)
    {
        const string CampName = "Camp Name";
        const string NumberOfCampers = "# of Campers";
        const string DayRangeOne = "12-17 days";
        const string DayRangeTwo = "18 days";
        const string DayRangeThree = "19+ days";

        DataSet dsOut = new DataSet();
        DataTable dtAllSelectedCamps = dsIn.Tables[0];
        DataTable dtRaw, dtCount;

        for (int i = 1; i < dsIn.Tables.Count; i += 2)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(CampName, System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn(NumberOfCampers, System.Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn(DayRangeOne, System.Type.GetType("System.Int32"))); // need this for the correct column order in excel report
            dt.Columns.Add(new DataColumn(DayRangeTwo, System.Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn(DayRangeThree, System.Type.GetType("System.Int32")));

            dtRaw = dsIn.Tables[i];
            dtCount = dsIn.Tables[i + 1];

            long CampID;
            foreach (DataRow dr in dtAllSelectedCamps.Rows)
            {
                CampID = (long)dr[0];

                DataRow newrow = dt.NewRow();

                newrow[CampName] = dr[1].ToString();

                DataRow[] tmprows = dtCount.Select("CampID = " + CampID.ToString());
                if (tmprows.Length > 0)
                    newrow[NumberOfCampers] = tmprows[0]["TotalCount"];
                else
                {
                    newrow[NumberOfCampers] = 0;
                }

                // set all other columns to zero so we can have calculation below
                newrow[DayRangeOne] = 0;
                newrow[DayRangeTwo] = 0;
                newrow[DayRangeThree] = 0;

                if ((int)newrow[NumberOfCampers] > 0)
                {
                    tmprows = dtRaw.Select("CampID = " + CampID);
                    if (tmprows.Length >= 0)
                    {
                        for (int j = 0; j < tmprows.Length; j += 2)
                        {
                            DateTime startDate = TransformIntoDateTime(tmprows[j]["Answer"].ToString());
                            // we need to add one more day because TimeSpan would not consider the last day
                            DateTime endDate = TransformIntoDateTime(tmprows[j + 1]["Answer"].ToString()).AddDays(1);

                            TimeSpan ts = endDate - startDate;

                            if (ts.Days == 18)
                            {
                                newrow[DayRangeTwo] = (int)newrow[DayRangeTwo] + 1;
                            }
                            else if (ts.Days > 18)
                            {
                                newrow[DayRangeThree] = (int)newrow[DayRangeThree] + 1;
                            }
                            else if (ts.Days >= 12)
                            {
                                newrow[DayRangeOne] = (int)newrow[DayRangeOne] + 1;
                            }
                        }
                    }
                }
                dt.Rows.Add(newrow);
            }

            dsOut.Tables.Add(dt);
        }

        return dsOut;
    }

    private static DateTime TransformIntoDateTime(string date_string)
    {
        string[] splits = date_string.Split(new char[]{'/'});

        if (splits.Length == 3)
        {
            int Year;
            if (splits[2].Length == 2)
                Year = int.Parse(splits[2]) + 2000;
            else
                Year = int.Parse(splits[2]);
            return new DateTime(Year, int.Parse(splits[0]), int.Parse(splits[1]));
        }

        splits = null;
        splits = date_string.Split(new char[] { '-' });

        if (splits.Length == 3)
        {
            int Year;
            if (splits[2].Length == 2)
                Year = int.Parse(splits[2]) + 2000;
            else
                Year = int.Parse(splits[2]);
            return new DateTime(Year, int.Parse(splits[0]), int.Parse(splits[1]));
        }

        return DateTime.Now;

    }

    public static DataTable GetCamperCountByCamp(int CampYearID, ProgramType Program, string CampID_List, string StatusName_List, int UserID)
    {
        StatusName_List += ", [Total By Camp]";
        DataTable dt = CamperApplicationDA.GetCamperCountByCamp(CampYearID, Program, CampID_List, StatusName_List, UserID);

        Utility.CreateTotalColumnAndRow(dt, "Total By Status");

        return dt;
    }

    /// <summary>
    /// Used for Excel reporting
    /// </summary>
    /// <param name="program"></param>
    /// <param name="FedID"></param>
    /// <param name="CampYearID"></param>
    /// <param name="CampID_List"></param>
    /// <param name="StatusID_List"></param>
    /// <returns></returns>
    public static DataSet GetFJCCamperReportInBatch(CamperOrgType CamperOrg, ProgramType program, int FedID, int CampYearID, string CampID_List, string StatusID_List, int TimesReceivedGrant)
    {
        DataTable dt;
        dt = CamperApplicationDA.GetFJCCamperReportInBatch(CamperOrg, program, FedID, CampYearID, CampID_List, StatusID_List);
        DataSet ds = new DataSet();

        // now, create each table per camp, and store each table into a dataset
        if (dt.Rows.Count > 0)
        {
            int LastCampID = Convert.ToInt32(dt.Rows[0]["CampID"]);
            int CampID = LastCampID;
            DataTable dtTemp = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                CampID = Convert.ToInt32(dr["CampID"]);

                if (CampID != LastCampID)
                {
                    ds.Tables.Add(dtTemp);
                    dtTemp = null;
                    dtTemp = dt.Clone();
                }

                LastCampID = CampID;

                dtTemp.ImportRow(dr);      
            }
            // the last table must also be added because it never goes into the if condition right above
            ds.Tables.Add(dtTemp);
        }

        // Delete the first column, because we don't need to display it
        foreach (DataTable dtt in ds.Tables)
        {
            dtt.Columns.RemoveAt(0); // Camp ID column
        }

        return ds;
    }

    public static DataSet GetCamperContactInfoReportInBatch(Role UserRole, int FedID, string FedID_List, int CampYearID, string CampID_List, string StatusID_List, int TimesReceivedGrant)
    {
        DataTable dt;
        dt = CamperApplicationDA.GetCamperContactInfoReportInBatch(UserRole, FedID, FedID_List, CampYearID, CampID_List, StatusID_List, TimesReceivedGrant);
        DataSet ds = new DataSet();

        // now, create each table per camp, and store each table into a dataset
        if (dt.Rows.Count > 0)
        {
            int LastCampID = Convert.ToInt32(dt.Rows[0]["CampID"]);
            int CampID = LastCampID;
            DataTable dtTemp = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                CampID = Convert.ToInt32(dr["CampID"]);

                if (CampID != LastCampID)
                {
                    ds.Tables.Add(dtTemp);
                    dtTemp = null;
                    dtTemp = dt.Clone();
                }

                LastCampID = CampID;

                dtTemp.ImportRow(dr);
            }
            // the last table must also be added because it never goes into the if condition right above
            ds.Tables.Add(dtTemp);
        }

        // Delete the first column, because we don't need to display it
        foreach (DataTable dtt in ds.Tables)
        {
            dtt.Columns.RemoveAt(0);
        }

        return ds;
    }

    public static DataSet GetCamperDetailReportInBatch(Role UserRole, int FedID, string FedID_List, int CampYearID, string CampID_List, string StatusID_List, int TimesReceivedGrant)
    {
        DataTable dt;
        dt = CamperApplicationDA.GetCamperDetailReportInBatch(UserRole, FedID, FedID_List, CampYearID, CampID_List, StatusID_List, TimesReceivedGrant);
        var ds = new DataSet();

        // now, create each table per camp, and store each table into a dataset
        if (dt.Rows.Count > 0)
        {
            int LastCampID = Convert.ToInt32(dt.Rows[0]["CampID"]);
            int CampID = LastCampID;
            DataTable dtTemp = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                CampID = Convert.ToInt32(dr["CampID"]);

                if (CampID != LastCampID)
                {
                    ds.Tables.Add(dtTemp);
                    dtTemp = null;
                    dtTemp = dt.Clone();
                }

                LastCampID = CampID;

                dtTemp.ImportRow(dr);
            }
            // the last table must also be added because it never goes into the if condition right above
            ds.Tables.Add(dtTemp);
        }

        // Delete the first column, because we don't need to display it
        foreach (DataTable dtt in ds.Tables)
        {
            dtt.Columns.RemoveAt(0);
        }

        return ds;
    }

    public static DataTable GetCamperSummaryReport(int CampYearID, int FedID, string CampID_List, string StatusID_List)
    {
        DataTable dt = CamperApplicationDA.GetCamperSummaryReport(CampYearID, FedID, CampID_List, StatusID_List);

        // we have to calculate total as a new column
        int total = 0;
        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                if (!(dr[i] == DBNull.Value))          
                    total += (int)dr[i];
            }
            dr[dt.Columns.Count - 1] = total;
            total = 0;
        }

        // the default column name is -1 set in store procedure, we have to change it back
        dt.Columns[dt.Columns.Count - 1].ColumnName = "Total";

        return dt;
    }

    public static DataSet GetCamperTimesInCampCountByState(int CampYearID, string FedID_List, string StatusName_List, int TimesReceivedGrant)
    {
        StatusName_List += ", [Total By Program]";
        DataSet ds = CamperApplicationDA.GetCamperCountByFed(CampYearID, FedID_List, StatusName_List, TimesReceivedGrant);

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
        foreach (string name in nameList)
        {
            ds.Tables[i].TableName = name;
            i++;
        }

        foreach (DataTable dt in ds.Tables)
        {
            Utility.CreateTotalColumnAndRow(dt, "Total By Status");
        }

        return ds;
    }

    public static DataTable GetDuplicateCampers(int CampYearID, string StatusID_List)
    {
        DataTable dt = CamperApplicationDA.GetDuplicateCampers(CampYearID, StatusID_List);

        //var nameList = new List<string>();
        //nameList.Add("AllCampers");

        //int i = 0;
        //foreach (string name in nameList)
        //{
        //    ds.Tables[i].TableName = name;
        //    i++;
        //}

        //foreach (DataTable dt in ds.Tables)
        //{
        //    Utility.CreateTotalColumnAndRow(dt, "Total By Status");
        //}

        return dt;
    }
}
