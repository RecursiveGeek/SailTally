using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SailTally
{
    public partial class Log : Page
    {
        #region Methods

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            tableLog.Visible = false;
        }

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            tableLog.Rows.Clear();
            tableLog.Visible = true;

            var ctx = new SailTallyDataContext();
            var log = from l in ctx.SS_Logs
                      select l;

            if (textUser.Text.Length > 0)
            {
                log = log.Where(p => p.Username == textUser.Text);
            }

            if (textDateStart.Text.Length > 0)
            {
                log = log.Where(p => p.LogDate >= Convert.ToDateTime(textDateStart.Text).Date);
            }

            if (textDateEnd.Text.Length > 0)
            {
                log = log.Where(p => p.LogDate <= Convert.ToDateTime(textDateEnd.Text).Date);
            }

            if (textEventID.Text.Length > 0)
            {
                log = log.Where(p => p.Event == Convert.ToInt32(textEventID.Text));
            }

            var logSort = log.OrderByDescending(p => p.LogDate);

            var row = new TableRow();

            row.Cells.Add(new TableHeaderCell { Text = "ID" });
            row.Cells.Add(new TableHeaderCell { Text = "Date" });
            row.Cells.Add(new TableHeaderCell { Text = "User" });
            row.Cells.Add(new TableHeaderCell { Text = "Event" });
            row.Cells.Add(new TableHeaderCell { Text = "Subject" });
            row.Cells.Add(new TableHeaderCell { Text = "Source" });
            row.Cells.Add(new TableHeaderCell { Text = "Operation" });
            row.Cells.Add(new TableHeaderCell { Text = "Reference" });
            row.Cells.Add(new TableHeaderCell { Text = "Details" });
            tableLog.Rows.Add(row);

            foreach (var logEntry in logSort)
            {
                row = new TableRow();
                row.Cells.Add(new TableCell { Text = logEntry.LogID.ToString() });
                row.Cells.Add(new TableCell { Text = logEntry.LogDate.ToString() });
                row.Cells.Add(new TableCell { Text = logEntry.Username });
                row.Cells.Add(new TableCell { Text = logEntry.Event.ToString() });
                row.Cells.Add(new TableCell { Text = logEntry.Subject });
                row.Cells.Add(new TableCell { Text = logEntry.Source });
                row.Cells.Add(new TableCell { Text = logEntry.Operation });
                row.Cells.Add(new TableCell { Text = logEntry.Reference });
                row.Cells.Add(new TableCell { Text = logEntry.Details });
                tableLog.Rows.Add(row);
            }
        }
        #endregion

    }
}