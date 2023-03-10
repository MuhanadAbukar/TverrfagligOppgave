using BusinessClasses;
using BusinessLayer;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace skrive_opgave
{

    public partial class Default : System.Web.UI.Page
    {
        BL bl = new BL();
        protected void Page_Load(object sender, EventArgs e)
        {


            CreateLabels();
            var cookie = Request.Cookies.Get("Log");
            if (cookie == null)
            {
                bl.LogUser(Request);
                bl.CreateLogCookie(Response);
            }
        }
        private void CreateLabels()
        {
            var btn = new Button
            {
                Text = "Admin Login"
            };
            btn.Attributes.Add("runat", "server");
            btn.Click += new EventHandler(BtnClick);
            form1.Controls.Add(btn);
            var dt = bl.GetAllText();
            var r = new Random();
            form1.Controls.Add(new HtmlGenericControl("br"));
            form1.Controls.Add(new HtmlGenericControl("h1") { InnerText = dt.Header });
            form1.Controls.Add(new HtmlGenericControl("br"));
            foreach(var paragraph in dt.Paragraph)
            {
                var lbl = new Label
                { Text = paragraph.Text, ID = r.Next(0, 10000).ToString() };
                lbl.Attributes.Add("textmode", "multiline");
                form1.Controls.Add(lbl);
                form1.Controls.Add(new HtmlGenericControl("br"));
            }
            form1.Controls.Add(new HtmlGenericControl("br"));

        }
    
        protected void BtnClick(object sender, EventArgs e)
        {
            Response.Redirect("Admin.aspx");
        }
    }
}