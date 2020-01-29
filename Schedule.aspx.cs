using System;
using System.Web.UI.WebControls;
using SailTally.Classes;

namespace SailTally
{
    public partial class Schedule : System.Web.UI.Page
    {
        #region Properties
        public string SelectedSeason // used to expose to PostBack pages
        {
            get
            {
                return listSeason.SelectedValue;
            }
        }

        public string SelectedDate // used to expose to PostBack pages
        {
            get
            {
                return hiddenLastDateUsed.Value;
            }
        }
        #endregion

        #region Methods
        protected void HideFilterOptions(bool hideFilter)
        {
            listSeason.Enabled = !hideFilter;
            buttonSelect.Visible = !hideFilter;
            buttonReset.Visible = hideFilter;

            panelSchedule.Visible = hideFilter;
        }

        protected bool ShowTheSchedule()
        {
            return !listSeason.Enabled;
        }

        private void BuildSchedule()
        {
            if (ShowTheSchedule())
            {
                CentralLibrary.DisplaySchedule(tableSchedule, "~/ScheduleModify.aspx", Convert.ToInt32(listSeason.SelectedValue), false);
            }
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CentralLibrary.GetSeasons(listSeason);

            var showSchedule = false;
            // ReSharper disable once MergeSequentialChecks (Done to support Visual Studio 2013)
            if (Page.PreviousPage != null && Page.PreviousPage.Master != null) // if a calling page (such as ScheduleModify), get its information to pull up the schedule
            {
                var cph = (ContentPlaceHolder)Page.PreviousPage.Master.FindControl("MainContent");
                var seasonId = (HiddenField)cph.FindControl("hiddenSeasonID");
                if (seasonId != null)
                {
                    listSeason.SelectedValue = seasonId.Value;
                    showSchedule=true;
                }
                var dateLastUsed = (Label)cph.FindControl("labelSchedule");
                if (dateLastUsed != null)
                {
                    hiddenLastDateUsed.Value = dateLastUsed.Text;
                }
            }

            HideFilterOptions(showSchedule);
        }

        protected override void OnPreRender(EventArgs e)
        {
            BuildSchedule();
        }

        protected void buttonSelect_Click(object sender, EventArgs e)
        {
            HideFilterOptions(true);
            BuildSchedule();
        }

        protected void buttonReset_Click(object sender, EventArgs e)
        {
            HideFilterOptions(false);
        }

        protected void buttonRaceID_Click(object sender, EventArgs e)
        {
            Server.Transfer("~/ScheduleModify.aspx" + Constant.Url.Parameters + Constant.GetVar.RaceId + Constant.Url.Assign + textRaceID.Text);
        }

        protected void linkNewSeries_Click(object sender, EventArgs e)
        {
            // ToDo: Implement
        }
        #endregion

    }

    [Serializable]
    public class SenderSchedule
    {
        public int SeasonId { get; set; }
        public int RaceId { get; set; }
    }
}