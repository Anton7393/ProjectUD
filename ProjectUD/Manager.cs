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
    /*Версия для отладки багов*/
    public partial class Manager : Form
    {
        /*<<<*/public EventGenerator ThisFormEventGenerator; /*>>>*/
        private List<YouTubeContext> ListYouTubeContext = new List<YouTubeContext>();
        private List<string> ListButton2NameStates = new List<string>();
        private List<bool> ListButton2NameChange = new List<bool>();
        private string button2Name = "";
        private bool button2NameChang = false;
        private DataContext DataContext_ = new DataContext();
        //Статусы кнопок
        private String[] states = { "stop", "reload", "open" };
        public bool usbd = true;
        private bool modelOpen = false;
    
    #region Form
        public Manager()
        {
            ThisFormEventGeneratorOn();
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
            contextMenuStripList.Items[0].Visible = false;
            contextMenuStripList.Items[1].Visible = false;
            contextMenuStripList.Items[2].Visible = false;
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
                this.ListYouTubeContext.Add(FormAddDownloads.returnContext());
                {
                    int q = this.ListYouTubeContext.Count - 1;
                    (new DataContext()).addDataToDB(this.ListYouTubeContext[q],0);
                    addItemsToListView(q, 0, true);
                    this.ListButton2NameChange.Add(true);
                    this.ListButton2NameStates.Add(states[0]);
                    this.ListYouTubeContext[q].startDownloadViaWebClient();
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
            using (var database = DataContext_)
            {
                var videoData = database.getDataFromDB();//VideoDatas;
                foreach (var videoItem in videoData)
                {
                    //addItemsToListView(new YouTubeContext(videoItem.Name, videoItem.Path, videoItem.Link), 100, true);
                    this.ListYouTubeContext.Add(new YouTubeContext(videoItem.Name, videoItem.Path, videoItem.Link));
                    {
                        int q = this.ListYouTubeContext.Count - 1;
                        (new DataContext()).addDataToDB(this.ListYouTubeContext[q], videoItem.Progress);
                        addItemsToListView(q, 0, true);
                        this.ListButton2NameChange.Add(true);
                        this.ListButton2NameStates.Add(states[0]);
                    }
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
                this.ListYouTubeContext.Add(FormAddDownloads.returnContext());
                {
                    int q = this.ListYouTubeContext.Count - 1;
                    (new DataContext()).addDataToDB(this.ListYouTubeContext[q]);
                    addItemsToListView(q, 0, true);
                    this.ListButton2NameChange.Add(true);
                    this.ListButton2NameStates.Add(states[0]);
                    this.ListYouTubeContext[q].startDownloadViaWebClient();
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
                   contextMenuStripList.Show(Cursor.Position);
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
		
   #endregion
    #region qwe        
        private Action<object, EventArgs> MainbttnReload(object LocalButtonSender)
        {

            return delegate(object MainButtonSender, EventArgs e)
            {
                //while (!button2NameChang){};
                
                int i = listViewExDownloads.IndexItems(LocalButtonSender as Control);
                if (i != -1)
              {
                   this.ListButton2NameChange[i]=false;
                   if ((this.ListButton2NameStates[i] == states[0]) &&
                       (this.button2Name == states[1]))
                   {
                       this.ThisFormEventGenerator.ButtonReload(LocalButtonSender, null);
                   }
                   else
                   if ((this.ListButton2NameStates[i] == states[1]) &&
                       (this.button2Name == states[0]))
                   {
                       this.ThisFormEventGenerator.ButtonReload(LocalButtonSender, null);
                   }
                   this.ListButton2NameChange[i] = true;
               }
           };
       }
       private void addItemsToListView(/*YouTubeContext _YTC*/int i, int _proc, bool _completed = false)
       {
            string _name = ListYouTubeContext[i].Name;
            string _path = ListYouTubeContext[i].Path;
            string _link = ListYouTubeContext[i].Link;
            Label label = new Label();
            label.Text = _path;
            label.MouseHover += delegate(object LocalButtonSender, EventArgs e)
            {
                toolTip.SetToolTip(label, label.Text);
            };
            Button buttonDel = new Button();
            buttonDel.Text = "";
            buttonDel.Image = Properties.Resources.cancel;
            buttonDel.Name = "delete";
            toolTip.SetToolTip(buttonDel, "Удалить");
            //Создаём событие на локальную кнопку удалить (ну и подписываем на это событие локальную кнопку)
            //buttonDel.Click += new EventHandler(this.bttnDel);
            buttonDel.Click += new EventHandler(this.ThisFormEventGenerator.ButtonDel());
            //Создаём событие на глобыльную кнопку удалить "удалить всё"(ну и подписываем на это событие глобальную кнопку "кнопку удалить всё")
            this.button1.Click += new EventHandler(this.ThisFormEventGenerator.MainButtonDel(buttonDel));
           
            //для подключения в трее использовать ....Click += new EventHandler(this.button1_Click);
            Button buttonReload = new Button();
            buttonReload.Text = "";
            buttonReload.Image = Properties.Resources.stop;
            buttonReload.Name = states[0];//В имени статус
            buttonReload.Click += new EventHandler(this.ThisFormEventGenerator.ButtonReload);
            this.button2.Click += new EventHandler(this.MainbttnReload(buttonReload));
            toolTip.SetToolTip(buttonReload, "Остановить");
            //для подключения в трее использовать ....Click += new EventHandler(this.button2_Click);
            TextBox textBox = new TextBox();
            textBox.ReadOnly = true;
            textBox.Text = _link;
            textBox.MouseHover += delegate(object sender, EventArgs e)
            {toolTip.SetToolTip(textBox, textBox.Text+"       _");};
            ProgressBar progressBar = new ProgressBar();
            ListYouTubeContext[i].SetProgressBarAction(
                (Action<object, System.Net.DownloadProgressChangedEventArgs>)
                delegate(object sender, System.Net.DownloadProgressChangedEventArgs e)
                {
                    progressBar.Value = e.ProgressPercentage;
                    int j = listViewExDownloads.IndexItems(sender as Control);
                    if (progressBar.Value == 100)
                    {
                        buttonReload.Name = states[2];
                        buttonReload.Image = Properties.Resources.open;
                        //toolTip.SetToolTip(progressBar, "Скачивание завершено");
                        toolTip.SetToolTip(buttonReload, "Открыть");
                        (new DataContext()).removeDataFromDB(this.ListYouTubeContext[j]);
                        (new DataContext()).addDataToDB(this.ListYouTubeContext[j], 100);
                    }
                }
            );
           //progressBar.Prog
            progressBar.MouseHover += delegate(object LocalButtonSender, EventArgs e)
            {
                if (progressBar.Value == 100)
                {
                    toolTip.SetToolTip(progressBar, "Видео загружено");

                }
                else
                {
                    toolTip.SetToolTip(progressBar, "Скачано: " + Convert.ToString(progressBar.Value) + "%");
                }
            };
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
            listViewExDownloads.AddEmbeddedControl(label, 1, listViewExDownloads.Items.Count - 1);
            listViewExDownloads.Update();
            this.Top = 1;
        }
       
       
       private void button2_Click(object sender, EventArgs e)
       {
           
           for (int w = 0; w < this.ListButton2NameChange.Count; w++) { this.ListButton2NameChange[w] = true; }
            if (button2Name == states[0])
               {
                   button2.Image = Properties.Resources.reload_icon;
                   button2Name = states[1];
                    toolTip.SetToolTip(button2, "Перезагрузить все");

                //button2.Image = Properties.Resources.stop;
                //button2Name = states[0];
            }
            //перезагрузка
            else if (button2Name == states[1])
               {
                   button2.Image = Properties.Resources.stop;
                   button2Name = states[0];
                   toolTip.SetToolTip(button2, "Остановить все");

            }
            //открыть
            else if (button2.Name == states[2])
               {
                   System.Diagnostics.Process.Start(listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(sender as Control), 2).Text);
               }
           
           button2NameChang = true;
           bool qwes=false;
           while (!qwes)
           {
               qwes =  true;
               for (int w = 0; w < this.ListButton2NameChange.Count; w++)
               { qwes = qwes && this.ListButton2NameChange[w]; }
           }
           
       }
   #endregion
       private void progressBar1_Click(object sender, EventArgs e){}
       #region ThisFormDataMetods
        /// <summary>Контейнер конструктора генератора экшенов</summary>
       private void ThisFormEventGeneratorOn()
       {
           this.ThisFormEventGenerator = new EventGenerator(
                   listViewExDownloads_Items_RemoveAt
                   , ListButton2NameChange_RemoveAt
                   , ListButton2NameStates_RemoveAt
                   , ListYouTubeContext_RemoveAt
                   , ListYouTubeContext_stopDownloadViaWebClient
                   , DataContext_removeDataFromDB
               /*------------*/
                   , listViewExDownloads_IndexItems
                   , ListYouTubeContext_startDownloadViaWebClient
                   , ListButton2NameStatesSet
                   , toolTip_SetToolTip
                   , listViewExDownloads_Item_SubItems1_Text
               /*------------*/
                   , ListButton2NameChangeSet
                   , ListButton2NameChangeGet
                   , button2NameGet
                   , button2NameSet
                   , button2NameChangGet
                   , ListButton2NameStatesGet
               );
       }
#region методы доступа к данным из генератора экшенов
       public void listViewExDownloads_Items_RemoveAt(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) listViewExDownloads.Items.RemoveAt(i);
       }
       public void ListButton2NameChange_RemoveAt(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) this.ListButton2NameChange.RemoveAt(i);
       }
       public void ListButton2NameStates_RemoveAt(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) this.ListButton2NameStates.RemoveAt(i);
       }
       public void ListYouTubeContext_RemoveAt(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) this.ListYouTubeContext.RemoveAt(i);
       }
       public void ListYouTubeContext_stopDownloadViaWebClient(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) this.ListYouTubeContext[i].stopDownloadViaWebClient();
       }
       public void DataContext_removeDataFromDB(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) (new DataContext()).removeDataFromDB(this.ListYouTubeContext[i]);
       }
       /*-------------------------------*/
       public int listViewExDownloads_IndexItems(object sender)
       {
           return listViewExDownloads.IndexItems(sender as Control);
       }
       public void ListYouTubeContext_startDownloadViaWebClient(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) this.ListYouTubeContext[i].startDownloadViaWebClient();
       }
       public void ListButton2NameStatesSet(object sender, string str)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) this.ListButton2NameStates[i] = str;
       }
       public void toolTip_SetToolTip(object sender, string str)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) toolTip.SetToolTip(((Button)sender), str);
       }
       public string listViewExDownloads_Item_SubItems1_Text(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1)
               return listViewExDownloads.Items[i].SubItems[1].Text;
           return "";
       }
       /*------------*/
       public void ListButton2NameChangeSet(object sender, bool qwe)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) this.ListButton2NameChange[i] = qwe;
       }
       public bool ListButton2NameChangeGet(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) return this.ListButton2NameChange[i];
           return false;
       }
       public string button2NameGet() { return this.button2Name; }
       public void button2NameSet(string qwe) { this.button2Name = qwe; }
       public bool button2NameChangGet() { return button2NameChang; }
       public string ListButton2NameStatesGet(object sender)
       {
           int i = listViewExDownloads.IndexItems(sender as Control);
           if (i != -1) return this.ListButton2NameStates[i];
           return "";
       }

#endregion
       #endregion
    }
}