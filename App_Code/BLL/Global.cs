using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// User Roles - copy from CIPMS db
/// </summary>
public enum Role
{
    Camper = 0,
    FJCAdmin,
    FederationAdmin,
    CampDirector,
    Approver,
    MovementAdmin = 6
}

/// <summary>
/// Meta Program Type
/// </summary>
public enum ProgramType
{
    NoUse = -1,
    CIP = 0,
    JWest = 1,
    All
}

public enum CamperOrgType
{
    EnrollmentConfirmationFJC = 0,
    EnrollmentConfirmationPartner,
    Synagogue,
    CamperContactInfo,
    CamperDetailReport
}

public enum SearchKeys
{
    Name = 0,
    Address,
    Email,
    BirthDate
}
