<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NumberOfCampersByProgram.aspx.cs" Inherits="NumberOfCampersByProgram" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">Status Summary Report by Program</span>
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
                <strong>Program:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllFeds" runat="server" Text="Select all programs" 
                        AutoPostBack="true" oncheckedchanged="chkAllFeds_CheckedChanged" />
                <asp:CheckBoxList ID="chklistFed" runat="server" RepeatColumns="2" RepeatDirection="Vertical" DataSourceID="odsFed" 
                    DataValueField="ID" DataTextField="Name" />
                <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederations">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>     
                </asp:ObjectDataSource>
                <br /><br />    
                <strong>Status:</strong>&nbsp;&nbsp;<span style="color:#cc6600;">-- Up to 5 status allowed --</span>
                <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" DataValueField="ID" DataTextField="Description" RepeatColumns="2" RepeatDirection="Vertical" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
                <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetAllStatusUsedInCamperApplications">
                    <SelectParameters>
                        <asp:Parameter Type="Boolean" Name="ExcludeIncompleteStatus" DefaultValue="false" />
                    </SelectParameters>
                </asp:ObjectDataSource>    
                <br /><br />
                <strong># of Years in Program:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllTimers" runat="server" Text="All" />
                <asp:CheckBox ID="chk1stTimers" runat="server" Text="1st year campers" />
                <asp:CheckBox ID="chk2ndTimers" runat="server" Text="2nd year campers" />
                <asp:CheckBox ID="chk3rdTimers" runat="server" Text="3rd year campers" />
                <br /><br />
                <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel> 
        <br />
        <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" />
        <br /><br />    
    </div>

    <div id="divReport" runat="server" visible="false">
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" 
            onclick="lnkbtnBack_Click"></asp:LinkButton>
        <span style="margin-left:10px"><asp:Button ID="btnExcelExport" 
        Text="Export to Excel" runat="server" onclick="btnExcelExport_Click" /></span>
        <br /><br />
        <div id="divAllTimers" visible="false" runat="server">
            <asp:Label ID="lblAllTimers" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gvAllTimers" runat="server"></asp:GridView>
        </div>
        <br />
        <div id="div1stTimers" visible="false" runat="server">
            <asp:Label ID="Label1" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gv1stTimers" runat="server"></asp:GridView>
        </div>
        <br />
        <div id="div2ndTimers" visible="false" runat="server">
            <asp:Label ID="Label2" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gv2ndTimers" runat="server"></asp:GridView>
        </div>
        <br />
        <div id="div3rdTimers" visible="false" runat="server">
            <asp:Label ID="Label3" runat="server" Text="3rd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gv3rdTimers" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

