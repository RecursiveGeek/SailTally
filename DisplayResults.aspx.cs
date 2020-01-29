using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class DisplayResults : System.Web.UI.Page
    {
        #region Properties
        public int SelectedSeason => Convert.ToInt32(listSeason.SelectedValue);

        #endregion

        #region Methods
        protected void ShowSummary(bool showSummary)
        {
            //this.listSeason.Enabled = !showSummary;
            //this.listFleet.Enabled = !showSummary;

            //this.buttonView.Visible = !showSummary;
            //this.buttonReset.Visible = showSummary;
            //this.panelSummary.Visible = showSummary;

            buttonReset.Visible = false; // TODO: Remove Reset Button and the ShowSummary Method
        }

        protected void GetFleetsInResults(DropDownList list)
        {
            var ctx=new SailTallyDataContext();
            var fleetNames = (from rs in ctx.SS_ResultSummaries
                              where rs.SeasonID == SelectedSeason
                              orderby rs.FleetName
                              select new { rs.FleetName }).Distinct();

            list.DataSource = fleetNames;
            list.DataTextField = "fleetname";
            list.DataValueField = "fleetname";
            list.DataBind();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CentralLibrary.GetSeasons(listSeason);
            GetFleetsInResults(listFleet);
            ShowSummary(false);
        }

        protected void buttonView_Click(object sender, EventArgs e)
        {
            ShowSummary(true);

            labelResults.Text = CentralLibrary.GetResultsHeader(listSeason.SelectedItem.Text, listFleet.SelectedValue) 
                + CentralLibrary.GetResultsKeyHeader()
                + CentralLibrary.GetResults(SelectedSeason, listFleet.SelectedValue) 
                + CentralLibrary.GetResultsKeyFooter();
        }

        protected void buttonReset_Click(object sender, EventArgs e)
        {
            ShowSummary(false);
        }

        protected void listSeason_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetFleetsInResults(listFleet);
        }
        #endregion
    }
}