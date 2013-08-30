<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CamperHoldingReport.aspx.cs" Inherits="CamperHoldingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
<span class="PageHeaderText">Camper Holding Report</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true">
                <asp:ListItem Text="2009" Value="1"></asp:ListItem>
                <asp:ListItem Text="2010" Value="2"></asp:ListItem>    
                <asp:ListItem Text="2011" Value="3"></asp:ListItem>  
                <asp:ListItem Text="2012" Value="4"></asp:ListItem>   
                <asp:ListItem Text="2013" Value="5" Selected="True"></asp:ListItem>            
            </asp:DropDownList>
            <br /><br />
            <asp:Button ID="btnReport" runat="server" Text="Download Report" onclick="btnReport_Click" />
</asp:Content>

