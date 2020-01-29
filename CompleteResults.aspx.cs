using System;
using System.Linq;

// Cannot support ExpresionBody yet since still Supporting Visual Studio 2013
// ReSharper disable ConvertPropertyToExpressionBody

namespace SailTally
{
    public partial class CompleteResults : System.Web.UI.Page
    {
        #region Properties
        protected int SelectedSeason { get { return Convert.ToInt32(listSeason.SelectedValue); } }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CentralLibrary.GetSeasons(listSeason);
            }
        }

        protected void buttonSelect_Click(object sender, EventArgs e)
        {
            var ctx = new SailTallyDataContext();

            var fleetNames = (from rs in ctx.SS_ResultSummaries
                              where rs.SeasonID == SelectedSeason
                              orderby rs.FleetName
                              select new { rs.FleetName }).Distinct();

            var resultsData = string.Empty;
            foreach (var fleetName in fleetNames)
            {
                resultsData += CentralLibrary.GetResultsHeader(listSeason.SelectedItem.Text, fleetName.FleetName) + CentralLibrary.GetResults(SelectedSeason, fleetName.FleetName, true);
            }

            labelResults.Text = resultsData + CentralLibrary.GetResultsKeyFooter();
        }
        #endregion
    }
}