using System;
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
        private YouTubeContext mYouTubeContext;

        public Manager()
        {
            InitializeComponent();
            //addItemsToListViewFromDB();
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
            AddDownloads FormAddDownloads = new AddDownloads();
            FormAddDownloads.ShowInTaskbar = false;
            FormAddDownloads.Owner = this;
            FormAddDownloads.ShowDialog();

            if (FormAddDownloads.DialogResult == DialogResult.OK)
            {
                mYouTubeContext = FormAddDownloads.returnContext();
                addItemsToListView(mYouTubeContext.Name, mYouTubeContext.Path, mYouTubeContext.Link, 0, true);
                mYouTubeContext.startDownloadViaWebClient();
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

        private void addItemsToListViewFromDB()
        {
            using (var database = new DataContext())
            {
                var videoData = database.VideoDatas;
                foreach (var videoItem in videoData)
                {
                    addItemsToListView(videoItem.Name, videoItem.Path, videoItem.Link, 100, true);
                }
            }
        }

        private void addItemsToListView(string _name, string _path, string _link, int _proc, bool _completed = false)
        {
            Label label = new Label();
            Button button = new Button();
            Button buttonReload = new Button();

            TextBox textBox = new TextBox();
            ProgressBar progressBar = new ProgressBar();

            label.Text = _path;
            button.Text = "";
            button.Image = Properties.Resources.cancel;
            button.Name = "button";
            buttonReload.Text = "";
            buttonReload.Image = Properties.Resources.reload_icon;
            buttonReload.Name = "reload";

            textBox.ReadOnly = true;
            textBox.Text = _link;
            listViewExDownloads.Items.Add(_name);

            if (_completed)
            {
                progressBar.Value = 100;
                listViewExDownloads.AddEmbeddedControl(button, 5, listViewExDownloads.Items.Count - 1);
                listViewExDownloads.AddEmbeddedControl(buttonReload, 4, listViewExDownloads.Items.Count - 1);

            }
            else
            {
                progressBar.Value = _proc; //Костыль!!!

            }

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
                //       this.ShowInTaskbar = false;
            }
        }

        private void HideForm()
        {
            contextMenuStrip1.Items[0].Visible = true;
            contextMenuStrip1.Items[1].Visible = false;
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

            if (FormAddDownloads.DialogResult == DialogResult.OK)
            {
                mYouTubeContext = FormAddDownloads.returnContext();
                addItemsToListView(mYouTubeContext.Name, mYouTubeContext.Path, mYouTubeContext.Link, 0, true);
                mYouTubeContext.startDownload();
            }
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
        }

        
    }
}
