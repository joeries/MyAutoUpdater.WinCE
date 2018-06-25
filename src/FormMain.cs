using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MyAutoUpdater.Common;
using MyAutoUpdater.Core;
using System.IO;
using System.Diagnostics;

namespace MyAutoUpdater
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.Text = string.Format("{0} {1}", this.Text, Assembly.GetExecutingAssembly().GetName().Version);
            labelVersion.Text = string.Format("{0}{1}", labelVersion.Text, Constants.CurVersion);
            UpdateHelper.OnEnd += new EndHandler(UpdateHelper_OnEnd);
            UpdateHelper.OnProgress += new ProgressHandler(UpdateHelper_OnProgress);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private void UpdateHelper_OnEnd(Exception ex, bool noRun)
        {
            if (null != ex)
            {
                Logger.Log("ERROR", "AutoUpdater Exception Occurs", ex);
            }
            if (!noRun)
            {
                if (File.Exists(Constants.MainExePath))
                {
                    Process.Start(Constants.MainExePath, "");
                }
                else
                {
                    Logger.Log("WARN", "Non-existent of MainExePath", null);
                }
            }
            Application.Exit();
        }

        private void UpdateHelper_OnProgress(ProgressEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                progressUpdate.Value = e.Percent;
                buttonUpdate.Text = e.Desc;                
            }));

            if (e.Code == "Finished")
            {
                UpdateHelper_OnEnd(null, e.NoRun);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            buttonUpdate.Enabled = false;
            buttonUpdate.Text = "正在检测新版本...";

            UpdateHelper.Start();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            timerUpdate.Enabled = false;
            buttonUpdate_Click(null, null);
        }

        private void FormMain_Closing(object sender, CancelEventArgs e)
        {
            UpdateHelper.OnEnd -= new EndHandler(UpdateHelper_OnEnd);
            UpdateHelper.OnProgress -= new ProgressHandler(UpdateHelper_OnProgress);
        }
    }
}