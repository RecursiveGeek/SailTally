using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Times : Page
    {
        #region Methods
        protected void ShowSummary(bool viewSummary)
        {
            linkNew.Visible = viewSummary;
            gridTimes.Visible = viewSummary;

            detailsTimes.Visible = !viewSummary;
            linkSummary.Visible = !viewSummary;
            labelHelp.Visible = !viewSummary;
        }

        public void DataUpdate()
        {
            ShowSummary(true);
            gridTimes.DataBind();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ShowSummary(true);
        }

        protected void gridTimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.SyncDataView(gridTimes, ref detailsTimes);
            ShowSummary(false);

        }

        protected void detailsTimes_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            CentralLibrary.SyncGridView(ref gridTimes, detailsTimes);
        }

        protected void linkNew_Click(object sender, EventArgs e)
        {
            ShowSummary(false);
            detailsTimes.ChangeMode(DetailsViewMode.Insert);
        }

        protected void linkSummary_Click(object sender, EventArgs e)
        {
            ShowSummary(true);
            detailsTimes.ChangeMode(DetailsViewMode.ReadOnly);
        }

        protected void detailsTimes_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsTimes_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            DataUpdate();
        }

        protected void detailsTimes_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            DataUpdate();
        }
        #endregion
    }
}