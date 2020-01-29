using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SailTally.Classes;

namespace SailTally
{
    public partial class ScheduleModify : System.Web.UI.Page
    {
        #region Data Members
        private List<FleetSeriesRace> _fleetSeriesRace;
        #endregion

        #region Properties
        public int SelectedSeason
        {
            get { return Convert.ToInt32(hiddenSeasonID.Value); }
        }

        public int SelectedRace
        {
            get
            {
                int raceNo;
                try
                {
                    raceNo = Convert.ToInt32(hiddenRaceID.Value);
                }
                catch
                {
                    raceNo = -1;
                }
                return raceNo;
            }
        }

        public int SelectedFleet
        {
            get { return Convert.ToInt32(listFleet.SelectedValue); }
        }

        public int SelectedFleetSeries
        {
            get 
            {
                if (listFleetSeries.SelectedValue.Length == 0)
                {
                    return -1;
                }
                return Convert.ToInt32(listFleetSeries.SelectedValue); }
        }

        public DateTime SelectedWarning
        {
            get { return Convert.ToDateTime((calendarRace.SelectedDate.ToShortDateString() + " " + listRace.Text).Trim()); }
        }

        public bool IsNewRace
        {
            get
            {
                return (string.IsNullOrEmpty(hiddenRaceID.Value));
            }
        }

        public bool IsScoredRace
        {
            get
            {
                return (hiddenScored.Value.Length != 0);
            }
        }
        #endregion

        #region Methods
        protected void UpdateDate()
        {
            try
            {
                var parsedSchedule = calendarRace.SelectedDate.ToShortDateString() + " " + listRace.Text;
                var calculatedSchedule = Convert.ToDateTime(parsedSchedule.Trim());
                labelSchedule.Text = calculatedSchedule.ToShortDateString() + " " + CentralLibrary.FormatTime(calculatedSchedule);
            }
            catch
            {
                labelSchedule.Text = "Invalid";
            }
        }

        protected void UpdateDate(DateTime date)
        {
            calendarRace.SelectedDate = date.Date;
            calendarRace.VisibleDate = date.Date;
            UpdateDate();
        }

        protected void UpdateDate(DateTime date, string time)
        {
            listRace.Text = time;
            UpdateDate(date);
        }

        protected void UpdateDate(string dateTime)
        {
            UpdateDate(Convert.ToDateTime(dateTime.Trim()));
        }

        protected void FindNextRaceNumber()
        {
            textRaceNo.Text = CentralLibrary.GetNextRaceNumber(SelectedSeason, SelectedFleetSeries).ToString();
        }

        protected bool IsFleetSeriesSelected(int fleetId, int fleetSeriesId)
        {
            return _fleetSeriesRace.Any(fsr => fsr.FleetId == fleetId && fsr.FleetSeriesId == fleetSeriesId);
        }

        protected void SetError()
        {
            SetError(string.Empty);
        }

        protected void SetError(string error)
        {
            labelError.Text = error;
        }

        protected string PreflightSaveCheck()
        {
            var ctx = new SailTallyDataContext();

            // NOTE: The SelectedRace is -1 if a new entry or the actual race ID if an edit.  As a result
            //       these following queries should work in both scenarios with the != comparison.

            // Check to make sure there are no other races scheduled at the same time - currently only
            // want one race per time slot.
            var races = (from r in ctx.SS_Races
                        where r.RaceID != SelectedRace && r.SeasonID == SelectedSeason && r.FirstWarningDate == SelectedWarning
                        select r);

            if (races.Any())
            {
                return "An existing race is already present in this time slot.  Please modify that one (Race ID: " + races.First().RaceID + ") to adjust the fleet/series assigned.";
            }

            for (var index = 0; index < _fleetSeriesRace.Count; index++)
            {
                var fsr = _fleetSeriesRace[index];

                // Check to make sure there are no potential duplicate FleetSeries for a given season
                var raceSerys = from rs in ctx.SS_RaceSeries
                                where rs.RaceID != SelectedRace && rs.SeasonID == SelectedSeason && rs.FleetSeriesID == fsr.FleetSeriesId && rs.RaceNumber == fsr.RaceNo
                                select rs;

                if (!raceSerys.Any()) continue;

                var raceSery = raceSerys.First();
                var race = (from r in ctx.SS_Races
                    where r.RaceID == raceSery.RaceID
                    select r).First();

                string raceDate;
                if (race != null)
                {
                    raceDate = "Check the schedule date " + race.FirstWarningDate;
                }
                else
                {
                    raceDate = "Unable to find date";
                }

                return "Existing Fleet/Series/Race# '" + listSelectedFleetSeries.Items[index].Text + "' already present.  " + raceDate + " (RaceSeries ID #" + raceSery.RaceSeriesID + ")";
            }

            return string.Empty;
        }

        protected void DeleteRace() // Deletes the currently selected race
        {
            // Presumes the caller has done the pre-flight check that it is okay to remove the races
            var ctx = new SailTallyDataContext();

            var raceSerys = from rs in ctx.SS_RaceSeries
                            where rs.RaceID == SelectedRace
                            select rs;

            foreach (var raceSeries in raceSerys)
            {
                ctx.SS_RaceSeries.DeleteOnSubmit(raceSeries);
            }
            ctx.SubmitChanges();

            // This code shouldn't be needed since deletion is no longer allowed if scored AND there is a RaceFleet record (the header to the scores)
            /*
            var raceFleets = from rf in ctx.SS_RaceFleets
                                where rf.RaceID == SelectedRace
                                select rf;

            foreach (var raceFleet in raceFleets)
            {
                ctx.SS_RaceFleets.DeleteOnSubmit(raceFleet);
            }
            ctx.SubmitChanges();
            */

            var races = from r in ctx.SS_Races
                        where r.RaceID == SelectedRace
                        select r;
            foreach (var race in races)
            {
                ctx.SS_Races.DeleteOnSubmit(race);
            }
            ctx.SubmitChanges();
        }

        protected void SaveRace()
        {
            // Note: This presumes a new Race ID is created (so if editing, the previous race should have been removed first)

            var ctx=new SailTallyDataContext();
            var race = new SS_Race() { SeasonID = SelectedSeason, FirstWarningDate = SelectedWarning };
            ctx.SS_Races.InsertOnSubmit(race);
            ctx.SubmitChanges();

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var fsr in _fleetSeriesRace)
            {
                var raceSeries = new SS_RaceSery
                {
                    SeasonID = SelectedSeason,
                    RaceID = race.RaceID,
                    FleetSeriesID = fsr.FleetSeriesId,
                    RaceNumber = fsr.RaceNo
                };
                // use the newly created primary key
                ctx.SS_RaceSeries.InsertOnSubmit(raceSeries);
            }
            ctx.SubmitChanges();
        }

        protected void UpdateRace()
        {
            // Note: This is available to allow minor edits to races previously scored

            var ctx=new SailTallyDataContext();
            var raceId = SelectedRace;

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var fsr in _fleetSeriesRace)
            {
                if (!fsr.Added) continue;

                var raceSeries = new SS_RaceSery
                {
                    SeasonID = SelectedSeason,
                    RaceID = raceId,
                    FleetSeriesID = fsr.FleetSeriesId,
                    RaceNumber = fsr.RaceNo
                };
                ctx.SS_RaceSeries.InsertOnSubmit(raceSeries);
            }

            ctx.SubmitChanges();
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                _fleetSeriesRace = (List<FleetSeriesRace>) ViewState["_FleetSeriesRace"]; // Load state
            }
            else
            {
                _fleetSeriesRace = new List<FleetSeriesRace>(); // allocate memory

                // Set the values via the IsCrossPagePostBack because the PreviousPage values won't be available after a PostBack here.
                hiddenSeasonID.Value = PreviousPage.SelectedSeason;

                hiddenRaceID.Value = Request[Constant.GetVar.RaceId];              

                CentralLibrary.GetTimes(listRace);
                CentralLibrary.GetFleets(listFleet);
                CentralLibrary.GetFleetSeries(listFleetSeries, Convert.ToInt32(listFleet.SelectedValue), SelectedSeason);
                FindNextRaceNumber();

                labelScored.Visible = false;

                if (IsNewRace) // Creating a new race entry
                {
                    linkDeleteRace.Visible = false;

                    if (PreviousPage.SelectedDate.Trim().Length > 0)
                    {
                        UpdateDate(PreviousPage.SelectedDate);
                    }
                    else
                    {
                        UpdateDate(CentralLibrary.GetCurrentDateTime());
                    }
                }
                else // Editing an existing race entry
                {
                    var ctx = new SailTallyDataContext();

                    // Get the data
                    var race = (from r in ctx.SS_Races
                                where r.RaceID == SelectedRace
                                select r).Single();
                    var raceSerys = from rs in ctx.SS_RaceSeries
                                    join fs in ctx.SS_FleetSeries on rs.FleetSeriesID equals fs.FleetSeriesID
                                    join f in ctx.SS_Fleets on fs.FleetID equals f.FleetID
                                    join s in ctx.SS_Series on fs.SeriesID equals s.SeriesID
                                    where rs.RaceID == SelectedRace
                                    select new { rs.RaceSeriesID, rs.RaceNumber, fs.FleetSeriesID, f.FleetID, f.FleetName, s.SeriesID, s.SeriesName };

                    // Put the data to the controls
                    UpdateDate(race.FirstWarningDate, CentralLibrary.FormatTime(race.FirstWarningDate));

                    foreach (var raceSeries in raceSerys)
                    {
                        var fsr = new FleetSeriesRace()
                                      {
                                          FleetId = raceSeries.FleetID, 
                                          FleetSeriesId = raceSeries.FleetSeriesID, 
                                          RaceNo = raceSeries.RaceNumber, 
                                          RaceSeriesId = raceSeries.RaceSeriesID, 
                                          FleetName =  raceSeries.FleetName, 
                                          SeriesName = raceSeries.SeriesName
                                      };
                        _fleetSeriesRace.Add(fsr);

                        var item = new ListItem();
                        var fleetSeries = fsr.Display(); //raceSeries.FleetName.Trim() + " / " + raceSeries.SeriesName.Trim() + " / " + raceSeries.RaceNumber.ToString();
                        item.Text = fleetSeries;
                        item.Value = fleetSeries;
                        listSelectedFleetSeries.Items.Add(item);
                    }

                    // Determine if there are any races scored (if so, disable editing since don't want to create a new RaceID)
                    var scores = from s in ctx.SS_Scores
                                 where s.RaceID == SelectedRace
                                 select s;

                    var raceFleets = from rf in ctx.SS_RaceFleets
                                     where rf.RaceID == SelectedRace
                                     select rf;

                    if (!scores.Any() && !raceFleets.Any()) return;

                    labelScored.Visible = true;
                    hiddenScored.Value = "1";

                    calendarRace.Enabled = false;
                    listRace.Enabled = false;
                    listSelectedFleetSeries.Enabled = false;
                    buttonRemoveFleetSeries.Enabled = false;
                    linkDeleteRace.Enabled = false;

                    // Note: Now allow race series insertion (Fleet/Series/RaceNo can be added even if previous scored)
                    //this.listFleet.Enabled = false;
                    //this.listFleetSeries.Enabled = false;
                    //this.textRaceNo.Enabled = false;
                    //this.buttonAddFleetSeries.Enabled = false;
                    //this.linkSave.Enabled = false;
                }
            }
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            ViewState.Add("_FleetSeriesRace", _fleetSeriesRace); // Save State
        }

        protected void linkSave_Click(object sender, EventArgs e)
        {
            SetError();

            if (labelSchedule.Text.Length == 0)
            {
                SetError("Unable to Save.  Invalid Date or Time.");
            }
            else if (_fleetSeriesRace.Count == 0)
            {
                SetError("Unable to Save.  Add Fleet/Series/Race# first.");
            }
            else
            {
                var error = PreflightSaveCheck();
                if (error.Length == 0)
                {
                    if (IsScoredRace)
                    {
                        UpdateRace();
                    }
                    else
                    {
                        if (!IsNewRace) { DeleteRace(); }
                        SaveRace();
                    }

                    Server.Transfer("~/Schedule.aspx");
                }
                else
                {
                    SetError(error);
                }
            }
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            // Redirection handled by PostBackUrl property
        }

        protected void linkDeleteRace_Click(object sender, EventArgs e)
        {
            DeleteRace(); // since event only accessible for non-new race, okay to call directly
        }

        protected void calendarRace_SelectionChanged(object sender, EventArgs e)
        {
            UpdateDate();
        }

        protected void listRace_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDate();
        }

        protected void listFleet_SelectedIndexChanged(object sender, EventArgs e)
        {
            CentralLibrary.GetFleetSeries(listFleetSeries, SelectedFleet, SelectedSeason);
            FindNextRaceNumber();
        }

        protected void buttonAddFleetSeries_Click(object sender, EventArgs e)
        {
            SetError();
            if (listFleet.SelectedItem == null) SetError("Fleet not Selected.");
            else if (listFleetSeries.SelectedItem == null) SetError("Series not Selected.");
            else if (textRaceNo.Text.Length < 1 || Convert.ToInt32(textRaceNo.Text) < 1) SetError("Race # not Specified.");
            else if (textPointsFactor.Text.Length < 1 || Convert.ToInt32(textPointsFactor.Text) < 1) SetError("Points Factor not specified");
            else if (IsFleetSeriesSelected(SelectedFleet, SelectedFleetSeries)) SetError("Cannot select a duplicate Series per Fleet for a given Race.");
            else
            {
                //string fleetSeries = this.listFleet.SelectedItem.Text + " / " + this.listFleetSeries.SelectedItem.Text + " / " + this.textRaceNo.Text;
                var fsr = new FleetSeriesRace()
                              {
                                  FleetId = Convert.ToInt32(listFleet.SelectedValue), 
                                  FleetSeriesId = Convert.ToInt32(listFleetSeries.SelectedValue), 
                                  RaceNo = Convert.ToInt32(textRaceNo.Text),
                                  PointsFactor = Convert.ToInt32(textPointsFactor.Text),
                                  Excludable = checkExcludable.Checked,
                                  FleetName = listFleet.SelectedItem.Text,
                                  SeriesName = listFleetSeries.SelectedItem.Text,
                                  Added = true
                              };
                var fleetSeries = fsr.Display();
                _fleetSeriesRace.Add(fsr);

                var item = new ListItem
                {
                    Text = fleetSeries,
                    Value = fleetSeries
                };
                listSelectedFleetSeries.Items.Add(item);
                textRaceNo.Text = string.Empty;
            }
        }

        protected void buttonRemoveFleetSeries_Click(object sender, EventArgs e)
        {
            SetError();
            if (listSelectedFleetSeries.SelectedItem != null)
            {
                var position = listSelectedFleetSeries.SelectedIndex;
                try
                {
                    // Save the removed information in the add area in case the user wishes to add back in with minor changes
                    listFleet.SelectedValue = _fleetSeriesRace[position].FleetId.ToString();
                    CentralLibrary.GetFleetSeries(listFleetSeries, SelectedFleet, SelectedSeason);
                    listFleetSeries.SelectedValue = _fleetSeriesRace[position].FleetSeriesId.ToString();
                    textRaceNo.Text = _fleetSeriesRace[position].RaceNo.ToString();
                    textPointsFactor.Text = _fleetSeriesRace[position].PointsFactor.ToString();
                    checkExcludable.Checked = _fleetSeriesRace[position].Excludable;
                }
                catch (Exception ex)
                {
                    SetError("Exception: " + ex);
                }

                _fleetSeriesRace.RemoveAt(listSelectedFleetSeries.SelectedIndex);
                listSelectedFleetSeries.Items.RemoveAt(listSelectedFleetSeries.SelectedIndex);
            }
            else
            {
                SetError("Select an existing Fleet/Series for removal first.");
            }

        }

        protected void listFleetSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindNextRaceNumber();
        }

        protected void buttonFleetSeriesRefresh_Click(object sender, EventArgs e)
        {
            CentralLibrary.GetFleetSeries(listFleetSeries, SelectedFleet, SelectedSeason);
            FindNextRaceNumber();
        }
        #endregion
    }
}