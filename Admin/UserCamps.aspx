<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="UserCamps.aspx.cs" Inherits="Admin_UserCamps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<h2>User Camps</h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul>
        <li>
            Camp Year: <asp:DropDownList ID="ddlCampYear" runat="server"></asp:DropDownList>
        </li>
        <li>
            <asp:Button ID="btnDelete" runat="server" Text="Delete Data" onclick="btnDelete_Click" />        
        </li>
        <li>
            <asp:Button ID="btnGenerate" runat="server" Text="Generate Data" onclick="btnGenerate_Click" />        
        </li>
        <li>
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        </li>
    </ul>
</asp:Content>

