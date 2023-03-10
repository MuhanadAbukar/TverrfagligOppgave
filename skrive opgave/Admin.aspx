<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.Aspx.cs" Inherits="skrive_opgave.Admin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
   <head runat="server">
      <title>Login Page</title>
      <link rel="stylesheet" href="Admin.css"/>
   </head>
   <body>
      <form id="form1" runat="server">
           <asp:Login id="Login1" runat="server" CssClass="width" OnAuthenticate="LoginButton_Click" >
               <LayoutTemplate>
                  <div class="box" runat="server">
                     <div class="content" runat="server">
                        <h1>Authentication Required</h1>
                        <asp:TextBox CssClass="field" placeholder="Username" id="UserName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator id="UserNameRequired" runat="server" controltovalidate="UserName" errormessage="User Name is required." tooltip="User Name is required." validationgroup="Login1">*</asp:RequiredFieldValidator>
                        <br>
                        <asp:TextBox CssClass="field" placeholder="Password" id="Password" runat="server" textmode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator id="PasswordRequired" runat="server" controltovalidate="Password" errormessage="Password is required." tooltip="Password is required." validationgroup="Login1">*</asp:RequiredFieldValidator>
                        <br>
                        <asp:Button CssClass="btn" id="LoginButton" runat="server" commandname="Login" text="Log In" validationgroup="Login1"></asp:Button>
                        <br>
                        <asp:Button CssClass="btn" id="Return" runat="server" Text="Return" OnClick="Return_Click"></asp:Button>
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="Label" CssClass="h1" Visible="false"></asp:Label>
                     </div>
                  </div>
              </LayoutTemplate>
            </asp:Login>
            
                  <div class="box2" runat="server">
                     <div class="content2" id="hello" runat="server">
                        <h1>Forgot password?</h1>
                        <h1> Enter Email </h1>
                        <asp:TextBox CssClass="field2" placeholder="Email" ID="UserName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Email is required." ToolTip="Email is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                        <asp:Label ID="Label2" runat="server" Text="Label" CssClass="h1" Visible="false"></asp:Label>
                        <asp:Button CssClass="btn2" ID="SubmitButton" runat="server" CommandName="Submit" Text="Get password reset link" OnClick="PasswordRecovery1_VerifyingUser1" />
                     </div>
                  </div>
             </form>
   </body>
</html>