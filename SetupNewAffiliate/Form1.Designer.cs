namespace SetupNewAffiliate
{
    partial class Form1
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
            this.AffiliateId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AffiliateName = new System.Windows.Forms.TextBox();
            this.Msg = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panelWebFolder = new System.Windows.Forms.Panel();
            this.Save = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.bannerPort = new System.Windows.Forms.TextBox();
            this.userPort = new System.Windows.Forms.TextBox();
            this.goPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.adminPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.adminSiteId = new System.Windows.Forms.TextBox();
            this.goSiteId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.userSiteId = new System.Windows.Forms.TextBox();
            this.bannerSiteId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.panelWebFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "AffiliateId";
            // 
            // AffiliateId
            // 
            this.AffiliateId.Location = new System.Drawing.Point(94, 50);
            this.AffiliateId.Name = "AffiliateId";
            this.AffiliateId.Size = new System.Drawing.Size(100, 20);
            this.AffiliateId.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Affiliate Name";
            // 
            // AffiliateName
            // 
            this.AffiliateName.Location = new System.Drawing.Point(94, 86);
            this.AffiliateName.Name = "AffiliateName";
            this.AffiliateName.Size = new System.Drawing.Size(100, 20);
            this.AffiliateName.TabIndex = 3;
            // 
            // Msg
            // 
            this.Msg.AutoSize = true;
            this.Msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Strikeout))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Msg.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Msg.Location = new System.Drawing.Point(221, 57);
            this.Msg.Name = "Msg";
            this.Msg.Size = new System.Drawing.Size(0, 15);
            this.Msg.TabIndex = 5;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(653, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // panelWebFolder
            // 
            this.panelWebFolder.Controls.Add(this.Save);
            this.panelWebFolder.Controls.Add(this.Msg);
            this.panelWebFolder.Controls.Add(this.label11);
            this.panelWebFolder.Controls.Add(this.label1);
            this.panelWebFolder.Controls.Add(this.AffiliateId);
            this.panelWebFolder.Controls.Add(this.label10);
            this.panelWebFolder.Controls.Add(this.AffiliateName);
            this.panelWebFolder.Controls.Add(this.label2);
            this.panelWebFolder.Controls.Add(this.label9);
            this.panelWebFolder.Controls.Add(this.bannerPort);
            this.panelWebFolder.Controls.Add(this.userPort);
            this.panelWebFolder.Controls.Add(this.goPort);
            this.panelWebFolder.Controls.Add(this.label8);
            this.panelWebFolder.Controls.Add(this.adminPort);
            this.panelWebFolder.Controls.Add(this.label4);
            this.panelWebFolder.Controls.Add(this.adminSiteId);
            this.panelWebFolder.Controls.Add(this.goSiteId);
            this.panelWebFolder.Controls.Add(this.label7);
            this.panelWebFolder.Controls.Add(this.label5);
            this.panelWebFolder.Controls.Add(this.userSiteId);
            this.panelWebFolder.Controls.Add(this.bannerSiteId);
            this.panelWebFolder.Controls.Add(this.label6);
            this.panelWebFolder.Location = new System.Drawing.Point(12, 44);
            this.panelWebFolder.Name = "panelWebFolder";
            this.panelWebFolder.Size = new System.Drawing.Size(532, 304);
            this.panelWebFolder.TabIndex = 7;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(19, 245);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(133, 23);
            this.Save.TabIndex = 4;
            this.Save.Text = "Create Folder Structure";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(244, 222);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Site Id Go";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(244, 190);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Site Id Banner";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(244, 162);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Site Id User";
            // 
            // bannerPort
            // 
            this.bannerPort.Location = new System.Drawing.Point(94, 183);
            this.bannerPort.Name = "bannerPort";
            this.bannerPort.Size = new System.Drawing.Size(100, 20);
            this.bannerPort.TabIndex = 8;
            // 
            // userPort
            // 
            this.userPort.Location = new System.Drawing.Point(94, 155);
            this.userPort.Name = "userPort";
            this.userPort.Size = new System.Drawing.Size(100, 20);
            this.userPort.TabIndex = 7;
            // 
            // goPort
            // 
            this.goPort.Location = new System.Drawing.Point(94, 219);
            this.goPort.Name = "goPort";
            this.goPort.Size = new System.Drawing.Size(100, 20);
            this.goPort.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(244, 129);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Site Id Admin";
            // 
            // adminPort
            // 
            this.adminPort.Location = new System.Drawing.Point(94, 123);
            this.adminPort.Name = "adminPort";
            this.adminPort.Size = new System.Drawing.Size(100, 20);
            this.adminPort.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Port Admin";
            // 
            // adminSiteId
            // 
            this.adminSiteId.Location = new System.Drawing.Point(334, 123);
            this.adminSiteId.Name = "adminSiteId";
            this.adminSiteId.Size = new System.Drawing.Size(100, 20);
            this.adminSiteId.TabIndex = 10;
            // 
            // goSiteId
            // 
            this.goSiteId.Location = new System.Drawing.Point(334, 215);
            this.goSiteId.Name = "goSiteId";
            this.goSiteId.Size = new System.Drawing.Size(100, 20);
            this.goSiteId.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Port Go";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Port User";
            // 
            // userSiteId
            // 
            this.userSiteId.Location = new System.Drawing.Point(334, 155);
            this.userSiteId.Name = "userSiteId";
            this.userSiteId.Size = new System.Drawing.Size(100, 20);
            this.userSiteId.TabIndex = 11;
            // 
            // bannerSiteId
            // 
            this.bannerSiteId.Location = new System.Drawing.Point(334, 183);
            this.bannerSiteId.Name = "bannerSiteId";
            this.bannerSiteId.Size = new System.Drawing.Size(100, 20);
            this.bannerSiteId.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Port Banner";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 625);
            this.Controls.Add(this.panelWebFolder);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Setup New Affiliate System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelWebFolder.ResumeLayout(false);
            this.panelWebFolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AffiliateId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AffiliateName;
        private System.Windows.Forms.Label Msg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panelWebFolder;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox bannerPort;
        private System.Windows.Forms.TextBox userPort;
        private System.Windows.Forms.TextBox goPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox adminPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox adminSiteId;
        private System.Windows.Forms.TextBox goSiteId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox userSiteId;
        private System.Windows.Forms.TextBox bannerSiteId;
        private System.Windows.Forms.Label label6;
    }
}

