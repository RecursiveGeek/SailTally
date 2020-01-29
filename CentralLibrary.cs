using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using SailTally.Classes;

namespace SailTally
{
    [Serializable]
    public class CentralLibrary
    {
        #region Constants
        public const int EventidInit          = 10000;
        public const int EventidLoginSuccess  = 10001;
        public const int EventidLoginFail     = 10002;
        public const int EventidTableUpdate   = 11001;
        public const int EventidTableInsert   = 11002;
        public const int EventidTableDelete   = 11003;
        public const int EventidTableSelect   = 11004;
        public const int EventidInternalError = 19999;
        #endregion

        #region Properties
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string Copyright => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright;

        public static string User
        {
            get
            {
                try
                {
                    var mUser = Membership.GetUser();
                    return (mUser == null ? "~UNKNOWN~" : mUser.UserName);
                }
                catch
                {
                    return "~UNKNOWN~";
                }
            }
        }
        #endregion

        #region Functions
        public static void SyncGridView(ref GridView gv, DetailsView dv)
        {
            gv.SelectedIndex = dv.PageIndex % gv.PageSize;
            gv.PageIndex = dv.PageIndex / gv.PageSize;
            gv.DataBind();
        }

        public static void SyncDataView(GridView gv, ref DetailsView dv)
        {
            dv.PageIndex = gv.SelectedIndex + (gv.PageIndex * gv.PageSize);
            dv.DataBind();
        }

        public static void Post(int eventId, string operation, string source, string subject, int reference, string details)
        {
            Post(eventId, operation, source, subject, reference.ToString(CultureInfo.InvariantCulture), details);
        }

        public static void Post(int eventId, string operation, string source, string subject, string reference, string details)
        {
            var ctx = new SailTallyDataContext();

            var log = new SS_Log
            {
                Event = eventId,
                Operation = operation,
                Source = source,
                Subject = subject,
                Reference = reference,
                Details = details,
                Username = User,
                LogDate = GetCurrentDateTime()
            };

            ctx.SS_Logs.InsertOnSubmit(log);
            ctx.SubmitChanges();
        }

        public static void Post(ChangeAction action, string table, int reference)
        {
            Post(action, table, reference.ToString(CultureInfo.InvariantCulture));
        }

        public static void Post(ChangeAction action, string table, string reference)
        {
            var actionStr = "Unknown";
            var eventId = EventidInit;
            var refId = reference;

            switch (action)
            {
                case ChangeAction.Update:
                    actionStr = "Update";
                    eventId = EventidTableUpdate;
                    break;
                case ChangeAction.Insert:
                    actionStr = "Insert";
                    eventId = EventidTableInsert;
                    refId = "";
                    break;
                case ChangeAction.Delete:
                    actionStr = "Delete";
                    eventId = EventidTableDelete;
                    break;
            }

            Post(eventId, actionStr, "Data", table, refId, "");
        }

        public static void GetSeasons(DropDownList list)
        {
            GetSeasons(list, false);
        }

        public static void GetSeasons(DropDownList list, bool excludeLocked)
        {
            var ctx = new SailTallyDataContext();

            var seasons = from s in ctx.SS_Seasons
                          where s.IsActive == true
                          orderby s.SeasonName descending
                          select new { s.SeasonID, s.SeasonName, s.LockResults };

            if (excludeLocked)
            {
                var seasonFilter = seasons.Where(s => !s.LockResults);
                list.DataSource = seasonFilter;
            }
            else
            {
                list.DataSource = seasons;
            }

            list.DataTextField = "seasonname";
            list.DataValueField = "seasonid";
            list.DataBind();       
        }

        public static string GetSeason(int seasonId)
        {
            var ctx = new SailTallyDataContext();

            var season = (from s in ctx.SS_Seasons
                          where s.SeasonID == seasonId
                          select s).First();

            return (season != null ? season.SeasonName : "~SEASON NOT FOUND~");
        }

        public static void GetFleets(DropDownList list)
        {
            var ctx = new SailTallyDataContext();

            var fleets = from f in ctx.SS_Fleets
                         where f.IsActive
                         orderby f.ListOrder, f.FleetName
                         select new { f.FleetID, f.FleetName };

            list.DataSource = fleets;
            list.DataTextField = "fleetname";
            list.DataValueField = "fleetid";
            list.DataBind();
        }

        public static void GetFleetSeries(DropDownList list, int fleetId, int seasonId)
        {
            var ctx = new SailTallyDataContext();

            var fleetSeries = from fs in ctx.SS_FleetSeries
                              join s in ctx.SS_Series on fs.SeriesID equals s.SeriesID
                              where fs.FleetID == fleetId && fs.SeasonID == seasonId && fs.IsActive
                              orderby s.SeriesName
                              select new { fs.FleetSeriesID, s.SeriesName };

            list.DataSource = fleetSeries;
            list.DataTextField = "seriesname";
            list.DataValueField = "fleetseriesid";
            list.DataBind();
        }

        public static void GetPenalties(DropDownList list)
        {
            var ctx = new SailTallyDataContext();

            var penalties = from p in ctx.SS_Penalties
                            orderby p.PenaltyName
                            select new { p.PenaltyID, p.PenaltyName };

            list.DataSource = penalties;
            list.DataTextField = "penaltyname";
            list.DataValueField = "penaltyid";
            list.DataBind();
        }

        public static void GetTimes(DropDownList list)
        {
            var ctx = new SailTallyDataContext();

            var times = from t in ctx.SS_Times
                        orderby t.SortOrder
                        select new { t.TimeID, t.TimeStr, t.DisplayTime };

            list.DataSource = times;
            list.DataTextField = "displaytime";
            list.DataValueField = "timestr";
            list.DataBind();
        }

        public static int GetNextRaceNumber(int seasonId, int fleetSeriesId)
        {
            var ctx = new SailTallyDataContext();

            var races = (from rs in ctx.SS_RaceSeries
                         where rs.SeasonID == seasonId
                            && rs.FleetSeriesID == fleetSeriesId
                         orderby rs.RaceNumber descending
                         select new { rs.RaceNumber });

            if (races.Any())
            {
                return races.First().RaceNumber + 1;
            }

            return 1;
        }

        public static string GetNextTime(string currentTime)
        {
            var ctx = new SailTallyDataContext();

            var times = from t in ctx.SS_Times
                        orderby t.NextTimeOrder
                        select t;
            var timeList = times.Select(time => time.TimeStr).ToList(); // Convert to an array for easier manipulation

            // Search for the current time
            var found=false;
            var timePos=0;
            while (!found && timePos<timeList.Count)
            {
                if (string.Equals(timeList[timePos], currentTime, StringComparison.CurrentCultureIgnoreCase))
                {
                    found = true;
                }
                else
                {
                    timePos++;
                }
            }

            if (found && timePos<timeList.Count-1) // make sure currentTime was found and there is a next time
            {
                return timeList[timePos+1];
            }
            return string.Empty;
        }

        // ToDo: Delete once DisplaySchedule_old is no longer needed
        public static bool IsBackToBack(string timeStr)
        {
            timeStr = FormatTime(timeStr);
            var ctx = new SailTallyDataContext();
            try
            {
                var time = (from t in ctx.SS_Times
                            where t.TimeStr == timeStr
                            select new { t.TimeID, t.TimeStr, t.IsBackToBackSlot }).First();

                if (time != null)
                {
                    return time.IsBackToBackSlot;
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch 
            {
                // Do nothing
            }
            return false;
        }

        public static string FormatTime(string timeStr)
        {
            return timeStr.Replace(" ", string.Empty).PadLeft(Constant.Schedule.TimeWidth, '0');
        }

        public static string FormatTime(DateTime dateTime)
        {
            return FormatTime(dateTime.ToShortTimeString());
        }

        public static string FormatDisplayTime(DateTime dt)
        {
            var actualTime = FormatTime(dt.ToShortTimeString());
            var ctx = new SailTallyDataContext();
            var displayTime = (from t in ctx.SS_Times
                              where t.TimeStr == actualTime
                              select t.DisplayTime).FirstOrDefault();
            return dt.ToShortDateString() + " " + displayTime;
        }

        public static string FormatDateAndTime(DateTime dt)
        {
            return dt.ToShortDateString() + " " + dt.ToShortTimeString();
        }

        public static int GetNextRaceNumber(int seasonId, int fleetId, int seriesId)
        {
            var ctx = new SailTallyDataContext();

            var fleetSeriesId = (from fs in ctx.SS_FleetSeries
                                 where fs.FleetID == fleetId
                                    && fs.SeriesID == seriesId
                                 select fs).First().FleetSeriesID;

            return GetNextRaceNumber(seasonId, fleetSeriesId);
        }

        public static string GetCurrentPageName()
        {
            var sPath = HttpContext.Current.Request.Url.AbsolutePath;
            var oInfo = new FileInfo(sPath);
            var sRet = oInfo.Name;
            return sRet;
        }

        public static void RemoveScores(int raceId)
        {
            var ctx = new SailTallyDataContext();

            // Scores
            var scores = from s in ctx.SS_Scores
                         where s.RaceID == raceId
                         select s;
            ctx.SS_Scores.DeleteAllOnSubmit(scores);

            // RaceFleets
            var raceFleets = from rf in ctx.SS_RaceFleets
                             where rf.RaceID == raceId
                             select rf;
            ctx.SS_RaceFleets.DeleteAllOnSubmit(raceFleets);

            ctx.SubmitChanges();
        }

        public static bool IsRacesScored(int raceId)
        {
            var ctx = new SailTallyDataContext();
            var scored = ((from s in ctx.SS_Scores
                           where s.RaceID == raceId
                           select s).Any());

            var header = ((from rf in ctx.SS_RaceFleets
                           where rf.RaceID == raceId
                           select rf).Any());

            return scored || header;
        }

        public static int ConvertToInt(string s, int emptyVal)
        {
            if (s.Length == 0) { return emptyVal; }
            try { return Convert.ToInt32(s); } catch { return emptyVal; }
        }

        public static double ConvertToDouble(string s, float emptyVal)
        {
            if (s.Length == 0) { return emptyVal; }
            try { return Convert.ToDouble(s); } catch { return emptyVal; }
        }

        public static double ScorePoints(int scoreMethodId, int place)
        {
            var ctx = new SailTallyDataContext();

            var scoreMethodDetail = (from smd in ctx.SS_ScoreMethodDetails
                                     where smd.ScoreMethodID == scoreMethodId && smd.Place == place
                                     select smd).Single();

            return scoreMethodDetail.Points;
        }

        public static int NumberThrowouts(int throwoutId, int racesCompleted)
        {
            var ctx = new SailTallyDataContext();

            var throwoutDetails = from t in ctx.SS_ThrowoutDetails
                                  where t.ThrowoutID == throwoutId
                                  orderby t.RaceCount
                                  select t;

            var numberThrowouts = 0;
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var throwoutDetail in throwoutDetails)
            {
                if (throwoutDetail.RaceCount <= racesCompleted)
                {
                    numberThrowouts = throwoutDetail.ThrowoutCount;
                }
            }

            return numberThrowouts;
        }

        public static bool IsFleetSeriesExist(int seasonId, int fleetId, int seriesId)
        {
            return IsFleetSeriesExist(seasonId, fleetId, seriesId, -999); // disregard existing fleetSeriesID (useful for inserting)
        }

        public static bool IsFleetSeriesExist(int seasonId, int fleetId, int seriesId, int fleetSeriesId)
        {
            var ctx = new SailTallyDataContext();

            var isPresent = (from fs in ctx.SS_FleetSeries
                              where fs.SeasonID == seasonId
                                    && fs.FleetID == fleetId
                                    && fs.SeriesID == seriesId
                                    && fs.FleetSeriesID != fleetSeriesId // don't find self (useful for editing)
                              select fs).Any();

            return isPresent;
        }

        // Delete once the new version if validated as operational
        public static void DisplaySchedule_old(Table tableSchedule, string postBackUrl, int seasonId, bool viewOnly)
        {
            var ctx = new SailTallyDataContext();

            // Get all of the fleets to display
            var fleets = from f in ctx.SS_Fleets
                         where f.IsActive
                         orderby f.ScheduleOrder, f.FleetName
                         select f;

            // Get all of the dates to display
            var dates = (from r in ctx.SS_Races
                         where r.SeasonID == seasonId
                         orderby r.FirstWarningDateOnly
                         select new { r.FirstWarningDateOnly }).Distinct();

            TableCell cell;
            tableSchedule.Rows.Clear(); // remove existing table data

            // Work through the fleets for the header
            var row = new TableRow();
            row.Cells.Add(new TableHeaderCell { Text = "Date" }); // Date Column
            foreach (var fleet in fleets)
            {
                cell = new TableHeaderCell { VerticalAlign = VerticalAlign.Top, Text = fleet.FleetName, Width = 100 };
                row.Cells.Add(cell);
            }
           tableSchedule.Rows.Add(row);

            // Work throught the dates
            foreach (var date in dates)
            {
                row = new TableRow();

                // First Column (Date)
                cell = new TableCell { VerticalAlign = VerticalAlign.Top };
                var labelDate = new Label { Text = date.FirstWarningDateOnly.Value.ToShortDateString() + "<br />(" + Convert.ToDateTime(date.FirstWarningDateOnly).DayOfWeek.ToString().Substring(0, 3) + ")" };
                cell.Controls.Add(labelDate);
                row.Cells.Add(cell);

                // Work through the fleets on a given date
                foreach (var fleet in fleets)
                {
                    // Get local copies to avoid compiler-specific implementation issues
                    var fleet1 = fleet;
                    var date1 = date;

                    // Get the appointments for the given fleet and date
                    var appts = from r in ctx.SS_Races
                                join rs in ctx.SS_RaceSeries on r.RaceID equals rs.RaceID
                                join fs in ctx.SS_FleetSeries on rs.FleetSeriesID equals fs.FleetSeriesID
                                join s in ctx.SS_Series on fs.SeriesID equals s.SeriesID
                                where r.SeasonID == seasonId && fs.FleetID == fleet1.FleetID && r.FirstWarningDateOnly == date1.FirstWarningDateOnly //&& fs.IsActive
                                orderby r.FirstWarningDate
                                select new { r.RaceID, rs.RaceSeriesID, r.FirstWarningDate, s.SeriesName, rs.RaceNumber };

                    var apptCount = 0;
                    var raceTime = string.Empty;
                    cell = new TableCell { VerticalAlign = VerticalAlign.Top };
                    Label labelRaceTime;
                    foreach (var appt in appts)
                    {
                        if (raceTime.Length == 0)
                        {
                            raceTime = appt.FirstWarningDate.ToShortTimeString();
                        }

                        if (raceTime != appt.FirstWarningDate.ToShortTimeString())
                        {
                            if (!IsBackToBack(appt.FirstWarningDate.ToShortTimeString())) // only display time if not flagged as back-to-back
                            {
                                labelRaceTime = new Label {Text = "<br />" + raceTime};
                                cell.Controls.Add(labelRaceTime);
                                raceTime = appt.FirstWarningDate.ToShortTimeString();
                            }
                        }

                        var displayAppt = appt.SeriesName.Trim() + " " + appt.RaceNumber.ToString(CultureInfo.InvariantCulture).Trim();
                        if (!viewOnly)
                        {
                            displayAppt += " (" + appt.RaceID + ")";
                        }
                        if (apptCount > 0)
                        {
                            displayAppt = "<br />" + displayAppt; // put a space on the previous scheduled item
                        }

                        if (viewOnly)
                        {
                            var label = new Label {Text = displayAppt};
                            cell.Controls.Add(label);
                        }
                        else
                        {
                            var linkButton = new LinkButton
                            {
                                Text = displayAppt,
                                PostBackUrl = postBackUrl + Constant.Url.Parameters + Constant.GetVar.RaceId + Constant.Url.Assign + appt.RaceID.ToString(CultureInfo.InvariantCulture)
                            };
                            cell.Controls.Add(linkButton);
                        }

                        apptCount++;
                    }
                    labelRaceTime = new Label {Text = "<br />" + raceTime};
                    cell.Controls.Add(labelRaceTime);
                    row.Cells.Add(cell);
                }
                tableSchedule.Rows.Add(row);
            }
        }

        public static void DisplaySchedule(Table tableSchedule, string postBackUrl, int seasonId, bool viewOnly)
        {
            var ctx = new SailTallyDataContext();

            tableSchedule.Rows.Clear(); // remove existing table data

            // Get the schedule for the entire season
            var appts = from r in ctx.SS_Races
                        join rs in ctx.SS_RaceSeries on r.RaceID equals rs.RaceID
                        join fs in ctx.SS_FleetSeries on rs.FleetSeriesID equals fs.FleetSeriesID
                        join s in ctx.SS_Series on fs.SeriesID equals s.SeriesID
                        join f in ctx.SS_Fleets on fs.FleetID equals f.FleetID
                        join t in ctx.SS_Times on r.FirstWarningTimeOnly equals t.TimeStr
                        where r.SeasonID == seasonId
                        orderby r.FirstWarningDateOnly, f.ScheduleOrder, f.FleetName, r.FirstWarningDate
                        select new { r.FirstWarningDateOnly, r.FirstWarningDate, f.FleetName, r.RaceID, rs.RaceSeriesID, s.SeriesName, rs.RaceNumber, t.IsBackToBackSlot, f.ScheduleOrder };

            // Get all of the fleets to display
            var fleetsQuery = from f in ctx.SS_Fleets
                              where f.IsActive
                              orderby f.ScheduleOrder, f.FleetName
                              select new { f.FleetName };

            //// Get the fleets that have data
            //var fleetsQuery = (from a in appts
            //                   orderby a.ScheduleOrder, a.FleetName
            //                   select new { a.FleetName }).Distinct();

            var fleets = new List<Fleet>();

            // Work through the fleets for the header
            var headerRow = new TableRow();
            headerRow.Cells.Add(new TableHeaderCell { Text = "Date" }); // Date Column
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var fleetQuery in fleetsQuery)
            {
                TableCell cell = new TableHeaderCell { VerticalAlign = VerticalAlign.Top, Text = fleetQuery.FleetName, Width = 100 };
                headerRow.Cells.Add(cell);
                fleets.Add(new Fleet(fleetQuery.FleetName));
            }
            tableSchedule.Rows.Add(headerRow);

            if (!appts.Any()) return; // No Data

            var apptFirst = appts.First();
            var lastFleetName = apptFirst.FleetName; // used to trigger a change
            var lastFirstWarningDateOnly = apptFirst.FirstWarningDateOnly; // used to trigger a change
            var lastFleetIndex = 0;
            var apptFleetDay = new List<Appointment>(); // one or more appointments for a given fleet on a given day
            TableRow row = null;

            foreach (var appt in appts) // Work through the records
            {
                if (lastFirstWarningDateOnly != appt.FirstWarningDateOnly) // will need to jump to a new row?
                {
                    lastFleetIndex = DisplayScheduleBuildToRow(tableSchedule, ref row, fleets, apptFleetDay.First().FleetName, lastFleetIndex, ref apptFleetDay, viewOnly, postBackUrl);
                }
                else if (lastFleetName != appt.FleetName)
                {
                    if (apptFleetDay.Any())
                    {
                        lastFleetIndex = DisplayScheduleBuildToColumn(tableSchedule, ref row, fleets, apptFleetDay.First().FleetName, lastFleetIndex, apptFleetDay, viewOnly, postBackUrl);
                    }
                }

                apptFleetDay.Add(new Appointment(appt.FirstWarningDateOnly, appt.FirstWarningDate, appt.FleetName, appt.RaceID, appt.RaceSeriesID, appt.SeriesName, appt.RaceNumber, appt.IsBackToBackSlot, appt.ScheduleOrder));
                lastFirstWarningDateOnly = appt.FirstWarningDateOnly;
                lastFleetName = appt.FleetName;
            }

            // catch any remaining records
            DisplayScheduleBuildToRow(tableSchedule, ref row, fleets, apptFleetDay.First().FleetName, lastFleetIndex, ref apptFleetDay, viewOnly, postBackUrl);
        }

        public static int DisplayScheduleBuildToRow(Table tableSchedule, ref TableRow row, List<Fleet> fleets, string fleetName, int fleetIndexStart, ref List<Appointment> appts, bool viewOnly, string postBackUrl)
        {
            // fleetIndexStart is the last/previous fleet addition in a given row.  fleetIndexEnd is where the current fleet was edded.
            var fleetIndexEnd = appts.Any() ? DisplayScheduleBuildToColumn(tableSchedule, ref row, fleets, appts.First().FleetName, fleetIndexStart, appts, viewOnly, postBackUrl) : fleetIndexStart;
            // Fill out the rest of the row
            for (var fleetIndex = fleetIndexEnd; fleetIndex < fleets.Count; fleetIndex++)
            {
                var cell = new TableCell { VerticalAlign = VerticalAlign.Top };
                row.Cells.Add(cell); // blank empty
            }
            tableSchedule.Rows.Add(row);
            row = null;
            return 0; // Start Over
        }

        public static int DisplayScheduleBuildToColumn(Table tableSchedule, ref TableRow row, List<Fleet> fleets, string fleetName, int fleetIndexStart, List<Appointment> appts, bool viewOnly, string postBackUrl)
        {
            TableCell cell;

            if (row == null)
            {
                row = new TableRow();
                // Build the first colume (Date)
                cell = new TableCell { VerticalAlign = VerticalAlign.Top };
                var firstWarningDateOnly = Convert.ToDateTime(appts.First().FirstWarningDateOnly);
                var labelDate = new Label { Text = firstWarningDateOnly.ToShortDateString() + "<br />(" + firstWarningDateOnly.DayOfWeek.ToString().Substring(0,3)+")" };
                cell.Controls.Add(labelDate);
                row.Cells.Add(cell);
            }

            var fleetIndexEnd = DisplayScheduleFindFleet(fleets, fleetName); // find the column the fleet will be added

            // Work through empty sells if any need to be created until the correct fleet column is found
            for (var fleetIndex = fleetIndexStart; fleetIndex < fleetIndexEnd; fleetIndex++)
            {
                cell = new TableCell { VerticalAlign = VerticalAlign.Top };
                row.Cells.Add(cell); // blank empty
            }

            cell = new TableCell { VerticalAlign = VerticalAlign.Top };
            var raceTime = string.Empty;
            var apptCount = 0;
            Label labelRaceTime;

            foreach (var appt in appts)
            {
                if (raceTime.Length == 0)
                {
                    raceTime = appt.FirstWarningDate.ToShortTimeString();
                }

                if (raceTime != appt.FirstWarningDate.ToShortTimeString())
                {
                    if (!appt.IsBackToBack) // only display time if not flagged as back-to-back
                    {
                        labelRaceTime = new Label { Text="<br/>" + raceTime };
                        cell.Controls.Add(labelRaceTime);
                        raceTime = appt.FirstWarningDate.ToShortTimeString();
                    }
                }

                var displayAppt = appt.SeriesName.Trim() + " " + appt.RaceNumber.ToString(CultureInfo.InvariantCulture).Trim();

                if (!viewOnly)
                {
                    displayAppt += "<span class=\"ScheduleRaceId\"> (" + appt.RaceId + ")</span>";
                }

                if (apptCount > 0)
                {
                    displayAppt = "<br />" + displayAppt; // put a space on the previous scheduled item
                }

                if (viewOnly)
                {
                    var label = new Label { Text = displayAppt };
                    cell.Controls.Add(label);
                }
                else
                {
                    var linkButton = new LinkButton
                    {
                        Text = displayAppt,
                        PostBackUrl = postBackUrl + Constant.Url.Parameters + Constant.GetVar.RaceId + Constant.Url.Assign + appt.RaceId.ToString(CultureInfo.InvariantCulture)
                    };
                    cell.Controls.Add(linkButton);
                }
                apptCount++;
            }

            labelRaceTime = new Label {Text = "<br />" + raceTime};
            cell.Controls.Add(labelRaceTime);
            row.Cells.Add(cell);

            appts.Clear();

            return fleetIndexEnd + 1;
        }

        public static int DisplayScheduleFindFleet(List<Fleet> fleets, string fleetName)
        {
            var index = 0;

            foreach (var fleet in fleets)
            {
                if (fleet.FleetName == fleetName) { return index; }
                index++;
            }
            return -1; // not found
        } 

        public static string GetScore(SS_Result result)
        {
            var score = result.Points.ToString(CultureInfo.InvariantCulture);

            if (result.IsAbandoned) { score += "A"; }
            if (result.IsAbsent) { score += "-"; }
            if (!result.IsNonPenalty && result.PenaltyName != null) { score += "/" + result.PenaltyName.Trim(); }
            if (result.IsThrowout) { score = "[" + score + "]"; }
            if (score.Trim().Length == 0) { score = "&nbsp;"; }
            return score;
        }

        public static string GetResultsHeader(string seasonStr, string fleetName)
        {
            var resultMsg = "<h2>" + seasonStr + " " + fleetName + " Fleet</h2>";
            return resultMsg;
        }


        public static string GetResults(int seasonId, string fleetName)
        {
            return GetResults(seasonId, fleetName, false);
        }

        public static string GetResults(int seasonId, string fleetName, bool updatePosition)
        {
            var ctx = new SailTallyDataContext();
            var resultMsg = "";

            var resultSeries = (from rs in ctx.SS_ResultSummaries
                                where rs.FleetName == fleetName
                                   && rs.SeasonID == seasonId
                                orderby rs.SeriesName
                                select new { rs.SeriesName, rs.FleetSeriesID }).Distinct();

            foreach (var resultSery in resultSeries)
            {
                resultMsg += "<h3>" + resultSery.SeriesName + "</h3>";
                var seriesName = resultSery.SeriesName;
                var resultSummares = from rs in ctx.SS_ResultSummaries
                                     where rs.SeriesName == seriesName
                                        && rs.FleetName == fleetName
                                        && rs.SeasonID == seasonId
                                     orderby rs.TotalPoints, rs.TieBreakerStr
                                     select rs;

                resultMsg += "<div class='table-responsive'><table class='table table-striped ResultsTable'>"; // rules='all' attribute and value removed from table tag

                // Table Header for the results
                resultMsg += "<tr>";
                resultMsg += "<th class='ResultsTitlePlace'>Place</th>";
                resultMsg += "<th class='ResultsTitleSail'>Sail #</th>";
                resultMsg += "<th class='ResultsTitleSkipper'>Skipper</th>";

                var fleetSeriesId = resultSery.FleetSeriesID;
                var raceSeries = from rs in ctx.SS_RaceSeries
                                 where rs.FleetSeriesID == fleetSeriesId
                                    && rs.SeasonID == seasonId
                                 orderby rs.RaceNumber
                                 select new { rs.RaceNumber, rs.RaceID };
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var raceSery in raceSeries)
                {
                    var raceNumber = raceSery.RaceNumber;

                    // verify there are results posted (not just the header of the raceSeries, but also scores)
                    var resultsPosted = (from r in ctx.SS_Results
                                         where r.FleetSeriesID == fleetSeriesId
                                            && r.RaceNumber == raceNumber
                                         select r).Count();
                    if (resultsPosted > 0)
                    {
                        resultMsg += "<th class='ResultsTitleRace'><a href='DisplayRace.aspx?r=" + raceSery.RaceID + "'>R" + raceSery.RaceNumber + "</a></th>";
                    }
                }

                resultMsg += "<th class='ResultsTitleScore'>Points</th>";
                resultMsg += "</tr>";

                // Details of the races results/scores
                var position = 0;
                var prevPoints = "";
                var lastPublished = "";
                foreach (var resultSummary in resultSummares)
                {
                    if (prevPoints != resultSummary.TotalPoints + resultSummary.TieBreakerStr) // Detect absolute tie (no tie breaker found, so same position as previous boat)
                    {
                        position++;
                        prevPoints = resultSummary.TotalPoints + resultSummary.TieBreakerStr;

                        if (lastPublished.Length == 0) // if not known, grab the first available published date from one instance of a record (and use going forward for all)
                        {
                            lastPublished = "<span class='ResultsPublished'>Published " + resultSummary.Created + " by " + resultSummary.CreatedBy+"</span>";
                        }
                    }

                    // Position, boat information, etc.
                    resultMsg += "<tr>";
                    resultMsg += "<td class='ResultsBodyPlace'>" + position + "</td><td class='ResultsBodySail'>" + (resultSummary.IsRegisteredBoat ? "" : "<span class='ResultsUnregistered'>**</span>") + resultSummary.SailNumber + "</td><td class='ResultsBodySkipper'>" + resultSummary.Skipper + "</td>";

                    if (updatePosition) // Update the position, since now figured out
                    {
                        UpdateResultSummaryPosition(resultSummary.ResultSummaryID, position);
                    }

                    var boatId = resultSummary.BoatID;
                    // Get the races that make up the race result columns
                    var resultDetails = from r in ctx.SS_Results
                                        where r.FleetSeriesID == fleetSeriesId
                                           && r.BoatID == boatId
                                        orderby r.RaceNumber
                                        select r;

                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var resultDetail in resultDetails)
                    {
                        resultMsg += "<td class='ResultsBodyRace'>" + GetScore(resultDetail) + "</td>";
                    }

                    resultMsg += "<td class='ResultsBodyScore'>" + resultSummary.TotalPoints + "</td>";
                    resultMsg += "</tr>";
                }
                resultMsg += "</table></div>";
                resultMsg += lastPublished + "<br />";
            }

            return resultMsg;
        }

        public static void UpdateResultSummaryPosition(int resultSummaryId, int position)
        {
            var ctx = new SailTallyDataContext();

            var resultSummary = (from rs in ctx.SS_ResultSummaries
                                 where rs.ResultSummaryID == resultSummaryId
                                 select rs).First(); // should only have a single record returned

            if (resultSummary == null) return;
            resultSummary.Position = position;
            ctx.SubmitChanges();
        }

        public static string GetResultsKeyHeader()
        {
            var resultMsg = string.Empty;
            resultMsg += "<img src='" + GetSiteRoot("/images/info.png") + "' />&nbsp;Click on the race number headers (e.g. R1) to get race-specific information, such as wind, protests, or boat scoring notes.<br />";
            return resultMsg;
        }

        public static string GetSiteRoot(string file = "")
        {
            var port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            port = ((port == null || port == "80" || port == "443") ? string.Empty : ":" + port);

            var protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            protocol = ((protocol == null || protocol == "0") ? "http://" : "https://");

            var siteRoot = protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + HttpContext.Current.Request.ApplicationPath;
            if (siteRoot.EndsWith("/")) { siteRoot = siteRoot.Substring(0, siteRoot.Length - 1); }

            if (file.Length <= 0) return siteRoot;
            if (!file.StartsWith("/"))
            {
                siteRoot += "/";
            }
            return siteRoot + file;
        }

        public static string GetResultsKeyFooter()
        {
            var resultMsg = string.Empty;

            resultMsg += "<br />";
            resultMsg += "<b>Key</b><br />";
            resultMsg += "Scores in [brackets] are Throwouts.&nbsp;&nbsp;A: Abandoned Race.&nbsp;&nbsp;-: Not Present for Race.&nbsp;&nbsp;<span class='unregistered'>**</span>: Unregistered Boat.<br />";

            var ctx = new SailTallyDataContext();
            var penalties = from p in ctx.SS_Penalties
                            where !p.IsNonPenalty
                            orderby p.PenaltyName
                            select p;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var penalty in penalties)
            {
                resultMsg += penalty.PenaltyName + ": " + penalty.Description + "&nbsp;&nbsp;";
            }

            return resultMsg;
        }

        public static string Encrypt(string data)
        {
            var source = Encoding.ASCII.GetBytes(data); // convert data to byte array
            var hash = new MD5CryptoServiceProvider().ComputeHash(source); // convert byte array into a hash
            var output = ""; // this will hold a hex version of the hash

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var b in hash)
            {
                output += b.ToString("X2");
            }

            return output;
        }

        public static bool Authenticated(string username, string passwordClear)
        {
            var ctx = new SailTallyDataContext();

            var users = from u in ctx.SS_Users
                        where u.Username == username // case-sensitive
                        select u;

            if (users.Count() == 1) // should only have one record if there is a match
            {
                var user = users.Single();
                var password = (user.IsEncryptedPwd ? Encrypt(passwordClear) : passwordClear);

                return (user.Password == password); // may be authenticated, depending on match
            }
            return false; // not authenticated
        }

        public static bool HasUserRole(string username, string rolename)
        {
            var ctx = new SailTallyDataContext();

            var roleFound = (from ur in ctx.SS_UserRoles
                              join u in ctx.SS_Users on ur.UserID equals u.UserID
                              join r in ctx.SS_Roles on ur.RoleID equals r.RoleID
                              where u.Username == username
                                 && r.RoleName == rolename
                              select ur).Count() > 1;
            return roleFound;
        }

        public static string PositionSuffix(int? number)
        {
            var result = string.Empty;
            if (number == null) return result;
            var numStr = number.ToString().Trim();
            if (numStr.Length <= 0) return result;
            switch (numStr.Last())
            {
                case '0': result = "th"; break;
                case '1': result = "st"; break;
                case '2': result = "nd"; break;
                case '3': result = "rd"; break;
                case '4': result = "th"; break;
                case '5': result = "th"; break;
                case '6': result = "th"; break;
                case '7': result = "th"; break;
                case '8': result = "th"; break;
                case '9': result = "th"; break;
                default: result = "th"; break;
            }
            result = numStr + result;
            return result;
        }

        /// <summary>
        /// Clones any object and returns the new cloned object.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="source">The original object.</param>
        /// <returns>The clone of the object.</returns>
        /// <remarks>http://stackoverflow.com/questions/2178080/linq-to-sql-copy-original-entity-to-new-one-and-save</remarks>
        public static T Clone<T>(T source)
        {
            var dcs = new DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                dcs.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)dcs.ReadObject(ms);
            }
        }
        public static string GetConfigurationSetting(string item)
        {
            return ConfigurationManager.AppSettings[item];
        }

        public static DateTime GetCurrentDateTime()
        {
            var serverTime = DateTime.Now;
            var localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, GetConfigurationSetting("TimeZone"));
            return localTime;
        }

        public static bool UseSsl()
        {
            return (GetConfigurationSetting("ForceSSL").ToLower() == "true");
        }
        #endregion
    }
}