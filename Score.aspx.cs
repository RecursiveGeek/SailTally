using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Score : System.Web.UI.Page
    {
        #region Data Members
        private List<BoatScore> _boatScores;
        #endregion

        #region Methods
        protected void ShowSummary(bool summaryVisible)
        {
            listSeason.Enabled = !summaryVisible;
            radioScheduledAll.Enabled = !summaryVisible;
            radioScheduledScored.Enabled = !summaryVisible;
            radioScheduledUnscored.Enabled = !summaryVisible;
            buttonSelect.Visible = !summaryVisible;
            buttonReset.Visible = summaryVisible;

            tableRaces.Visible = summaryVisible;
            panelScore.Visible = false;
        }

        protected void ShowDetail(bool showDetail)
        {
            tableRaces.Visible = !showDetail;
            panelScore.Visible = showDetail;
            ShowHeader(true);

            listFleet.Enabled = true;
            buttonScoreFleet.Enabled = true;
        }

        protected void ShowHeader(bool showHeader)
        {
            panelHeader.Visible = showHeader;
            panelResults.Visible = false;
        }

        protected TableCell AddColumn(string text, int raceId, bool header)
        {
            var cell = new TableCell();

            if (raceId >= 0)
            {
                var link = new LinkButton {Text = text};
                if (header)
                {
                   link.Font.Bold = true;
                }
                link.CommandArgument = raceId.ToString(CultureInfo.InvariantCulture);
                link.Click += LinkRaceButton_Click;
                cell.Controls.Add(link);
            }
            else
            {
                var label = new Label {Text = text};
                if (header)
                {
                    label.Font.Bold = true;
                }
                cell.Controls.Add(label);
            }

            return cell;
        }

        protected string GetFleetSeries(int raceId, int? fleetId)
        {
            var ctx = new SailTallyDataContext();
            var raceSeries = from rs in ctx.SS_RaceSeries
                             join fs in ctx.SS_FleetSeries on rs.FleetSeriesID equals fs.FleetSeriesID
                             join f in ctx.SS_Fleets on fs.FleetID equals f.FleetID
                             join s in ctx.SS_Series on fs.SeriesID equals s.SeriesID
                             where rs.RaceID == raceId
                             orderby f.ListOrder, f.FleetName, s.SeriesName
                             select new { f.FleetName, s.SeriesName, rs.RaceNumber, f.FleetID };

            if (fleetId != null)
            {
                raceSeries = raceSeries.Where(p => p.FleetID == fleetId);
            }

            var fleetSeries = "";
            foreach (var raceSery in raceSeries)
            {
                if (fleetSeries.Length > 0)
                {
                    fleetSeries += ", ";
                }
                fleetSeries += raceSery.FleetName + " " + raceSery.SeriesName + " " + raceSery.RaceNumber;
            }

            return fleetSeries;
        }

        protected void GetUnscoredBoats(int fleetId)
        {
            var ctx = new SailTallyDataContext();

            listBoatsUnscored.Items.Clear();

            // Available Boats
            var boats = from b in ctx.SS_Boats
                        where b.FleetID == fleetId
                           && b.IsActive
                        orderby b.BoatOrder, b.SailNumber
                        select b;

            foreach (var boat in boats) // list of available boats to score (not already in the scored list)
            {
                // Determine if the boat has already been scored
                var boatScored = false;
                var index = 0;
                while (index < _boatScores.Count && !boatScored)
                {
                    if (boat.SailNumber == _boatScores[index].SailNumber)
                    {
                        boatScored = true;
                    }
                    else
                    {
                        index++;
                    }
                }

                if (!boatScored) // go ahead and add to the list of unscored
                {
                    var item = new ListItem {Text = boat.SailNumber, Value = boat.BoatID.ToString(CultureInfo.InvariantCulture)};
                    listBoatsUnscored.Items.Add(item);
                }
            }
        }

        protected void ShowRaces()
        {
            if (tableRaces.Visible)
            {
                var ctx = new SailTallyDataContext();
                IQueryable<SS_Race> races;

                if (radioScheduledAll.Checked)
                {
                    races = from r in ctx.SS_Races
                            where r.SeasonID == Convert.ToInt32(listSeason.SelectedValue)
                            orderby r.FirstWarningDate
                            select r;
                }
                else if (radioScheduledScored.Checked)
                {
                    races = from r in ctx.SS_Races
                            where r.SeasonID == Convert.ToInt32(listSeason.SelectedValue) && r.DockedDate != null
                            orderby r.FirstWarningDate descending
                            select r;
                }
                else // Unscored
                {
                    races = from r in ctx.SS_Races
                            where r.SeasonID == Convert.ToInt32(listSeason.SelectedValue) && r.DockedDate == null
                            orderby r.FirstWarningDate
                            select r;
                }

                tableRaces.Rows.Clear();
                var row = new TableRow();

                var col = AddColumn("First Warning", -1, true);
                col.Width = 130;
                row.Cells.Add(col);
                //row.Cells.Add(this.AddColumn("Actual Warning", -1, true));
                row.Cells.Add(AddColumn("PRO", -1, true));
                row.Cells.Add(AddColumn("Series", -1, true));

                tableRaces.Rows.Add(row);

                foreach (var race in races)
                {
                    row = new TableRow();

                    row.Cells.Add(AddColumn(CentralLibrary.FormatDisplayTime(race.FirstWarningDate), race.RaceID, false));
                    //row.Cells.Add(this.AddColumn(race.ActualDate.ToString(), -1, false));
                    row.Cells.Add(AddColumn(race.PRO, -1, false));

                    var fleetSeries = GetFleetSeries(race.RaceID, null);

                    row.Cells.Add(AddColumn(fleetSeries, -1, false));
                    tableRaces.Rows.Add(row);
                }
            }
        }

        protected IQueryable<SS_Fleet> GetFleetsForRace(int raceId)
        {
            var ctx = new SailTallyDataContext();

            var fleets = (from f in ctx.SS_Fleets
                          join fs in ctx.SS_FleetSeries on f.FleetID equals fs.FleetID
                          join rs in ctx.SS_RaceSeries on fs.FleetSeriesID equals rs.FleetSeriesID
                          join r in ctx.SS_Races on rs.RaceID equals r.RaceID
                          where r.RaceID == raceId
                          orderby f.ListOrder, f.FleetName, f.FleetID
                          select f).Distinct();
            return fleets;
        }

        protected void ShowRace(int raceId)
        {
            var ctx = new SailTallyDataContext();
            var race = (from r in ctx.SS_Races
                        where r.RaceID == raceId
                        select r).Single(); // should only be one record

            // Populate Header fields
            hiddenRaceID.Value = raceId.ToString(CultureInfo.InvariantCulture); // save for future use
            labelWarning.Text = CentralLibrary.FormatDisplayTime(race.FirstWarningDate);
            labelSeries.Text = GetFleetSeries(raceId, null);
            textPRO.Text = race.PRO;
            textAssistPRO.Text = race.AssistPRO;
            textHelpers.Text = race.Helper;
            textDocked.Text = race.DockedDate?.ToShortTimeString() ?? CentralLibrary.GetCurrentDateTime().ToShortTimeString();
            textWindDirection.Text = race.WindDirection;
            textWindSpeed.Text = race.WindSpeed;
            textCourseChange.Text = race.CourseChange;
            listWindUnits.SelectedValue = (string.IsNullOrEmpty(race.WindUnits) ? "kts" : race.WindUnits);
            textProtests.Text = race.Protests;
            textComments.Text = race.Comments;

            var fleets = GetFleetsForRace(raceId);
            listFleet.DataSource = fleets;
            listFleet.DataValueField = "fleetid";
            listFleet.DataTextField = "fleetname";
            listFleet.DataBind();
            buttonScoreFleet.Enabled = (fleets.Any());

            buttonRemoveScored.Visible = (!fleets.Any() && CentralLibrary.IsRacesScored(raceId)); // special case to have this function available
            ButtonAbandonRaces.Enabled = true;
        }

        protected void ShowResults(int raceId, int fleetId)
        {
            panelResults.Visible = true;
            listFleet.Enabled = false;
            buttonScoreFleet.Enabled = false;
            ResetSelectedBoats();

            labelSeriesPerFleet.Text = GetFleetSeries(raceId, fleetId);

            // Locate the results (if any)
            var ctx=new SailTallyDataContext();
            var raceFleets = from rf in ctx.SS_RaceFleets
                             where rf.RaceID == raceId && rf.FleetID == fleetId
                             select rf; // should be 0 or 1 record selected

            if (!raceFleets.Any())
            {
                // No results yet
                checkAbandoned.Checked = false;
                textCourse.Text = "";
                textDistance.Text = "";
                textDistanceUnits.Text = "NM";
            }
            else
            {
                var raceFleet=raceFleets.Single(); // should only be one record - so get the first

                // Populate the results
                checkAbandoned.Checked = (raceFleet.IsAbandoned != null && (bool)raceFleet.IsAbandoned);
                textCourse.Text = raceFleet.Course;
                textDistance.Text = raceFleet.Distance.ToString();
                textDistanceUnits.Text = raceFleet.DistanceUnits;
            }

        }

        protected void AddScore(int position, BoatScore boatScore)
        {
            var item = new ListItem
            {
                Text = boatScore.FinishPosition + ": " + boatScore.SailNumber + (!boatScore.IsNormalPenalty ? " (" + boatScore.PenaltyName + ")" : ""),
                Value = boatScore.BoatId.ToString(CultureInfo.InvariantCulture)
            };

            if (position < 0)
            {
                listBoatsScored.Items.Add(item);
                _boatScores.Add(boatScore);
            }
            else
            {
                listBoatsScored.Items.Insert(position, item);
                _boatScores.Insert(position, boatScore);
            }

        }

        protected void AddScore(int fleetId, int boatId, string sailNumber, int finishPlace, int penaltyId, string penaltyName, bool isNormalPenalty)
        {
            AddScore(-1, fleetId, boatId, sailNumber, finishPlace, penaltyId, penaltyName, isNormalPenalty);
        }

        protected void AddScore(int position, int fleetId, int boatId, string sailNumber, int finishPlace, int penaltyId, string penaltyName, bool isNormalPenalty)
        {
            var boatScore = new BoatScore(fleetId, boatId, sailNumber, finishPlace, penaltyId, penaltyName, isNormalPenalty);
            AddScore(position, boatScore);
        }

        protected BoatScore RemoveScore(int position)
        {
            var boatScore = _boatScores[position];
            listBoatsScored.Items.RemoveAt(position);
            _boatScores.RemoveAt(position);

            return boatScore;
        }

        protected void ResetSelectedBoats()
        {
            listPenalty.Enabled = false;
            listPenalty.SelectedIndex = 0; // first item
            buttonUnscore.Enabled = false;
            ButtonMoveUp.Enabled = false;
            ButtonMoveDown.Enabled = false;
        }

        protected void SwapBoatScore(ref BoatScore boatScore1, ref BoatScore boatScore2)
        {
            var tempFinishPosition = boatScore1.FinishPosition;
            boatScore1.FinishPosition = boatScore2.FinishPosition;
            boatScore2.FinishPosition = tempFinishPosition;
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LabelAbandon.Text = "";
                CentralLibrary.GetSeasons(listSeason);
                CentralLibrary.GetPenalties(listPenalty);
                ShowSummary(false);
            }
            ShowRaces(); // Since dynamically generated, not part of ViewState

            if (ViewState["_fleetBoats"] != null)
            {
                _boatScores = (List<BoatScore>) ViewState["_fleetBoats"]; // Load state
            }
            else
            {
                _boatScores = new List<BoatScore>();
            }
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            ViewState.Add("_fleetBoats", _boatScores);
        }

        protected void buttonSelect_Click(object sender, EventArgs e)
        {
            LabelAbandon.Text = "";
            ShowSummary(true);
            ShowRaces();
        }

        protected void buttonReset_Click(object sender, EventArgs e)
        {
            ShowSummary(false);
        }

        protected void LinkRaceButton_Click(object sender, EventArgs e)
        {
            var raceId = Convert.ToInt32(((LinkButton)sender).CommandArgument); // get the stored race ID based on the link clicked

            ShowDetail(true);
            ShowRace(raceId);
        }

        private void SaveRace(int raceId)
        {
            var ctx = new SailTallyDataContext();
            var race = (from r in ctx.SS_Races
                        where r.RaceID == raceId
                        select r).Single(); // should only be one
            race.PRO = textPRO.Text;
            race.AssistPRO = textAssistPRO.Text;
            race.Helper = textHelpers.Text;
            race.DockedDate = Convert.ToDateTime(race.FirstWarningDate.ToShortDateString() + " " + textDocked.Text);
            race.WindDirection = textWindDirection.Text;
            race.WindSpeed = textWindSpeed.Text;
            race.CourseChange = textCourseChange.Text;
            race.WindUnits = listWindUnits.SelectedValue;
            race.Protests = textProtests.Text;
            race.Comments = textComments.Text;
            ctx.SubmitChanges(); // update
        }

        protected void buttonScoreFleet_Click(object sender, EventArgs e)
        {
            var raceId = Convert.ToInt32(hiddenRaceID.Value);
            var fleetId = Convert.ToInt32(listFleet.SelectedValue);
            var ctx = new SailTallyDataContext();

            // Save the header and hide it
            SaveRace(raceId);
            ShowDetail(true);
            ShowHeader(false);

            // Get the results (including the boats that have been scored
            var scoredBoats = from s in ctx.SS_Scores
                              join b in ctx.SS_Boats on s.BoatID equals b.BoatID
                              join p in ctx.SS_Penalties on s.PenaltyID equals p.PenaltyID
                              where s.RaceID == raceId && b.FleetID == fleetId
                              orderby s.FinishPlace
                              select new { s.ScoreID, s.BoatID, b.SailNumber, s.FinishPlace, s.PenaltyID, p.PenaltyName, b.FleetID, p.IsNonPenalty };

            // Populate the results (must be done prior to getting the available boats to score)
            listBoatsScored.Items.Clear();
            _boatScores.Clear();

            foreach (var scoredBoat in scoredBoats) // list of boats already scored
            {
                AddScore(scoredBoat.FleetID, scoredBoat.BoatID, scoredBoat.SailNumber, scoredBoat.FinishPlace, scoredBoat.PenaltyID, scoredBoat.PenaltyName, scoredBoat.IsNonPenalty);
            }

            // Get the available boats to score
            GetUnscoredBoats(fleetId);

            // Show the results
            ShowResults(raceId, fleetId);
        }
       
        protected void linkSave_Click(object sender, EventArgs e)
        {
            var raceId = Convert.ToInt32(hiddenRaceID.Value);
            var fleetId = Convert.ToInt32(listFleet.SelectedValue);
            var ctx = new SailTallyDataContext();

            // Delete the existing scores
            var scoredBoats = from s in ctx.SS_Scores
                              join b in ctx.SS_Boats on s.BoatID equals b.BoatID
                              where s.RaceID == raceId && b.FleetID == fleetId
                              select s;

            var raceFleets = from rf in ctx.SS_RaceFleets
                             where rf.RaceID == raceId && rf.FleetID == fleetId
                             select rf;

            ctx.SS_Scores.DeleteAllOnSubmit(scoredBoats);
            ctx.SS_RaceFleets.DeleteAllOnSubmit(raceFleets);
            ctx.SubmitChanges();
            
            // Save the scores
            var raceFleet = new SS_RaceFleet
            {
                RaceID = raceId,
                FleetID = fleetId,
                IsAbandoned = checkAbandoned.Checked,
                Course = textCourse.Text,
                Distance = CentralLibrary.ConvertToDouble(textDistance.Text, 0),
                DistanceUnits = textDistanceUnits.Text
            };
            ctx.SS_RaceFleets.InsertOnSubmit(raceFleet);
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var boatScore in _boatScores)
            {
                var score = new SS_Score
                {
                    RaceID = raceId,
                    BoatID = boatScore.BoatId,
                    PenaltyID = boatScore.PenaltyId,
                    FinishPlace = boatScore.FinishPosition
                };
                ctx.SS_Scores.InsertOnSubmit(score);
            }
            ctx.SubmitChanges();

            ShowDetail(true);
            ShowRaces();
        }

        protected void ButtonAbandonRaces_Click(object sender, EventArgs e)
        {
            var raceId = Convert.ToInt32(hiddenRaceID.Value);
            var fleets = GetFleetsForRace(raceId);
            if (fleets.Any()) SaveRace(raceId);
            var fleetsAbandoned = "";
            var ctx = new SailTallyDataContext();

            foreach (var fleet in fleets)
            {
                var fleetId = fleet.FleetID;
                var raceFleetExist = ((from rf in ctx.SS_RaceFleets
                                       where rf.RaceID == raceId && rf.FleetID == fleetId
                                       select rf).Any());

                if (raceFleetExist) continue; // Only abandon races that have no results

                var raceFleet = new SS_RaceFleet { FleetID = fleetId, RaceID = raceId, IsAbandoned = true };
                ctx.SS_RaceFleets.InsertOnSubmit(raceFleet);

                if (fleetsAbandoned.Length > 0)
                {
                    fleetsAbandoned += ", ";
                }
                fleetsAbandoned += fleet.FleetName;
            }
            ctx.SubmitChanges();
            LabelAbandon.Text = "Fleets Abandoned: " + fleetsAbandoned;

            ButtonAbandonRaces.Enabled = false; 
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            ShowDetail(true);
            ShowRaces();
        }

        protected void listBoatsUnscored_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Automatically move boat over to scored column
            var boatId = Convert.ToInt32(listBoatsUnscored.SelectedItem.Value);
            var sailNumber = listBoatsUnscored.SelectedItem.Text;

            var ctx = new SailTallyDataContext();

            // Get the boat selected information
            var boat = (from b in ctx.SS_Boats
                        where b.BoatID == boatId
                           && b.IsActive
                        select b).Single();

            // Get the default non-penalty information
            var penalty = (from p in ctx.SS_Penalties
                           where p.PenaltyName.Trim() == string.Empty
                              && p.IsNonPenalty
                           select p).Single();

            AddScore(boat.FleetID, boatId, sailNumber, listBoatsScored.Items.Count + 1, penalty.PenaltyID, penalty.PenaltyName, penalty.IsNonPenalty);

            // Remove boat from the unscored list
            listBoatsUnscored.Items.Remove(listBoatsUnscored.SelectedItem);
        }

        protected void listPenalty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listPenalty.SelectedItem == null) return;

            var ctx=new SailTallyDataContext();
            var penaltyId=Convert.ToInt32(listPenalty.SelectedItem.Value);
            var penaltyName = listPenalty.SelectedItem.Text;
            var isNormalPenalty = (from p in ctx.SS_Penalties
                where p.PenaltyID == penaltyId
                select p).Single().IsNonPenalty;

            // Save existing settings
            var position = listBoatsScored.SelectedIndex;
            var boatScore = _boatScores[position];

            // Remove existing entries
            listBoatsScored.Items.RemoveAt(position);
            _boatScores.RemoveAt(position);

            // Add entry back in to the existing location
            AddScore(position, boatScore.FleetId, boatScore.BoatId, boatScore.SailNumber, boatScore.FinishPosition, penaltyId, penaltyName, isNormalPenalty);

            // Re-highlight
            listBoatsScored.SelectedIndex = position;
        }

        protected void listBoatsScored_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoatsScored.SelectedItem == null) return;

            listPenalty.Enabled = true;
            buttonUnscore.Enabled = true;
            ButtonMoveUp.Enabled = (listBoatsScored.SelectedIndex > 0);
            ButtonMoveDown.Enabled = (listBoatsScored.SelectedIndex < listBoatsScored.Items.Count - 1);

            // Highlight the selected penalty
            listPenalty.SelectedValue = _boatScores[listBoatsScored.SelectedIndex].PenaltyId.ToString(CultureInfo.InvariantCulture);
        }

        protected void buttonUnscore_Click(object sender, EventArgs e)
        {
            var position = listBoatsScored.SelectedIndex;
            var boatScore = RemoveScore(position);

            var item = new ListItem {Text = boatScore.SailNumber, Value = boatScore.BoatId.ToString(CultureInfo.InvariantCulture)};
            listBoatsUnscored.Items.Add(item);

            ResetSelectedBoats();
        }

        protected void buttonRemoveScored_Click(object sender, EventArgs e)
        {
            var raceId = Convert.ToInt32(hiddenRaceID.Value);
            CentralLibrary.RemoveScores(raceId);
            buttonRemoveScored.Visible = false;
        }

        protected void buttonRefresh_Click(object sender, EventArgs e)
        {
            GetUnscoredBoats(Convert.ToInt32(listFleet.SelectedValue));
        }
       

        protected void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            var currPos = listBoatsScored.SelectedIndex;
            var prevPos = currPos - 1;
            var currBoatScore = RemoveScore(currPos); // order of removal important
            var prevBoatScore = RemoveScore(prevPos); // order of removal important
            SwapBoatScore(ref currBoatScore, ref prevBoatScore);
            AddScore(prevPos, currBoatScore);
            AddScore(currPos, prevBoatScore);
            listBoatsScored.SelectedIndex = prevPos;
            listBoatsScored_SelectedIndexChanged(sender, e);
        }

        protected void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            var currPos = listBoatsScored.SelectedIndex;
            var nextPos = currPos + 1;
            var nextBoatScore = RemoveScore(nextPos); // order of removal important
            var currBoatScore = RemoveScore(currPos); // order of removal important
            SwapBoatScore(ref currBoatScore, ref nextBoatScore);
            AddScore(currPos, nextBoatScore);
            AddScore(nextPos, currBoatScore);
            listBoatsScored.SelectedIndex = nextPos;
            listBoatsScored_SelectedIndexChanged(sender, e);
        }
        #endregion
    }

    #region Class
    [Serializable]
    public class BoatScore
    {
        public int FleetId { get; set; }
        public int BoatId { get; set; }
        public string SailNumber { get; set; }
        public int FinishPosition { get; set; }
        public int PenaltyId { get; set; }
        public string PenaltyName { get; set; }
        public bool IsNormalPenalty { get; set; }

        public BoatScore(int fleetId, int boatId, string sailNumber, int finishPlace, int penaltyId, string penaltyName, bool isNormalPenalty)
        {
            FleetId = fleetId;
            BoatId = boatId;
            SailNumber = sailNumber;
            FinishPosition = finishPlace;
            PenaltyId = penaltyId;
            PenaltyName = penaltyName;
            IsNormalPenalty = isNormalPenalty;
        }
    }
    #endregion
}