<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportFJCCampers.aspx.cs" Inherits="ReportFJCCampers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<asp:HyperLink ID="hylBack" runat="server" Text="Back to report menu" NavigateUrl="~/CampersByCampFJC.aspx"></asp:HyperLink>--%>
        <asp:LinkButton ID="lnkbtnBack" runat="server" Text="Back to report menu" 
            onclick="lnkbtnBack_Click"></asp:LinkButton>
        <asp:Button ID="btnReport" runat="server" Text="Export to Excel" 
            onclick="btnReport_Click" />
        <br /><br />
        <div>
            <span id="spanPagerTop" runat="server">
            <asp:LinkButton ID="lnkbtnPrevious" runat="server" Text="<<" onclick="lnkbtnPrevious_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtPage" Text="1" runat="server" Width="40" ontextchanged="txtPage_TextChanged" AutoPostBack="true"></asp:TextBox>
            &nbsp;&nbsp;of&nbsp;&nbsp;
            <asp:Label ID="lblPage" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkbtnNext" runat="server" Text=">>" onclick="lnkbtnNext_Click"></asp:LinkButton>
            </span>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>        
        </div>
        <br /><br />
        <strong><asp:Label ID="lblCampName" runat="server"></asp:Label></strong>
        <br />
        <div id="divPerCamp" runat="server" visible="false" class="CampLayout">            
            <asp:GridView ID="gvPerCamp" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvPerCamp_RowDataBound">
            <Columns>
                <asp:BoundField DataField="FJCID" HeaderText="FJCID" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="LastName" HeaderText="LastName" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Program" HeaderText="Program" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CampName" HeaderText="Camp" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Grade" HeaderText="Grade" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Session Name" HeaderText="Session Name" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="SessionDates" HeaderText="SessionDates" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="1st/2nd/3rd Timer" HeaderText="1st/2nd/3rd Timer" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Grant Amount" HeaderText="Grant Amount" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Parent Name" HeaderText="Parent Name" Visible="false" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="PermissionToContact" HeaderText="Permission to Contact" ItemStyle-HorizontalAlign="Center" />                
                <asp:BoundField DataField="Parent Email" HeaderText="Parent Email" Visible="true" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Parent Phone" HeaderText="Parent Phone" Visible="false" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ModifiedDate" HeaderText="Modified Date" Visible="true" ItemStyle-HorizontalAlign="Center" />              
            </Columns>
            </asp:GridView>
            <br />
            <asp:GridView ID="gvTotalPerCamp" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Total Campers" HeaderText="Total Campers" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Total Amount" HeaderText="Total Amount" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Center" />             
                <asp:BoundField DataField="Total First Timer" HeaderText="Total First Time" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="First Amount" HeaderText="First Amount" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Total Second Timer" HeaderText="Total Second Time" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Second Amount" HeaderText="Second Amount" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Center" />                                                                               
                <asp:BoundField DataField="Total Third Timer" HeaderText="Total Third Time" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Third Amount" HeaderText="Third Amount" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Center" /> 
            </Columns>
            </asp:GridView>             
        </div>  
        <div id="divPerSynag" runat="server" visible="false" class="CampLayout">            
            <asp:GridView ID="gvPerSynag" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="LastName" HeaderText="LastName" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Fed" HeaderText="Program" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Synag" HeaderText="Synagogue" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Camp" HeaderText="Camp" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Grant Amount" HeaderText="Grant Amount" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ParentName" HeaderText="Parent" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="PersonalEmail" HeaderText="Parent Email" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="HomePhone" HeaderText="Parent Phone" ItemStyle-HorizontalAlign="Center" />                
            </Columns>
            </asp:GridView>
            <br />
            <asp:GridView ID="gvTotalPerSynag" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Total Campers" HeaderText="Total Campers" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Total Amount" HeaderText="Total Amount" DataFormatString="{0:c}" HtmlEncode="False" ItemStyle-HorizontalAlign="Center" />                                                                             
            </Columns>
            </asp:GridView>             
        </div>  
        <div id="divCamperContactInfo" runat="server" visible="false" class="CampLayout">            
            <asp:GridView ID="gvCamperContactInfo" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvCamperContactInfo_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="FJCID" HeaderText="FJCID" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Program" HeaderText="Program" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CampName" HeaderText="Camp" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Year of Grant" HeaderText="Year of Grant" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Parent1FirstName" HeaderText="Parent 1 First Name" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent1LastName" HeaderText="Parent 1 Last Name" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent2FirstName" HeaderText="Parent 2 First Name" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent2LastName" HeaderText="Parent 2 Last Name" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Street" HeaderText="Street" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="City" HeaderText="City" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="State" HeaderText="State" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Zip" HeaderText="Zip" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PermissionToContact" HeaderText="Permission to Contact" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent1Email" HeaderText="Parent 1 Email" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent2Email" HeaderText="Parent 2 Email" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PrimaryPhone" HeaderText="Primary Phone" Visible="true" ItemStyle-HorizontalAlign="Center" />               
                    <asp:BoundField DataField="ModifiedDate" HeaderText="Modified Date" Visible="true" ItemStyle-HorizontalAlign="Center" /> 
                </Columns>
            </asp:GridView>            
        </div> 
        <div id="divCamperDetailReport" runat="server" visible="false" class="CampLayout">            
            <asp:GridView ID="gvCamperDetailReport" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvCamperDetailReport_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="FJCID" HeaderText="FJCID" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="StatusName" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" Visible="true" ItemStyle-HorizontalAlign="Center" /> 
                    <asp:BoundField DataField="SpecialCode" HeaderText="Code" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Camp Name" HeaderText="Camp" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="GrantAmount" HeaderText="Grant" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Days" HeaderText="Days" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent1FirstName" HeaderText="Parent First Name" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent1LastName" HeaderText="Parent Last Name" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Parent1Email" HeaderText="Parent Email" Visible="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PrimaryPhone" HeaderText="Primary Phone" Visible="true" ItemStyle-HorizontalAlign="Center" />      
                    <asp:BoundField DataField="Street" HeaderText="Street" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="City" HeaderText="City" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="State" HeaderText="State" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Zip" HeaderText="Zip" ItemStyle-HorizontalAlign="Center" />

             

                </Columns>
            </asp:GridView>            
        </div>                         
        <br /><br />
        <div>
            <span id="spanPagerBottom" runat="server">
            <asp:LinkButton ID="lnkbtnPrevious2" runat="server" Text="<<" onclick="lnkbtnPrevious_Click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtPage2" Text="1" runat="server" Width="40" ontextchanged="txtPage_TextChanged" AutoPostBack="true"></asp:TextBox>
            &nbsp;&nbsp;of&nbsp;&nbsp;
            <asp:Label ID="lblPage2" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkbtnNext2" runat="server" Text=">>" onclick="lnkbtnNext_Click"></asp:LinkButton>
            </span>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblMsg2" runat="server" ForeColor="Red"></asp:Label>        
        </div> 
        <br /><br />
        <%--<asp:HyperLink ID="hylBack2" runat="server" Text="Back to report menu" NavigateUrl="~/CampersByCampFJC.aspx"></asp:HyperLink>--%>   
        <asp:LinkButton ID="lnkbtnBack2" runat="server" Text="Back to report menu" 
            onclick="lnkbtnBack_Click"></asp:LinkButton>              
    </div>
    </form>
</body>
</html>
