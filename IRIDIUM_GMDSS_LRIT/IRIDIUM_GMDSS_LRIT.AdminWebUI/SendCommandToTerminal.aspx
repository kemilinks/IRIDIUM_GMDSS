<%@ Page Title="Send Data Command" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SendCommandToTerminal.aspx.cs" Inherits="IRIDIUM_GMDSS_LRIT.AdminWebUI.SendCommandToTerminal" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <b><u>Send Command To Terminal</u></b>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            Command Type:
        </div>
        <div class="col-md-2">
            <asp:DropDownList ID="ddlCommandType" runat="server">
                <asp:ListItem Value="Registration">Registration</asp:ListItem>
                <asp:ListItem Value="On-demand Poll" Selected="True">On-demand Poll</asp:ListItem>
                <asp:ListItem Value="Reporting Interval"> Reporting Interval</asp:ListItem>
                <asp:ListItem Value="Stop Reporting">Stop Reporting</asp:ListItem>
                <asp:ListItem Value="Deregistration">Deregistration</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-2">
            Interval:
        </div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlInterval" runat="server">
                <asp:ListItem Value="15">15 Mins</asp:ListItem>
                <asp:ListItem Value="30">30 Mins</asp:ListItem>
                <asp:ListItem Value="60">1 Hour</asp:ListItem>
                <asp:ListItem Value="180">3 Hours</asp:ListItem>
                <asp:ListItem Value="360" Selected="True">6 Hour</asp:ListItem>
                <asp:ListItem Value="720">12 Hour</asp:ListItem>
                <asp:ListItem Value="1439">24 Hour</asp:ListItem>
            </asp:DropDownList>*For Change Interval Only
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            Source:
        </div>
        <div class="col-md-2">
            <asp:TextBox ID="txtSource" runat="server">*993003</asp:TextBox>
        </div>
        <div class="col-md-2">
            Destination: 
        </div>
        <div class="col-md-6">
                <asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-2">
                IMO Number: 
        </div>
        <div class="col-md-10">
            <asp:TextBox ID="txtIMONumber" runat="server"></asp:TextBox>*For Activation Command Only
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
