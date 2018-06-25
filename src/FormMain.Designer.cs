namespace MyAutoUpdater
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelVersion = new System.Windows.Forms.Label();
            this.progressUpdate = new System.Windows.Forms.ProgressBar();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.timerUpdate = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.labelVersion.Location = new System.Drawing.Point(5, 68);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(230, 20);
            this.labelVersion.Text = "当前版本：";
            // 
            // progressUpdate
            // 
            this.progressUpdate.Location = new System.Drawing.Point(5, 102);
            this.progressUpdate.Name = "progressUpdate";
            this.progressUpdate.Size = new System.Drawing.Size(230, 27);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(5, 141);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(230, 30);
            this.buttonUpdate.TabIndex = 2;
            this.buttonUpdate.Text = "检测新版本";
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(238, 275);
            this.ControlBox = false;
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.progressUpdate);
            this.Controls.Add(this.labelVersion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "自动升级程序";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormMain_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.ProgressBar progressUpdate;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Timer timerUpdate;
    }
}

