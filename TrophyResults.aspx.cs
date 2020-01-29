using System;
using System.Linq;
using SailTally.Classes;

namespace SailTally
{
    public partial class TrophyResults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CentralLibrary.GetSeasons(listSeason);
        }

        protected void buttonTrophyResults_Click(object sender, EventArgs e)
        {
            listSeason.Enabled = false;
            buttonTrophyResults.Enabled = false;
            spanNotice.Visible = false;

            GetTrophyResults();
        }

        protected void GetTrophyResults()
        {
            var seasonId = Convert.ToInt32(listSeason.SelectedValue);
            var report = string.Empty;

            var ctx = new SailTallyDataContext();

            var trophyResults = from t in ctx.SS_Trophies
                                join f in ctx.SS_Fleets on t.FleetID equals f.FleetID
                                join s in ctx.SS_Series on t.SeriesID equals s.SeriesID
                                where t.SeasonID == seasonId
                                orderby f.FleetName, s.SeriesName, t.RaceNumber, t.TrophyName // Order is critical for processing abandoned races
                                select new { f.FleetName, t.FleetID, s.SeriesName, t.SeriesID, t.TrophyName, t.RaceNumber, t.Place, t.ShiftToNext };

            report += "<table class='table table-striped TableGeneral'>";
            report += "<tr><th>Fleet</th><th>Series</th><th>Scheduled Race #</th><th>Trophy</th><th>Boat</th><th>Race # Assigned</th></tr>";
            var racesWithTrophy = new Races();
            foreach (var trophyResult in trophyResults)
            {
                // On the fly calculation
                var assignTrophy = new AssignTrophy(trophyResult.FleetID, trophyResult.SeriesID, seasonId, trophyResult.RaceNumber, trophyResult.Place, trophyResult.ShiftToNext);
                assignTrophy.FindTrophy(racesWithTrophy);
                report += "<tr><td>" + trophyResult.FleetName + "</td><td>" + trophyResult.SeriesName + "</td><td>" + assignTrophy.RaceNumberStr() + "</td><td>" + trophyResult.TrophyName + "</td><td>" + assignTrophy.BoatWinner() + "</td><td>" + assignTrophy.RaceNumberAssigned() + "</td></tr>";
            }
            report += "</table>";

            labelTrophyResults.Text = report;
        }
    }

}