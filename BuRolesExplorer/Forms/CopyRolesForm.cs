using System.Linq;
using System.Windows.Forms;
using BuRolesExplorer.Proxy;
using Microsoft.Xrm.Sdk;

namespace BuRolesExplorer.Forms
{
    public partial class CopyRolesForm : Form
    {
        private BuRolesExplorer _bre;
        private Entity _targetUser;
        private Entity _sourceUser;

        public CopyRolesForm(BuRolesExplorer bre)
        {
            InitializeComponent();

            _bre = bre;
            _sourceUser = bre.SelectedUser;

            tbSourceUser.Text = _sourceUser.GetAttributeValue<string>("fullname");

            foreach (var user in bre.Users)
            {
                if (user.Id == bre.SelectedUser.Id)
                {
                    continue;
                }

                var userName = user.GetAttributeValue<string>("fullname");
                cbUsers.Items.Add(new ComboBoxItem<Entity>(userName, user));
            }

            if (cbUsers.Items.Count > 0)
            {
                cbUsers.SelectedIndex = 0;
                _targetUser = ((ComboBoxItem<Entity>)cbUsers.SelectedItem).Value;
            }

            // populate dgvRoles with selected user's roles
            foreach (var role in bre.UserRoles)
            {
                var roleName = role.GetAttributeValue<string>("name");
                var roleBuName = role.GetAttributeValue<EntityReference>("businessunitid").Name;

                dgvRoles.Rows.Add(roleName, roleBuName);
            }

            gbRoles.Text = $"Roles ({bre.UserRoles.Count})";
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void btnCopy_Click(object sender, System.EventArgs e)
        {
            var sourceUserName = _sourceUser.GetAttributeValue<string>("fullname");
            var targetUserName = _targetUser.GetAttributeValue<string>("fullname");

            var result = MessageBox.Show(
                $"You are about to COPY ALL SECURITY ROLES\n\n" +
                $"FROM user:\n  {sourceUserName}\n\n" +
                $"TO user:\n  {targetUserName}\n\n" +
                $"Do you want to continue?",
                "Confirm Copy Roles",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No) { return; }

            _bre.AddRoles(
                _targetUser.ToEntityReference(),
                _bre.UserRoles.Select(r => r.ToEntityReference()).ToList()
            );

            MessageBox.Show(
                $"Roles have been copied from '{sourceUserName}' to '{targetUserName}'.",
                "Roles Copied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            Close();
        }

        private void cbUsers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cbUsers.SelectedItem == null) { return; }

            _targetUser = ((ComboBoxItem<Entity>)cbUsers.SelectedItem).Value;
        }
    }
}
