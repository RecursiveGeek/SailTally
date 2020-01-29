using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Series : Page
    {
        #region Methods
        public void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsert.Visible = !detailVisible;
            gridSeries.Visible = !detailVisible;

            labelDetail.Visible = detailVisible;
            detailsSeries.Visible = detailVisible;
            linkSummary.Visible = detailVisible;

            detailsSeries.ChangeMode(DetailsViewMode.ReadOnly);
        }

        public void DataUpdate()
        {
            ShowDetail(false);
            gridSeries.DataBind();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ShowDetail(false);
        }

        protected void linkInsert_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            detailsSeries.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsSeries.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void gridSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridSeries, ref detailsSeries);
            ShowDetail(true);
        }

        protected void detailsSeries_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridSeries, detailsSeries);
        }

        protected void detailsSeries_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsSeries_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsSeries_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }
        #endregion

    }
}