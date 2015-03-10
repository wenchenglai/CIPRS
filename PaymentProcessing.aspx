<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentProcessing.aspx.cs" Inherits="PaymentProcessing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">Payment Processing Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <script type="text/javascript">
<%--        $(function () {
            $("#dialog").dialog({
                autoOpen: false,
                width: 400,
                modal: true,
                buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            $(this).dialog("close");
                            document.getElementById('<%= btnHiddenRun.ClientID %>').click();
                            }
                        },
                        {
                            text: "Cancel",
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
        });--%>

                function RunPayment() {
                    //$("#dialog").dialog();
                    document.getElementById('<%= btnHiddenRun.ClientID %>').click();

                }

        function RunPayment2() {
            //$("#dialog").dialog();
            document.getElementById('<%= btnHiddenRun2.ClientID %>').click();

        }

                function ReversePayment() {
                    document.getElementById('<%= btnHiddenReverse.ClientID %>').click();
        }
    </script>                        
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" AutoPostBack="true" Enabled="False" runat="server"></asp:DropDownList>
            <br /><br />    
            <strong>Program:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlFed" runat="server" DataSourceID="odsFed" DataValueField="ID" DataTextField="Name" AutoPostBack="true" 
                ondatabound="ddlFed_DataBound" onselectedindexchanged="ddlFed_SelectedIndexChanged" />
            <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllSelfFundingFederationsByUserRole">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                    <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
                    <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
                    <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />
                </SelectParameters>     
            </asp:ObjectDataSource>

            <asp:Panel runat="server" style="border: 1px solid black; padding-left:20px; max-width: 800px; margin-bottom: 20px; margin-top: 20px" ID="pnlInfo">
                <p>
                    You have <asp:Label runat="server" style="font-weight: bold;" ID="lblTotalCampersReady"></asp:Label> records ready for payments. 
                    <span style="font-style: italic;">(Campership Approved)</span>
                </p>
                <p>
                    You have already run payment for <asp:Label style="font-weight: bold;" runat="server" ID="lblTotalCampersDone"></asp:Label> records.
                    <span style="font-style: italic;">(Payment Requested)</span>
                </p>
            </asp:Panel>
            
            <div style="margin:20px">
                <asp:RadioButton runat="server" ID="rdoRunPayment" GroupName="a" Text="Run Payment for records in Campership Approved, Payment Pending" AutoPostBack="true" OnCheckedChanged="rdoReversePayment_CheckedChanged" />
                <br/>
                <asp:RadioButton runat="server" ID="rdoReversePayment" GroupName="a" Text="Reverse Payment of records in Payment Requested (last batch only)" AutoPostBack="true" OnCheckedChanged="rdoReversePayment_CheckedChanged" />
            </div>
            
            <asp:Panel runat="server" ID="pnlPaymentRun" Visible="False">
                <div style="margin:15px; border: 1px solid black; padding: 10px; max-width: 800px; background-color: lightgrey">
                    <p>
                        <span style="font-weight: bold">Stpe 1</span> - Run a 'preliminary' report to confirm the data.
                    </p>
                    <p>
                        <span style="font-weight: bold">Stpe 2</span> - Run a 'final' report.
                    </p>
                    <p>
                        A final report will automatically adjust those records to Payment Requested.  These records will not appear in future payment runs.  Save the final report on your local computer.
                    </p>
                </div>
                <asp:CheckBox ID="chkAllCamps" style="margin: 20px;" runat="server" Text="Select all camps" oncheckedchanged="chkAll_CheckedChanged" AutoPostBack="true" />

                <asp:CheckBoxList style="margin: 20px;"  ID="chklistCamp" runat="server" DataSourceID="odsCamp" DataTextField="Name" DataValueField="ID" RepeatDirection="Vertical" 
                    RepeatColumns="2" ondatabound="chklistCamp_DataBound" />
                <asp:ObjectDataSource ID="odsCamp" runat="server" TypeName="CampsDA" SelectMethod="GetAllCampsByFedID">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlCampYear" PropertyName="SelectedValue" Type="Int32" Name="CampYearID" />
                        <asp:ControlParameter ControlID="ddlFed" PropertyName="SelectedValue" Type="Int32" Name="FedID" />
                    </SelectParameters>
                </asp:ObjectDataSource>

                <asp:Button ID="btnRun" runat="server" Text="STEP 1: Run Preliminary Payment" OnClientClick="RunPayment();" Visible="False" />
                <asp:Button ID="btnRun2" runat="server" Text="STEP 2: Run Final Payment" OnClientClick="RunPayment2();" Visible="False" />
                <br />
                <div id="divWarning" style="color: red" runat="server">
                    <p>
                        We recommend you first run a preliminary report to confirm the data (grant amount, camp, session dates).  Running a FINAL report will update these records to Payment Requested.
                    </p>
                    <asp:CheckBox runat="server" ID="chkFinal" ForeColor="Red" Text="Click to proceed running FINAL mode."/>                    
                </div>
                
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlReversePayment" style="margin: 20px;" Visible="False">
                Currently it's empty
                <br/><br/>
                <asp:Button ID="btnReverse" runat="server" Text="Reverse a Payment" OnClientClick="ReversePayment();" Visible="false" />
            </asp:Panel>

            <asp:Label ID="lblMsgCamps" runat="server" ForeColor="Red"></asp:Label>
            <br/>
            <br/>
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label> 
        </ContentTemplate>
    </asp:UpdatePanel>    
        <asp:Button runat="server" ID="btnHiddenRun" OnClick="btnRunPayment_Click" style="display: none" />
        <asp:Button runat="server" ID="btnHiddenRun2" OnClick="btnRunPayment2_Click" style="display: none" />
        <asp:Button runat="server" ID="btnHiddenReverse" OnClick="btnReversePayment_Click" style="display: none" />
</asp:Content>

