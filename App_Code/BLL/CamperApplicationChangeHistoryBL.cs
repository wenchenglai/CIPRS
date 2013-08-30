using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

/// <summary>
/// Summary description for CamperApplicationChangeHistoryBL
/// </summary>
public class CamperApplicationChangeHistoryBL
{
    public static string NonDaySchoolColumn = "Public, Private, Home School";
    public static string TotalByProgram = "Total";

    public static DataSet GetSummaryByProgramReports(string campYearIDs, DateTime cutoffDate, string fedIDs, string statusNames, int timesReceivedGrant, Dictionary<int, string> feds, Dictionary<int, string> stati, List<int> yearList)
    {
        DataSet ds = CamperApplicationChangeHistoryDA.GetSummparyByProgramReports(campYearIDs, cutoffDate, fedIDs, statusNames, statusNames.Contains("Payment Requested Manual"));
        bool currentYearOnly = yearList.Count == 0;
        // I need to know if the selected date is future date for this year, to show special note
        bool isFuture = new DateTime(DateTime.Today.Year, cutoffDate.Month, cutoffDate.Day) > DateTime.Today;
        var dsRet = new DataSet();

        if ((timesReceivedGrant & 1) > 0)
        {
            dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[0], "CurrentYearAll", "TimeInCamp", feds, stati, currentYearOnly, isFuture));
        }

        if ((timesReceivedGrant & 2) > 0)
        {
            dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[0], "CurrentYear1st", "1", feds, stati, currentYearOnly, isFuture));
        }

        if ((timesReceivedGrant & 4) > 0)
        {
            dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[0], "CurrentYear2nd", "2", feds, stati, currentYearOnly, isFuture));
        }

        if ((timesReceivedGrant & 8) > 0)
        {
            dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[0], "CurrentYear3rd", "3", feds, stati, currentYearOnly, isFuture));
        }

        if (yearList.Any(y => y == 2012))
        {
            if ((timesReceivedGrant & 1) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[1], "2012All", "TimeInCamp", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 2) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[1], "20121st", "1", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 4) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[1], "20122nd", "2", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 8) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[1], "20123rd", "3", feds, stati, currentYearOnly, isFuture));
            }
        }

        if (yearList.Any(y => y == 2011))
        {
            if ((timesReceivedGrant & 1) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[2], "2011All", "TimeInCamp", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 2) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[2], "20111st", "1", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 4) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[2], "20112nd", "2", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 8) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[2], "20113rd", "3", feds, stati, currentYearOnly, isFuture));
            }
        }

        if (yearList.Any(y => y == 2010))
        {
            if ((timesReceivedGrant & 1) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[3], "2010All", "TimeInCamp", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 2) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[3], "20101st", "1", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 4) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[3], "20102nd", "2", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 8) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[3], "20103rd", "3", feds, stati, currentYearOnly, isFuture));
            }
        }

        if (yearList.Any(y => y == 2009))
        {
            if ((timesReceivedGrant & 1) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[4], "2009All", "TimeInCamp", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 2) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[4], "20091st", "1", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 4) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[4], "20092nd", "2", feds, stati, currentYearOnly, isFuture));
            }

            if ((timesReceivedGrant & 8) > 0)
            {
                dsRet.Tables.Add(ConstructBindingTargetTable(ds.Tables[4], "20093rd", "3", feds, stati, currentYearOnly, isFuture));
            }
        }

        return dsRet;
    }

    private static DataTable ConstructBindingTargetTable(DataTable dtSource, string tableName, string timeInCamp, Dictionary<int, string> feds, Dictionary<int, string> stati, bool currentYearOnly, bool isFuture)
    {
        // if it's current year, we show all the selected status, else, we just show the 5 eligible status for current year ONLY
        bool isCurrentYear = tableName.Substring(0, 1) == "C";
        var rightStati = isCurrentYear ? stati : GetEligibleStati(stati.Keys.Contains(39));

        DataTable dtCurrent = GetNewTable(tableName, rightStati, currentYearOnly);

        DataRow dr;
        int totalByProgram = 0, totalDaySchool = 0, totalUnknownSchool = 0;
        var totalStatusCounts = new Dictionary<string, int>();
        foreach (var status in rightStati)
        {
            totalStatusCounts.Add(status.Value, 0);
        }

        foreach (var fed in feds)
        {
            var statusCounts = new Dictionary<string, int>();
            foreach (var status in rightStati)
            {
                DataRow[] drs = dtSource.Select(String.Format("FedID = {0} AND ChangedValue = '{1}' AND TimeInCamp = {2}", fed.Key, status.Value, timeInCamp));
                statusCounts.Add(status.Value, drs.Length);
                totalStatusCounts[status.Value] += drs.Length;
            }

            dr = dtCurrent.NewRow();
            dr["Name"] = fed.Value;

            if (currentYearOnly)
                foreach (var item in statusCounts)
                {
                    dr[item.Key] = item.Value;
                }

            int total = statusCounts.Sum(d => d.Value);
            dr[TotalByProgram] = total;
            totalByProgram += total;

            int daySchoolCount = dtSource.Select(String.Format("FedID = {0} AND SchoolType = 4 AND TimeInCamp = {1}", fed.Key, timeInCamp)).Length;
            totalDaySchool += daySchoolCount;
            dr["Day School"] = daySchoolCount;

            int schoolUnknownCount = dtSource.Select(String.Format("FedID = {0} AND SchoolType = -1 AND TimeInCamp = {1}", fed.Key, timeInCamp)).Length;
            totalUnknownSchool += schoolUnknownCount;
            dr["School Type Unknown"] = schoolUnknownCount;

            dr[NonDaySchoolColumn] = total - daySchoolCount - schoolUnknownCount;

            // special footnote section
            // these contains tons of special code that has to show different footnote to the users
            // For cincinnati, which as offline data only
            if (fed.Key == 1)
                dr["Note"] = "Has offline data only";
            // for any date that's future date for this year, we show special note
            if (isCurrentYear)
                if (isFuture)
                {
                    string note = "Total numbers for current year are as of today’s date";
                    if (dr["Note"] == DBNull.Value)
                        dr["Note"] = note;
                    else
                        dr["Note"] = string.Format("{0};{1}", dr["Note"], note);
                }

            dtCurrent.Rows.Add(dr);
        }

        // The last row of a table is always the Total row
        dr = dtCurrent.NewRow();
        dr["Name"] = "Total By Status";

        // selected Status columns
        if (currentYearOnly)
            foreach (var item in totalStatusCounts)
            {
                dr[item.Key] = item.Value;
            }

        dr[TotalByProgram] = totalByProgram;
        dr[NonDaySchoolColumn] = totalByProgram - totalDaySchool - totalUnknownSchool;
        dr["Day School"] = totalDaySchool;
        dr["School Type Unknown"] = totalUnknownSchool;


        dtCurrent.Rows.Add(dr);

        return dtCurrent;
    }

    private static DataTable GetNewTable(string tableName, Dictionary<int, string> stati, bool currentYearOnly)
    {
        var table = new DataTable(tableName);

        table.Columns.Add(new DataColumn("Name", typeof(string)));

        // Only current year will show all status, if user select any other year, then we don't show status
        if (currentYearOnly)
            foreach (var status in stati)
            {
                table.Columns.Add(new DataColumn(status.Value, typeof(string)));
            }

        table.Columns.Add(new DataColumn(TotalByProgram, typeof(int)));
        table.Columns.Add(new DataColumn(NonDaySchoolColumn, typeof(int)));        
        table.Columns.Add(new DataColumn("Day School", typeof(int)));
        table.Columns.Add(new DataColumn("School Type Unknown", typeof(int)));
        table.Columns.Add(new DataColumn("Note", typeof(string)));

        return table;
    }

    // for any past years, we only show these 4 status no matter what.  Also, if user select manual data, then we must also add it
    private static Dictionary<int, string> GetEligibleStati(bool hasManualData)
    {
        if (hasManualData)
            return new Dictionary<int, string> 
            {  
                {14, "Campership approved; payment pending"},
                {1, "Eligible"},
                {7, "Eligible by staff"},
                {25, "Payment requested"},
                {39, "Payment Requested Manual"}
            };
        else
            return new Dictionary<int, string> 
            {  
                {14, "Campership approved; payment pending"},
                {1, "Eligible"},
                {7, "Eligible by staff"},
                {25, "Payment requested"}
            };
    }
}