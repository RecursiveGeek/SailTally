using System;
using System.Configuration;
using SailTally.Classes;

namespace SailTally
{
    public partial class SiteMaster : MasterPageBase
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterJavascriptBlock(Constant.BlockName.WorkingDialog, Constant.Scripts.HideWorkingLast);

            //var loginName = this.HeadLoginView.FindControl("HeadLoginView") as LoginName;
            //if (loginName != null)
            //{
            //    loginName.FormatString = "Full Name";
            //}
        }
        #endregion

        #region Functions
        public static string GetGoogleId()
        {
            return ConfigurationManager.AppSettings["GoogleAnalyticsTrackingId"];
        }
        #endregion
    }
}
