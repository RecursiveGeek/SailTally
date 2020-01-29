using System;

namespace SailTally
{
    [Serializable]
    public class FleetSeriesRace
    {
        #region Properties
        public int FleetId { get; set; }
        public int FleetSeriesId { get; set; }
        public int RaceNo { get; set; }
        public int PointsFactor { get; set; }
        public bool Excludable { get; set; }
        //public bool Action { get; set; }

        public int RaceSeriesId { get; set; }
        public string FleetName { get; set; }
        public string SeriesName { get; set; }
        public bool Edited { get; set; }
        public bool Added { get; set; }
        #endregion

        #region Constructors
        public FleetSeriesRace()
        {
            FleetId = -1;
            FleetSeriesId = -1;
            RaceSeriesId = -1;
            FleetName = "";
            SeriesName = "";
            PointsFactor = 1;
            Excludable = true;
            Edited = false;
            Added = false;
        }
        #endregion

        #region Methods
        public bool Modified()
        {
            return (Edited || Added);
        }

        public string Display()
        {
            return FleetName.Trim() + " | " + SeriesName + " | " + RaceNo + " | " + PointsFactor + " | " + (Excludable ? "Y" : "N") + " (" + RaceSeriesId + ")"; 
        }
        #endregion
    }
}