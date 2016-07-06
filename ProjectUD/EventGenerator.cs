using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectUD
{
    /// <summary>Класс - централизованый генератор экшенов</summary>
    public class EventGenerator
    {
        #region Параметры и конструктор метода
        private String[] states = { "stop", "reload", "open" };
        #region ThisFormDataMetods(делигаты для методов доступа к данным на фоме)
        
        private Action<object> listViewExDownloads_Items_RemoveAt;
        private Action<object> ListButton2NameChange_RemoveAt;
        private Action<object> ListButton2NameStates_RemoveAt;
        private Action<object> ListYouTubeContext_RemoveAt;
        private Action<object> ListYouTubeContext_stopDownloadViaWebClient;
        private Action<object> DataContext_removeDataFromDB;
        /*--------*/
        private Func<object, int> listViewExDownloads_IndexItems;
        private Action<object> ListYouTubeContext_startDownloadViaWebClient;
        private Action<object,string> ListButton2NameStatesSet;
        private Action<object,string> toolTip_SetToolTip;
        private Func<object, string> listViewExDownloads_Item_SubItems1_Text;
        /*----------------*/
        private Action<object, bool> ListButton2NameChangeSet;
        private Func<object, bool>  ListButton2NameChangeGet;
        private Func<string> button2NameGet;
        private Action<string> button2NameSet;
        private Func<bool> button2NameChangGet;
        private Func<object, string> ListButton2NameStatesGet;
        
        #endregion
        public EventGenerator() { }
        /// <summary>
        /// Конструктор
        /// </summary>        
        public EventGenerator(
            Action<object> _listViewExDownloads_Items_RemoveAt,
            Action<object> _ListButton2NameChange_RemoveAt,
            Action<object> _ListButton2NameStates_RemoveAt,
            Action<object> _ListYouTubeContext_RemoveAt,
            Action<object> _ListYouTubeContext_stopDownloadViaWebClient,
            Action<object> _DataContext_removeDataFromDB,
            /*--------*/
            Func<object, int> _listViewExDownloads_IndexItems,
            Action<object> _ListYouTubeContext_startDownloadViaWebClient,
            Action<object,string> _ListButton2NameStatesSet,
            Action<object,string> _toolTip_SetToolTip,
            Func<object, string> _listViewExDownloads_Item_SubItems1_Text,
            /*-----------*/
            Action<object, bool> _ListButton2NameChangeSet,
            Func<object, bool>  _ListButton2NameChangeGet,
            Func<string> _button2NameGet,
            Action<string> _button2NameSet,
            Func<bool> _button2NameChangGet,
            Func<object, string> _ListButton2NameStatesGet
            ) 
        {
            this.listViewExDownloads_Items_RemoveAt = _listViewExDownloads_Items_RemoveAt;
            this.ListButton2NameChange_RemoveAt = _ListButton2NameChange_RemoveAt;
            this.ListButton2NameStates_RemoveAt = _ListButton2NameStates_RemoveAt;
            this.ListYouTubeContext_RemoveAt=_ListYouTubeContext_RemoveAt;
            this.ListYouTubeContext_stopDownloadViaWebClient = _ListYouTubeContext_stopDownloadViaWebClient;
            this.DataContext_removeDataFromDB = _DataContext_removeDataFromDB;
            /*--------*/
            this.listViewExDownloads_IndexItems = _listViewExDownloads_IndexItems;
            this.ListYouTubeContext_startDownloadViaWebClient = _ListYouTubeContext_startDownloadViaWebClient;
            this.ListButton2NameStatesSet = _ListButton2NameStatesSet;
            this.toolTip_SetToolTip = _toolTip_SetToolTip;
            this.listViewExDownloads_Item_SubItems1_Text = _listViewExDownloads_Item_SubItems1_Text;
            /*--------*/
            this.ListButton2NameChangeSet = _ListButton2NameChangeSet;
            this.ListButton2NameChangeGet = _ListButton2NameChangeGet;
            this.button2NameGet = _button2NameGet;
            this.button2NameSet = _button2NameSet;
            this.button2NameChangGet = _button2NameChangGet;
            this.ListButton2NameStatesGet = _ListButton2NameStatesGet;
        }
        #endregion 
        #region Сами экшены
        public void ButtonDel(object Sender, EventArgs e)
        {
            DataContext_removeDataFromDB(Sender);
            ListYouTubeContext_stopDownloadViaWebClient(Sender);
            ListYouTubeContext_RemoveAt(Sender);
            ListButton2NameStates_RemoveAt(Sender);
            ListButton2NameChange_RemoveAt(Sender);
            listViewExDownloads_Items_RemoveAt(Sender);
        }
        public Action<object, EventArgs> ButtonDel()
        {
            return delegate(object Sender, EventArgs e)
            {
                DataContext_removeDataFromDB(Sender);
                ListYouTubeContext_stopDownloadViaWebClient(Sender);
                ListYouTubeContext_RemoveAt(Sender);
                ListButton2NameStates_RemoveAt(Sender);
                ListButton2NameChange_RemoveAt(Sender);
                listViewExDownloads_Items_RemoveAt(Sender);
            };
        }
        public Action<object, EventArgs> MainButtonDel(object LocalButtonSender)
        {
            return delegate(object Sender, EventArgs e) 
            {
                if (listViewExDownloads_IndexItems(LocalButtonSender) != -1)
                {
                    this.ButtonDel(LocalButtonSender, null); 
                }
            };
        }
        public void ButtonReload(object ButtonSender, EventArgs e)
        {
            /// Для данного метода sender и LocalButtonSender - одни и теже
            /// Это подразумевается, но чтобы убедиться в этом используем проверку на эквивалентность объектов.
            //System.Windows.Forms.MessageBox.Show("sender.Equals(LocalButtonSender) = " + sender.Equals(LocalButtonSender), "");
            
            if (listViewExDownloads_IndexItems(ButtonSender) != -1)
            {
                if (((Button)ButtonSender).Name == states[0])
                {
                    ListYouTubeContext_stopDownloadViaWebClient(ButtonSender);
                    ((Button)ButtonSender).Image = Properties.Resources.reload_icon;
                    ((Button)ButtonSender).Name = states[1];
                    ListButton2NameStatesSet(ButtonSender,states[1]);
                    toolTip_SetToolTip(ButtonSender, "Перезагрузить");

                }//стоп
                else if (((Button)ButtonSender).Name == states[1])
                {
                    ListYouTubeContext_stopDownloadViaWebClient(ButtonSender);
                    ListYouTubeContext_startDownloadViaWebClient(ButtonSender);
                    ((Button)ButtonSender).Image = Properties.Resources.stop;
                    ((Button)ButtonSender).Name = states[0];
                    ListButton2NameStatesSet(ButtonSender, states[0]);
                    toolTip_SetToolTip(ButtonSender, "Остановить");
                }//перезагрузка
                else if (((Button)ButtonSender).Name == states[2])
                {
                    if (
                        System.IO.File.Exists(
                            listViewExDownloads_Item_SubItems1_Text(ButtonSender))
                        )
                    {
                        System.Diagnostics.Process.Start(
                            listViewExDownloads_Item_SubItems1_Text(ButtonSender));
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Файл " + '"' + 
    listViewExDownloads_Item_SubItems1_Text(ButtonSender) + '"' + "не найден.","Файл не найден",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private Action<object, EventArgs> MainbttnReload(object LocalButtonSender)
        {

            return delegate(object MainButtonSender, EventArgs e)
            {
                while (!button2NameChangGet()) { };
                int i = listViewExDownloads_IndexItems(LocalButtonSender);
                if (i != -1)
                {
                    ListButton2NameChangeSet(LocalButtonSender,false);
                    if ((ListButton2NameStatesGet(LocalButtonSender) == states[0]) &&
                        (button2NameGet() == states[1]))
                    {
                        this.ButtonReload(LocalButtonSender, null);
                    }
                    else
                        if ((ListButton2NameStatesGet(LocalButtonSender) == states[1]) &&
                            (button2NameGet() == states[0]))
                        {
                            this.ButtonReload(LocalButtonSender, null);
                        }
                    ListButton2NameChangeSet(LocalButtonSender, true);
                }
            };
        }
        #endregion
    }
}
