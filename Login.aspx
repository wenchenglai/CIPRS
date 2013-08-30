<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table style="LEFT: 280px; POSITION: absolute; TOP: 210px">
        <tr>
            <td>
                <asp:Label ID="lblErr" runat="server" /></td></tr>
    </table>
    <table cellpadding="0" border="0" cellspacing="0" style="LEFT: 350px; POSITION: absolute; TOP: 240px">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" Text="Administration Maintenance" CssClass="headertext1" /></td></tr>
        <tr>
            <td colspan="2" style="height:10px"></td></tr>
        <tr>
            <td>User ID:&nbsp;</td>
            <td>
                <asp:TextBox ID="txtUsrId" runat="server" Width="120px" Text="1" cssClass="text" /></td></tr>
        <tr style="height:10px"><td colspan="2"></td></tr>
        <tr>
            <td>Password:&nbsp;</td>
            <td>
                <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" Width="120px" cssClass="text" /></td></tr>
        <tr style="height:10px"><td colspan="2"></td></tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnSubmit" Text="Submit" runat="server" Width="100px" cssClass="text" OnClick="btnSubmit_Click" /></td></tr>
    </table>
    </form>
</body>
</html>
