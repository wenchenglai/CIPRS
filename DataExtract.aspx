<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DataExtract.aspx.cs" Inherits="DataExtract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
    <span class="PageHeaderText">Data Extract</span>&nbsp;&nbsp;&nbsp;&nbsp; <span style="font-size:small; color:Red;">(The data from this report is always 1 day old)</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="divMenu" runat="server" visible="true">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <strong>Summer:</strong>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlYear_SelectedIndexChanged">
                    <asp:ListItem Text="2009" Value="1"></asp:ListItem>
                    <asp:ListItem Text="2010" Value="2"></asp:ListItem>
                    <asp:ListItem Text="2011" Value="3"></asp:ListItem>  
                    <asp:ListItem Text="2012" Value="4"></asp:ListItem>   
                    <asp:ListItem Text="2013" Value="5" Selected="True"></asp:ListItem>                                        
                </asp:DropDownList>
                <br /><br />    
                <strong>Program:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllFeds" runat="server" Text="Select all programs" AutoPostBack="true" oncheckedchanged="chkAllFeds_CheckedChanged" />
                <asp:CheckBoxList ID="chklistFed" runat="server" RepeatColumns="2" RepeatDirection="Vertical" DataSourceID="odsFed" 
                    DataValueField="ID" DataTextField="Name" OnSelectedIndexChanged="chklistFed_OnSelectedIndexChanged" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederationsByUserRole">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                        <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
                        <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
                    </SelectParameters>     
                </asp:ObjectDataSource>
                <br /><br /> 
                <strong>Camps:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllCamps" runat="server" Text="Select all camps" oncheckedchanged="chkAll_CheckedChanged" AutoPostBack="true" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label> 
                <br /><br />
                <asp:CheckBoxList ID="chklistCamp" runat="server" DataSourceID="odsCamp" DataTextField="Name" DataValueField="CampID" RepeatDirection="Vertical" 
                    RepeatColumns="2" ondatabound="chklistCamp_DataBound" OnSelectedIndexChanged="chklistCamp_OnSelectedIndexChanged" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsCamp" runat="server" TypeName="CampsBL" OnSelecting="odsCamp_OnSelecting" SelectMethod="GetAllCampsByYearIDAndFedList">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlYear" PropertyName="SelectedValue" Type="Int32" Name="CampYearID" />
                        <asp:Parameter Name="FedList" Type="Object" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
                <strong>Status: </strong>&nbsp;&nbsp;
                <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" DataValueField="ID" DataTextField="Description" RepeatDirection="Vertical" RepeatColumns="2" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
                <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetAllStatusUsedInCamperApplications">
                    <SelectParameters>
                        <asp:Parameter Type="Boolean" Name="ExcludeIncompleteStatus" DefaultValue="false" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
                <strong># of Years in Program:</strong>&nbsp;&nbsp;
                <asp:RadioButton ID="rdoAllTimers" runat="server" GroupName="abc" Text="All" Checked="true" />
                <asp:RadioButton ID="rdo1stTimers" runat="server" GroupName="abc" Text="1st year campers" />
                <asp:RadioButton ID="rdo2ndTimers" runat="server" GroupName="abc" Text="2nd year campers" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" />   
    </div>
    <div id="divReport" runat="server" visible="false">
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" onclick="lnkbtnBack_Click"></asp:LinkButton>
        <span style="margin-left:10px">
            <asp:Button ID="btnExcelExport" Enabled="true" Text="Export to Excel" runat="server" onclick="btnExcelExport_Click" />
        </span>
        <br /><br />
        <asp:GridView ID="gv" runat="server"></asp:GridView>
    </div>
</asp:Content>

