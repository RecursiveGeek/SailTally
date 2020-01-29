using System;

namespace SailTally.Classes
{
    public class Appointment
    {
        #region Properties
        public DateTime? FirstWarningDateOnly { get; set; }
        public DateTime FirstWarningDate { get; set; }
        public string FleetName { get; set; }
        public int RaceId { get; set; }
        public int RaceSeriesId { get; set; }
        public string SeriesName { get; set; }
        public int RaceNumber { get; set; }
        public bool IsBackToBack { get; set; }
        public int ScheduleOrder { get; set; }
        #endregion

        #region Constructors

        public Appointment(DateTime? firstWarningDateOnly, DateTime firstWarningDate, string fleetName, int raceId, int raceSeriesId, string seriesName, int raceNumber, bool isBackToBack, int scheduleOrder)
        {
            FirstWarningDateOnly = firstWarningDateOnly;
            FirstWarningDate = firstWarningDate;
            FleetName = fleetName;
            RaceId = raceId;
            RaceSeriesId = raceSeriesId;
            SeriesName = seriesName;
            RaceNumber = raceNumber;
            IsBackToBack = isBackToBack;
            ScheduleOrder = scheduleOrder;
        }
        #endregion
    }
}