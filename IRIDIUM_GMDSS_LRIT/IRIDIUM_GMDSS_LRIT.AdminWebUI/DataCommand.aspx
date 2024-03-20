<%@ Page Title="Query Data Command" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataCommand.aspx.cs" Inherits="IRIDIUM_GMDSS_LRIT.AdminWebUI.DataCommand" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <b><u>Query Data Commands</u></b>
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
             Destination: <asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>
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
            <asp:GridView ID="gvDataCommand" runat="server" AutoGenerateColumns="False" GridLines="Vertical" BackColor="White" 
                  BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="10" ForeColor="Black">
                <AlternatingRowStyle BackColor="#FFCC66" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" />
                    <asp:BoundField DataField="Direction" HeaderText="Direction" />
                    <asp:BoundField DataField="Type" HeaderText="Type" />
                    <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" />
                    <asp:BoundField DataField="Raw" HeaderText="Raw" />
                    <asp:BoundField DataField="H_Header" HeaderText="Header" Visible="false"/>
                    <asp:BoundField DataField="M_Message" HeaderText="Message" Visible="false"/>
                    <asp:BoundField DataField="P_PollPosition" HeaderText="Poll Position" />
                    <asp:BoundField DataField="SI_SetIMO" HeaderText="Set IMO" />
                    <asp:BoundField DataField="SO_SetOffset" HeaderText="Set Offset" />
                    <asp:BoundField DataField="ST_SetTimer" HeaderText="Set Timer" />
                    <asp:BoundField DataField="SD_SetReduced" HeaderText="Set Reduced" />
                    <asp:BoundField DataField="A_Ack" HeaderText="Ack" />
                    <asp:BoundField DataField="I_IMONumber" HeaderText="IMO Number" />
                    <asp:BoundField DataField="R_2BitReserved" HeaderText="2BitReserved" Visible="false"/>
                    <asp:BoundField DataField="O_Offset" HeaderText="Offset" />
                    <asp:BoundField DataField="T_Timer" HeaderText="Timer" />
                    <asp:BoundField DataField="D_Reduced" HeaderText="Reduced" />
                    <asp:BoundField DataField="R_3BitReserved" HeaderText="3BitReserved" Visible="false"/>
                    <asp:BoundField DataField="SN_SetResponsible" HeaderText="Set Responsible" />
                    <asp:BoundField DataField="N_Responsible" HeaderText="Responsible" />
                    <asp:BoundField DataField="V_Version" HeaderText="Version" Visible="false"/>
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