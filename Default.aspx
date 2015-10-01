<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">Report Menu - Descriptions</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--    Welcome to the new report application.  You are logged in as <asp:Label ID="lblName" runat="server"></asp:Label>
    <br />
    Your Authentication: <asp:Label ID="lblInfo" runat="server"></asp:Label>--%>
<div id="divOtherContent" runat="server" style="font-size:small">
    <h2>Administrative Reports</h2>
    <div class="row">
        <div style="font-weight:bold;">
            <a href="CampersByCampFed.aspx">Camper Enrollment Confirmation Report</a>: List of campers who have applied for a grant.<br />
        </div>
        <p>
            Pull list of “Eligible; Admin Approved” campers to send to each camp to confirm camper registration and session information.
        </p>
        <p>
            Important: be sure to only send each camp their list of campers. 
        </p>
        <p>
            Info included: Registration confirmation column (for camps to fill out), FJC ID, camper name, gender, grade, session dates, grant amount, and parent contact.
        </p>
    </div>
    
    <div class="row">
        <div style="font-weight:bold;">
            <a href="CamperDetail.aspx">Camper Detail Report</a>: Review key data about each camper application.<br />
        </div>
    </div>
	
    <div class="row">
        <div style="font-weight:bold;">
            <a href="CamperContactInfo.aspx">Camper Contact Info</a>: Camper & parent/guardian contact information helpful for a mailing or email.<br />
        </div>
    </div>

    <div class="row">
        <div style="font-weight:bold;">
            <a href="NumberOfCampersByCamp.aspx">Camper Status Report</a>: Number of campers in each status organized by camp.
        </div>        
    </div>


    <div class="row">
        <div style="font-weight:bold;">
            <a href="DataExtract.aspx">Full Data Extract</a>: This report will give you nearly every data point associated with the applicants’ record.
        </div>
    </div>


    <h2>Analytical Reports</h2>
    
    <div class="row">
        <div style="font-weight:bold;">
            <a href="ParentByCountry.aspx">Parent Origin Report</a>: Understand the countries your campers’ parents are from. Organized by camp, see the number of campers and camper parents from selected countries.  
        </div>
    </div>
    
    <div class="row">
        <div style="font-weight:bold;">
            <a href="CampersBySynagogue.aspx">Campers by Synagogue</a>: List of campers who currently have a relationship with a synagogue in your community.
        </div>
        <p>
            Report back to your local synagogues with a roster of their grant applicants. Note: this report is applicable only for community partners who have a designated synagogue question within their application. Important: be sure to send each synagogue only their own camper information.</li>        
        </p>    
    </div>
    
    <div class="row">
        <div style="font-weight:bold;">
            <a href="CamperSummaryReportByCamp.aspx">Session Length Report</a>: Number of your campers organized by number of days in camp (e.g. 12-18 days if applicable or 19+ day session).
        </div>
    </div>
    
    <div class="row">
        <div style="font-weight:bold;">
            <a href="CamperReportByState_TimesInCamp.aspx">Camper State Report</a>: Number of your campers by camp from different states (if applicable).
        </div>
    </div>

</div>

<div id="divFJCAdminContent" runat="server" style="font-size:small">
    <h2>Administrative Reports</h2>
    <span style="font-weight:bold;">
        <a href="CampersByCampFJC.aspx">Camper Detail Report (FJC)</a>: List of campers who have applied for a grant across all programs.<br />
    </span>
    <ul>
        <li>Info included: FJC ID, status, camper name, gender, grade, session dates, grant amount, and parent contact</li>
    </ul>
    <span style="font-weight:bold;">
        <a href="CampersByCampFed.aspx">Camper Detail by Camp</a>: List of campers who have applied for a grant within one program.<br />
    </span>
    <ul>
        <li>Info included: FJC ID, status, camper name, gender, grade, session dates, grant amount, and parent contact</li>
    </ul>
    <span style="font-weight:bold;">
        <a href="CamperContactInfo.aspx">Camper Contact Info</a>
    </span>
    <ul>
        <li>Camper & parent/guardian contact information helpful for a mailing or email</li>
    </ul>
    <span style="font-weight:bold;">
        <a href="NumberOfCampersByProgram.aspx">Camper Status by Program</a>: Number of campers in each status organized by program.
    </span>
    <br /><br />

    <span style="font-weight:bold;">
        <a href="NumberOfCampersByCamp.aspx">Camper Status by Camp</a>: Number of campers in each status organized by camp.
    </span>
    <br /><br />
    <span style="font-weight:bold;">
        <a href="DataExtract.aspx">Full Data Extract</a>: This report will give you nearly every data point associated with the applicants’ record.
    </span>
    <br /><br />
    <span style="font-weight:bold;">
        <a href="FJCAllocationReport.aspx">FJC Allocation</a>: Total grant dollars allocated organized by program.
    </span>
    <br /><br />
    <span style="font-weight:bold;">
        <a href="DuplicateIDReport.aspx">Duplicate Identification Report</a>
    </span>

    <h2>Analytical Reports</h2>

    <span style="font-weight:bold;">
        <a href="ParentByCountry.aspx">Parent Origin by Camp</a>: Understand the countries your campers’ parents are from. Organized by camp, see the number of campers and camper parents from selected countries.  
    </span>
    <br /><br />
    <span style="font-weight:bold;">
        <a href="CampersBySynagogue.aspx">Campers by Synagogue</a>: List of campers who currently have a relationship with a synagogue in your community.
    </span>
    <ul>
        <li>Report back to your local synagogues with a roster of their grant applicants. Note: this report is applicable only for community partners who have a designated synagogue question within their application. Important: be sure to send each synagogue only their own camper information.</li>
    </ul>
    <br />
    <span style="font-weight:bold;">
        <a href="CamperSummaryReportByCamp.aspx">Session Length by Camp</a>: Number of your campers organized by number of days in camp (e.g. 12-18 days if applicable or 19+ day session).
    </span>
    <br /><br />
    <span style="font-weight:bold;">
        <a href="CamperReportByState_TimesInCamp.aspx">Camper State by Camp</a>: Number of your campers by camp from different states (if applicable).
    </span>

    <h2>Other Reports</h2>

    <span style="font-weight:bold;">
        <a href="CamperApplicationSearchReport.aspx">Camper App Search</a>: Search by various criteria (Name, Address, Email, Birth date) to search applications and produce a list of campers.  
    </span>
    <br /><br />
    <span style="font-weight:bold;">
        <a href="SummaryByProgram.aspx">Summary by Program</a>: Number of campers in each status organized by program for current and previous years<br />
    </span>
    <ul>
        <li>Info included: Data for 2014, 2013, 2012, 2011, 2010 and 2009 can be computed in one report.</li>
    </ul>
</div>
    
</asp:Content>

