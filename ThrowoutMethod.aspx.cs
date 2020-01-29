using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class ThrowoutMethod : Page
    {
        #region Methods
        public void ShowDetail(bool detailVisible)
        {
            labelSummary.Visible = !detailVisible;
            linkInsertMethod.Visible = !detailVisible;
            gridThrowout.Visible = !detailVisible;

            labelDetail.Visible = detailVisible;
            detailsThrowoutMethod.Visible = detailVisible;
            linkSummary.Visible = detailVisible;
            linkInsertCount.Visible = detailVisible;
            gridThrowoutDetail.Visible = detailVisible;
        }

        public void DataUpdate()
        {
            ShowDetail(false);
            gridThrowout.DataBind();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowDetail(false);
            }
        }

        protected void gridThrowout_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridThrowout, ref detailsThrowoutMethod);
            ShowDetail(true);
        }

        protected void detailsThrowoutMethod_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridThrowout, detailsThrowoutMethod);
        }

        protected void gridThrowoutDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void linkInsertMethod_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            detailsThrowoutMethod.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsThrowoutMethod.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void linkInsertCount_Click(object sender, EventArgs e)
        {
            if (gridThrowout.SelectedDataKey == null) return;
            var throwoutId = Convert.ToInt32(gridThrowout.SelectedDataKey.Value); // the selected scoring method that has our attention
            var ctx = new SailTallyDataContext();

            var throwoutDetail = new SS_ThrowoutDetail()
            {
                ThrowoutID = throwoutId
            };

            ctx.SS_ThrowoutDetails.InsertOnSubmit(throwoutDetail);
            ctx.SubmitChanges();

            gridThrowoutDetail.DataBind();
            gridThrowoutDetail.SetEditRow(gridThrowout.SelectedIndex);
        }

        protected void detailsThrowoutMethod_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsThrowoutMethod_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsThrowoutMethod_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }
        #endregion
    }
}