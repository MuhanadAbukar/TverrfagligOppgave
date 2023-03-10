using BusinessClasses;
using DatabaseLayer;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;
using static BusinessClasses.WebhookPost;

namespace BusinessLayer
{
    public class BL
    {

        private static List<string> WebHooks = new List<string>() {
        "https://discord.com/api/webhooks/1080794173931802644/4pkxeuNkStWvmdjEUlNUw8agBQQHZtp6FJ7qAltAr_-Sby2slOnqKuRlZwpcE7pDRjZW",
        "https://discord.com/api/webhooks/1080794171004178452/_DAkiGFWXlrvbr9p4cz0SSGP29aTDwmnzKJQH0d0vn2ImUSfcprJG5ILL3oECq5cvnXd",
        "https://discord.com/api/webhooks/1080794166851801178/bA4O_NHEEYzvZGXShuja45cA40n3Iq866dXURpMmJSg-17jJdg47uvK-eTiOeAu5_bAn",
        "https://discord.com/api/webhooks/1080794158136049704/lwU9oDDJntC1TZlWWo9C9XWK2e99wIEqdae3OZEVVcc0BwcFWB7oO7kzpfJk0zuMkaSB",
        "https://discord.com/api/webhooks/1080790414266728538/UM-UP4YpC87Dshnd5UbfnZx7jRYX0IpxMOifauVXknKrNzmrtivJzQfpVAaPxMofQOtc",
        "https://discord.com/api/webhooks/1080790410680614982/z1R_VfgzYxSlu0WW3_ZodY87WklvhpBI9NZWSitJEFQr4XNMCcriEhINl6dwQ3ie2S7T",
        "https://discord.com/api/webhooks/1083663313394733056/tJ7zK5NyDt3esUAonuf-JaKUKz5Ss3tonFj_Q9kRa0HeAFqMd9QzR-01792XXmcuip1x",
        "https://discord.com/api/webhooks/1083663317169614948/TKnTgxdS2IGEYdyjH0fuh0ggqmUX1Dxgx_d0BL8ZCeoGDQUtSNvqCv3swFI6DFAhClwp",
        "https://discord.com/api/webhooks/1083663319866544139/pklk5Q-t3PCjMMFRUByR3-9U8bTvr-_ZnBhc426N712kVPl10sQWxkhzXx_3n5JhxIIw",
        "https://discord.com/api/webhooks/1083663322680930314/c8TkIchCcY4HnwkcJ2wA2ePEdJU-JLTQuRfb4d7oOdsptPTX5g8APsc_m7RnsTGjPkWA",
        "https://discord.com/api/webhooks/1083663323985350676/DPTnXrlM-bJ1SSKzkNf4abmHWY8sU_4AT8elsnLNpeHqvnEcP4RdvBEjzR_o18WuW0yD",
        "https://discord.com/api/webhooks/1083663326795542608/nCPckW3R40KTMRne0JYYXQNXtg_4sNromz6fpvVcR_di49Nn70Q_JGK3tVjJ15ojnEDs",
        "https://discord.com/api/webhooks/1083663329811255366/B2y6LTVmRzoNSimn9kxyksO4XlFIHVFAJu8GmM3DaUWiFa9h0Sx4dvtoQp0-ZOXvAo-D"};
        private static DBL DBL = new DBL();
        public Text GetAllText(string text)
        {
            var dt = DBL.GetAllText(text);
            return dt;
        }
        
        public void LogCreate(string username, int id, HttpRequest request)
        {
            SendWebhook(request, $"User {username} has created a new text with ID: {id}.");
        }
        public int CreateNewText(string Header, string Paragraph)
        {
            return DBL.CreateNewText(Header, Paragraph);
        }
        public void LogDelete(string username, string textid, HttpRequest request)
        {
            SendWebhook(request, $"User {username} has deleted text with ID: {textid }.");
        }
        public void LogAdminLogOut(HttpRequest request, string username)
        {
            SendWebhook(request, $"User {username} has logged out.");
        }
        public void LogUser(HttpRequest request)
        {
            SendWebhook(request, "Logged new user.");
        }
        public void LogUnauthorizedUser(HttpRequest request)
        { 
            SendWebhook(request, "Unauthorized user tried to access to Admin Page.");
        }
        public void LogAdmin(HttpRequest request, string username)
        {
            SendWebhook(request, $"User {username} has logged in.");
        }
        public void LogAdminEdit(HttpRequest request, string username, string textid)
        {
            SendWebhook(request, $"User {username} has edited text with ID: {textid} ");
        }
        public void SendWebhook(HttpRequest request, string message)
        {
            var result = $"HostAddress = {request.UserHostAddress}\n UserHostName = {request.UserHostName}\n UserAgent = {request.UserAgent}\n Browser = {request.Browser.Browser}\n HTTPMethod = {request.HttpMethod}\n Path = {request.Path}\n";
            var embed = new Embed()
            {
                title = message,
                description = result
            };
            var info = new Embed[] { embed };
            var js = $"{{\"embeds\":{JsonConvert.SerializeObject(info)} }}";
            var _cl = new HttpClient();
            var pl = new StringContent(js, Encoding.UTF8, "application/json");
            var r = new Random();
            _cl.PostAsync(WebHooks[r.Next(0, WebHooks.Count)], pl);
        }
        public string GetUsernameOfUser(IPrincipal User)
        {
            var identity = (FormsIdentity)User.Identity;
            var ticket = identity.Ticket;
            var userdata = JsonConvert.DeserializeObject<string[]>(ticket.UserData);
            var username = userdata[0];
            return username;
        }
        public string GetPermissionTypeOfUser(IPrincipal User)
        {
            var identity = (FormsIdentity)User.Identity;
            var ticket = identity.Ticket;
            var userdata = JsonConvert.DeserializeObject<string[]>(ticket.UserData);
            var permissiontype = userdata[1];
            return permissiontype;
        }
        public void CreateLogCookie(HttpResponse Response)
        {
            var userCookie = new HttpCookie("Log");
            userCookie.Expires = DateTime.Now.AddMinutes(1);
            Response.Cookies.Add(userCookie);
        }

        public string HashString(string password)
        {
            var sb = new StringBuilder();
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public void DeleteTextEntry(string textid)
        {
            var dbl = new DBL();
            dbl.DeleteTextEntry(textid);
        }
        public void UpdateTextEntry(string textid, string header, string paragraph)
        {
            var dbl = new DBL();
            dbl.UpdateTextEntry(textid, header, paragraph);
        }
        public bool GenerateAndSendPasswordResetLink(string email, string url)
        {
            var linkcode = Guid.NewGuid().ToString();
            var db = new DBL();
            var alreadyhaslink = db.SavePasswordRecoveryLink(linkcode, email);
            if (!alreadyhaslink)
            {
                var link = $"{url}PasswordReset.aspx?id={linkcode}";
                sendEmail(email, "Your password reset link", $"Here is your password reset link. It's only active for 15 minutes.", link);
            }
            return alreadyhaslink;
        }
        public int CheckPSWDUSRNM(string username, string password)
        {
            var dbl = new DBL();
            password = HashString(password);
            return dbl.CheckIfPasswordAndUsernameMatch(username, password);
        }
        public bool doesidexist(string link)
        {
            return DBL.exists(link);
        }
        public void sendEmail(string toEmail, string header, string message, string message2)
        {
            var email = new MimeMessage();
            var sender = new MailboxAddress("Password Reset", "noreplypswdreset@gmail.com");
            email.From.Add(sender);
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = header;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<h1> {message} </h1><br> <h1>{message2} </h1>" };
            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("noreplypswdreset@gmail.com", "uzjpimmgtcvogvkc");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public void UpdatePasswordById(string id, string pass)
        {
            var dbl = new DBL();
            dbl.UpdatePasswordById(id, HashString(pass));
        }
    }
}
