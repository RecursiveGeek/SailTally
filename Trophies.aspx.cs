using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Trophies : Page
    {
        #region Methods
        private void ShowSummary(bool summaryVisible)
        {
            ShowDetail(false);

            gridTrophy.Visible = false;
            buttonSelect.Visible = !summaryVisible;
            listSeason.Enabled = !summaryVisible;
            listFleet.Enabled = !summaryVisible;
            buttonReset.Visible = summaryVisible;

            labelSummary.Visible = summaryVisible;
            linkInsert.Visible = summaryVisible;

            gridTrophy.DataBind();
        }

        private void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsert.Visible = !detailVisible;
            gridTrophy.Visible = !detailVisible;

            labelDetails.Visible = detailVisible;
            detailsTrophy.Visible = detailVisible;
            linkSummary.Visible = detailVisible;
        }

        private void DataUpdate()
        {
            ShowDetail(false);
            gridTrophy.DataBind();
        }

        private string GetSeriesId()
        {
            string seriesId = null;
            var listSeries = (DropDownList)detailsTrophy.FindControl("listSeries");

            if (listSeries != null)
            {
                seriesId = listSeries.SelectedValue;
            }

            return seriesId;
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CentralLibrary.GetFleets(listFleet);
            CentralLibrary.GetSeasons(listSeason);
            ShowSummary(false);
        }
        
        protected void buttonSelect_Click(object sender, EventArgs e)
        {
            ShowSummary(true);
            ShowDetail(false);
        }

        protected void buttonReset_Click(object sender, EventArgs e)
        {
            ShowSummary(false);
        }

        protected void gridTrophy_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridTrophy, ref detailsTrophy);
            ShowDetail(true);
        }

        protected void detailsTrophy_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridTrophy, detailsTrophy);
        }

        protected void linkInsert_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            detailsTrophy.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsTrophy.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void detailsTrophy_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsTrophy_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsTrophy_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsTrophy_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            e.Values.Add("FleetID", listFleet.SelectedItem.Value); // Link to the filter condition
            e.Values.Add("SeasonID", listSeason.SelectedItem.Value); // Link to the filter condition
            e.Values.Add("SeriesID", GetSeriesId()); // get drop list value from the detailsview
        }

        protected void detailsTrophy_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            e.NewValues.Add("SeriesID", GetSeriesId()); // get the drop list value from the detailsview
        }

        protected void listSeries_PreRender(object sender, EventArgs e)
        {
            var listSeries = (DropDownList)sender;
            var labelSeries = (Label)detailsTrophy.FindControl("labelSeries");
            var ctx = new SailTallyDataContext();

            var series = from s in ctx.SS_Series
                         join fs in ctx.SS_FleetSeries on s.SeriesID equals fs.SeriesID
                         where fs.FleetID == Convert.ToInt32(listFleet.SelectedValue) && fs.SeasonID == Convert.ToInt32(listSeason.SelectedValue)
                         select new { s.SeriesID, s.SeriesName };

            listSeries.DataSource = series;
            listSeries.DataTextField = "seriesname";
            listSeries.DataValueField = "seriesid";
            listSeries.DataBind();

            if (labelSeries!=null)
            {
                listSeries.SelectedValue = labelSeries.Text; // get the Series ID value for an edit or view (in DetailsView template)
            }
        }

        /*
        protected void listFleet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList listSeries = (DropDownList)this.gridTrophy.Rows[this.gridTrophy.EditIndex].FindControl("listSeries");
            DropDownList listFleet = (DropDownList)sender;
            if (listSeries != null)
            {
                this.labelTest.Text = listSeries.SelectedValue;
                int fleetID = Convert.ToInt32(listFleet.SelectedValue);

                this.GetSeries(fleetID, listSeries);
            }
            else
            {
                this.labelTest.Text = "No control found";
            }
        }

        protected void gridTrophy_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DropDownList listSeries = (DropDownList)this.gridTrophy.Rows[e.NewEditIndex].FindControl("listSeries");
            DropDownList listFleet = (DropDownList)this.gridTrophy.Rows[e.NewEditIndex].FindControl("listFleet");

            if (listSeries != null && listFleet != null)
            {
                int fleetID = Convert.ToInt32(listFleet.SelectedValue);
                this.GetSeries(fleetID, listSeries);
            }
        }

        private void GetSeries(int fleetID, DropDownList listSeries)
        {
            SailTallyDataContext ctx = new SailTallyDataContext();
            var series = from s in ctx.SS_Series
                         join fs in ctx.SS_FleetSeries on s.SeriesID equals fs.SeriesID
                         join f in ctx.SS_Fleets on fs.FleetID equals f.FleetID
                         where f.FleetID == fleetID
                         select new { s.SeriesID, s.SeriesName };

            listSeries.DataSource = series;
            listSeries.DataTextField = "seriesname";
            listSeries.DataValueField = "seriesid";
        }
        */
        #endregion
    }
}