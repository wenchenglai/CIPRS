<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProgramProfileReport.aspx.cs" Inherits="ProgramProfileReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript">
    function SwapCheck(checkBox) {
        if (checkBox) {
            if (checkBox)
                HandleAllOnCheckAll(checkBox.checked);
        }
    }

    function HandleAllOnCheckAll(checked) {
        var chkLst = document.getElementById('<%=chkFedLst.ClientID %>');

        if (chkLst) {
            var checkBoxArray = chkLst.getElementsByTagName('input');

            for (var i = 0; i < checkBoxArray.length; i++) {
                var checkBoxRef = checkBoxArray[i];
                checkBoxRef.checked = checked;
            }
        }
    }

    function HandleOnCheckList() {
        var chkLst = document.getElementById('<%=chkFedLst.ClientID %>');
        var chkAll = document.getElementById('<%=chkAll.ClientID %>');
        var chkCount = 0;
        if (chkLst) {
            var checkBoxArray = chkLst.getElementsByTagName('input');
            for (var i = 0; i < checkBoxArray.length; i++) {
                var checkBoxRef = checkBoxArray[i];
                if (checkBoxRef.checked)
                    chkCount++;
            }
            if (checkBoxArray.length == chkCount)
                chkAll.checked = true;
            else
                chkAll.checked = false;
        }
    }


</script>
    <table class="text" width="100%" cellpadding="0" cellspacing="0" style="border-right: red 0.5pt solid;
        border-top: red 0.5pt solid; border-left: red 0.5pt solid; border-bottom: red 0.5pt solid;">
        <tr>
            <td style="height: 10px">
          
            </td>
        </tr>
        <tr>
            <td align="center">
                <span style="font-size: 14px; font-weight: bold; font-family: Verdana">Program Profile
                    Information Report</span>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td>
                <div id="divFederationSelection" runat="server">
                    <table class="text" width="100%" style="border-right: thin solid; border-top: thin solid;
                        border-left: thin solid; border-bottom: thin solid;">
                        <tr>
                            <td>
                                <asp:Label ID="lblReportMessage" runat="server" CssClass="lblpopup1" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFederation" runat="server" Text="Select Federation" Width="121px"
                                    CssClass="text" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;<input id="chkAll" type="checkbox" value="0" runat="server"  onclick="JavaScript:SwapCheck(this);"/>All
                                <asp:CheckBoxList ID="chkFedLst" runat="server" CssClass="text" RepeatColumns="2">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" CssClass="submitbtn" OnClick="BtnGenerateReportClick" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        
        
    </table>
</asp:Content>

