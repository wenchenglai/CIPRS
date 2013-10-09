<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ParentByCountry.aspx.cs" Inherits="ParentByCountry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
    <span class="PageHeaderText">Camper's Parent Country of Origin (Online Data Only)</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
    $(function () {
        $('#set1 *').tooltip();

        $("#foottip a").tooltip({
            bodyHandler: function () {
                return $($(this).attr("href")).html();
            },
            showURL: false
        });

        $('#tonus').tooltip({
            delay: 0,
            showURL: false,
            bodyHandler: function () {
                return $("<img/>").attr("src", this.src);
            }
        });

        $('#yahoo a').tooltip({
            track: true,
            delay: 0,
            showURL: false,
            showBody: " - ",
            fade: 250
        });

        $("select").tooltip({
            left: 25
        });

        $("map > area").tooltip({ positionLeft: true });

        $("#fancy, #fancy2").tooltip({
            track: true,
            delay: 0,
            showURL: false,
            fixPNG: true,
            showBody: " - ",
            extraClass: "pretty fancy",
            top: -15,
            left: 5
        });

        $('#pretty').tooltip({
            track: true,
            delay: 0,
            showURL: false,
            showBody: " - ",
            extraClass: "pretty",
            fixPNG: true,
            left: -120
        });

        $('#pretty2').tooltip({
            track: true,
            delay: 0,
            showURL: false,
            showBody: " - ",
            extraClass: "pretty2",
            fixPNG: true,
            left: -120
        });

        $('#right a').tooltip({
            track: true,
            delay: 0,
            showURL: false,
            extraClass: "right"
        });
        $('#right2 a').tooltip({ showURL: false, positionLeft: true });

        $("#block").click($.tooltip.block);

    });
</script>


    <div id="divMenu" runat="server" visible="true">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <strong>Summer:</strong>&nbsp;&nbsp;

                <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" runat="server" AutoPostBack="true" onselectedindexchanged="ddlYear_SelectedIndexChanged" />

                <br /><br />    
                <strong>Program:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllFeds" runat="server" Text="Select all programs" AutoPostBack="true" oncheckedchanged="chkAllFeds_CheckedChanged" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label> 
                <asp:CheckBoxList ID="chklistFed" runat="server" RepeatColumns="2" RepeatDirection="Vertical" DataSourceID="odsFed" 
                    DataValueField="ID" DataTextField="Name" OnSelectedIndexChanged="chklistFed_OnSelectedIndexChanged" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederationsByUserRole">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                        <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
                        <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
                    </SelectParameters>     
                </asp:ObjectDataSource>
                <br /><br /> 
                <strong>Camps:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllCamps" runat="server" Text="Select all camps" oncheckedchanged="chkAll_CheckedChanged" AutoPostBack="true" />


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
                <br /><br />
                <strong>Countries:</strong>&nbsp&nbsp;
                <asp:RadioButton ID="rdoGroupCountry" Text="Select by commonly used countries" 
                    runat="server" GroupName="abc" AutoPostBack="true" 
                    oncheckedchanged="rdoGroupCountry_CheckedChanged" Checked="true" />
                <asp:RadioButton ID="rdoIndividualCountry" Text="Select by individual country" 
                    runat="server" GroupName="abc" AutoPostBack="true" 
                    oncheckedchanged="rdoIndividualCountry_CheckedChanged" />
                <br /><br />
                <asp:CheckBoxList ID="chklistCountryPopular" runat="server" >
                    <asp:ListItem Text="Former Soviet Union -- tip" Value="1000"></asp:ListItem>
                    <asp:ListItem Text="Latin America -- tip" Value="1001"></asp:ListItem>
                    <asp:ListItem Text="Israel" Value="99"></asp:ListItem>
                    <asp:ListItem Text="All Countries" Value="1002"></asp:ListItem>
                    <asp:ListItem Text="All but US & Canada" Value="1003"></asp:ListItem>
                </asp:CheckBoxList>
                <asp:CheckBoxList Visible="false" ID="chklistCountry" runat="server" DataSourceID="odsCountry" DataTextField="CountryName" DataValueField="CountryID" RepeatDirection="Vertical" 
                    RepeatColumns="3" />
                <asp:ObjectDataSource ID="odsCountry" runat="server" TypeName="CountryBL" SelectMethod="GetCountryByCampYearID">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCampYear" PropertyName="SelectedValue" Type="Int32" Name="CampYearID" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br /><br />
                <strong>Type of report:</strong>&nbsp;&nbsp;
                <asp:CheckBox ID="chkByCamp" Checked="true" runat="server" Text="By Camp" />
                <asp:CheckBox ID="chkByProgram" Checked="true" runat="server" Text="By Program" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" />   
        <br />
    </div>
    <div id="divReport" runat="server" visible="false">
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" onclick="lnkbtnBack_Click"></asp:LinkButton>
        <span style="margin-left:10px">
            <asp:Button ID="btnExcelExport" Text="Export to Excel" Enabled="true" runat="server" onclick="btnExcelExport_Click" />
            <asp:Button ID="btnDataExtract" Visible="false" Text="Data Extract" Enabled="true" runat="server" onclick="btnDataExtract_Click" />
        </span>
        <br /><br />
        This report offers two different pieces of data.  “Number Of Campers” who have at least one parent from the country(s) selected and the list of countries with the number of corresponding parents.  Note: the 
        number of campers will typically be less than the total number of parents in this report.
        <br /><br />
        <div id="divRep1" visible="false" runat="server">
            <h2>By Program</h2>
            <div id="divAllTimers" visible="false" runat="server">
                <asp:Label ID="lblAllTimers" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gvAllTimers" runat="server">
                    <EmptyDataTemplate>
                        &nbsp;&nbsp;-- There is no record for this selection --&nbsp;&nbsp;
                    </EmptyDataTemplate>             
                </asp:GridView>
            </div>
            <br />
            <div id="div1stTimers" visible="false" runat="server">
                <asp:Label ID="Label1" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv1stTimers" runat="server">
                    <EmptyDataTemplate>
                        &nbsp;&nbsp;-- There is no record for this selection --&nbsp;&nbsp;
                    </EmptyDataTemplate>             
                </asp:GridView>
            </div>
            <br />
            <div id="div2ndTimers" visible="false" runat="server">
                <asp:Label ID="Label2" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv2ndTimers" runat="server">
                    <EmptyDataTemplate>
                        &nbsp;&nbsp;-- There is no record for this selection --&nbsp;&nbsp;
                    </EmptyDataTemplate> 
                </asp:GridView>
            </div>
            <br />
            <div id="div3rdTimers" visible="false" runat="server">
                <asp:Label ID="Label3" runat="server" Text="3rd year ampers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv3rdTimers" runat="server"></asp:GridView>
            </div>
        </div>
        <div id="divRep2" visible="false" runat="server">
            <h2>By Camp</h2>
            <div id="divAllTimers2" visible="false" runat="server">
                <asp:Label ID="Label4" runat="server" Text="All Campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gvAllTimers2" runat="server">
                    <EmptyDataTemplate>
                        &nbsp;&nbsp;-- There is no record for this selection --&nbsp;&nbsp;
                    </EmptyDataTemplate>             
                </asp:GridView>
            </div>
            <br />
            <div id="div1stTimers2" visible="false" runat="server">
                <asp:Label ID="Label5" runat="server" Text="1st year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv1stTimers2" runat="server">
                    <EmptyDataTemplate>
                        &nbsp;&nbsp;-- There is no record for this selection --&nbsp;&nbsp;
                    </EmptyDataTemplate>             
                </asp:GridView>
            </div>
            <br />
            <div id="div2ndTimers2" visible="false" runat="server">
                <asp:Label ID="Label6" runat="server" Text="2nd year campers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv2ndTimers2" runat="server">
                    <EmptyDataTemplate>
                        &nbsp;&nbsp;-- There is no record for this selection --&nbsp;&nbsp;
                    </EmptyDataTemplate> 
                </asp:GridView>
            </div>
            <br />
            <div id="div3rdTimers2" visible="false" runat="server">
                <asp:Label ID="Label7" runat="server" Text="3rd year ampers" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                <asp:GridView ID="gv3rdTimers2" runat="server"></asp:GridView>
            </div>        
        </div>
    </div>
</asp:Content>

