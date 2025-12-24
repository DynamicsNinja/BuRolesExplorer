namespace Fic.XTB.FlowExecutionHistory.Forms
{
    partial class AddRoleForm
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
            this.cbBusinessUnits = new System.Windows.Forms.ComboBox();
            this.lbRoles = new System.Windows.Forms.ListBox();
            this.tbRoleSearch = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbBusinessUnits
            // 
            this.cbBusinessUnits.FormattingEnabled = true;
            this.cbBusinessUnits.Location = new System.Drawing.Point(12, 12);
            this.cbBusinessUnits.Name = "cbBusinessUnits";
            this.cbBusinessUnits.Size = new System.Drawing.Size(776, 28);
            this.cbBusinessUnits.TabIndex = 0;
            this.cbBusinessUnits.SelectedIndexChanged += new System.EventHandler(this.cbBusinessUnits_SelectedIndexChanged);
            // 
            // lbRoles
            // 
            this.lbRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRoles.FormattingEnabled = true;
            this.lbRoles.ItemHeight = 20;
            this.lbRoles.Location = new System.Drawing.Point(12, 86);
            this.lbRoles.Name = "lbRoles";
            this.lbRoles.Size = new System.Drawing.Size(776, 364);
            this.lbRoles.TabIndex = 1;
            // 
            // tbRoleSearch
            // 
            this.tbRoleSearch.Location = new System.Drawing.Point(12, 46);
            this.tbRoleSearch.Name = "tbRoleSearch";
            this.tbRoleSearch.Size = new System.Drawing.Size(776, 26);
            this.tbRoleSearch.TabIndex = 2;
            this.tbRoleSearch.TextChanged += new System.EventHandler(this.tbRoleSearch_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(687, 456);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(580, 456);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(101, 40);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // AddRoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 509);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbRoleSearch);
            this.Controls.Add(this.lbRoles);
            this.Controls.Add(this.cbBusinessUnits);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddRoleForm";
            this.Text = "Add Role";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbBusinessUnits;
        private System.Windows.Forms.ListBox lbRoles;
        private System.Windows.Forms.TextBox tbRoleSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
    }
}