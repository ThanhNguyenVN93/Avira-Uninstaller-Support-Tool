namespace frmaviraunin
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.groupBoxHeader = new System.Windows.Forms.GroupBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBoxFolders = new System.Windows.Forms.GroupBox();
            this.listViewFolders = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUninstaller = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxBottom = new System.Windows.Forms.GroupBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonUninstallSelected = new System.Windows.Forms.Button();
            this.buttonDeleteSelected = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonDonate = new System.Windows.Forms.Button();
            this.groupBoxHeader.SuspendLayout();
            this.groupBoxFolders.SuspendLayout();
            this.groupBoxBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxHeader
            // 
            this.groupBoxHeader.Controls.Add(this.labelTitle);
            this.groupBoxHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.groupBoxHeader.Location = new System.Drawing.Point(0, 0);
            this.groupBoxHeader.Name = "groupBoxHeader";
            this.groupBoxHeader.Padding = new System.Windows.Forms.Padding(12);
            this.groupBoxHeader.Size = new System.Drawing.Size(684, 60);
            this.groupBoxHeader.TabIndex = 0;
            this.groupBoxHeader.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(660, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "📂 AVIRA RESIDUAL FOLDER SCANNER";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxFolders
            // 
            this.groupBoxFolders.Controls.Add(this.listViewFolders);
            this.groupBoxFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFolders.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBoxFolders.Location = new System.Drawing.Point(0, 60);
            this.groupBoxFolders.Name = "groupBoxFolders";
            this.groupBoxFolders.Padding = new System.Windows.Forms.Padding(12);
            this.groupBoxFolders.Size = new System.Drawing.Size(684, 280);
            this.groupBoxFolders.TabIndex = 1;
            this.groupBoxFolders.TabStop = false;
            this.groupBoxFolders.Text = "Residual Folders Found";
            // 
            // listViewFolders
            // 
            this.listViewFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.listViewFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewFolders.CheckBoxes = true;
            this.listViewFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnPath,
            this.columnUninstaller});
            this.listViewFolders.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listViewFolders.FullRowSelect = true;
            this.listViewFolders.GridLines = true;
            this.listViewFolders.HideSelection = false;
            this.listViewFolders.Location = new System.Drawing.Point(12, 28);
            this.listViewFolders.Name = "listViewFolders";
            this.listViewFolders.Size = new System.Drawing.Size(660, 240);
            this.listViewFolders.TabIndex = 0;
            this.listViewFolders.UseCompatibleStateImageBehavior = false;
            this.listViewFolders.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            this.columnName.Text = "Folder Name";
            this.columnName.Width = 180;
            // 
            // columnPath
            // 
            this.columnPath.Text = "Full Path";
            this.columnPath.Width = 340;
            // 
            // columnUninstaller
            // 
            this.columnUninstaller.Text = "Uninstaller";
            this.columnUninstaller.Width = 120;
            // 
            // groupBoxBottom
            // 
            this.groupBoxBottom.Controls.Add(this.labelStatus);
            this.groupBoxBottom.Controls.Add(this.buttonUninstallSelected);
            this.groupBoxBottom.Controls.Add(this.buttonDeleteSelected);
            this.groupBoxBottom.Controls.Add(this.buttonClose);
            this.groupBoxBottom.Controls.Add(this.buttonDonate);
            this.groupBoxBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxBottom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBoxBottom.Location = new System.Drawing.Point(0, 340);
            this.groupBoxBottom.Name = "groupBoxBottom";
            this.groupBoxBottom.Padding = new System.Windows.Forms.Padding(12);
            this.groupBoxBottom.Size = new System.Drawing.Size(684, 82);
            this.groupBoxBottom.TabIndex = 2;
            this.groupBoxBottom.TabStop = false;
            this.groupBoxBottom.Text = "Actions";
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.labelStatus.Location = new System.Drawing.Point(12, 24);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(660, 18);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Scanning...";
            // 
            // buttonUninstallSelected
            // 
            this.buttonUninstallSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.buttonUninstallSelected.Enabled = false;
            this.buttonUninstallSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUninstallSelected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonUninstallSelected.ForeColor = System.Drawing.Color.White;
            this.buttonUninstallSelected.Location = new System.Drawing.Point(12, 45);
            this.buttonUninstallSelected.Name = "buttonUninstallSelected";
            this.buttonUninstallSelected.Size = new System.Drawing.Size(200, 30);
            this.buttonUninstallSelected.TabIndex = 1;
            this.buttonUninstallSelected.Text = "🔧 Run Uninstallers";
            this.buttonUninstallSelected.UseVisualStyleBackColor = false;
            this.buttonUninstallSelected.Click += new System.EventHandler(this.buttonUninstallSelected_Click);
            // 
            // buttonDeleteSelected
            // 
            this.buttonDeleteSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.buttonDeleteSelected.Enabled = false;
            this.buttonDeleteSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeleteSelected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonDeleteSelected.ForeColor = System.Drawing.Color.White;
            this.buttonDeleteSelected.Location = new System.Drawing.Point(218, 45);
            this.buttonDeleteSelected.Name = "buttonDeleteSelected";
            this.buttonDeleteSelected.Size = new System.Drawing.Size(200, 30);
            this.buttonDeleteSelected.TabIndex = 2;
            this.buttonDeleteSelected.Text = "🗑️ Delete No-Uninstaller";
            this.buttonDeleteSelected.UseVisualStyleBackColor = false;
            this.buttonDeleteSelected.Click += new System.EventHandler(this.buttonDeleteSelected_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(584, 45);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(88, 30);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "✕ Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonDonate
            // 
            this.buttonDonate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(94)))), ((int)(((byte)(91)))));
            this.buttonDonate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDonate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.buttonDonate.ForeColor = System.Drawing.Color.White;
            this.buttonDonate.Location = new System.Drawing.Point(424, 45);
            this.buttonDonate.Name = "buttonDonate";
            this.buttonDonate.Size = new System.Drawing.Size(100, 30);
            this.buttonDonate.TabIndex = 4;
            this.buttonDonate.Text = "☕ Donate";
            this.buttonDonate.UseVisualStyleBackColor = false;
            this.buttonDonate.Click += new System.EventHandler(this.buttonDonate_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(684, 422);
            this.Controls.Add(this.groupBoxFolders);
            this.Controls.Add(this.groupBoxBottom);
            this.Controls.Add(this.groupBoxHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Avira Residual Folder Scanner";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.groupBoxHeader.ResumeLayout(false);
            this.groupBoxFolders.ResumeLayout(false);
            this.groupBoxBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxHeader;
        private System.Windows.Forms.GroupBox groupBoxFolders;
        private System.Windows.Forms.GroupBox groupBoxBottom;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ListView listViewFolders;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnPath;
        private System.Windows.Forms.ColumnHeader columnUninstaller;
        private System.Windows.Forms.Button buttonUninstallSelected;
        private System.Windows.Forms.Button buttonDeleteSelected;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonDonate;
    }
}
