<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="skrive_opgave.PasswordReset" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="New password"></asp:Label>
            <br />
            <asp:TextBox ID="NewPassword1" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Confirm new password"></asp:Label>
            <br />
            <asp:TextBox ID="NewPassword" runat="server"></asp:TextBox><br />
            <asp:Button ID="Button1" runat="server" Text="Update password" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
