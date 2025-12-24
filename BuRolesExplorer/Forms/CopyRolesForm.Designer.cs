namespace Fic.XTB.FlowExecutionHistory.Forms
{
    partial class CopyRolesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbUsers = new System.Windows.Forms.ComboBox();
            this.gbRoles = new System.Windows.Forms.GroupBox();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.controlButtons1 = new Microsoft.Xrm.Tooling.Ui.Styles.controlButtons();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.RoleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoleBu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSourceUser = new System.Windows.Forms.TextBox();
            this.gbRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy From";
            // 
            // cbUsers
            // 
            this.cbUsers.FormattingEnabled = true;
            this.cbUsers.Location = new System.Drawing.Point(104, 66);
            this.cbUsers.Name = "cbUsers";
            this.cbUsers.Size = new System.Drawing.Size(684, 28);
            this.cbUsers.TabIndex = 1;
            this.cbUsers.SelectedIndexChanged += new System.EventHandler(this.cbUsers_SelectedIndexChanged);
            // 
            // gbRoles
            // 
            this.gbRoles.Controls.Add(this.dgvRoles);
            this.gbRoles.Controls.Add(this.elementHost1);
            this.gbRoles.Location = new System.Drawing.Point(16, 118);
            this.gbRoles.Name = "gbRoles";
            this.gbRoles.Size = new System.Drawing.Size(772, 350);
            this.gbRoles.TabIndex = 2;
            this.gbRoles.TabStop = false;
            this.gbRoles.Text = "Roles";
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(564, 37);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(200, 100);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.controlButtons1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(687, 474);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 40);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(580, 474);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(101, 40);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // dgvRoles
            // 
            this.dgvRoles.AllowUserToAddRows = false;
            this.dgvRoles.AllowUserToDeleteRows = false;
            this.dgvRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRoles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RoleName,
            this.RoleBu});
            this.dgvRoles.Location = new System.Drawing.Point(14, 25);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.ReadOnly = true;
            this.dgvRoles.RowHeadersVisible = false;
            this.dgvRoles.RowHeadersWidth = 62;
            this.dgvRoles.RowTemplate.Height = 28;
            this.dgvRoles.Size = new System.Drawing.Size(758, 319);
            this.dgvRoles.TabIndex = 1;
            // 
            // RoleName
            // 
            this.RoleName.HeaderText = "Name";
            this.RoleName.MinimumWidth = 8;
            this.RoleName.Name = "RoleName";
            this.RoleName.ReadOnly = true;
            // 
            // RoleBu
            // 
            this.RoleBu.HeaderText = "Business Unit";
            this.RoleBu.MinimumWidth = 8;
            this.RoleBu.Name = "RoleBu";
            this.RoleBu.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Copy To";
            // 
            // tbSourceUser
            // 
            this.tbSourceUser.Location = new System.Drawing.Point(104, 17);
            this.tbSourceUser.Name = "tbSourceUser";
            this.tbSourceUser.ReadOnly = true;
            this.tbSourceUser.Size = new System.Drawing.Size(684, 26);
            this.tbSourceUser.TabIndex = 8;
            // 
            // CopyRolesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 526);
            this.Controls.Add(this.tbSourceUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbRoles);
            this.Controls.Add(this.cbUsers);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyRolesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy roles";
            this.gbRoles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbUsers;
        private System.Windows.Forms.GroupBox gbRoles;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private Microsoft.Xrm.Tooling.Ui.Styles.controlButtons controlButtons1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoleBu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSourceUser;
    }
}