using System;
using System.Net;
using System.ComponentModel;

namespace ProjectUD
{
    class Downloader
    {
        public event EventHandler<System.Net.DownloadProgressChangedEventArgs> DownloadProgressChanged;
        public event EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DownloadFileCompleted;

        public Downloader(VideoData _videoData)
        {
            mVideoData = _videoData;
        }

        public void startDownload()
        {
            
            this.mClient = new WebClient();
            mClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
            mClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
            this.mClient.DownloadFileAsync(new Uri(mVideoData.DirectLink), mVideoData.Path, mVideoData); 
        }

        public void stopDownload()
        {
            this.mClient.CancelAsync();
        }

        public void abortDownload()
        {
            this.mClient.CancelAsync();
            this.mClient.Dispose();
        }

        public void setAsActive()
        {
            mVideoData.Status = DownloadStatus.Active;
        }

        public void setAsFailure()
        {
            mVideoData.Status = DownloadStatus.Failure;
        }

        public string getDate()
        {
            return mVideoData.Date;
        }

        public string getPath()
        {
            return mVideoData.Path;
        }

        public DownloadStatus getStatus()
        {
            return mVideoData.Status;
        }

        private WebClient mClient;
        private VideoData mVideoData;
    }
}
