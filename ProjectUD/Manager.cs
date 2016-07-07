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
        private List<YouTubeContext> _LYTC = new List<YouTubeContext>();
        private List<string> _L_bttnReloadName_States = new List<string>();
        private List<bool> _L_bttnReloadName_Chang = new List<bool>();
        private string button2Name = "";
        private bool button2Name_Chang = false; 
        
        //Статусы кнопок
        private String[] states = { "stop", "reload", "open" };
        public bool usbd = true;
        private bool modelOpen = false;
    #region Form
        public Manager()
        {
            button2Name = states[0];
            InitializeComponent();
            addItemsToListViewFromDB();
            this.Resize += new System.EventHandler(this.Manager_Resize);
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            //Делаем недоступным пункты меню(отменить все закачки и стартовать все закачки)
            contextMenuStrip1.Items[3].Enabled = false;
            contextMenuStrip1.Items[4].Enabled = false;
            //Делаем недоступным пункты контекстного меню
            
            contextMenuStripList.Items[3].Click += new EventHandler(this.bttnDel);
            contextMenuStripList.Items[1].Click += new EventHandler(this.ctxStop);
            contextMenuStripList.Items[2].Click += new EventHandler(this.ctxReload);

            contextMenuStripList.Update();
            contextMenuStripList.Show();
            contextMenuStripList.Hide();
            
            notifyIcon1.Text = "YouTube Downloader";
            toolTip.SetToolTip(button2, "Остановить все");
            toolTip.SetToolTip(button1, "Удалить все");
            toolTip.SetToolTip(buttonInfo, "Информация о программе");

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
            modelOpen = true;
            AddDownloads FormAddDownloads = new AddDownloads();
      //      FormAddDownloads.ShowInTaskbar = false;
            FormAddDownloads.Owner = this;
            FormAddDownloads.ShowDialog();
            if (FormAddDownloads.DialogResult == DialogResult.OK)
            {
                this._LYTC.Add(FormAddDownloads.returnContext());
                {
                    int q = this._LYTC.Count - 1;
                    (new DataContext()).addDataToDB(this._LYTC[q]);
                    addItemsToListView(q, 0, true);
                    this._L_bttnReloadName_Chang.Add(true);
                    this._L_bttnReloadName_States.Add(states[0]);
                    this._LYTC[q].startDownloadViaWebClient();
                }
           }
            modelOpen = false;
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;

        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            modelOpen = true;
            Form FormInfo = new Info();
            FormInfo.ShowDialog();
            modelOpen = false;
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
                    //addItemsToListView(new YouTubeContext(videoItem.Name, videoItem.Path, videoItem.Link), 100, true);
                }
            }
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
                if (modelOpen == false)
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
        }
//#warning toolStripMenuItem6_Click
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ShowForm();
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = false;
            contextMenuStrip1.Items[2].Visible = false;
            modelOpen = true;
            AddDownloads FormAddDownloads = new AddDownloads();
            FormAddDownloads.ShowInTaskbar = false;
            FormAddDownloads.Owner = this;
            FormAddDownloads.ShowDialog();

            if (FormAddDownloads.DialogResult == DialogResult.OK)
            {
                /*
                YouTubeContext _YTC = FormAddDownloads.returnContext();
                if (usbd) (new DataContext()).addDataToDB(_YTC);
                addItemsToListView(_YTC, 0, true);
                _YTC.startDownloadViaWebClient();
                */
                this._LYTC.Add(FormAddDownloads.returnContext());
                {
                    int q = this._LYTC.Count - 1;
                    (new DataContext()).addDataToDB(this._LYTC[q]);
                    addItemsToListView(q, 0, true);
                    this._L_bttnReloadName_Chang.Add(true);
                    this._L_bttnReloadName_States.Add(states[0]);
                    this._LYTC[q].startDownloadViaWebClient();
                }
            }
            modelOpen = false;
            contextMenuStrip1.Items[0].Visible = false;
            contextMenuStrip1.Items[1].Visible = true;
            contextMenuStrip1.Items[2].Visible = true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
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
              if (System.IO.File.Exists(listViewExDownloads.Items[listViewExDownloads.SelectedItems[0].Index].SubItems[1].Text))
                  System.Diagnostics.Process.Start(listViewExDownloads.Items[listViewExDownloads.SelectedItems[0].Index].SubItems[1].Text);
              else
              {
                  MessageBox.Show(this, "Файл " + '"' + listViewExDownloads.GetEmbeddedControl(1, listViewExDownloads.SelectedItems[0].Index).Text + '"' + "не найден.",
                    "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              }
          }
      }
      private void contextMenuStripList_Opening(object sender, CancelEventArgs e)
       {

            if (listViewExDownloads.SelectedItems.Count == 1)
            {
                if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[0])
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = true;
                    contextMenuStripList.Items[2].Visible = false;
                    contextMenuStripList.Items[3].Visible = true;

                }
                else if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[1])
                {
                    contextMenuStripList.Items[0].Visible = false;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = true;
                    contextMenuStripList.Items[3].Visible = true;

                }
                else if (listViewExDownloads.GetEmbeddedControl(4, listViewExDownloads.SelectedItems[0].Index).Name == states[2])
                {
                    contextMenuStripList.Items[0].Visible = true;
                    contextMenuStripList.Items[1].Visible = false;
                    contextMenuStripList.Items[2].Visible = false;
                    contextMenuStripList.Items[3].Visible = true;
                }
            }
            else
                e.Cancel = true;
        }
		
   #endregion
    #region qwe
        private void bttnDel(object LocalButtonSender, EventArgs e)
        {
            int i = listViewExDownloads.IndexItems(LocalButtonSender as Control);
            if (i != -1)
            {
                (new DataContext()).removeDataFromDB(this._LYTC[i]);
                this._LYTC[i].stopDownloadViaWebClient();
                this._LYTC.RemoveAt(i);
                this._L_bttnReloadName_States.RemoveAt(i);
                this._L_bttnReloadName_Chang.RemoveAt(i);
                listViewExDownloads.Items.RemoveAt(i);
            }
        }
        private void ctxStop(object LocalButtonSender, EventArgs e)
        {
            int i = listViewExDownloads.SelectedItems[0].Index;
            if (i != -1)
            {
                this._LYTC[i].stopDownloadViaWebClient();
                //    ((Button)LocalButtonSender).Image = Properties.Resources.reload_icon;
                //    ((Button)LocalButtonSender).Name = states[1];
                ///    this._L_bttnReloadName_States[i] = states[1];
                //    toolTip.SetToolTip(((Button)LocalButtonSender), "Перезагрузить");
            }
        }
        private void ctxReload(object LocalButtonSender, EventArgs e)
        {
            int i = listViewExDownloads.SelectedItems[0].Index;
            if (i != -1)
            {
               
                this._LYTC[i].stopDownloadViaWebClient();
                this._LYTC[i].startDownloadViaWebClient();

                //    ((Button)LocalButtonSender).Image = Properties.Resources.stop;
                //    ((Button)LocalButtonSender).Name = states[0];
                //    this._L_bttnReloadName_States[i] = states[0];
                //    toolTip.SetToolTip(((Button)LocalButtonSender), "Остановить");
            }
        }
        private Action<object, EventArgs> MainbttnDel(object LocalButtonSender)
        {
            return delegate(object MainButtonSender, EventArgs e){this.bttnDel(LocalButtonSender, null);};
        }
        private void bttnReload(object LocalButtonSender, EventArgs e)
        {
            /// Для данного метода sender и LocalButtonSender - одни и теже
            /// Это подразумевается, но чтобы убедиться в этом используем проверку на эквивалентность объектов.
            //System.Windows.Forms.MessageBox.Show("sender.Equals(LocalButtonSender) = " + sender.Equals(LocalButtonSender), "");
            int i = listViewExDownloads.IndexItems(LocalButtonSender as Control);
            if (i != -1)
            {
                if (((Button)LocalButtonSender).Name == states[0])
                {
                    this._LYTC[i].stopDownloadViaWebClient();
                    ((Button)LocalButtonSender).Image = Properties.Resources.reload_icon;
                    ((Button)LocalButtonSender).Name = states[1];
                    this._L_bttnReloadName_States[i] = states[1];
                    toolTip.SetToolTip(((Button)LocalButtonSender), "Перезагрузить");

                }//стоп
                else if (((Button)LocalButtonSender).Name == states[1])
                {
                    this._LYTC[i].stopDownloadViaWebClient();
                    this._LYTC[i].startDownloadViaWebClient();
                    ((Button)LocalButtonSender).Image = Properties.Resources.stop;
                    ((Button)LocalButtonSender).Name = states[0];
                    this._L_bttnReloadName_States[i] = states[0];
                    toolTip.SetToolTip(((Button)LocalButtonSender), "Остановить");

                }//перезагрузка
                else if (((Button)LocalButtonSender).Name == states[2])
                {
                    if (System.IO.File.Exists(listViewExDownloads.Items[listViewExDownloads.IndexItems(LocalButtonSender as Control)].SubItems[1].Text))
                        System.Diagnostics.Process.Start(listViewExDownloads.Items[listViewExDownloads.IndexItems(LocalButtonSender as Control)].SubItems[1].Text);
                    else
                    {
                        MessageBox.Show(this, "Файл " + '"' + listViewExDownloads.Items[listViewExDownloads.IndexItems(LocalButtonSender as Control)].SubItems[1].Text + '"' + "не найден.",
                          "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private Action<object, EventArgs> MainbttnReload(object LocalButtonSender)
        {

            return delegate(object MainButtonSender, EventArgs e)
            {
                //while (!button2Name_Chang){};
                
                int i = listViewExDownloads.IndexItems(LocalButtonSender as Control);
                if (i != -1)
              {
                   this._L_bttnReloadName_Chang[i]=false;
                   if ((this._L_bttnReloadName_States[i] == states[0]) &&
                       (this.button2Name == states[1]))
                   {
                       this.bttnReload(LocalButtonSender, null);
                   }
                   else
                   if ((this._L_bttnReloadName_States[i] == states[1]) &&
                       (this.button2Name == states[0]))
                   {
                       this.bttnReload(LocalButtonSender, null);
                   }
                   this._L_bttnReloadName_Chang[i] = true;
                   /*
                   if (((Button)LocalButtonSender).Name != (((Button)MainButtonSender).Name))     
                   {}
                   button2Name = (((Button)MainButtonSender).Name);
                   */
               }
           };
       }

        private Action<object, EventArgs> MainbttnStop(object LocalButtonSender)
        {

            return delegate (object MainButtonSender, EventArgs e)
            {
                //while (!button2Name_Chang){};


                int i = listViewExDownloads.IndexItems(LocalButtonSender as Control);
                if (i != -1)
                {
                    this._L_bttnReloadName_Chang[i] = false;
                    if ((this._L_bttnReloadName_States[i] == states[0]) &&
                        (this.button2Name == states[1]))
                    {
                        this.bttnReload(LocalButtonSender, null);
                    }
                    else
                    if ((this._L_bttnReloadName_States[i] == states[1]) &&
                        (this.button2Name == states[0]))
                    {
                        this.bttnReload(LocalButtonSender, null);
                    }
                    this._L_bttnReloadName_Chang[i] = true;
                    /*
                    if (((Button)LocalButtonSender).Name != (((Button)MainButtonSender).Name))     
                    {}
                    button2Name = (((Button)MainButtonSender).Name);
                    */
                }
            };
        }
        private void addItemsToListView(/*YouTubeContext _YTC*/int i, int _proc, bool _completed = false)
       {
           string _name = _LYTC[i].Name;
           string _path = _LYTC[i].Path;
           string _link = _LYTC[i].Link;
           Label label = new Label();
               label.Text = _path;
           Button buttonDel = new Button();
               buttonDel.Text = "";
               buttonDel.Image = Properties.Resources.cancel;
               buttonDel.Name = "delete";
                toolTip.SetToolTip(buttonDel, "Удалить");

                //Создаём событие на локальную кнопку удалить (ну и подписываем на это событие локальную кнопку)
                buttonDel.Click += new EventHandler(this.bttnDel);
               //buttonDel.Click += new EventHandler(this.bttnDel(buttonDel));
               //Создаём событие на глобыльную кнопку удалить "удалить всё"(ну и подписываем на это событие глобальную кнопку "кнопку удалить всё")
               this.button1.Click += new EventHandler(this.MainbttnDel(buttonDel));
               //для подключения в трее использовать ....Click += new EventHandler(this.button1_Click);
               Button buttonReload = new Button();
               buttonReload.Text = "";
               buttonReload.Image = Properties.Resources.stop;
               buttonReload.Name = states[0];//В имени статус
               buttonReload.Click += new EventHandler(this.bttnReload);
               this.buttonReloadAll.Click += new EventHandler(this.MainbttnReload(buttonReload));
               this.buttonStopAll.Click += new EventHandler(this.MainbttnReload(buttonReload));

            toolTip.SetToolTip(buttonReload, "Остановить");

            //для подключения в трее использовать ....Click += new EventHandler(this.button2_Click);
            TextBox textBox = new TextBox();
               textBox.ReadOnly = true;
               textBox.Text = _link;
           ProgressBar progressBar = new ProgressBar();
           _LYTC[i].SetProgressBarAction(
               (Action<object, System.Net.DownloadProgressChangedEventArgs>)
               delegate(object sender, System.Net.DownloadProgressChangedEventArgs e)
               {progressBar.Value = e.ProgressPercentage;
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
            string[] row = { _name, _path };
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
           listViewExDownloads.AddEmbeddedControl(buttonDel, 5, listViewExDownloads.Items.Count - 1);
           listViewExDownloads.AddEmbeddedControl(buttonReload, 4, listViewExDownloads.Items.Count - 1);
           listViewExDownloads.AddEmbeddedControl(textBox, 2, listViewExDownloads.Items.Count - 1);
           listViewExDownloads.AddEmbeddedControl(progressBar, 3, listViewExDownloads.Items.Count - 1);
     //      listViewExDownloads.AddEmbeddedControl(label, 1, listViewExDownloads.Items.Count - 1);
           listViewExDownloads.Update();
            this.Top = 1;
        }
       
       private void button2_Click(object sender, EventArgs e)
       {
           
           for (int w = 0; w < this._L_bttnReloadName_Chang.Count; w++) { this._L_bttnReloadName_Chang[w] = true; }
            if (button2Name == states[0])
               {
                   button2.Image = Properties.Resources.reload_icon;
                   button2Name = states[1];
                    toolTip.SetToolTip(button2, "Перезагрузить все");
            }
            //перезагрузка
            else if (button2Name == states[1])
               {
                   button2.Image = Properties.Resources.stop;
                   button2Name = states[0];
                   toolTip.SetToolTip(button2, "Остановить все");

            }
            //открыть
          //  else if (button2.Name == states[2])
          //     {
          //         System.Diagnostics.Process.Start(listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(sender as Control), 2).Text);
           //    }
           
           button2Name_Chang = true;
           bool qwes=false;
           while (!qwes)
           {
               qwes =  true;
               for (int w = 0; w < this._L_bttnReloadName_Chang.Count; w++)
               { qwes = qwes && this._L_bttnReloadName_Chang[w]; }
           }
           
       }
       
   #endregion
   }
}