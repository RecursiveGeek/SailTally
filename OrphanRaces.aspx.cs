using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class OrphanRaces : Page
    {
        #region Enum
        protected enum CellType
        {
            Header,
            Normal,
            Delete
        };
        #endregion

        #region Methods
        protected void AddColumn(TableRow row, string display, CellType cellType)
        {
            AddColumn(row, display, cellType, -1);
        }

        protected void AddColumn(TableRow row, string display, CellType cellType, int raceId)
        {
            var cell = new TableCell();

            if (cellType == CellType.Delete)
            {
                var link = new LinkButton
                {
                    Text = display,
                    CommandArgument = raceId.ToString()
                };
                link.Click+=link_Click;

                cell.Controls.Add(link);
            }
            else
            {
                var label = new Label { Text = display };
                if (cellType == CellType.Header)
                {
                    label.Font.Bold = true;
                }
                cell.Controls.Add(label);
            }
            row.Cells.Add(cell);
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            var row = new TableRow();
            AddColumn(row, "Action", CellType.Header);
            AddColumn(row, "Race ID", CellType.Header);
            AddColumn(row, "First Warning", CellType.Header);
            AddColumn(row, "Scored Races", CellType.Header);
            tableOrphans.Rows.Add(row);

            var ctx=new SailTallyDataContext();

            // Sub Query
            var raceSeries = (from rs in ctx.SS_RaceSeries
                              orderby rs.RaceID
                              select rs.RaceID).Distinct();
            // Main Query
            var races = from r in ctx.SS_Races
                        where !raceSeries.Contains(r.RaceID)
                        select r;

            foreach (var race in races)
            {
                row = new TableRow();
                AddColumn(row, "Delete", CellType.Delete, race.RaceID);
                AddColumn(row, race.RaceID.ToString(), CellType.Normal);
                AddColumn(row, CentralLibrary.FormatDisplayTime(race.FirstWarningDate), CellType.Normal);
                AddColumn(row, (CentralLibrary.IsRacesScored(race.RaceID) ? "Yes" : "No"), CellType.Normal);
                tableOrphans.Rows.Add(row);
            }
        }

        protected void link_Click(object sender, EventArgs e)
        {
            var link = (LinkButton)sender;
            var raceId = Convert.ToInt32(link.CommandArgument);
            labelError.Text = "";

            try
            {
                CentralLibrary.RemoveScores(raceId);

                // Remove Races
                var ctx = new SailTallyDataContext();
                var race = (from r in ctx.SS_Races
                            where r.RaceID == raceId
                            select r).Single();
                ctx.SS_Races.DeleteOnSubmit(race);
                ctx.SubmitChanges();
                Server.Transfer("~/OrphanRaces.aspx");
            }
            catch (Exception ex)
            {
                labelError.Text = "Unable to remove Orphan Race (Race ID: " + raceId.ToString() + ").  Reason:" + ex.Message;
            }
        }
        #endregion
    }
}