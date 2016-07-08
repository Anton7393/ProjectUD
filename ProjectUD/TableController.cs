using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ProjectUD
{
    static class TableController
    {
        public static event EventHandler<EventArgs> stopButtonClick;
        public static event EventHandler<EventArgs> openButtonClick;
        public static event EventHandler<EventArgs> deleteButtonClick;
        public static event EventHandler<EventArgs> reloadButtonClick;

        private static List<VideoData> mActiveDownloadsList = new List<VideoData>();
        private static List<VideoData> mHistoryDownloadsList = new List<VideoData>();
        private static List<VideoData> mRenderList = new List<VideoData>();
        private static DataContext mContext = new DataContext();

        //Class interface methods

        public static void addNewDownload(this ListViewEx _listViewEx, VideoData _videoData)
        {
            pushDataToActiveList(_videoData);
            setDataAsActive(0);
            createRenderList();
            _listViewEx.renderAll();
        }

        public static void removeDownload(this ListViewEx _listViewEx, int _index)
        {
            removeDataFromRenderList(_index);
            _listViewEx.clearRow(_index);
        }

        public static void stopDownload(this ListViewEx _listViewEx, int _index)
        {
            setDataAsFailure(_index);
            createRenderList();
            _listViewEx.renderAll();
        }

        public static void restartDownload(this ListViewEx _listViewEx, int _index)
        {
            setDataAsActive(_index);
            createRenderList();
            _listViewEx.renderAll();
        }

        public static void completeDownload(this ListViewEx _listViewEx, int _index)
        {
            setDataAsComplete(_index);
            pushDataToHistorylist(mActiveDownloadsList[_index]);
            removeDataFromActiveList(_index);
            createRenderList();
            _listViewEx.renderAll();
        }

        public static void fillFromDB(this ListViewEx _listViewEx)
        {
            prepareActiveAndHistory();
            createHistoryListFromDB();
            createRenderList();
            _listViewEx.renderAll();
        }

        public static void restartAll(this ListViewEx _listViewEx)
        {
            setAllDataAsActive();
            createRenderList();
            _listViewEx.renderAll();
        }

        public static void stopAll(this ListViewEx _listViewEx)
        {
            setAllDataAsFailure();
            createRenderList();
            _listViewEx.renderAll();
        }

        public static List<VideoData> removeAll(this ListViewEx _listViewEx)
        {
            var renderList = new List<VideoData>();
            renderList.AddRange(mRenderList);
            clearAllData();
            _listViewEx.renderAll();
            return renderList;
        }

        public static int getIndex(VideoData _videoData)
        {
            int index = -1;
            index = mRenderList.FindIndex(p => p.Date == _videoData.Date);
            return index;
        }

        //Data control methods
        //This method group is working with Database, Lists and their coherence 

        private static void pushDataToActiveList(VideoData _videoData)
        {
            List<VideoData> tempList = new List<VideoData>();
            tempList.AddRange(mActiveDownloadsList);
            mActiveDownloadsList.Clear();
            mActiveDownloadsList.Add(_videoData);
            mActiveDownloadsList.AddRange(tempList);
        }

        private static void pushDataToHistorylist(VideoData _videoData)
        {
            List<VideoData> tempList = new List<VideoData>();
            tempList.AddRange(mHistoryDownloadsList);
            mHistoryDownloadsList.Clear();
            mHistoryDownloadsList.Add(_videoData);
            mHistoryDownloadsList.AddRange(tempList);
            mContext.addDataToDB(_videoData);
        }

        private static void createHistoryListFromDB()
        {
            mHistoryDownloadsList = mContext.getDataFromDB();
        }

        private static void createRenderList()
        {
            clearRenderList();
            mRenderList.AddRange(mActiveDownloadsList);
            mRenderList.AddRange(mHistoryDownloadsList);
        }

        private static void removeDataFromRenderList(int _index)
        {
            var activeListCount = mActiveDownloadsList.Count;
            var historyIndex = _index - activeListCount;

            mRenderList.RemoveAt(_index);

            if (historyIndex >= 0)
            {
                removeDataFromHistoryList(historyIndex);
            }
            else
            {
                removeDataFromActiveList(_index);
            }
        }

        private static void removeDataFromActiveList(int _index)
        {
            mActiveDownloadsList.RemoveAt(_index);
        }

        private static void removeDataFromHistoryList(int _index)
        {
            var dataToRemove = mHistoryDownloadsList[_index];
            mContext.removeDataFromDB(dataToRemove);
            mHistoryDownloadsList.RemoveAt(_index);
        }

        private static void setDataAsActive(int _index)
        {
            mActiveDownloadsList[_index].Status = DownloadStatus.Active;
        }

        private static void setDataAsFailure(int _index)
        {
            mActiveDownloadsList[_index].Status = DownloadStatus.Failure;
        }

        private static void setDataAsComplete(int _index)
        {
            mActiveDownloadsList[_index].Status = DownloadStatus.Completed;
        }

        private static void setAllDataAsActive()
        {
            foreach(var data in mActiveDownloadsList)
            {
                data.Status = DownloadStatus.Active;
            }
        }

        private static void setAllDataAsFailure()
        {
            foreach (var data in mActiveDownloadsList)
            {
                data.Status = DownloadStatus.Failure;
            }
        }

        private static void clearAllData()
        {
            clearRenderList();
            clearActiveList();
            clearHistory();
        }

        private static void clearHistory()
        {
            clearHistoryList();
            mContext.VideoDatas.RemoveRange(mContext.VideoDatas);
            mContext.SaveChanges();
        }

        private static void clearActiveList()
        {
            mActiveDownloadsList.Clear();
        }

        private static void clearRenderList()
        {
            mRenderList.Clear();
        }

        private static void clearHistoryList()
        {
            mHistoryDownloadsList.Clear();
        }

        private static void prepareActiveAndHistory()
        {
            clearActiveList();
            clearHistoryList();
        }

        //Render methods

        private static void renderAll(this ListViewEx _listViewEx)
        {
            _listViewEx.clearAll();
            foreach (var data in mRenderList)
            {
                _listViewEx.renderRow(data);
            }
        }

        private static void renderRow(this ListViewEx _listViewEx, VideoData _videoData)
        {
            var tag = _videoData;
            Container components = new Container();

            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonStop = new Button();
            Button buttonOpen = new Button();
            Button buttonDelete = new Button();
            Button buttonReload = new Button();
            ProgressBar progressBar = new ProgressBar();

            label.Text = _videoData.Path;

            textBox.ReadOnly = true;
            textBox.Text = _videoData.Link;

            buttonStop.Text = "";
            buttonStop.Image = Properties.Resources.stop;
            buttonStop.Tag = tag;

            buttonOpen.Text = "";
            buttonOpen.Image = Properties.Resources.open;
            buttonOpen.Tag = tag;

            buttonDelete.Text = "";
            buttonDelete.Image = Properties.Resources.cancel;
            buttonDelete.Tag = tag;

            buttonReload.Text = "";
            buttonReload.Image = Properties.Resources.reload_icon;
            buttonReload.Tag = tag;

            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.MarqueeAnimationSpeed = 10;

            _listViewEx.Items.Add(_videoData.Name);

            switch (_videoData.Status)
            {
                case DownloadStatus.Active:
                    _listViewEx.AddEmbeddedControl(buttonStop, 4, _listViewEx.Items.Count - 1);
                    buttonStop.Click += new EventHandler(stopButtonClick);
                    progressBar.Value = 0;
                    break;
                case DownloadStatus.Completed:
                    _listViewEx.AddEmbeddedControl(buttonOpen, 4, _listViewEx.Items.Count - 1);
                    buttonOpen.Click += new EventHandler(openButtonClick);
                    progressBar.Value = 100;

                    break;
                case DownloadStatus.Failure:
                    _listViewEx.AddEmbeddedControl(buttonReload, 4, _listViewEx.Items.Count - 1);
                    buttonReload.Click += new EventHandler(reloadButtonClick);
                    progressBar.Value = 0;
                    break;
                default:
                    throw new Exception("Nothing to do here!");
            }

            _listViewEx.AddEmbeddedControl(buttonDelete, 5, _listViewEx.Items.Count - 1);
            _listViewEx.AddEmbeddedControl(textBox, 2, _listViewEx.Items.Count - 1);
            _listViewEx.AddEmbeddedControl(progressBar, 3, _listViewEx.Items.Count - 1);
            _listViewEx.AddEmbeddedControl(label, 1, _listViewEx.Items.Count - 1);
            buttonDelete.Click += new EventHandler(deleteButtonClick);
            _listViewEx.Update();
        }

        private static void clearAll(this ListViewEx _listViewEx)
        {
            _listViewEx.Items.Clear();
            _listViewEx.removeAllControls();
        }

        private static void clearRow(this ListViewEx _listViewEx, int _index)
        {
            _listViewEx.Items.RemoveAt(_index);
        }
    }
}
