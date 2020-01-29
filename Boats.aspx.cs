using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Boats : Page
    {
        #region Methods
        public void ShowSummary(bool summaryVisible)
        {
            ShowDetail(false);

            gridBoats.Visible = false;
            buttonSelect.Visible = !summaryVisible;
            listFleet.Enabled = !summaryVisible;
            buttonReset.Visible = summaryVisible;

            labelSummary.Visible = summaryVisible;
            linkInsert.Visible = summaryVisible;

            gridBoats.DataBind();
        }

        public void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsert.Visible = !detailVisible;
            gridBoats.Visible = !detailVisible;

            labelDetails.Visible = detailVisible;
            detailsBoat.Visible = detailVisible;
            linkSummary.Visible = detailVisible;
        }

        private void DataUpdate()
        {
            ShowDetail(false);
            gridBoats.DataBind();
        }

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
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
        }

        protected void gridBoats_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridBoats, ref detailsBoat);
            ShowDetail(true);
        }

        protected void detailsBoat_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridBoats, detailsBoat);
        }

        protected void linkInsert_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            detailsBoat.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsBoat.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void detailsBoat_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsBoat_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsBoat_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsBoat_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            // Now handled visually via listFleetBoatInsert_PreRender event
            //e.Values.Add("FleetID", this.listFleet.SelectedItem.Value); // Link to the filter condition
        }

        protected void listFleetBoatInsert_PreRender(object sender, EventArgs e)
        {
            // Setup so visually the same as the filter
            ((DropDownList)sender).SelectedValue = listFleet.SelectedValue;
        }
        #endregion
    }
}