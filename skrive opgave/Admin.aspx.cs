using BusinessLayer;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace skrive_opgave
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("AdminPage.aspx");
            }
        }

        protected void PasswordRecovery1_VerifyingUser1(object sender, EventArgs e)
        {
            var valid = new EmailAddressAttribute();
            var label = Label2;
            var email = UserName.Text;
            if (!valid.IsValid(email))
            {

                label.Text = "Invalid Email.";
                label.Visible = true;
                return;
            }
            var url = Request.Url.GetLeftPart(UriPartial.Path);
            var newUrl = url.Substring(0, url.LastIndexOf("/") + 1);
            var bl = new BL();
            var sentlink = bl.GenerateAndSendPasswordResetLink(email, newUrl);
            label.Visible = true;
            if (!sentlink)
            {
                label.Text = "Sent password reset link.";
                return;
            }
            label.Text = "You already have an active password reset link.";
            return;
        }

        protected void LoginButton_Click(object sender, AuthenticateEventArgs e)
        {
            var name = Login1.UserName;
            var password = Login1.Password;
            var bl = new BL();
            var permissiontype = bl.CheckPSWDUSRNM(name, password);
            if (permissiontype == 0)
            {
                ((Label)Login1.FindControl("Label1")).Text = "The username or password you entered is incorrect.";
                ((Label)Login1.FindControl("Label1")).Visible = true;
                return;
            }

            var authticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(15), false, JsonConvert.SerializeObject(new[] { name, permissiontype.ToString() }));
            var encryptedticket = FormsAuthentication.Encrypt(authticket);
            var authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedticket)
            {
                Expires = authticket.Expiration
            };
            bl.LogAdmin(Request, name);
            Response.Cookies.Add(authcookie);
            Response.Redirect("AdminPage.aspx");

        }

        protected void Return_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}