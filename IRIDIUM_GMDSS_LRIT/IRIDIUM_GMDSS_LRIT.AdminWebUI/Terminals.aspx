<%@ Page Title="Terminal List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Terminals.aspx.cs" Inherits="IRIDIUM_GMDSS_LRIT.AdminWebUI.Terminals" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <b><u>Terminal List</u></b>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:GridView ID="gvTerminals" runat="server" AutoGenerateColumns="False" GridLines="Vertical" BackColor="White" 
                  BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"  class="gridView">
                <AlternatingRowStyle BackColor="#FFCC66" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" />
                    <asp:BoundField DataField="MSISDN" HeaderText="MSISDN" />
                    <asp:BoundField DataField="IMONumber" HeaderText="IMO Number" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                    <asp:BoundField DataField="ActivationTimestamp" HeaderText="Activation Timestamp"/>
                    <asp:BoundField DataField="DeactivationTimestamp" HeaderText="Deactivation Timestamp"/>
                    <asp:BoundField DataField="CreationTimestamp" HeaderText="Creation Timestamp" />
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
    <div class="row">
        <div class="col-md-12">
            <hr />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <b><u>Add New LT-3100s Terminal</u></b>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    
                </div>
                <div class="col-md-9">
                    
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    MDISDN
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="txtMSISDN" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    IMO Number
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="txtIMONumber" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    Description
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    Remarks
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    Application IDs [,]
                </div>
                <div class="col-md-9">
                    <asp:TextBox ID="txtApplicationIDs" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnAddNewTerminal" runat="server" Text="Add New Terminal" OnClick="btnAddNewTerminal_Click" />
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
