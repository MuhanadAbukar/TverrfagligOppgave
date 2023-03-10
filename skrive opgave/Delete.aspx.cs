using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace skrive_opgave
{
    [Authorize]
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var returnbutton = new Button
            {
                Text = $"Return",
                CssClass = "btn"
            };
            returnbutton.Click += new EventHandler(Return);
            returnbutton.Attributes.Add("runat", "server");
            returnbutton.ID = "Return";
            form1.Controls.Add(returnbutton);
            CheckIfAuthenticated();
            var bl = new BL();
            var everything = bl.GetAllText();
            foreach (var Text in everything)
            {
                form1.Controls.Add(new Label() { Text = $"Text ID: {Text.Id}", CssClass = "h1" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new Label() { Text = $"Header", CssClass = "h1" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new TextBox() { Text = Text.Header, ID = Text.Id.ToString() + "h", TextMode = TextBoxMode.MultiLine, Height = 250, Width = 320, CssClass = "field" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new Label() { Text = $"Paragraph", CssClass = "h1" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new TextBox() { Text = Text.Paragraph, ID = Text.Id.ToString() + "p", TextMode = TextBoxMode.MultiLine, Height = 250, Width = 320, CssClass = "field" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                var btn = new Button
                {
                    Text = $"Delete Text with ID {Text.Id}",
                    CssClass = "btn"
                };
                btn.Click += new EventHandler(EditText);
                btn.Attributes.Add("runat", "server");
                btn.ID = Text.Id.ToString();
                form1.Controls.Add(btn);
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new HtmlGenericControl("br"));
            }
        }
        protected void EditText(object sender, EventArgs e)
        {
            CheckIfAuthenticated();
            var btn = (Button)sender;
            var textid = btn.ID;
            var bl = new BL();
            bl.DeleteTextEntry(textid);
            bl.LogDelete(bl.GetUsernameOfUser(User), textid,Request);
            Response.Redirect(Request.RawUrl);
        }
        private void CheckIfAuthenticated()
        {
            if (!User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
        }
        private void Return(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }
    }
}