<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IRIDIUM_GMDSS_LRIT.AdminWebUI.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Iridium GMDSS LRIT Kemilinks Gateway Admin</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: auto;  width: 50%;  padding: 10px;">
        <br /><br /><br /><br /><br /><br /><br /><br /><br />
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMainLabel" runat="server" Text="Kemilinks Iridium GMDSS LRIT Admin Portal" Font-Size="X-Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUsermae" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                </td>
                <td>
                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="txtMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
