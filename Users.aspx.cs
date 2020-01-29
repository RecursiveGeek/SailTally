using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;

namespace SailTally
{
    public partial class Users : Page
    {
        // How To: Use Membership in ASP.NET 2.0
        // http://msdn.microsoft.com/en-us/library/ff648345.aspx

        #region Methods
        private void ReloadPage()
        {
            // This is done to deal with the dynamic creation of events in the table (Link controls)
            Server.Transfer("~/Users.aspx");
        }

        private void ShowSummary(bool showSummary)
        {
            tableUsers.Visible = showSummary;
            linkAdd.Visible = showSummary;

            panelAdd.Visible = false;
            panelPassword.Visible = false;
            panelEdit.Visible = false;
            panelDelete.Visible = false;

            if (showSummary)
            {
                ShowUsers();
            }
        }

        private void ShowUsers()
        {
            tableUsers.Rows.Clear();

            var ctx = new SailTallyDataContext();
            var membershipUsers = from mu in ctx.aspnet_Memberships
                                  join u in ctx.aspnet_Users on mu.UserId equals u.UserId
                                  orderby u.UserName
                                  select new { u.UserName, mu.Email, u.UserId, mu.IsLockedOut, mu.LastLoginDate, mu.LastLockoutDate, mu.LastPasswordChangedDate, mu.CreateDate };

            var row = new TableRow();
            row.Cells.Add(new TableHeaderCell { Text = "" });
            row.Cells.Add(new TableHeaderCell { Text = "Login", HorizontalAlign = HorizontalAlign.Left });
            row.Cells.Add(new TableHeaderCell { Text = "Email", HorizontalAlign = HorizontalAlign.Left });
            //row.Cells.Add(new TableHeaderCell { Text = "ID", HorizontalAlign = HorizontalAlign.Left });
            row.Cells.Add(new TableHeaderCell { Text = "Locked Out" });
            row.Cells.Add(new TableHeaderCell { Text = "Last Login", HorizontalAlign=HorizontalAlign.Left });
            row.Cells.Add(new TableHeaderCell { Text = "Password Changed", HorizontalAlign = HorizontalAlign.Left });
            row.Cells.Add(new TableHeaderCell { Text = "Created", HorizontalAlign = HorizontalAlign.Left });
            tableUsers.Rows.Add(row);

            foreach (var membershipUser in membershipUsers)
            {
                row = new TableRow();

                var cell = new TableCell();
                var link = new LinkButton {Text = "Password", CommandArgument = membershipUser.UserName};

                link.Click += linkPassword_Click;
                cell.Controls.Add(link);

                cell.Controls.Add(new Label { Text = "&nbsp;" });

                link = new LinkButton {Text = "Edit", CommandArgument = membershipUser.UserName};
                link.Click += linkEdit_Click;
                cell.Controls.Add(link);

                cell.Controls.Add(new Label { Text = "&nbsp;" });

                var user = Membership.GetUser();
                if (user != null && !String.Equals(membershipUser.UserName, user.UserName, StringComparison.CurrentCultureIgnoreCase))
                {
                    link = new LinkButton {Text = "Delete", CommandArgument = membershipUser.UserName};
                    link.Click += linkDelete_Click;
                    cell.Controls.Add(link);
                }

                cell.Controls.Add(new Label { Text = "&nbsp;&nbsp;" });

                row.Cells.Add(cell);

                row.Cells.Add(new TableCell { Text = membershipUser.UserName, HorizontalAlign = HorizontalAlign.Left });
                row.Cells.Add(new TableCell { Text = membershipUser.Email, HorizontalAlign = HorizontalAlign.Left });
                //row.Cells.Add(new TableCell { Text = membershipUser.UserId.ToString() });

                var checkLockedOut = new CheckBox { Checked = membershipUser.IsLockedOut, Enabled = false };
                cell = new TableCell();
                cell.Controls.Add(checkLockedOut);
                cell.HorizontalAlign = HorizontalAlign.Center;
                row.Cells.Add(cell);

                row.Cells.Add(new TableCell { Text = membershipUser.LastLoginDate.ToString(CultureInfo.InvariantCulture), HorizontalAlign = HorizontalAlign.Left });
                row.Cells.Add(new TableCell { Text = membershipUser.LastPasswordChangedDate.ToString(CultureInfo.InvariantCulture), HorizontalAlign = HorizontalAlign.Left });
                row.Cells.Add(new TableCell { Text = membershipUser.CreateDate.ToString(CultureInfo.InvariantCulture), HorizontalAlign = HorizontalAlign.Left });
                
                tableUsers.Rows.Add(row);
            }
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowSummary(true);
            }
            else
            {
                ShowUsers();
            }         
        }

        protected void linkAdd_Click(object sender, EventArgs e)
        {
            ShowSummary(false);
            panelAdd.Visible = true;
        }

        protected void linkPassword_Click(object sender, EventArgs e)
        {
            var linkPassword = (LinkButton)sender;
            labelErrorPassword.Visible = false;
            textUsername.Text = linkPassword.CommandArgument;

            ShowSummary(false);
            panelPassword.Visible = true;
        }

        protected void linkEdit_Click(object sender, EventArgs e)
        {
            var linkEdit = (LinkButton)sender;
            var mu = Membership.GetUser(linkEdit.CommandArgument);
            textEditUser.Text = linkEdit.CommandArgument;
            checkEditUnlockUser.Checked = false;
            checkEditUnlockUser.Enabled = false;

            if (mu != null)
            {
                textEditEmail.Text = mu.Email;
                var isLockedOut = mu.IsLockedOut;
                checkEditUnlockUser.Enabled = isLockedOut;
            }

            ShowSummary(false);
            panelEdit.Visible = true;
        }

        protected void linkDelete_Click(object sender, EventArgs e)
        {
            var linkDelete = (LinkButton)sender;
            textDeleteUser.Text = linkDelete.CommandArgument;

            var mu = Membership.GetUser(linkDelete.CommandArgument);
            if (mu != null) textDeleteEmail.Text = mu.Email;

            ShowSummary(false);
            panelDelete.Visible = true;
        }

        protected void buttonAdd_Click(object sender, EventArgs e)
        {
            Membership.CreateUser(textAddUser.Text, textAddPassword.Text, textAddEmail.Text);

            ReloadPage();
        }

        protected void buttonAddCancel_Click(object sender, EventArgs e)
        {
            ShowSummary(true);
        }

        protected void buttonPassword_Click(object sender, EventArgs e)
        {
            if (textPassword.Text != textPasswordConfirm.Text)
            {
                labelErrorPassword.Text = "Passwords do not match.";
                labelErrorPassword.Visible = true;
                return;
            }

            var mu = Membership.GetUser(textUsername.Text);
            if (mu == null)
            {
                labelErrorPassword.Text = "Unable to Get User Information";
                labelErrorPassword.Visible = true;
                return;
            }

            if (mu.IsLockedOut)
            {
                labelErrorPassword.Text = "Account is locked out.";
                labelErrorPassword.Visible = true;
                return;
            }
            mu.ChangePassword(mu.ResetPassword(), textPassword.Text);
            ReloadPage();
        }

        protected void buttonEdit_Click(object sender, EventArgs e)
        {
            var mu = Membership.GetUser(textEditUser.Text);
            if (mu != null)
            {
                mu.Email = textEditEmail.Text;
                if (checkEditUnlockUser.Checked)
                {
                    mu.UnlockUser();
                }
                Membership.UpdateUser(mu);
            }

            ReloadPage();
        }

        protected void buttonDelete_Click(object sender, EventArgs e)
        {
            Membership.DeleteUser(textDeleteUser.Text, true); // delete all information on the user

            ReloadPage();
        }

        // General Cancel (Shared)
        protected void buttonCancel_Click(object sender, EventArgs e)
        {
            ReloadPage();
        }
        #endregion
    }
}