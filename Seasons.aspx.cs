using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Seasons : Page
    {
        #region Methods
        public void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsert.Visible = !detailVisible;
            gridSeasons.Visible = !detailVisible;

            labelDetail.Visible = detailVisible;
            detailsSeasons.Visible = detailVisible;
            linkSummary.Visible = detailVisible;

            detailsSeasons.ChangeMode(DetailsViewMode.ReadOnly);
        }

        public void DataUpdate()
        {
            ShowDetail(false);
            gridSeasons.DataBind();
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
            detailsSeasons.ChangeMode(DetailsViewMode.Insert);
        }
        
        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsSeasons.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void gridSeasons_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridSeasons, ref detailsSeasons);
            ShowDetail(true);
        }

        protected void detailsSeasons_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridSeasons, detailsSeasons);
        }

        protected void detailsSeasons_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsSeasons_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsSeasons_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }
        #endregion

    }
}