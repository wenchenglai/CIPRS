<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SummaryByProgram.aspx.cs" Inherits="SummaryByProgram" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
    <span style="color:Red"><strong>REPORT UNDER CONSTRUCTION DO NOT USE</strong></span><br /><br />
    <span class="PageHeaderText">Summary Program</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="divMenu" runat="server" visible="true" style="width:900px;">
        <div>
            Subtotals for each status is only available for the current year and will only be shown if no previous years are selected. Previous years will only show the total number of campers for selected statuses (not subtotaled by status). The more years selected the slower the report may run.  Thank you for your patience.    
        </div>
        <br />
        <ul style="list-style-type:circle">
            <li>Current year:  subtotals for each status will only be shown if no previous years are selected</li>
            <li>Previous years:  will only show the total # of campers registered as of the selected date.  You are unable to view subtotals by status (i.e. eligible vs. eligible by staff, etc.)</li>
            <li>The more years selected the slower the report may run. Thank you for your patience.</li>
        </ul>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <strong>Select:</strong><br />
                <asp:RadioButton ID="rdoToday" Checked="true" runat="server" Text="Today's Date" GroupName="one" AutoPostBack="true" oncheckedchanged="rdoToday_CheckedChanged" /> or 
                <br /><br />
                <asp:RadioButton ID="rdoOtherDate" runat="server" GroupName="one" Text="Another Date " AutoPostBack="true" oncheckedchanged="rdoToday_CheckedChanged" />
                <div id="divCalendar" runat="server" visible="false" style="margin-top:40px; display:inline; ">
                    <asp:TextBox ID="txtOtherDate" Width="80" runat="server"></asp:TextBox>
                    
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                        TargetControlID="txtOtherDate"
                        CssClass="ajax__calendar_container"
                        Format="MM/dd/yyyy"
                        PopupButtonID="btnCalendar" />
                    <asp:ImageButton ID="btnCalendar" runat="server" ImageUrl="~/Images/calendarbutton.png" />
                </div>
                <br />
                <br />
                <strong>Select:</strong><br />
                &nbsp;Compare to previous years?<br />
                <asp:CheckBoxList ID="cblYearsToday" DataTextField="CampYear" RepeatDirection="Horizontal" DataValueField="ID" runat="server" style="float:left; padding-right:400px" /> 
                <br /><br /><br />
                <strong>Program:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllFeds" runat="server" Text="Select all programs" 
                        AutoPostBack="true" oncheckedchanged="chkAllFeds_CheckedChanged" />
                <asp:CheckBoxList ID="chklistFed" runat="server" RepeatColumns="2" RepeatDirection="Vertical" DataValueField="ID" DataTextField="Name" /> 
                <br /><br />    
                <strong>Status:</strong>&nbsp;&nbsp;<asp:CheckBox ID="chkAllStatus" runat="server" Text="Check all status" AutoPostBack="true" oncheckedchanged="chkAllStatus_CheckedChanged" />
                <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" DataValueField="ID" DataTextField="Description" RepeatColumns="2" RepeatDirection="Vertical" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
                <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetMostUsedStatus">
                </asp:ObjectDataSource>    
                <br /><br />
                <strong># of Years in Program:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllTimers" runat="server" Text="All" />
                <asp:CheckBox ID="chk1stTimers" runat="server" Text="1st year campers" />
                <asp:CheckBox ID="chk2ndTimers" runat="server" Text="2nd year campers" />
                <asp:CheckBox ID="chk3rdTimers" runat="server" Text="3rd year campers" />
                <br /><br />
            </ContentTemplate>
        </asp:UpdatePanel> 
        <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
        <br /><br />                
        <asp:Button ID="btnReport" Text="Report" onclick="btnReport_Click" runat="server" />          
    </div>
    <div id="divReport" runat="server" visible="false">
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" onclick="lnkbtnBack_Click"></asp:LinkButton>
        <br /><br />
        <asp:Label ID="lblSelectedDate" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
        <span style="margin-left:10px">
            <asp:Button ID="btnExcelExport" Text="Export to Excel" runat="server" onclick="btnExcelExport_Click" />
        </span>
        <br /><br />
        <div id="divCurrentYear" runat="server">
            <strong>2013</strong>
            <br />
            <div id="divCYAllTimers" visible="false" runat="server">
                <asp:Label ID="lblAllTimers" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gvCYAllTimers" runat="server" onrowdatabound="gvCurrentYear_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="divCY1stTimers" visible="false" runat="server">
                <asp:Label ID="Label1" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gvCY1stTimers" runat="server" onrowdatabound="gvCurrentYear_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="divCY2ndTimers" visible="false" runat="server">
                <asp:Label ID="Label2" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gvCY2ndTimers" runat="server" onrowdatabound="gvCurrentYear_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="divCY3rdTimers" visible="false" runat="server">
                <asp:Label ID="Label3" runat="server" Text="3rd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gvCY3rdTimers" runat="server" onrowdatabound="gvCurrentYear_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />             
        </div>
        <br /><br />
        <div id="div2012" visible="false" runat="server">
            <strong>2012</strong>
            <br />
            <div id="div2012AllTimers" visible="false" runat="server">
                <asp:Label ID="Label4" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv2012AllTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20121stTimers" visible="false" runat="server">
                <asp:Label ID="Label5" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20121stTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20122ndTimers" visible="false" runat="server">
                <asp:Label ID="Label6" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20122ndTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20123rdTimers" visible="false" runat="server">
                <asp:Label ID="Label7" runat="server" Text="3rd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20123rdTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />             
        </div>
        <br /><br />
        <div id="div2011" visible="false" runat="server">
            <strong>2011</strong>
            <br />
            <div id="div2011AllTimers" visible="false" runat="server">
                <asp:Label ID="Label8" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv2011AllTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20111stTimers" visible="false" runat="server">
                <asp:Label ID="Label9" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20111stTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20112ndTimers" visible="false" runat="server">
                <asp:Label ID="Label10" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20112ndTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20113rdTimers" visible="false" runat="server">
                <asp:Label ID="Label11" runat="server" Text="3rd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20113rdTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />             
        </div>
        <br /><br />
        <div id="div2010" visible="false" runat="server">
            <strong>2010</strong>
            <br />
            <div id="div2010AllTimers" visible="false" runat="server">
                <asp:Label ID="Label12" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv2010AllTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20101stTimers" visible="false" runat="server">
                <asp:Label ID="Label13" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20101stTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20102ndTimers" visible="false" runat="server">
                <asp:Label ID="Label14" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20102ndTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20103rdTimers" visible="false" runat="server">
                <asp:Label ID="Label15" runat="server" Text="3rd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20103rdTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />             
        </div>
        <br /><br />
        <div id="div2009" visible="false" runat="server">
            <strong>2009</strong>
            <br />
            <div id="div2009AllTimers" visible="false" runat="server">
                <asp:Label ID="Label16" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv2009AllTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20091stTimers" visible="false" runat="server">
                <asp:Label ID="Label17" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20091stTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20092ndTimers" visible="false" runat="server">
                <asp:Label ID="Label18" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20092ndTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />
            <div id="div20093rdTimers" visible="false" runat="server">
                <asp:Label ID="Label19" runat="server" Text="3rd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv20093rdTimers" runat="server" onrowdatabound="gv_RowDataBound" ondatabound="gv_DataBound"></asp:GridView>
            </div>
            <br />             
        </div>
    </div>
</asp:Content>    

