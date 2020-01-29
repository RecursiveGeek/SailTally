using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class ScoreMethod : Page
    {
        #region Methods
        public void ShowDetail(bool detailVisible)
        {
            labelScoreMethod.Visible = !detailVisible;
            linkInsertMethod.Visible = !detailVisible;
            gridScoreMethod.Visible = !detailVisible;

            labelScoreMethodDetail.Visible = detailVisible;
            detailsScoreMethod.Visible = detailVisible;
            linkSummary.Visible = detailVisible;
            linkInsertPlace.Visible = detailVisible;
            gridScoreMethodDetail.Visible = detailVisible;
        }

        public void DataUpdate()
        {
            ShowDetail(false);
            gridScoreMethod.DataBind();
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

        protected void gridScoreMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridScoreMethod, ref detailsScoreMethod);
            ShowDetail(true);
        }

        protected void detailsScoreMethod_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridScoreMethod, detailsScoreMethod);
        }

        protected void gridScoreMethodDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void linkInsertMethod_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            detailsScoreMethod.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkInsertPlace_Click(object sender, EventArgs e)
        {
            if (gridScoreMethod.SelectedDataKey == null) return;
            var scoreMethodId = Convert.ToInt32(gridScoreMethod.SelectedDataKey.Value); // the selected scoring method that has our attention
            var ctx = new SailTallyDataContext();

            var scoreMethodDetail = new SS_ScoreMethodDetail()
            {
                ScoreMethodID = scoreMethodId
            };

            ctx.SS_ScoreMethodDetails.InsertOnSubmit(scoreMethodDetail);
            ctx.SubmitChanges();

            gridScoreMethodDetail.DataBind();
            gridScoreMethodDetail.SetEditRow(gridScoreMethodDetail.SelectedIndex);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowDetail(false);
            detailsScoreMethod.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void detailsScoreMethod_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsScoreMethod_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsScoreMethod_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }
        #endregion

    }
}