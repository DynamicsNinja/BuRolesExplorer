using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace BuRolesExplorer
{
    public partial class BuRolesExplorer : PluginControlBase
    {
        private Settings mySettings;

        private readonly Timer _searchTimer = new Timer { Interval = 300 };

        public List<Entity> Users;
        public List<Entity> FilteredUsers;
        public Entity SelectedUser;

        public List<Entity> UserRoles;

        public BuRolesExplorer()
        {
            InitializeComponent();

            _searchTimer.Tick += SearchTimer_Tick;
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings == null || detail == null) { return; }
            mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
            LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
        }

        private void BuRolesExplorer_ConnectionUpdated(object sender, ConnectionUpdatedEventArgs e)
        {
            ExecuteMethod(LoadUsers);
        }

        private void LoadUsers()
        {
            WorkAsync(new WorkAsyncInfo("Loading users...",
                (eventargs) =>
                {
                    var qe = new QueryExpression("systemuser");
                    qe.ColumnSet = new ColumnSet("fullname");
                    qe.AddOrder("fullname", OrderType.Ascending);

                    eventargs.Result = Service.RetrieveMultiple(qe).Entities.ToList();
                    Users = eventargs.Result as List<Entity>;
                    FilteredUsers = new List<Entity>(Users);
                })
            {
                PostWorkCallBack = (completedargs) =>
                {
                    if (completedargs.Error != null)
                    {
                        ShowErrorDialog(completedargs.Error);
                    }
                    else
                    {
                        var entities = completedargs.Result as List<Entity>;

                        lbUsers.Items.Clear();

                        if (entities == null) { return; }

                        foreach (var entity in entities)
                        {
                            lbUsers.Items.Add(entity.GetAttributeValue<string>("fullname"));
                        }
                    }
                }
            });
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();

            var searchText = tbUserSearch.Text.Trim().ToLowerInvariant();

            FilteredUsers = string.IsNullOrEmpty(searchText)
                ? Users
                : Users.Where(u =>
                    u.GetAttributeValue<string>("fullname")?
                        .ToLowerInvariant()
                        .Contains(searchText) == true
                ).ToList();

            lbUsers.BeginUpdate();
            lbUsers.Items.Clear();

            foreach (var user in FilteredUsers)
            {
                lbUsers.Items.Add(user.GetAttributeValue<string>("fullname"));
            }

            lbUsers.EndUpdate();
        }


        private void tbUserSearch_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get selected user
            var selectedIndex = lbUsers.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= Users.Count) { return; }

            SelectedUser = FilteredUsers[selectedIndex];

            ExecuteMethod(LoadUserRoles);
        }

        private void LoadUserRoles()
        {
            WorkAsync(new WorkAsyncInfo("Loading roles...",
                (eventargs) =>
                {
                    var fetch = $@"
                    <fetch>
                      <entity name='role'>
                        <attribute name='businessunitid' />
                        <attribute name='name' />
                        <link-entity name='systemuserroles' from='roleid' to='roleid' alias='S' intersect='true'>
                          <filter>
                            <condition attribute='systemuserid' operator='eq' value='{SelectedUser.Id:D}' />
                          </filter>
                        </link-entity>
                      </entity>
                    </fetch>";

                    eventargs.Result = Service.RetrieveMultiple(new FetchExpression(fetch)).Entities.ToList();
                })
            {
                PostWorkCallBack = (completedargs) =>
                {
                    if (completedargs.Error != null)
                    {
                        ShowErrorDialog(completedargs.Error);
                    }
                    else
                    {
                        var roles = completedargs.Result as List<Entity>;

                        if (roles == null)
                        {
                            return;
                        }

                        // populate dgvUserRoles
                        dgvUserRoles.Rows.Clear();
                        foreach (var role in roles)
                        {
                            var roleName = role.GetAttributeValue<string>("name");
                            var businessUnit = role.GetAttributeValue<EntityReference>("businessunitid")?.Name ?? "N/A";
                            dgvUserRoles.Rows.Add(roleName, businessUnit);
                        }

                        if (dgvUserRoles.Rows.Count > 0)
                        {
                            dgvUserRoles.Rows[0].Selected = true;
                        }

                        UserRoles = roles;
                    }
                }
            });
        }

        private void dgvUserRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // select whole row on cell click
            if (e.RowIndex >= 0)
            {
                dgvUserRoles.Rows[e.RowIndex].Selected = true;
            }
        }
    }
}