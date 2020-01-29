using System.Linq;

namespace SailTally.Classes
{
    public class AssignRace
    {
        #region Fields
        private readonly int _maxRaces;
        private readonly int _seasonId;
        private readonly int _fleetSeriesId;
        private readonly int? _raceNumber; // the (original) race number to search for
        private readonly int? _place;
        private readonly bool _findNextAvailable;
        private readonly int _fleetId;
        private readonly int _seriesId;
        private readonly Races _racesAlreadyAssignedTrophy;
        #endregion

        #region Properties
        public int BoatId { get; set; }
        public string BoatName { get; set; }
        public string SailNumber { get; set; }
        public int? RaceNumberAssigned { get; set; }
        public string Skipper { get; set; }   
        #endregion

        #region Constructors
        public AssignRace(int seasonId, int fleetSeriesId, int? raceNumber, int? place, bool findNextAvailable, Races racesAlreadyAssigned)
        {
            _seasonId = seasonId;
            _fleetSeriesId = fleetSeriesId;
            _raceNumber = raceNumber;
            _place = place;
            _findNextAvailable = findNextAvailable;
            _racesAlreadyAssignedTrophy = racesAlreadyAssigned;

            // Find the maximum races
            var ctx = new SailTallyDataContext();

            var raceNumbers = from r in ctx.SS_Results
                              where r.SeasonID == _seasonId
                                    && r.FleetSeriesID == _fleetSeriesId
                                    && r.FinishPlace == _place
                              select r.RaceNumber;

            _maxRaces = (raceNumbers.Any() ? raceNumbers.Max() : 0);

            // find the fleet ID and series ID for the given FleetSeriesID (since that is what trophies use)
            var fleetSery = (from fs in ctx.SS_FleetSeries
                             where fs.FleetSeriesID == _fleetSeriesId
                                && fs.SeasonID == _seasonId
                             select new { fs.FleetID, fs.SeriesID }).Single();
            _fleetId = fleetSery.FleetID;
            _seriesId = fleetSery.SeriesID;
        }
        #endregion

        #region Methods
        public bool FindRace()
        {
            return FindRace(_raceNumber, _raceNumber); // uses recursion to find the next race to assign, so starting with the desired race
        }

        private bool FindRace(int? raceNumberSearch, int? raceNumberOriginal)
        {
            if (raceNumberSearch == null) { return false; }

            var found = false;
            var racePotentialForTrophy = new Race(_fleetId, _seriesId, _seasonId, raceNumberSearch, _place);

            if (_maxRaces > 0) // If no races, then can't find a trophy
            {
                var ctx = new SailTallyDataContext();

                // Attempt to find the specific race winner
                var results = (from r in ctx.SS_Results
                               join b in ctx.SS_Boats on r.BoatID equals b.BoatID
                               where r.SeasonID == _seasonId
                                     && r.FleetSeriesID == _fleetSeriesId
                                     && r.RaceNumber == raceNumberSearch
                                     && r.FinishPlace == _place
                                     && b.IsRegistered
                               select new { r.BoatID, b.BoatName, b.SailNumber, b.Skipper, r.IsAbandoned });

                // if an appropriate race is not found or the race was abandoned or the race alread has an ssigned trophy, see about looking for another
                if (!results.Any() || results.Single().IsAbandoned || _racesAlreadyAssignedTrophy.Contains(racePotentialForTrophy)) 
                {
                    if (_findNextAvailable) // if allowed to find the next available, do so...
                    {
                        var raceNumberNext = raceNumberSearch + 1;
                        var searchNextRace = true;

                        while (searchNextRace)
                        {
                            if (raceNumberNext > _maxRaces) // not out of bounds?
                            {
                                raceNumberNext = 1; // wrap around and start over
                            }

                            if (raceNumberNext != raceNumberOriginal) // make sure we are not back were we started? (if so, then stop searching)
                            {
                                // Find out if there is a trophy already assigned to this race (one or more)
                                var next = raceNumberNext;
                                var trophies = (from t in ctx.SS_Trophies
                                                where t.SeasonID == _seasonId
                                                      && t.FleetID == _fleetId
                                                      && t.SeriesID == _seriesId
                                                      && t.RaceNumber == next
                                                select t);

                                if (!trophies.Any()) // make sure no trophy assigned (if there is, go onto the next race number
                                {
                                    searchNextRace = false; // stop searching...  let recursion to the remaining searching
                                    found = FindRace(raceNumberNext, raceNumberOriginal); // find the next race recursively and record
                                }
                                else
                                {
                                    raceNumberNext++; // find one that doesn't already have a trophy assigned
                                }
                            }
                            else
                            {
                                searchNextRace = false; // stop searching, not found
                            }
                        }
                    }
                }
                else
                {
                    // Found and record the results
                    found = true;
                    BoatId = results.Single().BoatID;
                    BoatName = results.Single().BoatName;
                    SailNumber = results.Single().SailNumber;
                    Skipper = results.Single().Skipper;
                    RaceNumberAssigned = raceNumberSearch; // what was actually found (as opposed to this.RaceNumber, which was original desired)
                    _racesAlreadyAssignedTrophy.Add(racePotentialForTrophy); // to keep from getting again
                }
            }
            return found;
        }
        #endregion
    }
}