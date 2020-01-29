using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SailTally
{
    public partial class Results : System.Web.UI.Page
    {
        private const int TiebreakerPadding = 3;

        #region Methods
        protected void ShowSummary(bool showSummary)
        {
            listSeason.Enabled = !showSummary;
            buttonSelect.Visible = !showSummary;
            buttonReset.Visible = showSummary;
            panelResults.Visible = showSummary;
            buttonGenerate.Enabled = true;
            checkDetailedOutput.Enabled = true;
            linkDisplayResults.Visible = false;
            labelError.Text = string.Empty;
            labelResults.Text = string.Empty;
        }

        protected bool FindScored(List<BoatList> boatList, int boatId)
        {
            for (var index = 0; index < boatList.Count(); index++)
            {
                if (boatList[index].BoatId == boatId)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            ShowSummary(false);
            CentralLibrary.GetSeasons(listSeason, true);
        }

        protected void buttonSelect_Click(object sender, EventArgs e)
        {
            ShowSummary(true);
        }

        protected void buttonReset_Click(object sender, EventArgs e)
        {
            ShowSummary(false);
        }
        
        protected void buttonGenerate_Click(object sender, EventArgs e)
        {
            buttonGenerate.Enabled = false;
            var seasonId = Convert.ToInt32(listSeason.SelectedItem.Value);
            var ctx = new SailTallyDataContext();
            var generated = CentralLibrary.GetCurrentDateTime();

            var resultsMsg = string.Empty;

            try
            {
                // Delete the existing results
                ctx.ExecuteCommand("delete from SS_Result where seasonID = {0}", seasonId);
                // Replaced the cove below with the above line for efficency sake - it was calling a delete for each record in the result set
                /*
                var resultOld = from r in ctx.SS_Results
                                where r.SeasonID == seasonID
                                select r;
                ctx.SS_Results.DeleteAllOnSubmit(resultOld);
                ctx.SubmitChanges();
                */

                // Delete the existing results summary
                ctx.ExecuteCommand("delete from SS_ResultSummary where seasonID = {0}", seasonId);
                // Replaced the cove below with the above line for efficency sake - it was calling a delete for each record in the result set
                /*
                var resultsSummaryOld = from rs in ctx.SS_ResultSummaries
                                        where rs.SeasonID == seasonID
                                        select rs;
                ctx.SS_ResultSummaries.DeleteAllOnSubmit(resultsSummaryOld);
                ctx.SubmitChanges();
                */

                // LEVEL 1: Work through all fleets
                var fleets = from f in ctx.SS_Fleets
                             where f.IsActive
                             orderby f.ListOrder, f.FleetName
                             select f;
                foreach (var fleet in fleets)
                {
                    // LEVEL 2: Work through the series for a given fleet in a season
                    var fleetCopy = fleet;

                    var fleetSeries = from fs in ctx.SS_FleetSeries
                                      join s in ctx.SS_Series on fs.SeriesID equals s.SeriesID
                                      where fs.IsActive 
                                            && fs.FleetID == fleetCopy.FleetID
                                            && fs.SeasonID == seasonId
                                      select new { fs.FleetSeriesID, fs.FleetID, fs.SeriesID, fs.ScoreMethodID, fs.ThrowoutID, s.SeriesName };

                    foreach (var fleetSery in fleetSeries)
                    {
                        // Find the boats within a series that have been scored
                        var fleetSeryCopy = fleetSery;

                        var boatsInSeries = (from b in ctx.SS_Boats
                                             join sc in ctx.SS_Scores on b.BoatID equals sc.BoatID
                                             join r in ctx.SS_Races on sc.RaceID equals r.RaceID
                                             join rs in ctx.SS_RaceSeries on r.RaceID equals rs.RaceID
                                             join fs in ctx.SS_FleetSeries on rs.FleetSeriesID equals fs.FleetSeriesID
                                             join f in ctx.SS_Fleets on b.FleetID equals f.FleetID
                                             where b.IsActive
                                                && f.FleetID == fleetSeryCopy.FleetID
                                                && fs.SeriesID == fleetSeryCopy.SeriesID
                                                && fs.FleetID == f.FleetID
                                                && fs.SeasonID == seasonId
                                             orderby b.BoatOrder, b.SailNumber, b.BoatID
                                             select new { b.BoatID, b.SailNumber, b.BoatName, b.IsRegistered, b.Skipper, b.Crew }).Distinct();

                        if (boatsInSeries.Any()) // one or more boats scored in a series
                        {
                            resultsMsg += "Fleet: <b>" + fleet.FleetName + "</b>  Series: <b>" + fleetSery.SeriesName + "</b>  Boats: ";
                            foreach (var boat in boatsInSeries)
                            {
                                resultsMsg += boat.SailNumber + "(BoatID=" + boat.BoatID + ")  ";
                            }
                            resultsMsg += "  FleetSeriesID=" + fleetSeryCopy.FleetSeriesID;
                            resultsMsg = resultsMsg.TrimEnd() + "<br />";

                            // Find the races for the series
                            var racesInSeriesCount = 0; // number of races to count that have been completed in the series
                            var raceSeries = from rs in ctx.SS_RaceSeries
                                             where rs.FleetSeriesID == fleetSeryCopy.FleetSeriesID
                                             orderby rs.RaceNumber
                                             select rs;
                            foreach (var raceSery in raceSeries)
                            {
                                var raceSeryCopy = raceSery;

                                if (checkDetailedOutput.Checked)
                                {
                                    resultsMsg += "&nbsp;&nbsp;Race #" + raceSery.RaceNumber + "(RaceSeriesID=" + raceSery.RaceSeriesID + "): ";
                                }
                                
                                // Determine if the race has been abandoned
                                SS_RaceFleet raceFleet = null;

                                try
                                {
                                    raceFleet = (from rf in ctx.SS_RaceFleets
                                                 where rf.RaceID == raceSeryCopy.RaceID
                                                    && rf.FleetID == fleetCopy.FleetID
                                                 select rf).Single();
                                }
                                // ReSharper disable once EmptyGeneralCatchClause
                                catch
                                {
                                    // Purposely do nothing - in the process of being scored.
                                }

                                if (raceFleet != null) // If races scored
                                {
                                    if (raceFleet.IsAbandoned == true) // abandoned
                                    {
                                        foreach (var boat in boatsInSeries)
                                        {
                                            var result = new SS_Result
                                            {
                                                SeasonID = seasonId,
                                                IsAbandoned = true,
                                                FleetSeriesID = fleetSery.FleetSeriesID,
                                                BoatID = boat.BoatID,
                                                SailNumber = boat.SailNumber,
                                                RaceNumber = raceSery.RaceNumber,
                                                Points = 0,
                                                FinishPlace = 0,
                                                CanThrowout = false,
                                                IsAbsent = false
                                            };
                                            ctx.SS_Results.InsertOnSubmit(result);
                                            ctx.SubmitChanges();
                                        }
                                    } // abandoned
                                    else // there are scores
                                    {
                                        // Get the scores from the race
                                        var scores = from s in ctx.SS_Scores
                                                     join b in ctx.SS_Boats on s.BoatID equals b.BoatID
                                                     join p in ctx.SS_Penalties on s.PenaltyID equals p.PenaltyID
                                                     join rs in ctx.SS_RaceSeries on s.RaceID equals rs.RaceID
                                                     where rs.RaceSeriesID == raceSeryCopy.RaceSeriesID
                                                        && b.FleetID == fleetCopy.FleetID
                                                     orderby b.IsRegistered descending, s.FinishPlace // REGISTERED : Get the registered boats first, then the unregistered...  Then sort by finish place
                                                     select new { s.RaceID, s.ScoreID, s.BoatID, s.PenaltyID, b.SailNumber, b.BoatName, b.Skipper, b.Crew, b.IsRegistered, s.FinishPlace, p.PenaltyName, p.IsNonPenalty, p.IsDisqualified, p.IsExcludable, p.IsLastPlusOne, p.IsLastPlusTwo, p.PenaltyRate, p.UsePlace, p.IsDoublePoints };
                                        if (scores.Any())
                                        {
                                            racesInSeriesCount++; // another race in the series found
                                        }
                                        var boatsScored = new List<BoatList>(); // list of boats scored (useful to find the boats not scored)

                                        var boatsWithGoodFinishes = scores.Where(p => p.UsePlace).Where(b => b.IsRegistered).Count(); // REGISTERED number of boats with proper scores and that are registered

                                        var adjustedFinishPlace = 0; // REGISTERED This contains the adjusted finish place (in case boats are tossed and thus reordered due to not being reigstered, etc.)
                                        var doublePoints = false; // set to true if at least one double points row encountered

                                        foreach (var score in scores)
                                        {
                                            adjustedFinishPlace++;

                                            if (!doublePoints && score.IsDoublePoints)
                                            {
                                                doublePoints = true;
                                            }

                                            boatsScored.Add(new BoatList(score.BoatID, score.SailNumber, score.BoatName, score.Skipper, score.Crew));

                                            var result = new SS_Result
                                            {
                                                SeasonID = seasonId,
                                                IsAbandoned = false,
                                                FleetSeriesID = fleetSery.FleetSeriesID,
                                                BoatID = score.BoatID,
                                                SailNumber = score.SailNumber,
                                                RaceNumber = raceSery.RaceNumber,
                                                PenaltyID = score.PenaltyID,
                                                PenaltyName = score.PenaltyName,
                                                FinishPlace = score.FinishPlace,
                                                IsAbsent = false,
                                                IsNonPenalty = score.IsNonPenalty,
                                                CanThrowout = score.IsExcludable && raceSeryCopy.IsScoreExcludable
                                            };

                                            if (score.IsLastPlusOne | score.IsLastPlusTwo | !score.IsRegistered)
                                            {
                                                //result.Points = CentralLibrary.ScorePoints(fleetSery.ScoreMethodID, boatsWithGoodFinishes + (score.IsLastPlusTwo | !score.IsRegistered ? 2 : 1)) * (score.IsDoublePoints ? 2 : 1); // TODO: need to have a setting to know if score.IsRegistered is really IsLastPlusTwo or IsLastPlusOne - hard coded for now
                                                result.Points = CentralLibrary.ScorePoints(fleetSery.ScoreMethodID, boatsWithGoodFinishes + (score.IsLastPlusTwo | !score.IsRegistered ? 2 : 1)) * raceSeryCopy.ScorePointsFactor; // TODO: need to have a setting to know if score.IsRegistered is really IsLastPlusTwo or IsLastPlusOne - hard coded for now
                                            }
                                            else
                                            {
                                                //result.Points = CentralLibrary.ScorePoints(fleetSery.ScoreMethodID, adjustedFinishPlace) * (score.IsDoublePoints ? 2 : 1); // REGISTERED boat
                                                result.Points = CentralLibrary.ScorePoints(fleetSery.ScoreMethodID, adjustedFinishPlace) * raceSeryCopy.ScorePointsFactor; // REGISTERED boat
                                                if (score.PenaltyRate > 0.0)
                                                {
                                                    result.Points += Math.Round(boatsWithGoodFinishes * score.PenaltyRate) * (score.IsDoublePoints ? 2 : 1); // e.g. Z-Flag (ZFP) penalty
                                                }
                                            }
                                            ctx.SS_Results.InsertOnSubmit(result);
                                            ctx.SubmitChanges();

                                            if (checkDetailedOutput.Checked)
                                            {
                                                resultsMsg += score.SailNumber + "(Place=" + score.FinishPlace + " Points=" + result.Points + (!score.IsNonPenalty ? " Penalty=" + score.PenaltyName.Trim() : "") + ") ";
                                            }
                                        }

                                        // Find the boats that have raced at least once in the series but weren't present for this race
                                        foreach (var boat in boatsInSeries)
                                        {
                                            if (FindScored(boatsScored, boat.BoatID)) continue;
                                            var result = new SS_Result
                                            {
                                                SeasonID = seasonId,
                                                IsAbandoned = false,
                                                FleetSeriesID = fleetSery.FleetSeriesID,
                                                BoatID = boat.BoatID,
                                                SailNumber = boat.SailNumber,
                                                RaceNumber = raceSery.RaceNumber,
                                                //CanThrowout = true,
                                                CanThrowout = raceSeryCopy.IsScoreExcludable,
                                                //Points = CentralLibrary.ScorePoints(fleetSery.ScoreMethodID, boatsWithGoodFinishes + 2) * (doublePoints ? 2 : 1),
                                                Points = CentralLibrary.ScorePoints(fleetSery.ScoreMethodID, boatsWithGoodFinishes + 2) * raceSeryCopy.ScorePointsFactor,
                                                FinishPlace = boatsWithGoodFinishes + 2,
                                                IsAbsent = true,
                                                IsNonPenalty = true
                                            };
                                            ctx.SS_Results.InsertOnSubmit(result);
                                            ctx.SubmitChanges();
                                        }
                                    } // there are scores
                                } // if races scored
                                if (checkDetailedOutput.Checked)
                                {
                                    resultsMsg = resultsMsg.TrimEnd() + "<br />";
                                }
                            } // work through races in a series

                            // Calculate the throwouts and series tiebreaker
                            var numberThrowouts = CentralLibrary.NumberThrowouts(fleetSery.ThrowoutID, racesInSeriesCount);
                            foreach (var boat in boatsInSeries) // give each boat their throwouts
                            {
                                // Tiebreakers are handled by a big string (to allow nice sorting via queries)
                                // Appendix A8.1:
                                //    If there is a series-score tie between two or more boats, each boat's race
                                //    scores shall be listed in order of the best to worst, and at the first
                                //    point(s) where there is a difference the tie shall be broken in favour
                                //    of the boat(s) with the best score(s).  No excluded scores shall be used.
                                // Appendix A8.2:
                                //   If a tie remains between two or more boats, they shall be ranked in order 
                                //   of their scores in the last race.  Any remaining ties shall be broken by 
                                //   using the tied boats' scores in the next-to-last race and so on until all
                                //   ties are broken.  These scores shall be used even if some of them are 
                                //   excluded scores.
                                //
                                // So, two strings are created.  One for A8.1 (which doesn't include excludable scores, and one for A8.2 (which includes all scores).
                                // Enough padding is used to convert the numeric valus of the scores to strings, which are concantenated in the proper order.
                                // For example, if the following race results occurs:
                                //                              A8.1 String       A8.2 String
                                //  Boat 1:    1 2 3 4          001002003004      004003002001
                                //  Boat 2:    2 1 4 3          001002003004      003004001002
                                //  Boat 3:    4 3 1 2          001002003004      002001003004
                                //  Boat 4:    3 4 2 1          001002003004      001002004003
                                //
                                // The two strings are concatenated together to make up the Tiebreaker string, and thus allow a secondary sort (after the totalpoints)
                                // to determine the correct place (for the series).  Using this example, the finish order would be: Boat 4, Boat 3, Boat 2, and Boat 1.
                                //
                                // Side note: Based on this menthod, the system is limited by 10^TIEBREAKER_PADDING-1 boats and SizeOf(SS_ResultSummary.TiebreakStr)/TIEBREAKER_PADDING 
                                //   races (presuming no throwouts).  

                                var boatCopy = boat;

                                // Find all of the results for that boat (in the given series), sorted to prioritize the throwouts, and handle A8.1 ties
                                var resultsThrow = from r in ctx.SS_Results
                                                   where r.BoatID == boatCopy.BoatID
                                                      && r.SeasonID == seasonId
                                                      && r.FleetSeriesID == fleetSeryCopy.FleetSeriesID
                                                   orderby r.Points descending, r.RaceNumber
                                                   select r;

                                var calcPoints = 0.0;
                                var throwoutsRemain = numberThrowouts;
                                foreach (var result in resultsThrow)
                                {
                                    if (result.CanThrowout && throwoutsRemain > 0)
                                    {
                                        result.IsThrowout = true;
                                        ctx.SubmitChanges();
                                        throwoutsRemain--;
                                    }
                                    else
                                    {
                                        calcPoints += result.Points;
                                    }
                                }

                                // Find all of the non-excluced scores (in the best finish order) for A81 (in the given series), to determine the tiebreaker's for A8.1
                                var resultsA81 = from r in ctx.SS_Results
                                                 where r.BoatID == boatCopy.BoatID
                                                    && r.SeasonID == seasonId
                                                    && r.FleetSeriesID == fleetSeryCopy.FleetSeriesID
                                                    && !r.IsThrowout
                                                 orderby r.Points ascending, r.RaceNumber
                                                 select r;
                                var tieA81 = "";
                                // ReSharper disable once LoopCanBeConvertedToQuery
                                foreach (var resultA81 in resultsA81)
                                {
                                    tieA81 += resultA81.Points.ToString(CultureInfo.InvariantCulture).Trim().PadLeft(TiebreakerPadding, '0');
                                }

                                // Find all of the results, in reverse order and including Excluded Races, for that boat (in the given series), to determine the tiebreaker's for A8.2
                                var resultsA82 = from r in ctx.SS_Results
                                                 where r.BoatID == boatCopy.BoatID
                                                    && r.SeasonID == seasonId
                                                    && r.FleetSeriesID == fleetSeryCopy.FleetSeriesID
                                                 orderby r.RaceNumber descending
                                                 select r;
                                var tieA82 = string.Empty;
                                // ReSharper disable once LoopCanBeConvertedToQuery
                                foreach (var resultA82 in resultsA82)
                                {
                                    tieA82 += resultA82.Points.ToString(CultureInfo.InvariantCulture).Trim().PadLeft(TiebreakerPadding, '0');
                                }

                                var resultSummary = new SS_ResultSummary()
                                {
                                    SeasonID = seasonId,
                                    FleetSeriesID = fleetSery.FleetSeriesID,
                                    BoatID = boat.BoatID,
                                    BoatName = boat.BoatName,
                                    SailNumber = boat.SailNumber,
                                    IsRegisteredBoat = boat.IsRegistered,
                                    SeriesName = fleetSery.SeriesName,
                                    FleetName = fleet.FleetName,
                                    Skipper = boat.Skipper,
                                    Crew = boat.Crew,
                                    TotalPoints = calcPoints,
                                    TieBreakerStr = tieA81 + tieA82,
                                    Created = generated,
                                    CreatedBy = CentralLibrary.User
                                };
                                ctx.SS_ResultSummaries.InsertOnSubmit(resultSummary);
                                ctx.SubmitChanges();
                            }
                        } // one or more boats scored in a series

                    } // work through Fleet Series
                } // work through Fleets
            }
            catch (Exception ex)
            {
                labelError.Text = "An error was encountered while generating results: " + ex;
            }

            // Finished
            if (labelError.Text.Length == 0)
            {
                resultsMsg += "<br />Results calculation for the " + listSeason.SelectedItem.Text + " Season has been completed.";
            }

            labelResults.Text = resultsMsg;
            linkDisplayResults.Visible = true;
        }
        #endregion
    }

    #region Class
    public class BoatList
    {
        public int BoatId { get; set; }
        public string SailNumber { get; set; }
        public string BoatName { get; set; }
        public string SkipperName { get; set; }
        public string CrewName { get; set; }

        public BoatList(int boatId, string sailNumber, string boatName, string skipperName, string crewName)
        {
            BoatId = boatId;
            SailNumber = sailNumber;
            BoatName = boatName;
            SkipperName = skipperName;
            CrewName = crewName;
        }
    }
    #endregion
}