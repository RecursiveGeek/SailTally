using System;
using System.Web.UI;

namespace SailTally
{
    public partial class DisplaySchedule : Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CentralLibrary.GetSeasons(listSeason);
        }

        protected void buttonSelect_Click(object sender, EventArgs e)
        {
            labelSeason.Text = listSeason.SelectedItem.Text;
            //CentralLibrary.DisplaySchedule(this.tableSchedule, null, "", Convert.ToInt32(this.listSeason.SelectedValue), true);
            CentralLibrary.DisplaySchedule(tableSchedule, "", Convert.ToInt32(listSeason.SelectedValue), true);
        }
        #endregion
    }
}