using BusinessLayer;
using System;

namespace skrive_opgave
{
    public partial class PasswordReset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var id = Request.QueryString["id"];
                var bl = new BL();
                if (id == null || !bl.doesidexist(id))
                {
                    Response.Redirect("Admin.aspx");
                }

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            var bl = new BL();
            var newpass = NewPassword.Text;
            if (newpass != NewPassword1.Text)
            {
                Button1.Text = "Passwords do not match.";
                return;
            }
            Button1.Text = "Updated password";
            bl.UpdatePasswordById(id, newpass);
        }
    }
}