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
        public bool usbd = true;
        private string button2Name = "";
        public Manager()
        {
            button2Name = states[0];
            InitializeComponent();
            if (usbd) addItemsToListViewFromDB();
            this.Resize += new System.EventHandler(this.Manager_Resize);
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            //Делаем недоступным пункты меню(отменить все закачки и стартовать все закачки)
            contextMenuStrip1.Items[3].Enabled = false;
            contextMenuStrip1.Items[4].Enabled = false;
            //Делаем недоступным пункты контекстного меню
            contextMenuStripList.Items[0].Visible = false;
            contextMenuStripList.Items[1].Visible = false;
            contextMenuStripList.Items[2].Visible = false;

  

        }
        private void Manager_Load(object sender, EventArgs e)
        {
            //if (usbd)addItemsToListViewFromDB();
            //this.button2.Image = Properties.Resources.stop;this.button1.Image = Properties.Resources.stop;
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
                if (usbd) (new DataContext()).addDataToDB(_YTC);
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
            label.Text =  _path;
            Button buttonDel = new Button();
            buttonDel.Text = "";
            buttonDel.Image = Properties.Resources.cancel;
            buttonDel.Name = "delete";
            Action<object, EventArgs> bttnDel = delegate (object sender, EventArgs e)
            {
                _YTC.stopDownloadViaWebClient();
                if (usbd) (new DataContext()).removeDataFromDB(_YTC);
                listViewExDownloads.Items.RemoveAt(listViewExDownloads.IndexItems(sender as Control));
            };
            buttonDel.Click += new EventHandler(bttnDel);//на кнопку отмены одного скачивания
            this.button1.Click += new EventHandler(bttnDel);//на кнопку отмены всех скачиваний

            Button buttonReload = new Button();
            buttonReload.Text = "";
            buttonReload.Image = Properties.Resources.stop;
            buttonReload.Name = states[0];//В имени статус
            Action<int> bttnReload_if1 = delegate (int i)
            {
                //int i = listViewExDownloads.IndexItems(sender as Control);
                Button pb = listViewExDownloads.GetEmbeddedControl(4, i) as Button;
                if (pb.Name == states[1])
                {
                    pb.Image = Properties.Resources.stop;
                    pb.Name = states[0];
                    toolTip.SetToolTip(buttonReload, "Стоп");

                    listViewExDownloads.AddEmbeddedControl(pb, 4, i);
                    listViewExDownloads.Update();
                    _YTC.stopDownloadViaWebClient();
                    _YTC.startDownloadViaWebClient();
                }
            };
            Action<int> bttnReload_if0 = delegate (int i)
           {
               //int i = listViewExDownloads.IndexItems(sender as Control);
               Button pb = listViewExDownloads.GetEmbeddedControl(4, i) as Button;
               if (pb.Name == states[0])
               {
                   _YTC.stopDownloadViaWebClient();
                   pb.Image = Properties.Resources.reload_icon;
                   pb.Name = states[1];
                   toolTip.SetToolTip(buttonReload, "Перезагрузка");

                   listViewExDownloads.AddEmbeddedControl(pb, 4, i);
                   listViewExDownloads.Update();
               }
           };

            buttonReload.Click += delegate (object sender, EventArgs e)
                {//этот Action нужно подключить в два места.
                    int i = listViewExDownloads.IndexItems(sender as Control);
                    Button pb = listViewExDownloads.GetEmbeddedControl(4, i) as Button;
                    //MessageBox.Show(pb.Name);
                    if (pb.Name == states[0]) { bttnReload_if0(i); }//стоп
                    else if (pb.Name == states[1]) { bttnReload_if1(i); }//перезагрузка
                    else if (pb.Name == states[2])
                    {
                        if (System.IO.File.Exists(_path))
                            System.Diagnostics.Process.Start(_path);
                        else {
                            MessageBox.Show(this, "Файл " + '"' + _path + '"' + "не найден.",
                              "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                };
            this.button2.Click += delegate (object sender, EventArgs e)
            {
                if (this.button2Name == states[0])
                {
                    for (int i = 0; i < this.listViewExDownloads.Items.Count; i++)
                        bttnReload_if0(i);
                }
                else
                    if (this.button2Name == states[1])
                {
                    for (int i = 0; i < this.listViewExDownloads.Items.Count; i++)
                        bttnReload_if1(i);
                }
            };

            TextBox textBox = new TextBox();
            textBox.ReadOnly = true;
            textBox.Text = _link;
            ProgressBar progressBar = new ProgressBar();
            _YTC.SetProgressBarAction(
                (Action<object, System.Net.DownloadProgressChangedEventArgs>)
                delegate (object sender, System.Net.DownloadProgressChangedEventArgs e)

                { progressBar.Value = e.ProgressPercentage;
                    toolTip.SetToolTip(progressBar, "Скачано: " + e.ProgressPercentage + "%");

                    if (progressBar.Value == 100)
                    {
                        buttonReload.Name = states[2];
                        buttonReload.Image = Properties.Resources.open;
                        toolTip.SetToolTip(progressBar, "Скачивание завершено");
                        toolTip.SetToolTip(buttonReload, "Открыть");

                    }

                }
            );
            string[] row = { _name, _path};
            var listViewItem = new ListViewItem(row);

            listViewExDownloads.Items.Add(listViewItem);
            if (_completed)
            {
                //progressBar.Value = 100;
                buttonDel.Visible = false;
                buttonReload.Visible = false;
            }
            else
            {
                //progressBar.Value = _proc; //Костыль!!!

            }
            toolTip.SetToolTip(buttonDel, "Удалить из истории");
            toolTip.SetToolTip(buttonReload, "Стоп");


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

            listViewExDownloads.Columns[0].Width = Convert.ToInt16((listViewExDownloads.Width-200) * 0.186);
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
                YouTubeContext _YTC = FormAddDownloads.returnContext();
                if (usbd) (new DataContext()).addDataToDB(_YTC);
                addItemsToListView(_YTC, 0, true);
                _YTC.startDownloadViaWebClient();

            }
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2Name == states[0])
            {
                button2.Image = Properties.Resources.reload_icon;
                button2Name = states[1];
            }
            //перезагрузка
            else if (button2Name == states[1])
            {
                button2.Image = Properties.Resources.stop;
                button2Name = states[0];
            }
            //открыть
            else if (button2.Name == states[2])
            {
            //    System.Diagnostics.Process.Start(listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(sender as Control), 2).Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (listViewExDownloads.SelectedItems.Count == 1)
            {

                if (System.IO.File.Exists(listViewExDownloads.GetEmbeddedControl(1, listViewExDownloads.SelectedItems[0].Index).Text))
                    System.Diagnostics.Process.Start(listViewExDownloads.GetEmbeddedControl(1, listViewExDownloads.SelectedItems[0].Index).Text);
                else
                {
                    MessageBox.Show(this, "Файл " + '"' + listViewExDownloads.GetEmbeddedControl(1, listViewExDownloads.SelectedItems[0].Index).Text + '"' + "не найден.",
                      "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
                

        private void listViewExDownloads_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listViewExDownloads.SelectedItems.Count == 1)
            {
    
                if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[0])
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = true;
                    contextMenuStripList.Items[2].Visible = false;

                }
                else if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[1])
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = true;
                }
                else if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[2])
                {
                    contextMenuStripList.Items[0].Visible = true;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = false;
                }
            }
            else
            {
                contextMenuStripList.Items[0].Visible = false;
                contextMenuStripList.Items[1].Visible = false;
                contextMenuStripList.Items[2].Visible = false;

            }
            
        }

        private void listViewExDownloads_Click(object sender, EventArgs e)
        {
            if (listViewExDownloads.SelectedItems.Count == 1)
            {

                if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[0])
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = true;
                    contextMenuStripList.Items[2].Visible = false;

                }
                else if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[1])
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = true;
                }
                else if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[2])
                {
                    contextMenuStripList.Items[0].Visible = true;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = false;
                }
            }
            else
            {
                contextMenuStripList.Items[0].Visible = false;
                contextMenuStripList.Items[1].Visible = false;
                contextMenuStripList.Items[2].Visible = false;

            }
            
        }
    }
}
