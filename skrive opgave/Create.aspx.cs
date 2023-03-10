using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Web.UI.WebControls;

namespace skrive_opgave
{
    public partial class Create : System.Web.UI.Page
    {
        [Authorize]
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
            d1.Controls.Add(returnbutton);
        }

        protected void CreateButton_Click(object sender, EventArgs e)
        {
            var bl = new BL();
            var id = bl.CreateNewText(Header.Text, Paragraph.Text);
            bl.LogCreate(bl.GetUsernameOfUser(User), id, Request);
            CreateButton.Text = "Created new text";
        }
        private void Return(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }
    }
}