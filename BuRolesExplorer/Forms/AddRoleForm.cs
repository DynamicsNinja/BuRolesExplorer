using BuRolesExplorer.Proxy;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace BuRolesExplorer.Forms
{
    public partial class AddRoleForm : Form
    {
        private readonly BuRolesExplorer _bre;

        private const string RoleSearchPlaceholder = "Search roles...";
        private readonly Color _placeholderColor = Color.Gray;
        private readonly Color _textColor = Color.Black;

        private readonly Timer _searchDebounceTimer;

        public AddRoleForm(BuRolesExplorer bre)
        {
            InitializeComponent();

            _bre = bre ?? throw new ArgumentNullException(nameof(bre));

            _searchDebounceTimer = new Timer { Interval = 300 };
            _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;

            InitializeForm();
            InitializeSearchPlaceholder();
            LoadBusinessUnits();
        }

        private void InitializeForm()
        {
            var userName = _bre.SelectedUser?.GetAttributeValue<string>("fullname") ?? "Unknown User";
            Text = $"Add role for {userName}";

            cbBusinessUnits.DisplayMember = nameof(ComboBoxItem<Entity>.Text);
            cbBusinessUnits.ValueMember = nameof(ComboBoxItem<Entity>.Value);

            lbRoles.DisplayMember = nameof(ComboBoxItem<Entity>.Text);

            cbBusinessUnits.SelectedIndexChanged += cbBusinessUnits_SelectedIndexChanged;
            tbRoleSearch.TextChanged += tbRoleSearch_TextChanged;
        }

        #region Search Placeholder + Debounce

        private void InitializeSearchPlaceholder()
        {
            tbRoleSearch.Text = RoleSearchPlaceholder;
            tbRoleSearch.ForeColor = _placeholderColor;

            tbRoleSearch.Enter += tbRoleSearch_Enter;
            tbRoleSearch.Leave += tbRoleSearch_Leave;
        }

        private void tbRoleSearch_Enter(object sender, EventArgs e)
        {
            if (tbRoleSearch.Text == RoleSearchPlaceholder)
            {
                tbRoleSearch.Text = string.Empty;
                tbRoleSearch.ForeColor = _textColor;
            }
        }

        private void tbRoleSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbRoleSearch.Text))
            {
                tbRoleSearch.Text = RoleSearchPlaceholder;
                tbRoleSearch.ForeColor = _placeholderColor;
            }
        }

        private string GetSearchText()
        {
            return tbRoleSearch.Text == RoleSearchPlaceholder
                ? string.Empty
                : tbRoleSearch.Text.Trim();
        }

        private void tbRoleSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbRoleSearch.Text == RoleSearchPlaceholder)
            {
                return;
            }

            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        private void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();
            LoadRolesForSelectedBusinessUnit();
        }

        #endregion

        private void LoadBusinessUnits()
        {
            var buItems = _bre.BusinessUnits
                .Select(bu => new ComboBoxItem<Entity>(
                    bu.GetAttributeValue<string>("name"),
                    bu))
                .ToList();

            cbBusinessUnits.DataSource = buItems;

            if (!buItems.Any())
            {
                return;
            }

            cbBusinessUnits.SelectedIndex = 0;
            LoadRolesForSelectedBusinessUnit();
        }

        private void cbBusinessUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRolesForSelectedBusinessUnit();
        }

        private void LoadRolesForSelectedBusinessUnit()
        {
            var selectedBuItem = cbBusinessUnits.SelectedItem as ComboBoxItem<Entity>;
            if (selectedBuItem == null)
            {
                return;
            }

            var selectedBuId = selectedBuItem.Value.Id;
            var searchText = GetSearchText();

            lbRoles.DataSource = null;

            var roleItems = _bre.Roles
                .Where(r =>
                    r.GetAttributeValue<EntityReference>("businessunitid")?.Id == selectedBuId &&
                    _bre.UserRoles.All(ur => ur.Id != r.Id) &&
                    (string.IsNullOrEmpty(searchText) ||
                     r.GetAttributeValue<string>("name")
                         .IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                )
                .Select(r => new ComboBoxItem<Entity>(
                    r.GetAttributeValue<string>("name"),
                    r))
                .OrderBy(r => r.Text)
                .ToList();

            lbRoles.DataSource = roleItems;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var selectedRole = (lbRoles.SelectedItem as ComboBoxItem<Entity>)?.Value;
            var selectedUser = _bre.SelectedUser;

            if (selectedRole == null)
            {
                MessageBox.Show(
                    "Please select a role to add.",
                    "No Role Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bre.AddRole(
                    selectedUser.ToEntityReference(),
                    selectedRole.ToEntityReference());

                MessageBox.Show(
                    $"Role '{selectedRole.GetAttributeValue<string>("name")}' has been assigned to user '{selectedUser.GetAttributeValue<string>("fullname")}'.",
                    "Role Assigned",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while assigning the role:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
