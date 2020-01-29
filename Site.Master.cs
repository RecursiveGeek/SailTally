using System;
using System.Configuration;
using SailTally.Classes;

namespace SailTally
{
    public partial class SiteMaster : MasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterJavascriptBlock(Constant.BlockName.WorkingDialog, Constant.Scripts.HideWorkingLast);
            var googleAnalyticsTrackingId = ConfigurationManager.AppSettings["GoogleAnalyticsTrackingId"];
            if (!string.IsNullOrEmpty(googleAnalyticsTrackingId))
            {
                LiteralBody.Text = "<script>(function(i, s, o, g, r, a, m){i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function(){(i[r].q = i[r].q ||[]).push(arguments)},i[r].l = 1 * new Date(); a = s.createElement(o),m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)})(window, document,'script','https://www.google-analytics.com/analytics.js','ga');ga('create', '" + googleAnalyticsTrackingId + "', 'auto');ga('send', 'pageview');</ script >";
            }
            //var loginName = this.HeadLoginView.FindControl("HeadLoginView") as LoginName;
            //if (loginName != null)
            //{
            //    loginName.FormatString = "Full Name";
            //}
        }
    }
}
