using System;
using System.Web;
using System.Web.UI;

namespace SailTally.Account
{
    public partial class Login : Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (Request.IsLocal || Request.IsSecureConnection || !CentralLibrary.UseSsl()) return;
            var redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            Response.Redirect(redirectUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginUser_LoggedIn(object sender, EventArgs e)
        {
            CentralLibrary.Post(CentralLibrary.EventidLoginSuccess, "Login", "Login", "Successful Login: "+LoginUser.UserName, "", "");
        }

        protected void LoginUser_LoginError(object sender, EventArgs e)
        {
            CentralLibrary.Post(CentralLibrary.EventidLoginFail, "Login", "Login", "Failed Login: "+LoginUser.UserName, "", "");
        }
    }
}
