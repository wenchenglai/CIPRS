<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CampersByCampFJC.aspx.cs" Inherits="CampersByCampFJC" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">Camper Enrollment Confirmation Report (FJC) (Online Data Only)</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" runat="server"  AutoPostBack="true" />
            <br /><br />
            <strong>Program:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlProgram" runat="server" AutoPostBack="true" 
            onselectedindexchanged="ddlProgram_SelectedIndexChanged">
                <asp:ListItem Text="CIP" Value="0"></asp:ListItem>
                <asp:ListItem Text="JWest" Value="1"></asp:ListItem>
                <asp:ListItem Text="All" Value="2"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:CheckBox ID="chkAllCamps" runat="server" Text="Select all camps" 
                AutoPostBack="true" oncheckedchanged="chkAllCamps_CheckedChanged" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>        
            <br /><br />
            <asp:CheckBoxList ID="chklistCamp" runat="server" DataSourceID="odsCamps" 
                DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                RepeatColumns="2" ondatabound="chklistCamp_DataBound"></asp:CheckBoxList>
            <asp:ObjectDataSource ID="odsCamps" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByProgram">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                    <asp:ControlParameter ControlID="ddlProgram" Name="type" PropertyName="SelectedValue" Type="Int32" />
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

