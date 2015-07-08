<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SchoolTypeReport.aspx.cs" Inherits="SchoolTypeReport" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">School Type Report</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" runat="server" AutoPostBack="true" />
    <br />
    <br />
    <strong>Program:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="ddlFed" runat="server" DataSourceID="odsFed" DataValueField="ID" DataTextField="Name" />
    <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederationsByUserRole">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
            <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
            <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
            <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
    <br />
    <asp:Button ID="btnReport" runat="server" Text="Report" OnClick="btnReport_Click" />

    <br />
    <br />
    <div id="divReport" runat="server" visible="false">
        <asp:GridView ID="gv" runat="server" />
        <asp:ObjectDataSource ID="ods" runat="server" TypeName="CamperAnswerDA" SelectMethod="GetSchoolTypeReport">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlFed" PropertyName="SelectedValue" Name="FedID" Type="Int32" />
                <asp:ControlParameter ControlID="ddlCampYear" PropertyName="SelectedValue" Name="CampYearID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>

