using BusinessClasses;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace skrive_opgave
{

    public partial class Default : System.Web.UI.Page
    {
        static BL bl = new BL();
        static List<Text> texts = bl.GetAllTexts();
        static string activeId = "Arly";
        protected void Page_Load(object sender, EventArgs e)
        { 
            createbuttons();
            createtext(activeId);
            var cookie = Request.Cookies.Get("Log");
            if (cookie == null)
            {
                bl.LogUser(Request);
                bl.CreateLogCookie(Response);
            }
        }
        
        protected void RecreateTexts(object sender, EventArgs e)
        {
            var id = ((Button)sender).CommandArgument;
            activeId = id;
            for (int i = form1.Controls.Count - 1; i >= 0; i--)
            {
                var control = form1.Controls[i];
                if (control.ID != null)
                {
                    form1.Controls.Remove(control);
                }
            }
            form1.Controls.OfType<HtmlGenericControl>().Where(c => c.TagName == "br").ToList().ForEach(c => form1.Controls.Remove(c));
            createbuttons();
            createtext(activeId);
        }
        protected void createbuttons()
        {
            var btn = new Button
            {
                Text = "Admin Login",
                CssClass = "btn4",
                ID="AdminLogin"
            };
            btn.Attributes.Add("runat", "server");
            btn.Click += new EventHandler(BtnClick);
            form1.Controls.Add(btn);
            foreach (var text in texts)
            {

                var ViewBtn = new Button
                {
                    Text = $"View text: {text.Header}",
                    CssClass = "btn4",
                    ID = $"SHOWBTN{text.Id}",
                    CommandArgument = text.Id
                };
                ViewBtn.Attributes.Add("runat", "server");
                ViewBtn.Click += new EventHandler(RecreateTexts);
                form1.Controls.Add(ViewBtn);
            }
        }
        protected void createtext(string id)
        {
            var r = new Random();
            var txt = texts.Where(x => x.Id == id).ToList()[0];
            form1.Controls.Add(new HtmlGenericControl("br"));
            form1.Controls.Add(new Label() { Text = txt.Header, CssClass = "h3", ID=$"header{txt.Id}" });
            form1.Controls.Add(new HtmlGenericControl("br"));
            foreach (var paragraph in txt.Paragraph)
            {
                var lbl = new Label
                { Text = paragraph.Text, ID = r.Next(0, 10000).ToString(), CssClass = "h2" };
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