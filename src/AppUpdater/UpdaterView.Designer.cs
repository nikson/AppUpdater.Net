namespace AppUpdater
{
    partial class UpdaterView
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
            this.BtnDownload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUpdateVersion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblInstalledVersion = new System.Windows.Forms.Label();
            this.BtnFinish = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnSkip = new System.Windows.Forms.Button();
            this.downloadui = new DownloadingUI();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnDownload
            // 
            this.BtnDownload.Location = new System.Drawing.Point(258, 195);
            this.BtnDownload.Name = "BtnDownload";
            this.BtnDownload.Size = new System.Drawing.Size(87, 28);
            this.BtnDownload.TabIndex = 0;
            this.BtnDownload.Text = "Download";
            this.BtnDownload.UseVisualStyleBackColor = true;
            this.BtnDownload.Click += new System.EventHandler(this.BtnDownloadClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available Version: ";
            // 
            // lblUpdateVersion
            // 
            this.lblUpdateVersion.AutoSize = true;
            this.lblUpdateVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateVersion.Location = new System.Drawing.Point(143, 105);
            this.lblUpdateVersion.Name = "lblUpdateVersion";
            this.lblUpdateVersion.Size = new System.Drawing.Size(0, 14);
            this.lblUpdateVersion.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "Installed Version:";
            // 
            // lblInstalledVersion
            // 
            this.lblInstalledVersion.AutoSize = true;
            this.lblInstalledVersion.Location = new System.Drawing.Point(143, 133);
            this.lblInstalledVersion.Name = "lblInstalledVersion";
            this.lblInstalledVersion.Size = new System.Drawing.Size(0, 14);
            this.lblInstalledVersion.TabIndex = 4;
            // 
            // BtnFinish
            // 
            this.BtnFinish.Location = new System.Drawing.Point(151, 195);
            this.BtnFinish.Name = "BtnFinish";
            this.BtnFinish.Size = new System.Drawing.Size(87, 28);
            this.BtnFinish.TabIndex = 5;
            this.BtnFinish.Text = "Finish";
            this.BtnFinish.UseVisualStyleBackColor = true;
            this.BtnFinish.Visible = false;
            
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 83);
            this.panel1.TabIndex = 6;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(36, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(358, 31);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "A new version of  HSDL Application  is available.Do you want to download it now?";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "Application update";
            // 
            // BtnSkip
            // 
            this.BtnSkip.Location = new System.Drawing.Point(44, 195);
            this.BtnSkip.Name = "BtnSkip";
            this.BtnSkip.Size = new System.Drawing.Size(87, 28);
            this.BtnSkip.TabIndex = 7;
            this.BtnSkip.Text = "Try Later";
            this.BtnSkip.UseVisualStyleBackColor = true;
            this.BtnSkip.Click += new System.EventHandler(this.BtnSkipClick);
            // 
            // downloadui
            // 
            this.downloadui.Location = new System.Drawing.Point(34, 104);
            this.downloadui.Name = "downloadui";
            this.downloadui.ShowFileName = true;
            this.downloadui.ShowRemainTime = false;
            this.downloadui.Size = new System.Drawing.Size(311, 89);
            this.downloadui.TabIndex = 8;
            // 
            // UpdaterView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 240);
            this.ControlBox = false;
            this.Controls.Add(this.downloadui);
            this.Controls.Add(this.BtnSkip);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BtnFinish);
            this.Controls.Add(this.lblInstalledVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblUpdateVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnDownload);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "UpdaterView";
            this.Text = "An Update Version is available";
            this.Load += new System.EventHandler(this.UpdaterViewLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdaterViewFormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnDownload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnFinish;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnSkip;
        internal System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.Label lblUpdateVersion;
        internal System.Windows.Forms.Label lblInstalledVersion;
        private DownloadingUI downloadui;
    }
}