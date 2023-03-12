<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="skrive_opgave.Create" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
   <head runat="server">
      <title>Create</title>
      <link rel="stylesheet" href="Admin.css"/>
   </head>
   <body>
      <form id="form1" runat="server">
         <div class="box" runat="server">
            <div class="content" runat="server" id="d1">
               <h1>Create new Text</h1>
               <asp:Label ID="Label1" runat="server" CssClass="h1" Text="Header"></asp:Label>
               <asp:TextBox CssClass="field" id="Header" Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox>
               <br/>
               <br/>
               <asp:Button CssClass="btn" id="CreateParagraph" runat="server" Text="Create new paragraph" OnClick="CreateParagraph_Click"></asp:Button>
               <br />
               <asp:Button CssClass="btn" id="CreateButton" runat="server" Text="Create" OnClick="CreateButton_Click"></asp:Button>
               <br/>
               <asp:Button CssClass="btn" id="ReturnButton" runat="server" Text="Return" OnClick="Return"></asp:Button>

            </div>
         </div>
      </form>
   </body>
</html>