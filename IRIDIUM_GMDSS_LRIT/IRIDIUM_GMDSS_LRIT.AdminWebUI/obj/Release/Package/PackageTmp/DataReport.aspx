<%@ Page Title="Query Data Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataReport.aspx.cs" Inherits="IRIDIUM_GMDSS_LRIT.AdminWebUI.DataReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <b><u>Query Data Reports</u></b>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:DropDownList ID="ddlPeriod" runat="server">
                <asp:ListItem Value="60">Last 1 Hour</asp:ListItem>
                <asp:ListItem Value="180">Last 3 Hours</asp:ListItem>
                <asp:ListItem Value="360">Last 6 Hours</asp:ListItem>
                <asp:ListItem Value="1440">Last 1 Day</asp:ListItem>
                <asp:ListItem Value="10080">Last 7 Days</asp:ListItem>
                <asp:ListItem Value="44640">Last 1 Month</asp:ListItem>
                <asp:ListItem Value="-1">Unlimited</asp:ListItem>
            </asp:DropDownList>
             Source: <asp:TextBox ID="txtSource" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <hr />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:GridView ID="gvDataReport" runat="server" AutoGenerateColumns="False" GridLines="Vertical" BackColor="White" 
                  BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" class="gridView">
                <AlternatingRowStyle BackColor="#FFCC66" />
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="ReceiveTimestamp" HeaderText="Receive Timestamp" />
                    <asp:BoundField DataField="Raw" HeaderText="Raw" />
                    <asp:BoundField DataField="Header" HeaderText="Header" />
                    <asp:BoundField DataField="Message" HeaderText="Message" />
                    <asp:BoundField DataField="Latitude_Hemisphere" HeaderText="Lat Hemisphere" />
                    <asp:BoundField DataField="Latitude_Degree" HeaderText="Lat Degree" />
                    <asp:BoundField DataField="Latitude_Minute" HeaderText="Lat Minute" />
                    <asp:BoundField DataField="Latiitude_MinuteDecimal" HeaderText="Lat MinuteDecimal" />
                    <asp:BoundField DataField="Longitude_Hemisphere" HeaderText="Long Hemisphere" />
                    <asp:BoundField DataField="Longitude_Degree" HeaderText="Long Degree" />
                    <asp:BoundField DataField="Longitude_Minute" HeaderText="Long Minute" />
                    <asp:BoundField DataField="Longitude_MinuteDecimal" HeaderText="Long MinuteDecimal" />
                    <asp:BoundField DataField="Event" HeaderText="Event" />
                    <asp:BoundField DataField="Month" HeaderText="Month" />
                    <asp:BoundField DataField="Day" HeaderText="Day" />
                    <asp:BoundField DataField="Hour" HeaderText="Hour" />
                    <asp:BoundField DataField="Minute" HeaderText="Minute" />
                    <asp:BoundField DataField="Reserved" HeaderText="Reserved" />
                </Columns>
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#F7F7DE" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
