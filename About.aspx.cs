using System;
using System.Web.UI;

namespace SailTally
{
    public partial class About : Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            labelVersion.Text = "Version " + CentralLibrary.Version;
        }
        #endregion
    }
}
