using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BuRolesExplorer.Forms;
using Microsoft.Xrm.Sdk.Messages;
using XrmToolBox.Extensibility;

namespace BuRolesExplorer
{
    public partial class BuRolesExplorer : PluginControlBase
    {
        private Settings mySettings;

        private readonly Timer _searchTimer = new Timer { Interval = 300 };

        public List<Entity> Users;
        public List<Entity> FilteredUsers;
        public List<Entity> BusinessUnits;
        public List<Entity> Roles;

        public Entity SelectedUser;
        public Entity SelectedRole;

        public List<Entity> UserRoles;

        public BuRolesExplorer()
        {
            InitializeComponent();

            _searchTimer.Tick += SearchTimer_Tick;
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
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
            ExecuteMethod(LoadBusinessUnits);
            ExecuteMethod(LoadRoles);
        }

        private void LoadRoles()
        {
            WorkAsync(new WorkAsyncInfo("Loading security roles...",
                (eventargs) =>
                {
                    var qe = new QueryExpression("role");
                    qe.ColumnSet = new ColumnSet("name", "businessunitid");
                    qe.AddOrder("name", OrderType.Ascending);

                    eventargs.Result = Service.RetrieveMultiple(qe).Entities.ToList();
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
                        Roles = completedargs.Result as List<Entity>;
                    }
                }
            });
        }


        private void LoadBusinessUnits()
        {
            WorkAsync(new WorkAsyncInfo("Loading business units...",
                (eventargs) =>
                {
                    var qe = new QueryExpression("businessunit");
                    qe.ColumnSet = new ColumnSet("name");
                    qe.AddOrder("name", OrderType.Ascending);

                    eventargs.Result = Service.RetrieveMultiple(qe).Entities.ToList();
                    BusinessUnits = eventargs.Result as List<Entity>;
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
                        BusinessUnits = completedargs.Result as List<Entity>;
                    }
                }
            });
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
            var selectedIndex = lbUsers.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= Users.Count) { return; }

            SelectedUser = FilteredUsers[selectedIndex];

            tsbAddRole.Enabled = true;

            ExecuteMethod(LoadUserRoles);
        }

        private void RemoveRole(EntityReference userRef, EntityReference roleRef)
        {
            WorkAsync(new WorkAsyncInfo("Removing security role...",
                (eventargs) =>
                {
                    var disassociateRequest = new DisassociateRequest
                    {
                        Target = userRef,
                        RelatedEntities = new EntityReferenceCollection { roleRef },
                        Relationship = new Relationship("systemuserroles_association")
                    };

                    var disassociateResponse = Service.Execute(disassociateRequest) as DisassociateResponse;

                    eventargs.Result = disassociateResponse;
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
                        // Reload user roles
                        ExecuteMethod(LoadUserRoles);
                    }
                }
            });
        }

        public void AddRole(EntityReference userRef, EntityReference roleRef)
        {
            WorkAsync(new WorkAsyncInfo("Adding security role...",
                (eventargs) =>
                {
                    var associateRequest = new AssociateRequest
                    {
                        Target = userRef,
                        RelatedEntities = new EntityReferenceCollection { roleRef },
                        Relationship = new Relationship("systemuserroles_association")
                    };

                    var associateResponse = Service.Execute(associateRequest) as AssociateResponse;

                    eventargs.Result = associateResponse;
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
                        ExecuteMethod(LoadUserRoles);
                    }
                }
            });
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
                            SelectedRole = roles[0];
                            tsbDeleteRole.Enabled = true;
                        }

                        UserRoles = roles;
                    }
                }
            });
        }

        private void dgvUserRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // select whole row on cell click
            if (e.RowIndex < 0) { return; }

            dgvUserRoles.Rows[e.RowIndex].Selected = true;

            SelectedRole = UserRoles[e.RowIndex];
            tsbDeleteRole.Enabled = true;
        }

        private void tsbDeleteRole_Click(object sender, EventArgs e)
        {
            // prompt for confirmation
            if (SelectedUser == null || SelectedRole == null)
            {
                MessageBox.Show("Please select a user and a role to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var userName = SelectedUser.GetAttributeValue<string>("fullname");
            var roleName = SelectedRole.GetAttributeValue<string>("name");
            var buName = SelectedRole.GetAttributeValue<EntityReference>("businessunitid")?.Name ?? "N/A";

            var result = MessageBox.Show(
                $"Are you sure you want to remove this role assignment?\n\n" +
                $"User: {userName}\n" +
                $"Role: {roleName}\n" +
                $"Business Unit: {buName}",
                "Confirm Role Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No) { return; }

            RemoveRole(SelectedUser.ToEntityReference(), SelectedRole.ToEntityReference());
        }

        private void tsbAddRole_Click(object sender, EventArgs e)
        {
            using (var addRoleForm = new AddRoleForm(this))
            {
                // Center relative to the parent form
                addRoleForm.StartPosition = FormStartPosition.CenterParent;

                var dialogResult = addRoleForm.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    ExecuteMethod(LoadUserRoles);
                }
            }
        }

    }
}