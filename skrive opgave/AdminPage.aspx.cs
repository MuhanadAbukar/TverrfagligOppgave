using BusinessClasses;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace skrive_opgave
{
    [Authorize]
    public partial class Admin_Page : System.Web.UI.Page
    {
        protected Dictionary<PermissionType, Action> Helper = new Dictionary<PermissionType, Action>();
        public Admin_Page()
        {
            Helper.Add(PermissionType.Create, () => AddCreateButton());
            Helper.Add(PermissionType.Edit, () => AddEditButton());
            Helper.Add(PermissionType.All, () => { AddCreateButton(); AddDeleteButton(); AddEditButton(); });
            Helper.Add(PermissionType.Delete, () => AddDeleteButton());
        }


        protected void Page_Load(object sender, EventArgs e) => HandleAdminPageLogin();
        private void HandleAdminPageLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                var bl = new BL();
                var permissiontype = (PermissionType)int.Parse(bl.GetPermissionTypeOfUser(User));
                var username = bl.GetUsernameOfUser(User);
                div1.Controls.Add(new HtmlGenericControl("br"));
                Helper.TryGetValue(permissiontype, out var AddButtons);
                AddButtons();
                AddLogOutButton();
                AdminHeader.Text = $"Velkommen {username}. Hva vil du gjøre?";
                div1.Controls.Add(new HtmlGenericControl("br"));

            }
            else
            {
                var cookie = Request.Cookies.Get("Log");
                var bl = new BL();
                bl.LogUnauthorizedUser(Request);
                if (cookie == null)
                {
                    bl.CreateLogCookie(Response);
                }
                Response.Redirect("Admin.aspx");

            }

        }
        protected void CreateText(object sender, EventArgs e)
        {
            Response.Redirect("Create.aspx");
        }
        protected void EditText(object sender, EventArgs e)
        {
            Response.Redirect("Edit.aspx");
        }
        protected void DeleteText(object sender, EventArgs e)
        {
            Response.Redirect("Delete.aspx");
        }
        protected void AddCreateButton()
        {
            var btn = new Button
            {
                Text = $"Create new Text",
                CssClass = "btn"
            };
            btn.Attributes.Add("runat", "server");
            btn.Click += new EventHandler(CreateText);
            btn.ID = "Create";
            div1.Controls.Add(btn);
            div1.Controls.Add(new HtmlGenericControl("br"));

        }
        protected void AddEditButton()
        {
            var btn = new Button
            {
                Text = $"Edit existing text",
                CssClass = "btn"
            };
            btn.Attributes.Add("runat", "server");
            btn.Click += new EventHandler(EditText);
            btn.ID = "Edit";
            div1.Controls.Add(btn);
        }
        protected void AddDeleteButton()
        {
            var btn = new Button
            {
                Text = $"Delete a text",
                CssClass = "btn"
            };
            btn.Attributes.Add("runat", "server");
            btn.Click += new EventHandler(DeleteText);
            btn.ID = "Delete";
            div1.Controls.Add(btn);
            div1.Controls.Add(new HtmlGenericControl("br"));

        }
        protected void AddLogOutButton()
        {
            var btn = new Button
            {
                Text = $"Log Out",
                CssClass = "btn"
            };
            btn.Attributes.Add("runat", "server");
            btn.Click += new EventHandler(LogOut);
            btn.ID = "LOGOUT";
            div1.Controls.Add(btn);
            div1.Controls.Add(new HtmlGenericControl("br"));

        }
        protected void LogOut(object sender, EventArgs e)
        {
            var bl = new BL();
            var username = bl.GetUsernameOfUser(User);
            bl.LogAdminLogOut(Request, username);
            FormsAuthentication.SignOut();
            Response.Redirect("Admin.aspx");
        }
    }
}