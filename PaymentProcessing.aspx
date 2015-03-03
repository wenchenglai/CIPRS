<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentProcessing.aspx.cs" Inherits="PaymentProcessing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">Payment Processing Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <strong>Summer:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlCampYear" DataValueField="id" DataTextField="text" AutoPostBack="true" runat="server"></asp:DropDownList>
            <br /><br />    
            <strong>Program:</strong>&nbsp;&nbsp;
            <asp:DropDownList ID="ddlFed" runat="server" DataSourceID="odsFed" DataValueField="ID" DataTextField="Name" AutoPostBack="true" 
                ondatabound="ddlFed_DataBound" onselectedindexchanged="ddlFed_SelectedIndexChanged" />
            <asp:ObjectDataSource ID="odsFed" runat="server" TypeName="FederationsDA" SelectMethod="GetAllFederationsByUserRole">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlCampYear" Name="CampYearID" PropertyName="SelectedValue" Type="Int32" />
                    <asp:SessionParameter SessionField="RoleID" Name="UserRole" Type="Int32" />
                    <asp:SessionParameter SessionField="FedID" Name="FedID" Type="Int32" />
                    <asp:SessionParameter SessionField="UserID" Name="UserID" Type="Int32" />
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
                    <asp:ControlParameter ControlID="ddlCampYear" PropertyName="SelectedValue" Type="Int32" Name="CampYearID" />
                    <asp:ControlParameter ControlID="ddlFed" PropertyName="SelectedValue" Type="Int32" Name="FedID" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:Label ID="lblMsgCamps" runat="server" ForeColor="Red"></asp:Label> 
            <br />
            <br />
            <asp:Panel runat="server" ID="pnlInfo">
                <p>
                    You have <asp:Label runat="server" ID="lblTotalCampersReady"></asp:Label> campers ready to be paid 
                    across <asp:Label runat="server" ID="lblTotalCampsReady"></asp:Label> camps (Status: of Campership Approved Payment Pending).                    
                </p>
                <p>
                    You have already processed payment for <asp:Label runat="server" ID="lblTotalCampersDone"></asp:Label> campers 
                    across <asp:Label runat="server" ID="lblTotalCampsDone"></asp:Label> camps (Status:  Payment Requested).
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>    
    <br />
    <br />
    <asp:Button ID="btnReport" runat="server" Text="Run Payment" onclick="btnRunPayment_Click" />
    <asp:Button ID="Button1" runat="server" Text="Reverse a Payment" onclick="btnReversePayment_Click" />
</asp:Content>

