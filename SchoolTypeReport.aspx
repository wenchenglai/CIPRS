<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SchoolTypeReport.aspx.cs" Inherits="SchoolTypeReport" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">School Type Report</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gv" runat="server" DataSourceID="ods">
    </asp:GridView>
    <asp:ObjectDataSource ID="ods" runat="server" TypeName="CamperAnswerDA" SelectMethod="GetSchoolTypeReport">
        <SelectParameters>
            <asp:Parameter Name="FedID" DefaultValue="63" />
            <asp:Parameter Name="CampYearID" DefaultValue="4" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

