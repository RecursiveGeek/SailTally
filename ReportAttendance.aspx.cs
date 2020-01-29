using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class ReportAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ButtonGenerate.Visible = true;
            TableResults.Visible = false;

        }

        protected void ButtonGenerate_Click(object sender, EventArgs e)
        {
            //ButtonGenerate.Visible = false;
            ButtonGenerate.Text = "Refresh";
            TableResults.Visible = true;
            TableResults.Rows.Clear();

            var ctx = new SailTallyDataContext();

            var attendanceByBoat = from results in ctx.SS_Results
                                   join seasons in ctx.SS_Seasons on results.SeasonID equals seasons.SeasonID
                                   join boats in ctx.SS_Boats on results.BoatID equals boats.BoatID
                                   join fleets in ctx.SS_Fleets on boats.FleetID equals fleets.FleetID
                                   join fleetSeries in ctx.SS_FleetSeries on fleets.FleetID equals fleetSeries.FleetID
                                   join series in ctx.SS_Series on fleetSeries.SeriesID equals series.SeriesID
                                   where (seasons.IsActive ?? false) && (!results.IsAbsent) && (fleetSeries.FleetSeriesID == results.FleetSeriesID)
                                   orderby fleets.FleetName, fleets.FleetID, series.SeriesName, series.SeriesID, seasons.SeasonName, seasons.SeasonID, results.RaceNumber, results.BoatID
                                   select new { fleets.FleetName, fleets.FleetID, series.SeriesName, series.SeriesID, seasons.SeasonName, seasons.SeasonID, results.FleetSeriesID, results.RaceNumber, results.BoatID, results.IsAbandoned };

            var attendanceByRace = attendanceByBoat
              .GroupBy(g => new { g.FleetName, g.FleetID, g.SeriesName, g.SeriesID, g.SeasonName, g.SeasonID, g.FleetSeriesID, g.RaceNumber, g.IsAbandoned })
              .Select(s => new { s.Key.FleetName, s.Key.FleetID, s.Key.SeriesName, s.Key.SeriesID, s.Key.SeasonName, s.Key.SeasonID, s.Key.FleetSeriesID, s.Key.RaceNumber, s.Key.IsAbandoned, BoatCount = s.Count(b => !b.IsAbandoned) });

            var attendanceStats = attendanceByRace
              .GroupBy(g => new { g.FleetName, g.FleetID, g.SeriesName, g.SeriesID, g.SeasonName, g.SeasonID, g.FleetSeriesID })
              .Select(s => new { s.Key.FleetName, s.Key.FleetID, s.Key.SeriesName, s.Key.SeriesID, s.Key.SeasonName, s.Key.SeasonID, s.Key.FleetSeriesID, MaxBoats = s.Where(a => !a.IsAbandoned).Max(b => b.BoatCount), MinBoats = s.Where(a => !a.IsAbandoned).Min(b => b.BoatCount), AverageBoats = s.Where(a => !a.IsAbandoned).Average(b => Convert.ToDouble(b.BoatCount)), RacesCompleted = s.Count(b => !b.IsAbandoned), RacesTotal = s.Count() });

            //var totalRaces = (from raceSeries in ctx.SS_RaceSeries
            //                  orderby raceSeries.FleetSeriesID
            //                  select new { raceSeries.FleetSeriesID })
            //                  .GroupBy(g => new { g.FleetSeriesID })
            //                  .Select(s => new { s.Key.FleetSeriesID, Races = s.Count() });

            var headerRow = new TableRow();
            headerRow.Cells.Add(new TableHeaderCell { Text = "Fleet" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Series" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Season" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Max Boats", CssClass = "ColumnNumeric" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Min Boats", CssClass = "ColumnNumeric" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Average Boats", CssClass = "ColumnNumeric" }) ;
            headerRow.Cells.Add(new TableHeaderCell { Text = "Total Races", CssClass = "ColumnNumeric" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Completed Races", CssClass = "ColumnNumeric" });
            headerRow.Cells.Add(new TableHeaderCell { Text = "Abandoned Races", CssClass = "ColumnNumeric" });
            TableResults.Rows.Add(headerRow);
            foreach (var stat in attendanceStats)
            {
                //var totalRaceCount = totalRaces.First(w => w.FleetSeriesID == stat.FleetSeriesID).Races;

                var row = new TableRow();
                row.Cells.Add(new TableCell { Text = stat.FleetName });
                row.Cells.Add(new TableCell { Text = stat.SeriesName });
                row.Cells.Add(new TableCell { Text = stat.SeasonName });
                row.Cells.Add(new TableCell { Text = stat.MaxBoats.ToString(CultureInfo.InvariantCulture), CssClass = "ColumnNumeric" });
                row.Cells.Add(new TableCell { Text = stat.MinBoats.ToString(CultureInfo.InvariantCulture), CssClass = "ColumnNumeric" });
                row.Cells.Add(new TableCell { Text = stat.AverageBoats.ToString("0.00"), CssClass = "ColumnNumeric" });
                row.Cells.Add(new TableCell { Text = stat.RacesTotal.ToString(CultureInfo.InvariantCulture), CssClass = "ColumnNumeric" });
                row.Cells.Add(new TableCell { Text = stat.RacesCompleted.ToString(CultureInfo.InvariantCulture), CssClass = "ColumnNumeric" });
                row.Cells.Add(new TableCell { Text = (stat.RacesTotal - stat.RacesCompleted).ToString(CultureInfo.InvariantCulture), CssClass = "ColumnNumeric" });
                TableResults.Rows.Add(row);
            }
        }
    }
}