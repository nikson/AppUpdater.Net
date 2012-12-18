namespace AppUpdater
{
    partial class DownloadingUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlFilename = new System.Windows.Forms.Label();
            this._progressbar = new System.Windows.Forms.ProgressBar();
            this.dlRemaintime = new System.Windows.Forms.Label();
            this.dlCompleted = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dlFilename
            // 
            this.dlFilename.AutoEllipsis = true;
            this.dlFilename.AutoSize = true;
            this.dlFilename.Location = new System.Drawing.Point(10, 11);
            this.dlFilename.Name = "dlFilename";
            this.dlFilename.Size = new System.Drawing.Size(72, 13);
            this.dlFilename.TabIndex = 0;
            this.dlFilename.Text = "Downloading ";
            // 
            // _progressbar
            // 
            this._progressbar.Location = new System.Drawing.Point(13, 35);
            this._progressbar.Name = "_progressbar";
            this._progressbar.Size = new System.Drawing.Size(208, 18);
            this._progressbar.Step = 1;
            this._progressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progressbar.TabIndex = 1;
            // 
            // dlRemaintime
            // 
            this.dlRemaintime.AutoSize = true;
            this.dlRemaintime.Location = new System.Drawing.Point(156, 60);
            this.dlRemaintime.Name = "dlRemaintime";
            this.dlRemaintime.Size = new System.Drawing.Size(51, 13);
            this.dlRemaintime.TabIndex = 2;
            this.dlRemaintime.Text = "Remains:";
            // 
            // dlCompleted
            // 
            this.dlCompleted.AutoSize = true;
            this.dlCompleted.Location = new System.Drawing.Point(228, 38);
            this.dlCompleted.Name = "dlCompleted";
            this.dlCompleted.Size = new System.Drawing.Size(15, 13);
            this.dlCompleted.TabIndex = 3;
            this.dlCompleted.Text = "%";
            // 
            // DownloadingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dlCompleted);
            this.Controls.Add(this.dlRemaintime);
            this.Controls.Add(this._progressbar);
            this.Controls.Add(this.dlFilename);
            this.Name = "DownloadingUI";
            this.Size = new System.Drawing.Size(253, 89);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label dlFilename;
        private System.Windows.Forms.ProgressBar _progressbar;
        private System.Windows.Forms.Label dlRemaintime;
        private System.Windows.Forms.Label dlCompleted;
    }
}
