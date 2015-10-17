using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains necessary data for data reporting.  This is created to facilitate the data transfer between pages inside a session object
/// </summary>
/// <summary>
/// Contains necessary data for data reporting.  This is created to facilitate the data transfer between pages inside a session object
/// </summary>

/// <summary>
/// Contains necessary data for data reporting.  This is created to facilitate the data transfer between pages inside a session object
/// </summary>
public class ReportParamCampersFJC
{
    public CamperOrgType CamperOrg { get; set; }
    public ProgramType ProgramTypeID { get; set; }
    public int FedID { get; set; }
    public int CampYearID { get; set; }
    public int CampYear { get; set; }

    public string FedID_List { get; set; }
    public string CampID_List { get; set; }
    public string CampID_HaveData_List { get; set; }
    public string StatusID_List { get; set; }

    public Dictionary<int, string> Fed_Dict { get; set; }
    public Dictionary<int, string> Camp_Dict { get; set; }
    public Dictionary<int, string> CampsThatHaveDataDict { get; set; }
    public Dictionary<int, string> Status_Dict { get; set; }
    
    public int TimesReceivedGrant { get; set; }


    public ReportParamCampersFJC()
    {
        Camp_Dict = new Dictionary<int, string>();
        Status_Dict = new Dictionary<int, string>();
        CampsThatHaveDataDict = new Dictionary<int, string>();
        CamperOrg = CamperOrgType.EnrollmentConfirmationFJC;
        Fed_Dict = new Dictionary<int, string>();
    }

    /// <summary>
    /// Build CampID_List and StatusID_List strings
    /// </summary>
    public void BuildStrings()
    {
        // build a StatusID list string 
        int lastkey = Status_Dict.Last().Key;
        foreach (KeyValuePair<int, string> item in Status_Dict)
        {
            StatusID_List += item.Key;
            if (lastkey != item.Key)
                StatusID_List += ", ";
        }

        // build a CampID list string
        lastkey = Camp_Dict.Last().Key;
        foreach (KeyValuePair<int, string> item in Camp_Dict)
        {
            CampID_List += item.Key;
            if (lastkey != item.Key)
                CampID_List += ", ";
        }

        // build a CampID list string

        if (Fed_Dict.Count > 0)
        {
            lastkey = Fed_Dict.Last().Key;
            foreach (KeyValuePair<int, string> item in Fed_Dict)
            {
                FedID_List += item.Key;
                if (lastkey != item.Key)
                    FedID_List += ", ";
            }
        }
    }
}
