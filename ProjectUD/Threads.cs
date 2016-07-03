using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Threading;
using System.Windows.Forms;

namespace ProjectUD
{
    public class Threads
    {//https://www.youtube.com/watch?v=i0dYLOoT1kg
        #region делигаты вроде как
        public Action<List<object>> _ui;
        #endregion
        /// <summary>СписокID</summary>
        public List<int> ListID = new List<int>();
        /// <summary>СписокСостояний</summary>
        public List<bool> ListYTCStatys = new List<bool>();
        /// <summary>СписокПотоковЗагрузок</summary>
        public List<System.Threading.Thread> ListDownloadThreads = new List<Thread>();
        /// <summary>СписокЮтубКонтекстов</summary>
        public List<YouTubeContext> LYTC = new List<YouTubeContext>();
        public Threads() { }
        /// <summary>Конструктор+ПолучалкаДелигатов</summary>
        public Threads(Action<List<object>> _ui_ui)
        {
            _ui = _ui_ui;
        }
        public void AddNewYTCDownload(YouTubeContext YTC)
        {
            int NewId = 0;
            {//Пытаемся сгенерировать новый неповторяющийся айдишник=); 
                //задачи могут добавляться и удаляться, но различать их нужно всегда
                Random Rand = new Random();
                bool flag = true;
                do
                {
                    flag = true;
                    NewId = Rand.Next(-2147483640, 2147483640);
                    foreach (int qse in this.ListID)
                        flag = flag && (qse != NewId);
                } while (flag == false);
            }
            this.ListID.Add(NewId);
            this.LYTC.Add(YTC);
            this.ListYTCStatys.Add(true);//true-значит процесс не отменён
            //Запускаем поток загрузки файла
            System.Threading.Thread tread = new System.Threading.Thread(_MultiThread);
            tread.IsBackground = true;
            //tread.Priority = System.Threading.ThreadPriority.Lowest;
            ListDownloadThreads.Add(tread);
            ListDownloadThreads[ListDownloadThreads.Count() - 1].Start();
        }
        public bool HallmarkOk(int _Hallmark)
        {
            foreach (int qwd in this.ListID)
                if (_Hallmark == qwd) return true;
            return false;
        }
        public void AbortThread(int _Hallmark)
        {
            if (HallmarkOk(_Hallmark))
            {
                int i = this.GetI(_Hallmark);
                this.ListYTCStatys[i] = false;
                try { this.ListDownloadThreads[i].Abort(); }
                catch { }
            }
        }
        //public void AbortOllThreads(int _Hallmark){foreach (int qwe in this.ListID)this.AbortThread(qwe);}
        public void RemoveAt(int _Hallmark)
        {
            if (HallmarkOk(_Hallmark))
            {
                int i = this.GetI(_Hallmark);
                try { this.ListDownloadThreads[i].Abort(); }
                catch { }
                this.ListDownloadThreads.RemoveAt(i);
                this.ListYTCStatys.RemoveAt(i);
                this.LYTC.RemoveAt(i);
                this.ListID.RemoveAt(i);

            }
        }
        public void MultiThread()
        {
            int ThreadIdHallmark = this.ListID[this.ListID.Count() - 1];
            //теперь мы знаем кто мы (_Hallmark)
            //Начинаем скачивать.
            this.LYTC[ThreadIdHallmark].startDownload();
        }
        public int GetI(int ThreadIdHallmark)
        {
            int i = 0;
            for (i = 0; i < this.ListID.Count(); i++)
                if (this.ListID[i] == ThreadIdHallmark)
                    break;
            return i;
        }
        public void ProgresSeter(int _Hallmark, int progres)
        {
            if (HallmarkOk(_Hallmark))
            {

            }
        }
        public void _MultiThread()
        {
            int ThreadIdHallmark = this.ListID[this.ListID.Count() - 1];
            //теперь мы знаем кто мы (_Hallmark)
            //Начинаем скачивать.
            //this.LYTC[_Hallmark].startDownload();
            {
                int i = GetI(ThreadIdHallmark);
                {
                    object[] qwe = { i, this.LYTC[i], 50 };
                    _ui(qwe.ToList());
                }
                {
                    object[] qwe = { i, this.LYTC[i], 50 };
                    _ui(qwe.ToList());
                }
                this.LYTC[i].startDownload();
                //Thread.Sleep(2000);
            }
            {
                //MessageBox.Show("_+_", "_-_");
            }
        }
    }
}
