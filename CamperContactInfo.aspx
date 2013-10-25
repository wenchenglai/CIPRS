<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CamperContactInfo.aspx.cs" Inherits="CamperContactInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
    <span class="PageHeaderText">Camper Contact Information (Online Data Only)</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" runat="server" AutoPostBack="true" />
            <br /><br />
                <strong>Program:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllFeds" runat="server" Text="Select all programs" AutoPostBack="true" oncheckedchanged="chkAllFeds_CheckedChanged" />
                <asp:CheckBoxList ID="chklistFed" runat="server" RepeatColumns="2" RepeatDirection="Vertical" DataSourceID="odsFed" 
                    DataValueField="ID" DataTextField="Name" OnSelectedIndexChanged="chklistFed_OnSelectedIndexChanged" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederationsByUserRole">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                        <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
                        <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
                    </SelectParameters>     
                </asp:ObjectDataSource>
                <br /><br /> 
            <asp:CheckBox ID="chkAllCamps" runat="server" Text="Select all camps" AutoPostBack="true" oncheckedchanged="chkAllCamps_CheckedChanged" />
            <br /><br />
                <asp:CheckBoxList ID="chklistCamp" runat="server" DataSourceID="odsCamp" DataTextField="Name" DataValueField="CampID" RepeatDirection="Vertical" 
                    RepeatColumns="2" ondatabound="chklistCamp_DataBound" OnSelectedIndexChanged="chklistCamp_OnSelectedIndexChanged" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsCamp" runat="server" TypeName="CampsBL" OnSelecting="odsCamp_OnSelecting" SelectMethod="GetAllCampsByYearIDAndFedList">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCampYear" PropertyName="SelectedValue" Type="Int32" Name="CampYearID" />
                        <asp:Parameter Name="FedList" Type="Object" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
            <strong>Status:</strong>&nbsp;&nbsp;
            <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" 
                DataValueField="ID" DataTextField="Description" RepeatColumns="2" 
                RepeatDirection="Vertical" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
            <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetAllStatusUsedInCamperApplications">
                <SelectParameters>
                    <asp:Parameter Type="Boolean" Name="ExcludeIncompleteStatus" DefaultValue="true" />
                </SelectParameters>    
            </asp:ObjectDataSource>
                <br /><br />
                <strong># of Years in Program:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chk1stTimers" runat="server" Text="1st year campers" />
                <asp:CheckBox ID="chk2ndTimers" runat="server" Text="2nd year campers" />
                <asp:CheckBox ID="chk3rdTimers" runat="server" Text="3rd year campers" />
            <br /><br />
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

