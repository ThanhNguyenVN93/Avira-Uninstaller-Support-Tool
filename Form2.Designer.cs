namespace frmaviraunin
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.groupBoxHeader = new System.Windows.Forms.GroupBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBoxRegistry = new System.Windows.Forms.GroupBox();
            this.buttonCleanRegistry = new System.Windows.Forms.Button();
            this.listBoxRegistry = new System.Windows.Forms.ListBox();
            this.groupBoxFolders = new System.Windows.Forms.GroupBox();
            this.buttonCleanFolders = new System.Windows.Forms.Button();
            this.listBoxFolders = new System.Windows.Forms.ListBox();
            this.buttonCleanAll = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonDonate = new System.Windows.Forms.Button();
            this.groupBoxBottom = new System.Windows.Forms.GroupBox();
            this.groupBoxHeader.SuspendLayout();
            this.groupBoxRegistry.SuspendLayout();
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
            this.groupBoxHeader.Size = new System.Drawing.Size(700, 60);
            this.groupBoxHeader.TabIndex = 0;
            this.groupBoxHeader.TabStop = false;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(676, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "🔍 AVIRA RESIDUAL FILES & REGISTRY CLEANER";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxRegistry
            // 
            this.groupBoxRegistry.Controls.Add(this.buttonCleanRegistry);
            this.groupBoxRegistry.Controls.Add(this.listBoxRegistry);
            this.groupBoxRegistry.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRegistry.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBoxRegistry.Location = new System.Drawing.Point(0, 60);
            this.groupBoxRegistry.Name = "groupBoxRegistry";
            this.groupBoxRegistry.Padding = new System.Windows.Forms.Padding(12);
            this.groupBoxRegistry.Size = new System.Drawing.Size(700, 200);
            this.groupBoxRegistry.TabIndex = 1;
            this.groupBoxRegistry.TabStop = false;
            this.groupBoxRegistry.Text = "📋 Registry Entries";
            // 
            // buttonCleanRegistry
            // 
            this.buttonCleanRegistry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCleanRegistry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.buttonCleanRegistry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCleanRegistry.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonCleanRegistry.ForeColor = System.Drawing.Color.Black;
            this.buttonCleanRegistry.Location = new System.Drawing.Point(598, 24);
            this.buttonCleanRegistry.Name = "buttonCleanRegistry";
            this.buttonCleanRegistry.Size = new System.Drawing.Size(90, 164);
            this.buttonCleanRegistry.TabIndex = 1;
            this.buttonCleanRegistry.Text = "🗑️\r\nClean\r\nRegistry";
            this.buttonCleanRegistry.UseVisualStyleBackColor = false;
            // 
            // listBoxRegistry
            // 
            this.listBoxRegistry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxRegistry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.listBoxRegistry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxRegistry.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listBoxRegistry.ItemHeight = 15;
            this.listBoxRegistry.Location = new System.Drawing.Point(12, 24);
            this.listBoxRegistry.Name = "listBoxRegistry";
            this.listBoxRegistry.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxRegistry.Size = new System.Drawing.Size(580, 152);
            this.listBoxRegistry.TabIndex = 0;
            // 
            // groupBoxFolders
            // 
            this.groupBoxFolders.Controls.Add(this.buttonCleanFolders);
            this.groupBoxFolders.Controls.Add(this.listBoxFolders);
            this.groupBoxFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFolders.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBoxFolders.Location = new System.Drawing.Point(0, 260);
            this.groupBoxFolders.Name = "groupBoxFolders";
            this.groupBoxFolders.Padding = new System.Windows.Forms.Padding(12);
            this.groupBoxFolders.Size = new System.Drawing.Size(700, 262);
            this.groupBoxFolders.TabIndex = 2;
            this.groupBoxFolders.TabStop = false;
            this.groupBoxFolders.Text = "📁 Residual Folders";
            // 
            // buttonCleanFolders
            // 
            this.buttonCleanFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCleanFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.buttonCleanFolders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCleanFolders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonCleanFolders.ForeColor = System.Drawing.Color.White;
            this.buttonCleanFolders.Location = new System.Drawing.Point(598, 24);
            this.buttonCleanFolders.Name = "buttonCleanFolders";
            this.buttonCleanFolders.Size = new System.Drawing.Size(90, 164);
            this.buttonCleanFolders.TabIndex = 1;
            this.buttonCleanFolders.Text = "🗑️\r\nClean\r\nFolders";
            this.buttonCleanFolders.UseVisualStyleBackColor = false;
            // 
            // listBoxFolders
            // 
            this.listBoxFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.listBoxFolders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxFolders.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listBoxFolders.ItemHeight = 15;
            this.listBoxFolders.Location = new System.Drawing.Point(12, 24);
            this.listBoxFolders.Name = "listBoxFolders";
            this.listBoxFolders.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxFolders.Size = new System.Drawing.Size(580, 197);
            this.listBoxFolders.TabIndex = 0;
            // 
            // buttonCleanAll
            // 
            this.buttonCleanAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCleanAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.buttonCleanAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCleanAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonCleanAll.ForeColor = System.Drawing.Color.White;
            this.buttonCleanAll.Location = new System.Drawing.Point(12, 22);
            this.buttonCleanAll.Name = "buttonCleanAll";
            this.buttonCleanAll.Size = new System.Drawing.Size(150, 35);
            this.buttonCleanAll.TabIndex = 3;
            this.buttonCleanAll.Text = "🗑️ Clean All";
            this.buttonCleanAll.UseVisualStyleBackColor = false;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(588, 22);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 35);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "✕ Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            // 
            // buttonDonate
            // 
            this.buttonDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDonate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(94)))), ((int)(((byte)(91)))));
            this.buttonDonate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDonate.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.buttonDonate.ForeColor = System.Drawing.Color.White;
            this.buttonDonate.Location = new System.Drawing.Point(295, 22);
            this.buttonDonate.Name = "buttonDonate";
            this.buttonDonate.Size = new System.Drawing.Size(110, 35);
            this.buttonDonate.TabIndex = 5;
            this.buttonDonate.Text = "☕ Donate";
            this.buttonDonate.UseVisualStyleBackColor = false;
            // 
            // groupBoxBottom
            // 
            this.groupBoxBottom.Controls.Add(this.buttonCleanAll);
            this.groupBoxBottom.Controls.Add(this.buttonDonate);
            this.groupBoxBottom.Controls.Add(this.buttonClose);
            this.groupBoxBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxBottom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBoxBottom.Location = new System.Drawing.Point(0, 487);
            this.groupBoxBottom.Name = "groupBoxBottom";
            this.groupBoxBottom.Padding = new System.Windows.Forms.Padding(12);
            this.groupBoxBottom.Size = new System.Drawing.Size(700, 65);
            this.groupBoxBottom.TabIndex = 5;
            this.groupBoxBottom.TabStop = false;
            this.groupBoxBottom.Text = "Actions";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(700, 552);
            this.Controls.Add(this.groupBoxFolders);
            this.Controls.Add(this.groupBoxRegistry);
            this.Controls.Add(this.groupBoxHeader);
            this.Controls.Add(this.groupBoxBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Avira Residual Cleaner";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBoxHeader.ResumeLayout(false);
            this.groupBoxRegistry.ResumeLayout(false);
            this.groupBoxFolders.ResumeLayout(false);
            this.groupBoxBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxHeader;
        private System.Windows.Forms.GroupBox groupBoxRegistry;
        private System.Windows.Forms.GroupBox groupBoxFolders;
        private System.Windows.Forms.ListBox listBoxRegistry;
        private System.Windows.Forms.ListBox listBoxFolders;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonCleanRegistry;
        private System.Windows.Forms.Button buttonCleanFolders;
        private System.Windows.Forms.Button buttonCleanAll;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonDonate;
        private System.Windows.Forms.GroupBox groupBoxBottom;
    }
}
