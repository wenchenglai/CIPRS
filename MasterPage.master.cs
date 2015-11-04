using System;
using System.Configuration;
using System.Drawing;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Role i_am = (Role)(Convert.ToInt32(Session["RoleID"]));
        if (Session["RoleID"] == null)
        {
            Response.Redirect(Application["RedirectLogin"].ToString());
        }
        else if (i_am == Role.FJCAdmin)
        {
            divOtherLeftMenuPanel.Visible = false;
            divFJCAdminLeftMenuPanel.Visible = true;
        }
        else
        {
            // Currently, it's either FJC Admin or Others (Fed Admin, Camp Director)
            // 2012-11-14 Now we need to differentiate between FJC admin, Fed Admin and Others, primarily for PJ Library
            divOtherLeftMenuPanel.Visible = true;
            divFJCAdminLeftMenuPanel.Visible = false;
        }

        if (!IsPostBack)
        {
            // Get UserID, easier to debug
            string UserID = Session["UserID"] as string;

            if (UserID != null)
            {
                UserBL.SetUserDetailsInSession(UserID);
                lblID.Text = String.Format("{0} {1}", Session["FirstName"], Session["LastName"]);
            }

            // 2012-11-13 Val wants only two people (besides FJC admin) to access SchoolType Report
            //if (Convert.ToInt32(Session["FedID"]) == (int)Fed.PJ)
            //{
            //    LiSchoolTypeReport.Visible = true;
            //}

            // 2012-11-13 Val wants only two people (besides FJC admin) to access SchoolType Report
            if (UserID == "496" || UserID == "392" || UserID == "393")
            {
                LiSchoolTypeReport.Visible = true;
            }

            // get the selected file name to highlight the menu item
            string[] strarray = Request.Url.LocalPath.Split(new char[] { '/' });
            string filename = strarray[strarray.Length - 1];

            if (i_am == Role.FJCAdmin)
            {
                switch (filename)
                {
                    case "CampersByCampFJC.aspx":
                        hylCampersByCampFJC.ForeColor = Color.DarkBlue;
                        break;

                    case "CampersByCampFed.aspx":
                        hylCampersByCampFed.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperDetail.aspx":
                        hylCamperDetails.ForeColor = Color.DarkBlue;
                        break;

                    case "NumberOfCampersByProgram.aspx":
                        hylNumberOfCampersByProgram.ForeColor = Color.DarkBlue;
                        break;

                    case "NumberOfCampersByCamp.aspx":
                        hylNumberOfCampersByCamp.ForeColor = Color.DarkBlue;
                        break;

                    case "FJCAllocationReport.aspx":
                        hylFJCAllocationReport.ForeColor = Color.DarkBlue;
                        break;

                    case "CampersBySynagogue.aspx":
                        hylCampersBySynagogue.ForeColor = Color.DarkBlue;
                        break;

                    case "ProgramProfileReport.aspx":
                        hylProgramProfile.ForeColor = Color.DarkBlue;
                        break;

                    case "SchoolTypeReport.aspx":
                        hylSchoolType.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperSummaryReportByCamp.aspx":
                        hylCamperSummaryReportByCamp.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperApplicationSearchReport.aspx":
                        hylCamperApplicationSearchReport.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperReportByState_TimesInCamp.aspx":
                        hylCamperReportByState_TimesInCamp.ForeColor = Color.DarkBlue;
                        break;

                    case "DataExtract.aspx":
                        hylDataExtract.ForeColor = Color.DarkBlue;
                        break;

                    case "ParentByCountry.aspx":
                        hylParentByCountry.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperContactInfo.aspx":
                        hylCamperContactInfo.ForeColor = Color.DarkBlue;
                        break;

                    case "DuplicateIDReport.aspx":
                        hylDuplicateIDReport.ForeColor = Color.DarkBlue;
                        break;

                    case "SummaryByProgram.aspx":
                        hylSummaryByProgram.ForeColor = Color.DarkBlue;
                        break;

                    case "SummaryByCamp.aspx":
                        hylSummaryByCamp.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperHoldingReport.aspx":
                        hylCamperHolding.ForeColor = Color.DarkBlue;
                        break;

                    case "PaymentProcessing.aspx":
                        hylPaymentProcessing.ForeColor = Color.DarkBlue;
                        break;

                    default:
                        break;
                }
            }
            else 
            {
                switch (filename)
                {
                    case "CampersByCampFed.aspx":
                        hylCampersByCampFed2.ForeColor = Color.DarkBlue;
                        break;

                    case "NumberOfCampersByCamp.aspx":
                        hylNumberOfCampersByCamp2.ForeColor = Color.DarkBlue;
                        break;

                    case "CampersBySynagogue.aspx":
                        hylCampersBySynagogue2.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperSummaryReportByCamp.aspx":
                        hylCamperSummaryReportByCamp2.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperReportByState_TimesInCamp.aspx":
                        hylCamperReportByState_TimesInCamp2.ForeColor = Color.DarkBlue;
                        break;

                    case "DataExtract.aspx":
                        hylDataExtract2.ForeColor = Color.DarkBlue;
                        break;

                    case "ParentByCountry.aspx":
                        hylParentByCountry2.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperContactInfo.aspx":
                        hylCamperContactInfo2.ForeColor = Color.DarkBlue;
                        break;

                    case "SchoolTypeReport.aspx":
                        hylDuplicateIDReport.ForeColor = Color.DarkBlue;
                        break;

                    case "CamperDetail.aspx":
                        hylCamperDetail.ForeColor = Color.DarkBlue;
                        break;

                    case "PaymentProcessing.aspx":
                        hylPaymentProcessing2.ForeColor = Color.DarkBlue;
                        break;

                    default:
                        break;
                }            
            }
        }
    }
    protected void lnkbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        // log out, we must redirect back to the original app instead of delete the cookies.  Should I notify CIPMS admin?
        string loginUrl = ConfigurationManager.AppSettings["CIPMSDefaultLogin"].ToString();
        Response.Redirect(loginUrl);
    }
}
