using BusinessClasses;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace skrive_opgave
{
    [Authorize]

    public partial class Create : System.Web.UI.Page
    {
        static int i = 1;
        static int x = 0;
        static List<TextBox> paragraphs = new List<TextBox>();
        static List<Label> labels = new List<Label>();
        [Authorize]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("Admin.aspx");
            var x = 0;
            foreach (var tb in paragraphs)
            {

                var index = d1.Controls.IndexOf((TextBox)d1.FindControl("Header"));
                d1.Controls.AddAt(index + x + 2, labels[paragraphs.IndexOf(tb)]);
                d1.Controls.AddAt(index + x + 3, tb);
                x += 2;
            }
        }

        protected void CreateButton_Click(object sender, EventArgs e)
        {
            var bl = new BL();
            var text = new Text()
            {
                Header = Header.Text,
                Paragraph = paragraphs.Select(prg => new Paragraphs()
                {
                    Text = prg.Text
                }).ToList()
            };
            var id = bl.CreateNewText(text);
            bl.LogCreate(bl.GetUsernameOfUser(User), 2, Request);
            ((Button)sender).Text = "Created new text";
        }
        protected void Return(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }

        protected void CreateParagraph_Click(object sender, EventArgs e)
        {

            var txt = new TextBox()
            {
                TextMode = TextBoxMode.MultiLine,
                CssClass = "field",
                Rows = 10,
                ID = "txtParagraph" + i.ToString(),
            };

            var lbl = new Label()
            {
                Text = "Paragraph " + i.ToString(),
                CssClass = "h1",
                AssociatedControlID = "txtParagraph" + i.ToString(),
            };

            d1.Controls.AddAt(d1.Controls.IndexOf((TextBox)d1.FindControl("Header")) + 2 + x, lbl);
            d1.Controls.AddAt(d1.Controls.IndexOf((TextBox)d1.FindControl("Header")) + 3 + x, txt);
            paragraphs.Add(txt);
            labels.Add(lbl);
            i++;
            x += 2;
        }
    }
}