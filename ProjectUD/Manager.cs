using System;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows.Forms; 

namespace ProjectUD
{
    public partial class Manager : Form
    {
        private enum ButtonCondition
        {
            reload,
            stop
        }

        private List<Downloader> downloaderList;
        private ButtonCondition condition = ButtonCondition.stop;
        

        public Manager()
        {
            InitializeComponent();
            notifyIcon1.Text = "YouTube Downloader";
            downloaderList = new List<Downloader>();
            this.Resize += new System.EventHandler(this.Manager_Resize);
            
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            //Делаем недоступным пункты меню(отменить все закачки и стартовать все закачки)
            contextMenuStrip1.Items[3].Enabled = true;
            contextMenuStrip1.Items[4].Enabled = true;
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            TableController.deleteButtonClick += buttonDelete_Click;
            TableController.reloadButtonClick += buttonReload_Click;
            TableController.stopButtonClick += buttonStop_Click;
            TableController.openButtonClick += buttonOpen_Click;
            listViewExDownloads.fillFromDB();
        }

        private void buttonAddDownloads_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            AddDownloads FormAddDownloads = new AddDownloads();
            //FormAddDownloads.ShowInTaskbar = false;
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

                    if (checkExistingDownloads(downloader))
                    {
                        MessageBox.Show("Загрузка файла с таким именем уже существует!", 
                                        "Error!",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    downloaderList.Add(downloader);
                    downloaderList[index].DownloadFileCompleted += DownloadFileCompleted;
                    downloaderList[index].DownloadProgressChanged += DownloadProgressChanged;
                    downloaderList[index].startDownload();
                    downloaderList[index].setAsActive();
                    listViewExDownloads.addNewDownload(videoData);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
            }

            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var ansver = MessageBox.Show("Удалить загруженный файл?"
                , "Удаление загрузок", MessageBoxButtons.YesNo
                , MessageBoxIcon.Question);

            Button button = sender as Button;
            VideoData data = button.Tag as VideoData;
            var path = data.Path;
            var index = TableController.getIndex(data);
            var downIndex = downloaderIndex(index);

            if ((downIndex < downloaderList.Count) && (downIndex >= 0))
            {
                downloaderList[downIndex].abortDownload();
                downloaderList.RemoveAt(downIndex);
            }

            if (File.Exists(path) && (ansver == DialogResult.Yes))
            {
                Thread.Sleep(100);
                File.Delete(path);
            }

            listViewExDownloads.removeDownload(index);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            VideoData data = button.Tag as VideoData;
            var index = TableController.getIndex(data);
            var downIndex = downloaderIndex(index);
            var path = data.Path;

            listViewExDownloads.stopDownload(index);
            downloaderList[downIndex].stopDownload();
            downloaderList[downIndex].setAsFailure();

            Thread.Sleep(100);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            VideoData data = button.Tag as VideoData;
            var index = TableController.getIndex(data);
            var downIndex = downloaderIndex(index);

            listViewExDownloads.restartDownload(index);
            downloaderList[downIndex].startDownload();
            downloaderList[index].DownloadFileCompleted += DownloadFileCompleted;
            downloaderList[index].DownloadProgressChanged += DownloadProgressChanged;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            VideoData data = button.Tag as VideoData;

            if (File.Exists(data.Path))
            {
                Process.Start(data.Path);
            }
            else
            {
                var ansver = MessageBox.Show(String.Format("Не удаётся открыть. Файл: {0} не существует или повреждён.", data.Path )
                , "Ошибка открытия файла", MessageBoxButtons.OK
                , MessageBoxIcon.Question);
            }
            
        }

        private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                VideoData data = e.UserState as VideoData;
                var index = TableController.getIndex(data);
                var downIndex = downloaderIndex(index);

                listViewExDownloads.completeDownload(index);
                downloaderList.RemoveAt(downIndex);
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            VideoData data = e.UserState as VideoData;
            var index = TableController.getIndex(data);
            var bytesReceived = (double)e.BytesReceived;
            var totalBytesToReceive = (double)e.TotalBytesToReceive;
            int progressPercentage = Convert.ToInt32((bytesReceived / totalBytesToReceive) * 100);

            listViewExDownloads.setProgressPercentage(index, progressPercentage);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < downloaderList.Count; i++)
            {
                if (downloaderList[i].getStatus() == DownloadStatus.Active)
                {
                    downloaderList[i].stopDownload();
                    downloaderList[i].setAsFailure();

                    Thread.Sleep(100);

                    if (File.Exists(downloaderList[i].getPath()))
                    {
                        File.Delete(downloaderList[i].getPath());
                    }
                }
            }
            listViewExDownloads.stopAll();     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var downloader in downloaderList)
            {
                if (downloader.getStatus() == DownloadStatus.Failure)
                {
                    downloader.startDownload();
                    downloader.DownloadFileCompleted += DownloadFileCompleted;
                    downloader.DownloadProgressChanged += DownloadProgressChanged;
                    downloader.setAsActive();
                }
            }
            listViewExDownloads.restartAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ansver = MessageBox.Show("Удалить загруженные файлы?"
                , "Удаление загрузок", MessageBoxButtons.YesNo
                , MessageBoxIcon.Question);

            var deleteList = new List<VideoData>();

            while (downloaderList.Count != 0)
            {
                if (downloaderList[0].getStatus() == DownloadStatus.Active)
                {
                    downloaderList[0].abortDownload();
                }
                downloaderList.RemoveAt(0);
            }

            deleteList = listViewExDownloads.removeAll();
            
            if (ansver == DialogResult.Yes)
            {
                Thread.Sleep(100);
                foreach (var element in deleteList)
                {
                    if (File.Exists(element.Path))
                    {
                        File.Delete(element.Path);
                    }
                }
            }
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;

            Form FormInfo = new Info();
            FormInfo.ShowDialog();

            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
        }

        //Отменить все
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < downloaderList.Count; i++)
            {
                if (downloaderList[i].getStatus() == DownloadStatus.Active)
                {
                    downloaderList[i].stopDownload();
                    downloaderList[i].setAsFailure();

                    Thread.Sleep(100);

                    if (File.Exists(downloaderList[i].getPath()))
                    {
                        File.Delete(downloaderList[i].getPath());
                    }
                }
            }
            listViewExDownloads.stopAll();
        }

        //Стартовать все
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            foreach (var downloader in downloaderList)
            {
                if (downloader.getStatus() == DownloadStatus.Failure)
                {
                    downloader.startDownload();
                    downloader.DownloadFileCompleted += DownloadFileCompleted;
                    downloader.DownloadProgressChanged += DownloadProgressChanged;
                    downloader.setAsActive();
                }
            }
            listViewExDownloads.restartAll();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private int downloaderIndex(int _index)
        {
            return downloaderList.Count - (1 + _index);
        }

        private void contextMenuStripList_Opening(object sender, CancelEventArgs e)
        {
            if (listViewExDownloads.SelectedItems.Count == 1)
            {
                var data = listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Tag as VideoData;
                var status = data.Status;

                if (status == DownloadStatus.Active)
                {
                    contextMenuStripList.Items[0].Visible = true;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = false;
                    contextMenuStripList.Items[3].Visible = true;

                }
                else if (status == DownloadStatus.Failure)
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = true;
                    contextMenuStripList.Items[2].Visible = false;
                    contextMenuStripList.Items[3].Visible = true;

                }
                else if (status == DownloadStatus.Completed)
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = true;
                    contextMenuStripList.Items[3].Visible = true;
                }
            }
            else
                e.Cancel = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listViewExDownloads.SelectedItems[0].Index;
            if (index >= 0)
            {
                var data = listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Tag as VideoData;
                var downIndex = downloaderIndex(index);
                var path = data.Path;

                listViewExDownloads.stopDownload(index);
                downloaderList[downIndex].stopDownload();
                downloaderList[downIndex].setAsFailure();

                Thread.Sleep(100);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listViewExDownloads.SelectedItems[0].Index;
            if (index >= 0)
            {
                var downIndex = downloaderIndex(index);

                listViewExDownloads.restartDownload(index);
                downloaderList[downIndex].startDownload();
                downloaderList[index].DownloadFileCompleted += DownloadFileCompleted;
                downloaderList[index].DownloadProgressChanged += DownloadProgressChanged;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listViewExDownloads.SelectedItems[0].Index;
            if (index >= 0)
            {
                var data = listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Tag as VideoData;
                Process.Start(data.Path);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listViewExDownloads.SelectedItems[0].Index;
            if (index >= 0)
            {
                var ansver = MessageBox.Show("Удалить загруженные файлы?"
                , "Удаление загрузок", MessageBoxButtons.YesNo
                , MessageBoxIcon.Question);

                var data = listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Tag as VideoData;
                var path = data.Path;
                var downIndex = downloaderIndex(index);

                if ((downIndex < downloaderList.Count) && (downIndex >= 0))
                {
                    downloaderList[downIndex].abortDownload();
                    downloaderList.RemoveAt(downIndex);
                }

                if (File.Exists(path) && (ansver == DialogResult.Yes))
                {
                    Thread.Sleep(100);
                    File.Delete(path);
                }

                listViewExDownloads.removeDownload(index);
            }
        }

        private bool checkExistingDownloads(Downloader _downloader)
        {
            foreach(var downloader in downloaderList)
            {
                if (downloader.getPath() == _downloader.getPath())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
