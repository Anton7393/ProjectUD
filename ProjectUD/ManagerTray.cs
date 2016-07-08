using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectUD
{
    public partial class Manager : Form
    {
        private void ShowForm()
        {
            this.Show();
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        private void HideForm()
        {
            contextMenuStrip1.Items[0].Visible = true;
            contextMenuStrip1.Items[1].Visible = false;
            notifyIcon1.ShowBalloonTip(3000);
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            this.ShowInTaskbar = false;
        }


        private void Manager_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
                HideForm();
            }

            listViewExDownloads.Columns[0].Width = Convert.ToInt16((listViewExDownloads.Width - 200) * 0.186);
            listViewExDownloads.Columns[1].Width = Convert.ToInt16((listViewExDownloads.Width - 200) * 0.3896);
            listViewExDownloads.Columns[2].Width = Convert.ToInt16((listViewExDownloads.Width - 200) * 0.3549);
            listViewExDownloads.Columns[3].Width = 124;
            listViewExDownloads.Columns[4].Width = 38;
            listViewExDownloads.Columns[5].Width = 38;
        }

        private void Manager_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = false;
            this.Show();
        }

        private void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (e.Cancel == true)
                HideForm();
        }

        //Развернуть
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        //Свернуть
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            HideForm();
        }


        //Выход 
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (this.WindowState == FormWindowState.Minimized)
                    ShowForm();
                else
                    HideForm();
            }
            else if (e.Button == MouseButtons.Right)
            {
                notifyIcon1.ContextMenuStrip.Show();
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ShowForm();
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            AddDownloads FormAddDownloads = new AddDownloads();
            FormAddDownloads.ShowInTaskbar = false;
            FormAddDownloads.Owner = this;
            FormAddDownloads.ShowDialog();

            try
            {
                if (FormAddDownloads.DialogResult == DialogResult.OK)
                {
                    var index = downloaderList.Count;
                    YouTubeContext youTubeContext = FormAddDownloads.returnContext();
                    VideoData videoData = youTubeContext.getVideoData();
                    Downloader downloader = new Downloader(videoData);

                    downloaderList.Add(downloader);
                    downloaderList[index].DownloadFileCompleted += DownloadFileCompleted;
                    downloaderList[index].DownloadProgressChanged += DownloadProgressChanged;
                    downloaderList[index].startDownload();
                    downloaderList[index].setAsActive();
                    listViewExDownloads.addNewDownload(videoData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }

            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
        }

        

    }
}
