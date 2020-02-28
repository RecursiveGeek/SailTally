using System;
using System.Linq;

namespace SailTally.Classes
{
    public class AssignTrophy : Race
    {
        #region Fields
        private bool _trophySearched;
        #endregion

        #region Properties
        public bool FindNextIfUnavail { get; set; }

        private int? RaceNumberAssign { get; set; }
        //private int? BoatId { get; set; }
        private string BoatName { get; set; }
        private string SailNumber { get; set; }
        private string Skipper { get; set; }
        private string Error { get; set; }
        #endregion

        public AssignTrophy(int fleetId, int seriesId, int seasonId, int? raceNumber, int? place, bool findNextIfUnavail) : base(fleetId, seriesId, seasonId, raceNumber, place)
        {          
            FindNextIfUnavail = findNextIfUnavail;
        }

        private void CheckIfSearched()
        {
            if (!_trophySearched)
            {
                throw new Exception("Trophy Data Accessed without First being Searched");
            }
        }

        public void FindTrophy(Races racesWithTrophy)
        {
            /*
            NOTES:
            If a series, then go right into SS_ResultSummary
            If a race #, then go into SS_Result
            If a race # is specified, must be a place number specified.
            A place number should always be specified (with the current implementation)
            Both SS_ResultSummary and SS_Result rely on FleetSeriesID.  So need to find that right off.
            */

            // Initialize (in case of multiple calls)
            Error = string.Empty; // no error encountered...  yet.
            RaceNumberAssign = null;
            //this.BoatId = null;
            BoatName = string.Empty;
            SailNumber = string.Empty;
            Skipper = string.Empty;

            var ctx = new SailTallyDataContext();

            // Get the FleetSeriesID
            var fleetSeriesId = (from fs in ctx.SS_FleetSeries
                                 where fs.SeasonID == SeasonId
                                    && fs.FleetID == FleetId
                                    && fs.SeriesID == SeriesId
                                 select new { fs.FleetSeriesID }).Single().FleetSeriesID;

            // Determine if a series result or specific race
            if (RaceNumber == null || RaceNumber < 0)
            {
                // series result
                try
                {
                    var resultSummaries = (from rs in ctx.SS_ResultSummaries
                                           join b in ctx.SS_Boats on rs.BoatID equals b.BoatID
                                           where rs.SeasonID == SeasonId
                                              && rs.FleetSeriesID == fleetSeriesId
                                              && rs.Position == Place
                                              && b.IsRegistered
                                           select new { rs.BoatID, rs.BoatName, rs.SailNumber, rs.Skipper });

                    if (resultSummaries.Any())
                    {
                        //this.BoatId = resultSummaries.Single().BoatID;
                        BoatName = resultSummaries.Single().BoatName;
                        SailNumber = resultSummaries.Single().SailNumber;
                        Skipper = resultSummaries.Single().Skipper;
                    }
                    else
                    {
                        Error = "No Results Found";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Looking up: SeasonID=" + SeasonId + "  fleetSeriesID=" + fleetSeriesId + "  place=" + Place + "  Error: " + ex);
                }

            }
            else
            {
                // race result
                var assignRace = new AssignRace(SeasonId, fleetSeriesId, RaceNumber, Place, FindNextIfUnavail, racesWithTrophy);
                var raceFound = assignRace.FindRace(); // TODO: Change
                if (raceFound)
                {
                    //this.BoatId = raceAssign.BoatID;
                    BoatName = assignRace.BoatName;
                    SailNumber = assignRace.SailNumber;
                    RaceNumberAssign = assignRace.RaceNumberAssigned;
                    Skipper = assignRace.Skipper;
                }
                else
                {
                    Error = "No Trophy Assigned";
                }
            }

            _trophySearched = true;
        }

        public string BoatWinner()
        {
            CheckIfSearched();
            if (Error.Length > 0)
            {
                return Error;
            }
            return "#" + SailNumber + " " + BoatName + ", " + Skipper;
        }

        public string RaceNumberStr()
        {
            CheckIfSearched();
            return (RaceNumber == null ? string.Empty : RaceNumber.ToString());
        }

        public string RaceNumberAssigned()
        {
            CheckIfSearched();
            return (RaceNumberAssign == null ? string.Empty : RaceNumberAssign.ToString());
        }
    }
}