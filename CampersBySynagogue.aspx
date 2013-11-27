<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CampersBySynagogue.aspx.cs" Inherits="CampersBySynagogue" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">Synagogue Report (Online Data Only)</span>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" AutoPostBack="true" runat="server" />
            <br /><br />    
            <strong>Program:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlFed" runat="server" DataSourceID="odsFed" Enabled="false" DataValueField="ID" 
            DataTextField="Name" AutoPostBack="true" ondatabound="ddlFed_DataBound" onselectedindexchanged="ddlFed_SelectedIndexChanged" />
            <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederationsByUserRole">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                    <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
                    <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
                </SelectParameters>    
            </asp:ObjectDataSource>   
            <br /><br />
            <asp:CheckBox ID="chkAllSynags" runat="server" Text="Select all synagogues" AutoPostBack="true" oncheckedchanged="chkAllSynags_CheckedChanged" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>        
            <br /><br />
            <asp:CheckBoxList ID="chklistSynag" runat="server" DataSourceID="odsSynags" DataTextField="Name" DataValueField="ID" 
            RepeatDirection="Vertical" RepeatColumns="2" ondatabound="chklistSynag_DataBound"></asp:CheckBoxList>
            <asp:ObjectDataSource ID="odsSynags" runat="server" TypeName="SynagsDA" SelectMethod="GetAllSynagByFedID">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter ControlID="ddlFed" Name="FedID" PropertyName="SelectedValue" Type="Int32" />
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
            <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" /> 
        </ContentTemplate>
    </asp:UpdatePanel>   
</asp:Content>

