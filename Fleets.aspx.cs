using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Fleets : Page
    {
        #region Methods
        public void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsert.Visible = !detailVisible;
            gridFleets.Visible = !detailVisible;

            labelDetail.Visible = detailVisible;
            detailsFleets.Visible = detailVisible;
            linkSummary.Visible = detailVisible;
        }

        public void DataUpdate()
        {
            ShowDetail(false);
            gridFleets.DataBind();
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
            detailsFleets.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsFleets.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void gridFleets_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridFleets, ref detailsFleets);
            ShowDetail(true);
        }

        protected void detailsFleets_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridFleets, detailsFleets);
        }

        protected void detailsFleets_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsFleets_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsFleets_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }
        #endregion
    }
}