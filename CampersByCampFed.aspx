﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CampersByCampFed.aspx.cs" Inherits="CampersByCampFed" %>
<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
<span class="PageHeaderText">Camper Detail Report (Online Data Only)</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true">
                <asp:ListItem Text="2009" Value="1"></asp:ListItem>
                <asp:ListItem Text="2010" Value="2"></asp:ListItem>    
                <asp:ListItem Text="2011" Value="3"></asp:ListItem>  
                <asp:ListItem Text="2012" Value="4"></asp:ListItem>   
                <asp:ListItem Text="2013" Value="5" Selected="True"></asp:ListItem>            
            </asp:DropDownList>
            <br /><br />    
            <strong>Program:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlFed" runat="server" DataSourceID="odsFed" DataValueField="ID" DataTextField="Name" AutoPostBack="true" 
                ondatabound="ddlFed_DataBound" onselectedindexchanged="ddlFed_SelectedIndexChanged" />
            <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederations">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>     
            </asp:ObjectDataSource>
            <br /><br />
            <asp:CheckBox ID="chkAllCamps" runat="server" Text="Select all camps" oncheckedchanged="chkAll_CheckedChanged" AutoPostBack="true" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label> 
            <br /><br />
            <asp:CheckBoxList ID="chklistCamp" runat="server" DataSourceID="odsCamp" DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                RepeatColumns="2" ondatabound="chklistCamp_DataBound" />
            <asp:ObjectDataSource ID="odsCamp" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByFedID">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlYear" PropertyName="SelectedValue" Type="Int32" Name="CampYearID" />
                    <asp:ControlParameter ControlID="ddlFed" PropertyName="SelectedValue" Type="Int32" Name="FedID" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:Label ID="lblMsgCamps" runat="server" ForeColor="Red"></asp:Label> 
            <br /><br />
            <strong>Status:</strong>&nbsp;&nbsp;
            <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" DataValueField="ID" DataTextField="Description" RepeatDirection="Vertical" RepeatColumns="2" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
            <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetAllStatusUsedInCamperApplications">
                <SelectParameters>
                    <asp:Parameter Type="Boolean" Name="ExcludeIncompleteStatus" DefaultValue="true" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            <br />
            <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

