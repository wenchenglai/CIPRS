<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CamperApplicationSearchReport.aspx.cs" Inherits="CamperApplicationSearchReport" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
<span class="PageHeaderText">Camper App Search</span>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong>Summer:</strong>&nbsp;&nbsp;
    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true">
        <asp:ListItem Text="2009" Value="1"></asp:ListItem>
        <asp:ListItem Text="2010" Value="2"></asp:ListItem>      
        <asp:ListItem Text="2011" Value="3"></asp:ListItem>  
        <asp:ListItem Text="2012" Value="4"></asp:ListItem>   
        <asp:ListItem Text="2013" Value="5" Selected="True"></asp:ListItem>      
    </asp:DropDownList>
    <br /><br /> 
    <asp:DropDownList ID="ddlSearchKey" runat="server">
        <asp:ListItem Text="Name" Value="0"></asp:ListItem>
        <asp:ListItem Text="Address" Value="1"></asp:ListItem>     
        <asp:ListItem Text="Email" Value="2"></asp:ListItem>
        <asp:ListItem Text="Birth Date" Value="3"></asp:ListItem>              
    </asp:DropDownList>
    <asp:TextBox ID="txtSearchText" runat="server"></asp:TextBox>
    <br /><br />
    <asp:Button ID="btnSearch" Text="Search" runat="server" 
        onclick="btnSearch_Click" />
    <br /><br />
    <asp:GridView ID="gv" runat="server">
    </asp:GridView>
    <asp:ObjectDataSource ID="ods" runat="server" TypeName="CamperApplicationDA" SelectMethod="GetCamperApplicationsWithSearchCriteria">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlYear" Type="Int32" PropertyName="SelectedValue" Name="CampYearID" />
            <asp:ControlParameter ControlID="ddlSearchKey" Type="Int32" PropertyName="SelectedValue" Name="SearchKey" />
            <asp:ControlParameter ControlID="txtSearchText" Type="String" PropertyName="Text" Name="SearchText" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

