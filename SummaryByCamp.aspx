<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SummaryByCamp.aspx.cs" Inherits="SummaryByCamp" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
    <span style="color:Red"><strong>REPORT UNDER CONSTRUCTION DO NOT USE</strong></span><br /><br />
    <span class="PageHeaderText">Camp Program</span>
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
                <asp:CheckBoxList ID="cblYearsToday" AutoPostBack="true" DataTextField="CampYear" 
                    RepeatDirection="Horizontal" DataValueField="ID" runat="server" 
                    style="float:left; padding-right:400px" 
                    onselectedindexchanged="cblYearsToday_SelectedIndexChanged" /> 
                <br /><br /><br />
                <strong>Program:</strong>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlProgram" runat="server" AutoPostBack="true" onselectedindexchanged="ddlProgram_SelectedIndexChanged">
                    <asp:ListItem Text="CIP" Value="0"></asp:ListItem>
                    <asp:ListItem Text="JWest" Value="1"></asp:ListItem>
                    <asp:ListItem Text="All" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <br /><br />
                <asp:CheckBox ID="chkAllCamps" runat="server" Text="Select all camps" AutoPostBack="true" oncheckedchanged="chkAllCamps_CheckedChanged" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;      
                <br /><br />
                <span style="color:Red; font-weight:bold">Current Year:</span>
                <br />
                <asp:CheckBoxList ID="chklistCamp2013" runat="server" DataSourceID="odsCamps2013" DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                        RepeatColumns="2" ondatabound="chklistCamp_DataBound"></asp:CheckBoxList>
                <asp:ObjectDataSource ID="odsCamps2013" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByRoleID">
                    <SelectParameters>
                        <asp:Parameter Name="CampYearID" DefaultValue="5" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlProgram" Name="type" PropertyName="SelectedValue" Type="Int32" />        
                        <asp:SessionParameter SessionField="RoleID" Name="RoleID" Type="Int32" />
                        <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />            
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
                <div id="div2012CampList" visible="false" runat="server">   
                    <span style="color:Red; font-weight:bold">2012:</span>
                    <br />
                    <asp:CheckBoxList ID="chklistCamp2012" runat="server" DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                            RepeatColumns="2" ondatabound="chklistCamp_DataBound"></asp:CheckBoxList>
                    <asp:ObjectDataSource ID="odsCamps2012" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByRoleID">
                        <SelectParameters>
                            <asp:Parameter Name="CampYearID" DefaultValue="4" Type="Int32" />
                            <asp:ControlParameter ControlID="ddlProgram" Name="type" PropertyName="SelectedValue" Type="Int32" />        
                            <asp:SessionParameter SessionField="RoleID" Name="RoleID" Type="Int32" />
                            <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />            
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br /><br />
                </div>
                <div id="div2011CampList" visible="false" runat="server">   
                    <span style="color:Red; font-weight:bold">2011:</span>
                    <br />
                    <asp:CheckBoxList ID="chklistCamp2011" runat="server" DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                            RepeatColumns="2" ondatabound="chklistCamp_DataBound"></asp:CheckBoxList>
                    <asp:ObjectDataSource ID="odsCamps2011" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByRoleID">
                        <SelectParameters>
                            <asp:Parameter Name="CampYearID" DefaultValue="3" Type="Int32" />
                            <asp:ControlParameter ControlID="ddlProgram" Name="type" PropertyName="SelectedValue" Type="Int32" />        
                            <asp:SessionParameter SessionField="RoleID" Name="RoleID" Type="Int32" />
                            <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />            
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br /><br />
                </div>
                <div id="div2010CampList" visible="false" runat="server">   
                    <span style="color:Red; font-weight:bold">2010:</span>
                    <br />
                    <asp:CheckBoxList ID="chklistCamp2010" runat="server" DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                            RepeatColumns="2" ondatabound="chklistCamp_DataBound"></asp:CheckBoxList>
                    <asp:ObjectDataSource ID="odsCamps2010" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByRoleID">
                        <SelectParameters>
                            <asp:Parameter Name="CampYearID" DefaultValue="2" Type="Int32" />
                            <asp:ControlParameter ControlID="ddlProgram" Name="type" PropertyName="SelectedValue" Type="Int32" />        
                            <asp:SessionParameter SessionField="RoleID" Name="RoleID" Type="Int32" />
                            <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />            
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br /><br />
                </div>
                <div id="div2009CampList" visible="false" runat="server">   
                    <span style="color:Red; font-weight:bold">2009:</span>
                    <br />
                    <asp:CheckBoxList ID="chklistCamp2009" runat="server" DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                            RepeatColumns="2" ondatabound="chklistCamp_DataBound"></asp:CheckBoxList>
                    <asp:ObjectDataSource ID="odsCamps2009" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByRoleID">
                        <SelectParameters>
                            <asp:Parameter Name="CampYearID" DefaultValue="1" Type="Int32" />
                            <asp:ControlParameter ControlID="ddlProgram" Name="type" PropertyName="SelectedValue" Type="Int32" />        
                            <asp:SessionParameter SessionField="RoleID" Name="RoleID" Type="Int32" />
                            <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />            
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br /><br />
                </div>                                               
                <strong>Status:</strong>&nbsp;&nbsp;<asp:CheckBox ID="chkAllStatus" runat="server" Text="Check all status" AutoPostBack="true" oncheckedchanged="chkAllStatus_CheckedChanged" />
                <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" DataValueField="ID" DataTextField="Description" RepeatColumns="2" RepeatDirection="Vertical" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
                <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetMostUsedStatus"></asp:ObjectDataSource>    
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
</asp:Content>

