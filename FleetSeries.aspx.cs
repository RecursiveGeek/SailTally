using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class FleetSeries : Page
    {
        #region Properties
        protected int SelectedSeason => Convert.ToInt32(listSeason.SelectedValue);

        protected int SelectedFleet => Convert.ToInt32(listFleet.SelectedValue);

        #endregion

        #region Methods
        public void ShowSummary(bool summaryVisible)
        {
            ShowDetail(false);
            gridFleetSeries.Visible = false;
            buttonSelect.Visible = !summaryVisible;
            listSeason.Enabled = !summaryVisible;
            listFleet.Enabled = !summaryVisible;
            buttonReset.Visible = summaryVisible;

            labelSummary.Visible = summaryVisible;
            linkInsert.Visible = summaryVisible;
        }

        public void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsert.Visible = !detailVisible;
            gridFleetSeries.Visible = !detailVisible;

            labelDetail.Visible = detailVisible;
            detailsFleetSeries.Visible = detailVisible;
            linkSummary.Visible = detailVisible;

            gridFleetSeries.DataBind();
            labelDetailError.Text = "";
        }

        public void DataUpdate()
        {
            ShowDetail(false);
            gridFleetSeries.DataBind();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CentralLibrary.GetSeasons(listSeason);
            CentralLibrary.GetFleets(listFleet);
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
            detailsFleetSeries.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void linkInsert_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            detailsFleetSeries.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
        }

        protected void gridFleetSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridFleetSeries, ref detailsFleetSeries);
            ShowDetail(true);
        }

        protected void detailsFleetSeries_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridFleetSeries, detailsFleetSeries);
        }

        protected void detailsFleetSeries_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsFleetSeries_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsFleetSeries_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsFleetSeries_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            var listSeries = (DropDownList)((DetailsView)sender).FindControl("listSeriesInsert");
            var seriesId = Convert.ToInt32(listSeries.SelectedValue);

            if (CentralLibrary.IsFleetSeriesExist(SelectedSeason, SelectedFleet, seriesId))
            {
                labelDetailError.Text = listFleet.SelectedItem.Text + " " + listSeries.SelectedItem.Text + " for " + listSeason.SelectedItem.Text + " already exists.  Unable to add.";
                e.Cancel = true;
            }
            else
            {
                labelDetailError.Text = "";
                e.Values.Add("SeasonID", listSeason.SelectedValue);
                e.Values.Add("FleetID", listFleet.SelectedValue);
            }
        }

        protected void detailsFleetSeries_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            var listSeries = (DropDownList)((DetailsView)sender).FindControl("listSeriesEdit");
            var seriesId = Convert.ToInt32(listSeries.SelectedValue);
            var fleetSeriesId = Convert.ToInt32(detailsFleetSeries.SelectedValue);

            if (CentralLibrary.IsFleetSeriesExist(SelectedSeason, SelectedFleet, seriesId, fleetSeriesId))
            {
                labelDetailError.Text = listFleet.SelectedItem.Text + " " + listSeries.Text + " for " + listSeason.SelectedItem.Text + " already exists.  Unable to change.";
                e.Cancel = true;
            }
            else
            {
                labelDetailError.Text = "";
            }
        }

        protected void checkActiveInsert_Load(object sender, EventArgs e)
        {
            var checkInsert = (CheckBox)sender;
            checkInsert.Checked = true; // set the default value
        }
        #endregion
    }
}