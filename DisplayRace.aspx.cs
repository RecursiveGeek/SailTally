using System;
using System.Linq;
using System.Text;

namespace SailTally
{
    public partial class DisplayRace : System.Web.UI.Page
    {
        protected string Eol = "\r\n";

        protected string GetDisplayDate(DateTime? dtn, string ts = "")
        {
            var dt = dtn.GetValueOrDefault();
            return dt.ToLongDateString() + " " + (ts.Length == 0 ? dt.ToShortTimeString() : ts);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var ctx = new SailTallyDataContext();
            var raceId = -1;

            try
            {
                raceId = Convert.ToInt32(Request.QueryString["r"]);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // do nothing
            }

            var race = (from r in ctx.SS_Races where r.RaceID == raceId select r).Single();
            if (race!=null)
            {

                var timeStr = race.FirstWarningDate.ToShortTimeString();

                try
                {
                    var time = (from t in ctx.SS_Times where t.TimeStr == timeStr select t).Single();
                    if (time != null)
                    {
                        timeStr = time.DisplayTime;
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    // do nothing
                }

                sb.Append("<h2>General Information</h2>");
                sb.Append("<div class='table-responsive'>");
                sb.Append("  <table class='table table-striped'>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Race Warning:" + "</th><td class='RaceDetail'>" + GetDisplayDate(race.FirstWarningDate, timeStr) + "</td></tr>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Docked:" + "</th><td class='RaceDetail'>" + GetDisplayDate(race.DockedDate) + "</td></tr>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Primary Race Officer (PRO):" + "</th><td class='RaceDetail'>" + race.PRO + "</td></tr>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Race Officers:" + "</th><td class='RaceDetail'>" + race.AssistPRO + "</td></tr>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Helpers:" + "</th><td class='RaceDetail'>" + race.Helper + "</td></tr>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Wind Direction:" + "</th><td class='RaceDetail'>" + race.WindDirection + "</td></tr>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Wind Speed:" + "</th><td class='RaceDetail'>" + race.WindSpeed + " " + race.WindUnits + "</td></tr>" + Eol);
                if (!string.IsNullOrEmpty(race.CourseChange))
                {
                    sb.Append("    <tr><th class='RaceHeader'>Course Change:" + "</th><td class='RaceDetail'>" + race.CourseChange + "</td></tr>" + Eol);
                }
                sb.Append("    <tr><th class='RaceHeader'>Protests:" + "</th><td class='RaceDetail'>" + race.Protests.Replace(Eol, "<br/>") + "</td></tr>" + Eol);
                sb.Append("    <tr><th class='RaceHeader'>Comments:" + "</th><td class='RaceDetail'>" + race.Comments.Replace(Eol, "<br/>") + "</td></tr>" + Eol);
                sb.Append("  </table>" + Eol);
                sb.Append("</div>" + Eol);

                sb.Append("<h2>Fleets Scheduled</h2>");
                sb.Append("<div class='table-responsive'>");
                sb.Append("  <table class='table table-striped'>" + Eol);
                sb.Append("    <tr><th>Fleet</th><th>Series</th><th>Race #</th><th>Excludable</th></tr>" + Eol);
                var fleetsScheduled = (from rs in ctx.SS_RaceSeries
                    join fs in ctx.SS_FleetSeries on rs.FleetSeriesID equals fs.FleetSeriesID
                    join f in ctx.SS_Fleets on fs.FleetID equals f.FleetID
                    join s in ctx.SS_Series on fs.SeriesID equals s.SeriesID
                    where rs.RaceID == raceId
                    select new {rs.RaceNumber, rs.IsScoreExcludable, f.FleetName, s.SeriesName});

                foreach (var fleetsSchedule in fleetsScheduled)
                {
                    sb.Append("    <tr><td>" + fleetsSchedule.FleetName + "</td><td>" + fleetsSchedule.SeriesName + "</td><td>" + fleetsSchedule.RaceNumber + "</td><td> " + (fleetsSchedule.IsScoreExcludable ? "Yes" : "No") + "</td></tr>" + Eol);
                }

                sb.Append("  </table>" + Eol);
                sb.Append("</div>" + Eol);
            }
            labelDisplayRace.Text = sb.ToString();
        }
    }
}