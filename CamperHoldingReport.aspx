<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CamperHoldingReport.aspx.cs" Inherits="CamperHoldingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
<span class="PageHeaderText">Camper Holding Report</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" runat="server" />
            <br /><br />
            <asp:Button ID="btnReport" runat="server" Text="Download Report" onclick="btnReport_Click" />
</asp:Content>

