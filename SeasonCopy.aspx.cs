using System;
using System.Linq;

namespace SailTally
{
    public partial class SeasonCopy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            CentralLibrary.GetSeasons(listSeasonFrom);
            CentralLibrary.GetSeasons(listSeasonTo);

            labelPreflight.Visible = false;
            buttonCopy.Visible = false;
            labelReport.Visible = false;
        }

        protected void buttonStart_Click(object sender, EventArgs e)
        {
            labelPreflight.Visible = true;

            // Pre-flight check
            var fromSeasonId = Convert.ToInt32(listSeasonFrom.SelectedValue);
            var toSeasonId = Convert.ToInt32(listSeasonTo.SelectedValue);

            if (fromSeasonId == toSeasonId)
            {
                labelPreflight.Text = "The two seasons selected must be different.";
                return;
            }

            var ctx = new SailTallyDataContext();

            var fleetSeriesCount = (from fs in ctx.SS_FleetSeries
                                    where fs.SeasonID == toSeasonId
                                    select fs).Count();
            var raceCount = (from r in ctx.SS_Races
                             where r.SeasonID == toSeasonId
                             select r).Count();
            var trophyCount = (from t in ctx.SS_Trophies
                               where t.SeasonID == toSeasonId
                               select t).Count();

            if (fleetSeriesCount != 0 || raceCount != 0 || trophyCount != 0)
            {
                labelPreflight.Text = "There is data in the destination season.  Cannot copy.";
                return;
            }

            labelPreflight.Text = "Ready to start copying.";
            buttonStart.Enabled = false;
            listSeasonFrom.Enabled = false;
            listSeasonTo.Enabled = false;
            buttonCopy.Visible = true;

        }

        protected void buttonCopy_Click(object sender, EventArgs e)
        {
            var fromSeasonId = Convert.ToInt32(listSeasonFrom.SelectedValue);
            var toSeasonId = Convert.ToInt32(listSeasonTo.SelectedValue);

            buttonCopy.Enabled = false;
            labelReport.Visible = true;

            // Transfer Data
            var report = "";
            var count = 0;

            var ctx = new SailTallyDataContext();

            // Add new data
            report += "<br/>Copy data:<br/>";
            try
            {
                var trophies = from t in ctx.SS_Trophies
                               where t.SeasonID == fromSeasonId
                               select t;
                foreach (var trophy in trophies)
                {
                    var newTrophy = new SS_Trophy()
                                        {
                                            TrophyName = trophy.TrophyName,
                                            Donor = trophy.Donor,
                                            SeriesID = trophy.SeriesID,
                                            FleetID = trophy.FleetID,
                                            RaceNumber = trophy.RaceNumber,
                                            Place = trophy.Place,
                                            ShiftToNext = trophy.ShiftToNext,
                                            SeasonID = toSeasonId,
                                            BestSeason = trophy.BestSeason,
                                            Notes = trophy.Notes
                                        };
                    ctx.SS_Trophies.InsertOnSubmit(newTrophy);
                    count++;
                }
                ctx.SubmitChanges();

                report += "<br/>Trophies... " + count + " record(s) duplicated.<br/>";

            }
            catch (Exception ex)
            {
                report += "<br/>Trophies... Error encountered: " + ex + "<br/>";
            }

            count = 0;

            try
            {
                var fleetSeries = from fs in ctx.SS_FleetSeries
                                  where fs.SeasonID == fromSeasonId
                                  select fs;
                foreach (var fleetSery in fleetSeries)
                {
                    var newFleetSery = new SS_FleetSery()
                                           {
                                               SeriesID = fleetSery.SeriesID,
                                               SeasonID = toSeasonId,
                                               FleetID = fleetSery.FleetID,
                                               ScoreMethodID = fleetSery.ScoreMethodID,
                                               ThrowoutID = fleetSery.ThrowoutID,
                                               PrizeID = fleetSery.PrizeID,
                                               IsActive = fleetSery.IsActive
                                           };
                    ctx.SS_FleetSeries.InsertOnSubmit(newFleetSery);
                    count++;
                }
                ctx.SubmitChanges();

                report += "<br/>Fleet Series... " + count + " record(s) duplicated.<br/>";
            }
            catch (Exception ex)
            {
                report += "<br/>Fleet Series... Error encountered: " + ex + "<br/>";
            }

            labelReport.Text = report;
        }
    }
}