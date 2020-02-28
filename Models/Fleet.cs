namespace SailTally.Classes
{
    public class Fleet
    {
        #region Properties
        public string FleetName { get; set; }
        public int ScheduleOrder { get; set; }
        #endregion

        #region Constructors

        public Fleet(string fleetName)
        {
            FleetName = fleetName;
            ScheduleOrder = 0;
        }

        public Fleet(string fleetName, int scheduleOrder)
        {
            FleetName = fleetName;
            ScheduleOrder = scheduleOrder;
        }
        #endregion
    }
}