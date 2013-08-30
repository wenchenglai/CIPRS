<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NumberOfCampersByCamp.aspx.cs" Inherits="NumberOfCampersByCamp" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">Status Summary Report by Camp (Online Data Only)</span>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="divMenu" runat="server" visible="true">  
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
                <asp:ObjectDataSource ID="odsCamps" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByRoleID">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlProgram" Name="type" PropertyName="SelectedValue" Type="Int32" />        
                        <asp:SessionParameter SessionField="RoleID" Name="RoleID" Type="Int32" />
                        <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />            
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
                <strong>Status:</strong>&nbsp;&nbsp;
                <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" DataValueField="ID" DataTextField="Description" RepeatColumns="2" RepeatDirection="Vertical" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
                <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetAllStatusUsedInCamperApplications">
                    <SelectParameters>
                        <asp:Parameter Type="Boolean" Name="ExcludeIncompleteStatus" DefaultValue="false" />
                    </SelectParameters>
                </asp:ObjectDataSource>   
            </ContentTemplate>
        </asp:UpdatePanel>         
        <br /><br />
        <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" />
    </div>

    <div id="divReport" runat="server" visible="false">
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" 
                onclick="lnkbtnBack_Click"></asp:LinkButton>
        <span style="margin-left:10px"><asp:Button ID="btnExcelExport" 
            Text="Export to Excel" runat="server" onclick="btnExcelExport_Click" /></span>
        <br /><br />
        <asp:GridView ID="gv" runat="server"></asp:GridView>
    </div>
</asp:Content>

