<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FJCAllocationReport.aspx.cs" Inherits="FJCAllocationReport" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <span class="PageHeaderText">FJC Allocation</span>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="divMenu" runat="server" visible="true">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <strong>Year:</strong>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlYear_SelectedIndexChanged">
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
            </ContentTemplate>
        </asp:UpdatePanel>               
        <br />
        <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
        <br />
        <asp:Button ID="btnReport" runat="server" Text="Report" onclick="btnReport_Click" /> 
       
    </div>
    <div id="divReport" runat="server" visible="false">
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" 
            onclick="lnkbtnBack_Click"></asp:LinkButton>
        <span style="margin-left:10px"><asp:Button ID="btnExcelExport" 
        Text="Export to Excel" runat="server" onclick="btnExcelExport_Click" /></span>            
        <br /><br />    
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="Program Name" DataField="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField HeaderText="Total $ Allocated" DataField="Total Allocated" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:C0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField HeaderText="FJC $ Standard" DataField="FJC Standard" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:C0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField HeaderText="FJC $ Overage" DataField="FJC Overage" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:C0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField HeaderText="Partner $ Allocated" DataField="Partner Allocated" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:C0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField HeaderText="Total Campers" DataField="Total Campers" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField HeaderText="First Time" DataField="First Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField HeaderText="Second Time" DataField="Second Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />                                                                                
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

