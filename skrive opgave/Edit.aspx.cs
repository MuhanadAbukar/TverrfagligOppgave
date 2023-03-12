using BusinessClasses;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace skrive_opgave
{
    public partial class Edit : System.Web.UI.Page
    {
        static List<storage> storage = new List<storage>();
        [Authorize]
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckIfAuthenticated();
            var bl = new BL();
            var everything = bl.GetAllTexts();
            var returnbutton = new Button
            {
                Text = $"Return",
                CssClass = "btn"
            };
            returnbutton.Click += new EventHandler(Return);
            returnbutton.Attributes.Add("runat", "server");
            returnbutton.ID = "Return";
            form1.Controls.Add(returnbutton);
            var i = 0;
            var j = 0;
            foreach (var Text in everything)
            {
                form1.Controls.Add(new Label() { Text = $"Text ID: {Text.Id}", CssClass = "h1" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new Label() { Text = $"Header", CssClass = "h1" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                form1.Controls.Add(new TextBox() { Text = Text.Header, ID = Text.Id.ToString() + "h", TextMode = TextBoxMode.MultiLine, Height = 250, Width = 320, CssClass = "field" });
                form1.Controls.Add(new HtmlGenericControl("br"));
                if (Text.Paragraph.Count > 0) { form1.Controls.Add(new Label() { Text = $"Paragraph", CssClass = "h1" }); }
                form1.Controls.Add(new HtmlGenericControl("br"));
                var list = new List<TextBox>();
                foreach (Paragraphs paragraph in Text.Paragraph)
                {
                    var textBox = new TextBox
                    {
                        Text = paragraph.Text,
                        ID = Text.Id+$"p{i}",
                        TextMode = TextBoxMode.MultiLine,
                        Height = 250,
                        Width = 320,
                        CssClass = "field"
                    };
                    list.Add(textBox);
                    form1.Controls.Add(textBox);
                    i++;
                }
                storage.Add(new storage());
                storage[j].textboxs = list;
                storage[j].header = Text.Id;
                j++;
                form1.Controls.Add(new HtmlGenericControl("br"));
                var editbutton = new Button
                {
                    Text = $"Edit Text with ID {Text.Id}",
                    CssClass = "btn"
                };
                editbutton.Click += new EventHandler(EditText);
                editbutton.Attributes.Add("runat", "server");
                editbutton.ID = Text.Id.ToString();
                form1.Controls.Add(editbutton);
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
            var text = new Text();
            var storedbox = storage.FirstOrDefault(x => x.header == textid);
            text.Header = ((TextBox)form1.FindControl(storedbox.header+"h")).Text;
            text.Paragraph = storedbox.textboxs.Select(p => new Paragraphs { Text = p.Text }).ToList();
            bl.UpdateTextEntry(textid, text);
            var username = bl.GetUsernameOfUser(User);
            bl.LogAdminEdit(Request, username, textid);
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