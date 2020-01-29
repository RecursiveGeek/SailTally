using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Penalties : Page
    {
        #region Methods
        public void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsert.Visible = !detailVisible;
            gridPenalties.Visible = !detailVisible;
            labelNotice.Visible = !detailVisible;

            labelDetail.Visible = detailVisible;
            detailsPenalties.Visible = detailVisible;
            linkSummary.Visible = detailVisible;
        }

        public void DataUpdate(object sender)
        {
            ShowDetail(false);
            gridPenalties.DataBind();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ShowDetail(false);
        }

        protected void gridPenalties_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridPenalties, ref detailsPenalties);
            ShowDetail(true);
        }

        protected void detailsPenalties_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridPenalties, detailsPenalties);
        }

        protected void linkInsert_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            detailsPenalties.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsPenalties.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void detailsPenalties_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate(sender);
        }

        protected void detailsPenalties_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate(sender);
        }

        protected void detailsPenalties_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate(sender);
        }
        #endregion

    }
}