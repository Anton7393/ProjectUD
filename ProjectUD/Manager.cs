﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
namespace ProjectUD
{
    public partial class Manager : Form
    {
//        private YouTubeContext _YTC;
        //Статусы кнопок
        private String[] states = { "stop", "reload", "open" };

        public Manager()
        {
            InitializeComponent();
            addItemsToListViewFromDB();
            this.Resize += new System.EventHandler(this.Manager_Resize);
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            //Делаем недоступным пункты меню(отменить все закачки и стартовать все закачки)
            contextMenuStrip1.Items[3].Enabled = false;
            contextMenuStrip1.Items[4].Enabled = false;
        }
        private void Manager_Load(object sender, EventArgs e)
        {
            addItemsToListViewFromDB();
        }

        private void buttonAddDownloads_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            AddDownloads FormAddDownloads = new AddDownloads();
      //      FormAddDownloads.ShowInTaskbar = false;
            FormAddDownloads.Owner = this;
            FormAddDownloads.ShowDialog();

            
            if (FormAddDownloads.DialogResult == DialogResult.OK)
            {
                YouTubeContext _YTC = FormAddDownloads.returnContext();
                (new DataContext()).addDataToDB(_YTC);
                addItemsToListView(_YTC, 0, true);
                _YTC.startDownloadViaWebClient();
            }

            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;

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

        private void addItemsToListViewFromDB()
        {
            using (var database = new DataContext())
            {
                var videoData = database.getDataFromDB();//VideoDatas;
                foreach (var videoItem in videoData)
                {
                    addItemsToListView(new YouTubeContext(videoItem.Name, videoItem.Path, videoItem.Link), 100, true);
                }
            }
        }
        private void addItemsToListView(YouTubeContext _YTC, int _proc, bool _completed = false)
        {
            
            string _name = _YTC.Name;
            string _path = _YTC.Path;
            string _link = _YTC.Link;
            Label label = new Label();
                label.Text = _path;
            Button buttonDel = new Button();
                buttonDel.Text = "";
                buttonDel.Image = Properties.Resources.cancel;
                buttonDel.Name = "delete";
                buttonDel.Click += delegate(object sender, EventArgs e)
                {
                    _YTC.stopDownloadViaWebClient();
                    (new DataContext()).removeDataFromDB(_YTC);
                    listViewExDownloads.Items.RemoveAt(listViewExDownloads.IndexItems(sender as Control));
                };
            Button buttonReload = new Button();
                buttonReload.Text = "";
                buttonReload.Image = Properties.Resources.stop;
                buttonReload.Name = states[0];//В имени статус
                buttonReload.Click += delegate(object sender, EventArgs e)
                {
                    int i = listViewExDownloads.IndexItems(sender as Control);
                    Button pb = listViewExDownloads.GetEmbeddedControl(4, i) as Button;
                    MessageBox.Show(pb.Name);
                    //стоп
                    if (pb.Name == states[0])
                    {
                        _YTC.stopDownloadViaWebClient();
                        pb.Image = Properties.Resources.reload_icon;
                        pb.Name = states[1];
                        listViewExDownloads.AddEmbeddedControl(pb, 4, i);
                        listViewExDownloads.Update();
                    }
                    //перезагрузка
                    else if (pb.Name == states[1])
                    {
                        _YTC.stopDownloadViaWebClient();
                        _YTC.startDownload();
                        pb.Image = Properties.Resources.stop;
                        pb.Name = states[0];
                        listViewExDownloads.AddEmbeddedControl(pb, 4, i);
                        listViewExDownloads.Update();
                    }
                    //открыть
                    else if (pb.Name == states[2])
                    {
                        System.Diagnostics.Process.Start(listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(sender as Control), 2).Text);
                    }
                };
            TextBox textBox = new TextBox();
                textBox.ReadOnly = true;
                textBox.Text = _link;
            ProgressBar progressBar = new ProgressBar();
            listViewExDownloads.Items.Add(_name);
            if (_completed)
            {
                progressBar.Value = 100;
                buttonDel.Visible = false;
                buttonReload.Visible = false;
            }
            else
            {
                progressBar.Value = _proc; //Костыль!!!

            }

            listViewExDownloads.AddEmbeddedControl(buttonDel, 5, listViewExDownloads.Items.Count - 1);
            listViewExDownloads.AddEmbeddedControl(buttonReload, 4, listViewExDownloads.Items.Count - 1);
            listViewExDownloads.AddEmbeddedControl(textBox, 2, listViewExDownloads.Items.Count - 1);
            listViewExDownloads.AddEmbeddedControl(progressBar, 3, listViewExDownloads.Items.Count - 1);
            listViewExDownloads.AddEmbeddedControl(label, 1, listViewExDownloads.Items.Count - 1);
            listViewExDownloads.Update();
        }

        private void clearItemsToListView()
        {
            listViewExDownloads.Clear();
        }


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
            if(e.Cancel == true)
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
        //    HideForm();
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

            if (FormAddDownloads.DialogResult == DialogResult.OK)
            {
                /*
                _YTC = FormAddDownloads.returnContext();
                addItemsToListView(_YTC.Name, _YTC.Path, _YTC.Link, 0, true);
                _YTC.startDownload();
                 */ 
            }
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
