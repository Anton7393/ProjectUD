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
        public YouTubeContext _YTC;
        public List<YouTubeContext> _LYTC = new List<YouTubeContext>();
        //Статусы кнопок
        private String[] states = { "stop", "reload", "open" };
        public bool usbd = false;
        private bool chenged = false;
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
                this._LYTC.Add(FormAddDownloads.returnContext());
                if (usbd) (new DataContext()).addDataToDB(_YTC);
                addItemsToListView(this._LYTC.Count - 1, 0, true);
                this._LYTC[this._LYTC.Count - 1].startDownloadViaWebClient();
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
                    //addItemsToListView(new YouTubeContext(videoItem.Name, videoItem.Path, videoItem.Link), 100, true);
                }
            }
        }
        /// <summary>Событие клика на локальную кнопку удалить</summary>
        /// <param name="LocalButtonSender">Кнопка на которую кликнули(На прямую задаваемый экземпляр объекта кнопки, который ивляется издателем этого события)</param>
        /// <param name="sender">Кнопка на которую кликнули(Системно передаваемый при вызове события экземпляр объекта кнопки, который является издателем этого события)</param>
        /// <returns> Метод возращает делегат события, останется только подписаться на событие.</returns>
        private void bttnDel(object LocalButtonSender, EventArgs e)
        {
            int i = listViewExDownloads.IndexItems(LocalButtonSender as Control);
            if (i != -1)
            {
                if (usbd) (new DataContext()).removeDataFromDB(_YTC);
                this._LYTC[i].stopDownloadViaWebClient();
                this._LYTC.RemoveAt(i);
                listViewExDownloads.Items.RemoveAt(i);
            }         
        }
        /// <summary>Событие клика на локальную кнопку удалить</summary>
        /// <param name="LocalButtonSender">Кнопка на которую кликнули(На прямую задаваемый экземпляр объекта кнопки, который ивляется издателем этого события)</param>
        /// <param name="sender">Кнопка на которую кликнули(Системно передаваемый при вызове события экземпляр объекта кнопки, который является издателем этого события)</param>
        /// <returns> Метод возращает делегат события, останется только подписаться на событие.</returns>
        private Action<object, EventArgs> bttnDel(object LocalButtonSender) 
        {
            return delegate(object sender, EventArgs e)
            {
                /// Для данного метода MainButtonSender и LocalButtonSender - одни и теже (локальная кнопка,сгенерировавшая событие, одна и таже)
                /// Это подразумевается, но чтобы убедиться в этом используем проверку на эквивалентность объектов.
                //System.Windows.Forms.MessageBox.Show("sender.Equals(LocalButtonSender) = " + sender.Equals(LocalButtonSender), "");
                if (sender.Equals(LocalButtonSender))
                {
                    int i = listViewExDownloads.IndexItems(sender as Control);
                    if (i != -1)
                    {
                        if (usbd) (new DataContext()).removeDataFromDB(_YTC);
                        this._LYTC[i].stopDownloadViaWebClient();
                        this._LYTC.RemoveAt(i);
                        listViewExDownloads.Items.RemoveAt(i);
                    }
                }
            };
        }
        /// <summary>Событие клика на глобальную кнопку удалить (удалить всё)</summary>
        /// <param name="LocalButtonSender">Кнопка на которую кликнули(На прямую задаваемый экземпляр объекта кнопки, который ивляется издателем этого события)</param>
        /// <param name="MainButtonSender">Кнопка на которую кликнули(Системно передаваемый при вызове события экземпляр объекта кнопки, который является издателем этого события)</param>
        /// <returns> Метод возращает делегат события, останется только подписаться на событие.</returns>
        private Action<object, EventArgs> bttnDel_(object LocalButtonSender)
        {
            return   delegate(object MainButtonSender, EventArgs e)
            {
                //System.Windows.Forms.MessageBox.Show("Object LocalButtonSender = " + LocalButtonSender.ToString() + "\n" + "Name =" + ((Button)LocalButtonSender).Name + "\n", "");
                //System.Windows.Forms.MessageBox.Show("Object MainButtonSender = " + MainButtonSender.ToString() + "\n" + "Name =" + ((Button)MainButtonSender).Name + "\n", "");
                //System.Windows.Forms.MessageBox.Show("MainButtonSender.Equals(LocalButtonSender) = " + MainButtonSender.Equals(LocalButtonSender), "");
                //System.Windows.Forms.MessageBox.Show("MainButtonSender.Equals(MainButtonSender) = " + MainButtonSender.Equals(MainButtonSender), "");
                //
                
                /// Для данного метода MainButtonSender и LocalButtonSender - не одни и теже
                /// Это подразумевается, но чтобы убедиться в этом используем проверку на эквивалентность объектов.
                //System.Windows.Forms.MessageBox.Show("MainButtonSender.Equals(LocalButtonSender) = " + MainButtonSender.Equals(LocalButtonSender), "");
                
                if (!MainButtonSender.Equals(LocalButtonSender))
                {
                    this.bttnDel(LocalButtonSender, null);
                    /*
                    int i = listViewExDownloads.IndexItems(LocalButtonSender as Control);
                    if (i != -1)
                    {//Проверка. Если такой локальной кнопки не существует, то и действие не выполняется.
                        if (usbd) (new DataContext()).removeDataFromDB(_YTC);
                        listViewExDownloads.Items.RemoveAt(i);
                        this._LYTC[i].stopDownloadViaWebClient();
                        this._LYTC.RemoveAt(i);
                        listViewExDownloads.Items.RemoveAt(i);
                    }
                    */
                }
            };
        }
        /// <summary>Событие клика на локальную кнопку остановит/перезапустить</summary>
        /// <param name="LocalButtonSender">Кнопка на которую кликнули(На прямую задаваемый экземпляр объекта кнопки, который ивляется издателем этого события)</param>
        /// <param name="MainButtonSender">Кнопка на которую кликнули(Системно передаваемый при вызове события экземпляр объекта кнопки, который является издателем этого события)</param>
        /// <returns> Метод возращает делегат события, останется только подписаться на событие.</returns>
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
                }//стоп
                else if (((Button)LocalButtonSender).Name == states[1])
                {
                    this._LYTC[i].stopDownloadViaWebClient();
                    this._LYTC[i].startDownloadViaWebClient();
                    ((Button)LocalButtonSender).Image = Properties.Resources.stop;
                    ((Button)LocalButtonSender).Name = states[0];
                }//перезагрузка
                else if (((Button)LocalButtonSender).Name == states[2])
                {
                    if (System.IO.File.Exists(listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(LocalButtonSender as Control), 2).Text))
                        System.Diagnostics.Process.Start(listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(LocalButtonSender as Control), 2).Text);
                    else
                    {
                        MessageBox.Show(this, "Файл " + '"' + listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(LocalButtonSender as Control), 2).Text + '"' + "не найден.",
                            "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private Action<object, EventArgs> bttnReload_(object LocalButtonSender)
        {
            
            return delegate(object MainButtonSender, EventArgs e)
            {
                this.bttnReload(LocalButtonSender, null);
                /*
                if (((Button)LocalButtonSender).Name != (((Button)MainButtonSender).Name))     
                {
                    
                }
                button2Name = (((Button)MainButtonSender).Name);
                */
                
            };
        }
        private void addItemsToListView(/*YouTubeContext _YTC,*/int i, int _proc, bool _completed = false)
        {
            
            string _name = _LYTC[i].Name;
            string _path = _LYTC[i].Path;
            string _link = _LYTC[i].Link;
            Label label = new Label();
                label.Text = _path;
            //Создаём локальную кнопку удалить
            Button buttonDel = new Button();
                buttonDel.Text = "";
                buttonDel.Image = Properties.Resources.cancel;
                buttonDel.Name = "delete";
                //Создаём событие на локальную кнопку удалить (ну и подписываем на это событие локальную кнопку)
                buttonDel.Click += new EventHandler(this.bttnDel);
                //buttonDel.Click += new EventHandler(this.bttnDel(buttonDel));
                //Создаём событие на глобыльную кнопку удалить "удалить всё"(ну и подписываем на это событие глобальную кнопку "кнопку удалить всё")
                this.button1.Click += new EventHandler(this.bttnDel_(buttonDel));
                //для подключения в трее использовать ....Click += new EventHandler(this.button1_Click);
            
            Button buttonReload = new Button();
                buttonReload.Text = "";
                buttonReload.Image = Properties.Resources.stop;
                buttonReload.Name = states[0];//В имени статус
                buttonReload.Click += new EventHandler(this.bttnReload);
                this.button2.Click += new EventHandler(this.bttnReload_(buttonReload));
                //для подключения в трее использовать ....Click += new EventHandler(this.button2_Click);


            TextBox textBox = new TextBox();
                textBox.ReadOnly = true;
                textBox.Text = _link;
            ProgressBar progressBar = new ProgressBar();
            this._LYTC[i].SetProgressBarAction(
                (Action<object, System.Net.DownloadProgressChangedEventArgs>)
                delegate(object sender, System.Net.DownloadProgressChangedEventArgs e)
                {progressBar.Value = e.ProgressPercentage;}
            );
            listViewExDownloads.Items.Add(_name);
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
        
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (button2Name == states[0])
            {
                button2.Image = Properties.Resources.reload_icon;
            //    button2Name = states[1];
            }
            //перезагрузка
            else if (button2Name == states[1])
            {
                button2.Image = Properties.Resources.stop;
              //  button2Name = states[0];
            }
            //открыть
            else if (button2.Name == states[2])
            {
                System.Diagnostics.Process.Start(listViewExDownloads.GetEmbeddedControl(listViewExDownloads.IndexItems(sender as Control), 2).Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
