using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class ScheduleSeries : Page
    {
        #region Data Members
        List<FleetSeriesDateRace> _fleetSeriesDateRace;
        #endregion

        #region Properties
        public int SelectedSeason
        {
            get { return Convert.ToInt32(hiddenSeasonID.Value); }
        }

        public int SelectedFleet
        {
            get { return Convert.ToInt32(listFleet.SelectedValue); }
        }

        public int SelectedFleetSeries
        {
            get
            {
                if (listFleetSeries.SelectedItem == null)
                {
                    return -1;
                }
                return Convert.ToInt32(listFleetSeries.SelectedValue); 
            }
        }

        public int SelectedNextRaceNumber
        {
            get { return Convert.ToInt32(hiddenNextRaceNumber.Value); }
        }

        #endregion

        #region Methods
        private void AddRaceDate(DateTime raceWhen, int raceNumber)
        {
            var insertPos = 0;
            var foundPos = false;
            var fsdr = new FleetSeriesDateRace(raceWhen, raceNumber);
            var item = new ListItem {Text = fsdr.Print()};

            while (!foundPos && insertPos < _fleetSeriesDateRace.Count)
            {
                if (_fleetSeriesDateRace[insertPos].When > fsdr.When)
                {
                    foundPos = true;
                }
                else
                {
                    insertPos++;
                }
            }

            if (foundPos)
            {
                // Put into position
                listSelectedRace.Items.Insert(insertPos, item);
                _fleetSeriesDateRace.Insert(insertPos, fsdr);
            }
            else
            {
                // Add to the end
                listSelectedRace.Items.Add(item);
                _fleetSeriesDateRace.Add(fsdr);
            }
        }

        private void RecalcRaceNumbers()
        {
            var datesCount = _fleetSeriesDateRace.Count;
            var fsdr=new FleetSeriesDateRace[datesCount];
            _fleetSeriesDateRace.CopyTo(fsdr); // make a backup copy

            listSelectedRace.Items.Clear();
            _fleetSeriesDateRace.Clear();

            for (var index = 0; index < datesCount; index++)
            {
                AddRaceDate(fsdr[index].When, SelectedNextRaceNumber + index);
            }
        }

        protected void UpdateDate(DateTime date)
        {
            calendarRace.SelectedDate = date.Date;
            calendarRace.VisibleDate = date.Date;
        }

        protected void UpdateDate(string dateTime)
        {
            UpdateDate(Convert.ToDateTime(dateTime.Trim()));
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                _fleetSeriesDateRace = (List<FleetSeriesDateRace>) ViewState["_FleetSeriesDateRace"]; // Load state
            }
            else
            {
                _fleetSeriesDateRace = new List<FleetSeriesDateRace>(); // allocate memory

                // Set the values via the IsCrossPagePostBack because the PreviousPage values won't be available after a PostBack here.
                hiddenSeasonID.Value = PreviousPage.SelectedSeason;
                labelSeason.Text = CentralLibrary.GetSeason(SelectedSeason);

                if (PreviousPage.SelectedDate.Trim().Length > 0)
                {
                    UpdateDate(PreviousPage.SelectedDate);
                }
                else
                {
                    UpdateDate(CentralLibrary.GetCurrentDateTime());
                }

                CentralLibrary.GetFleets(listFleet); // Get the fleets
                CentralLibrary.GetFleetSeries(listFleetSeries, SelectedFleet, SelectedSeason); // After getting the fleets, get the series for the selected fleet

                CentralLibrary.GetTimes(listRace);

                panelSchedule.Visible = false;
                labelErrorSeries.Visible = false;
                labelErrorSave.Visible = false;
                buttonRemove.Enabled = false;
            }
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            ViewState.Add("_FleetSeriesDateRace", _fleetSeriesDateRace); // Save State
        }

        protected void listFleet_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.GetFleetSeries(listFleetSeries, SelectedFleet, SelectedSeason);
        }

        protected void buttonSelect_Click(object sender, EventArgs e)
        {
            if (listFleetSeries.SelectedItem != null)
            {
                listFleet.Enabled = false;
                listFleetSeries.Enabled = false;
                buttonSelect.Enabled = false;
                buttonCancel.Visible = false;

                panelSchedule.Visible = true;
                labelErrorSeries.Visible = false;

                // Store the next race number for use going forward
                hiddenNextRaceNumber.Value = CentralLibrary.GetNextRaceNumber(SelectedSeason, SelectedFleetSeries).ToString();
            }
            else
            {
                labelErrorSeries.Visible = true;
            }
        }

        protected void buttonAdd_Click(object sender, EventArgs e)
        {
            // Make sure to add to find the correct position (so raceNumber can be 0 when added) and then RecalcRaceNumbers

            var when = Convert.ToDateTime(calendarRace.SelectedDate.ToShortDateString()+" "+listRace.SelectedValue);

            int repeatWeeks;
            try
            {
                repeatWeeks = Convert.ToInt32(textRepeatWeeks.Text);
            }
            catch
            {
                repeatWeeks = 1;
            }

            while (repeatWeeks > 0)
            {
                AddRaceDate(when, 0); // raceNumber = 0 for now since finding slot and will recalc later
                if (checkBackToBack.Checked)
                {
                    AddRaceDate(Convert.ToDateTime(when.ToShortDateString() + " " + CentralLibrary.GetNextTime(listRace.SelectedValue)), 0); // raceNumber = 0 for now since finding slot and will recalc later
                }

                when = when.AddDays(7); // next week
                repeatWeeks--;
            }

            RecalcRaceNumbers();
        }

        protected void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listSelectedRace.SelectedItem == null) return;
            var removePos = listSelectedRace.SelectedIndex;

            listSelectedRace.Items.RemoveAt(removePos);
            _fleetSeriesDateRace.RemoveAt(removePos);

            RecalcRaceNumbers();
                
            buttonRemove.Enabled = false;
        }

        protected void linkSave_Click(object sender, EventArgs e)
        {
            labelErrorSave.Visible = false;

            /*
            if (this._FleetSeriesDateRace.Count > 0)
            {
                SailTallyDataContext ctx = new SailTallyDataContext();
                int seasonID = this.SelectedSeason;
                int fleetID = this.SelectedFleet;
                int fleetSeriesID = this.SelectedFleetSeries;
                int raceID;

                for (int index = 0; index < this._FleetSeriesDateRace.Count; index++)
                {
                    var races = from r in ctx.SS_Races
                                where r.FirstWarningDate == this._FleetSeriesDateRace[index].When
                                   && r.SeasonID == seasonID
                                select r;

                    if (races.Count() > 0)
                    {
                        raceID = races.First().RaceID; // existing race -- will add to it
                    }
                    else
                    {
                        // New race
                        SS_Race race = new SS_Race() { SeasonID = seasonID, FirstWarningDate = this._FleetSeriesDateRace[index].When };
                        ctx.SS_Races.InsertOnSubmit(race);
                        ctx.SubmitChanges();
                        raceID = race.RaceID;
                    }

                    SS_RaceSery raceSeries = new SS_RaceSery() { SeasonID = seasonID, RaceID = raceID, RaceNumber = this._FleetSeriesDateRace[index].RaceNumber, FleetSeriesID = fleetSeriesID };
                    ctx.SS_RaceSeries.InsertOnSubmit(raceSeries);
                }

                Server.Transfer("~/Schedule.aspx");
            }
            else
            {
                this.labelErrorSave.Visible = true;
            }
            */
        }

        protected void listSelectedRace_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = (listSelectedRace.SelectedItem != null && listSelectedRace.Items.Count > 0);
        }
        #endregion
    }

    [Serializable]
    public class FleetSeriesDateRace
    {
        public DateTime When { get; set; }
        public int RaceNumber { get; set; }

        public FleetSeriesDateRace(DateTime when, int raceNumber)
        {
            When = when;
            RaceNumber = raceNumber;
        }

        public string Print()
        {
            return When.ToShortDateString() + " / " + When.ToShortTimeString() + " / " + RaceNumber.ToString();
        }
    }
}