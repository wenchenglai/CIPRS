<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CamperSummaryReportByCamp.aspx.cs" Inherits="CamperSummaryReportByCamp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
    <span class="PageHeaderText">Session Length by Camp (Online Data Only)</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="divMenu" runat="server" visible="true">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <strong>Summer:</strong>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" runat="server" AutoPostBack="true" />
                <br /><br />    
                <strong>Program:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllFeds" runat="server" Text="Select all programs" AutoPostBack="true" oncheckedchanged="chkAllFeds_CheckedChanged" />
                <asp:CheckBoxList ID="chklistFed" runat="server" RepeatColumns="2" RepeatDirection="Vertical" DataSourceID="odsFed" 
                    DataValueField="ID" DataTextField="Name" OnSelectedIndexChanged="chklistFed_OnSelectedIndexChanged" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederationsByUserRole">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                        <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
                        <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
                        <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />
                    </SelectParameters>     
                </asp:ObjectDataSource>
                <br /><br /> 
                <strong>Camps:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllCamps" runat="server" Text="Select all camps" oncheckedchanged="chkAll_CheckedChanged" AutoPostBack="true" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label> 
                <br /><br />
                <asp:CheckBoxList ID="chklistCamp" runat="server" DataSourceID="odsCamp" DataTextField="Name" DataValueField="CampID" RepeatDirection="Vertical" 
                    RepeatColumns="2" ondatabound="chklistCamp_DataBound" OnSelectedIndexChanged="chklistCamp_OnSelectedIndexChanged" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsCamp" runat="server" TypeName="CampsBL" OnSelecting="odsCamp_OnSelecting" SelectMethod="GetAllCampsByYearIDAndFedList">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCampYear" PropertyName="SelectedValue" Type="Int32" Name="CampYearID" />
                        <asp:Parameter Name="FedList" Type="Object" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
                <strong>Status: </strong>&nbsp;&nbsp;
                <asp:CheckBoxList ID="chklistStatus" runat="server" DataSourceID="odsStatus" DataValueField="ID" DataTextField="Description" RepeatDirection="Vertical" RepeatColumns="2" ondatabound="chklistStatus_DataBound"></asp:CheckBoxList>
                <asp:ObjectDataSource ID="odsStatus" runat="server" TypeName="StatusDA" SelectMethod="GetAllStatusUsedInCamperApplications">
                    <SelectParameters>
                        <asp:Parameter Type="Boolean" Name="ExcludeIncompleteStatus" DefaultValue="true" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
                <strong># of Years in Program:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllTimers" runat="server" Text="All" />
                <asp:CheckBox ID="chk1stTimers" runat="server" Text="1st year campers" />
                <asp:CheckBox ID="chk2ndTimers" runat="server" Text="2nd year campers" />
                <asp:CheckBox ID="chk3rdTimers" runat="server" Text="3rd year campers" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" />
    </div>
    <div id="divReport" runat="server" visible="false">
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" onclick="lnkbtnBack_Click"></asp:LinkButton>       
        <span style="margin-left:10px">
            <asp:Button ID="btnExcelExport" Text="Export to Excel" runat="server" onclick="btnExcelExport_Click" />
        </span>        
        <br /><br />
        <div id="divAllTimers" visible="false" runat="server">
            <asp:Label ID="lblAllTimers" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gvAllTimers" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="Camp Name" DataField="Camp Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="# of Campers" DataField="# of Campers" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="12-17 days" DataField="12-17 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="18 days" DataField="18 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="19+ days" DataField="19+ days" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div id="div1stTimers" visible="false" runat="server">
            <asp:Label ID="Label1" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gv1stTimers" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="Camp Name" DataField="Camp Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="# of Campers" DataField="# of Campers" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="12-17 days" DataField="12-17 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="18 days" DataField="18 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="19+ days" DataField="19+ days" ItemStyle-HorizontalAlign="Center" />
                </Columns>            
            </asp:GridView>
        </div>
        <br />
        <div id="div2ndTimers" visible="false" runat="server">
            <asp:Label ID="Label2" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gv2ndTimers" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="Camp Name" DataField="Camp Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="# of Campers" DataField="# of Campers" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="12-17 days" DataField="12-17 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="18 days" DataField="18 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="19+ days" DataField="19+ days" ItemStyle-HorizontalAlign="Center" />
                </Columns>            
            </asp:GridView>
        </div>
        <br />
        <div id="div3rdTimers" visible="false" runat="server">
            <asp:Label ID="Label3" runat="server" Text="3rd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
            <asp:GridView ID="gv3rdTimers" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="Camp Name" DataField="Camp Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="# of Campers" DataField="# of Campers" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="12-17 days" DataField="12-17 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="18 days" DataField="18 days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="19+ days" DataField="19+ days" ItemStyle-HorizontalAlign="Center" />
                </Columns>            
            </asp:GridView>
        </div>     
    </div>
</asp:Content>

