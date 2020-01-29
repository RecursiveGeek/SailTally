using System;
using System.Linq;
using System.Web.UI;

namespace SailTally
{
    public partial class BoatResults : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CentralLibrary.GetSeasons(listSeason);
        }

        protected void buttonBoatResults_Click(object sender, EventArgs e)
        {
            listSeason.Enabled = false;
            buttonBoatResults.Enabled = false;
            spanNotice.Visible = false;

            GetBoatResults();
        }

        private void GetBoatResults()
        {
            var report = "";

            var ctx = new SailTallyDataContext();

            var boatResults = from rs in ctx.SS_ResultSummaries
                              join b in ctx.SS_Boats on rs.BoatID equals b.BoatID
                              join f in ctx.SS_Fleets on b.FleetID equals f.FleetID
                              where rs.SeasonID == Convert.ToInt32(listSeason.SelectedValue)
                              orderby f.FleetName, b.BoatID, rs.Position, rs.SeriesName
                              select new { rs.FleetName, rs.BoatName, rs.Skipper, rs.SeriesName, rs.Position, rs.BoatID, rs.SailNumber };

            var boatId = -1;

            report += "<table>";
            foreach (var boatResult in boatResults)
            {
                if (boatId != boatResult.BoatID) // Have we moved to another boat?  If so, new "header"
                {
                    boatId = boatResult.BoatID;
                    report += "<tr><td colspan='2'><h3>" + boatResult.FleetName + " #" + boatResult.SailNumber + (boatResult.BoatName!=null && boatResult.BoatName.Length > 0 ? " " : "") + boatResult.BoatName + ", " + boatResult.Skipper + "</h3></td></tr>";
                }

                report += "<tr><td>" + boatResult.SeriesName + "</td><td>" + CentralLibrary.PositionSuffix(boatResult.Position) + "</td></tr>";
            }
            report += "</table>";

            labelBoatResults.Text = report;
        }
    }
}