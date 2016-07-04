using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoLibrary;

namespace ProjectUD
{
    public class YouTubeContext
    {
        private YouTube mYouTube;
        private YouTubeVideo mSelectedVideo;
        private List<YouTubeVideo> mVideoList;
        private List<YouTubeVideo> mSortedVideoList;
        private Dictionary<string, YouTubeVideo> mVideoDictionary;
        private WebClient mClient = new WebClient();
        private Action<object, System.Net.DownloadProgressChangedEventArgs> mActionProgressBar = delegate(object sender, System.Net.DownloadProgressChangedEventArgs e) { };
        public void SetProgressBarAction(Action<object, System.Net.DownloadProgressChangedEventArgs> _mActionProgressBar){this.mActionProgressBar+=_mActionProgressBar;}
        
        public YouTubeContext(string Name, string Path, string Link)
        {
            this.Name = Name;
            this.Path = Path;
            this.Link = Link;
            mYouTube = YouTube.Default;
            ResolutionList = new List<string>();
            mVideoList = new List<YouTubeVideo>();
            mSortedVideoList = new List<YouTubeVideo>();
            mVideoDictionary = new Dictionary<string, YouTubeVideo>();
            mSelectedVideo = null;
        }
        public YouTubeContext()
        {
            mYouTube = YouTube.Default;
            ResolutionList = new List<string>();
            mVideoList = new List<YouTubeVideo>();
            mSortedVideoList = new List<YouTubeVideo>();
            mVideoDictionary = new Dictionary<string, YouTubeVideo>();
            mSelectedVideo = null;
        }

        public void extractYouTubeMeta(string _link)
        {
            Link = _link;
            mVideoList = mYouTube.GetAllVideos(Link).ToList();
            Title = mVideoList[0].Title;
            buildResolutionList();
        }

        public void selectVideoQualuty(int _index)
        {
            mSelectedVideo = mSortedVideoList[_index];
            Format = mSelectedVideo.Format.ToString();
            Resolution = mSelectedVideo.Resolution;
            FileExtention = mSelectedVideo.FileExtension;
        }

        public void pathBuilder()
        {
            string pattern = @"(\\)$";
            var filter = new Regex(pattern);
            if (!(filter.IsMatch(Path)))
            {
                Path += @"\";
            }
            Name = fileNameBuilder(Name);
            Path += Name;
        }

        public void startDownload()
        {
            Date = DateTime.Now;
            File.WriteAllBytes(Path, mSelectedVideo.GetBytes());
        }

        public void startDownloadViaWebClient()
        {
            Date = DateTime.Now;
            this.mClient = new WebClient();
            this.mClient.DownloadFileAsync(new Uri(mSelectedVideo.Uri), Path);
            mClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(mActionProgressBar);
        }
        public void stopDownloadViaWebClient()
        {            
            this.mClient.CancelAsync();
        }

        public List<string> ResolutionList { get; private set; }
        public string FileExtention { get; private set; }
        public int Resolution { get; private set; }
        public string Format { get; private set; }
        public DateTime Date { get; private set; }
        public string Title { get; private set; }
        public string Link { get; private set; }
        public string Path { get; set; }
        public string Name { get; set; }    

        private void buildResolutionList()
        {
            foreach(var video in mVideoList)
            {
                var formatCode = video.FormatCode;
                var extention = video.FileExtension;
                var resolution = video.Resolution;
                var key = resolution.ToString() + "p -- " + extention.ToString();

                if (isMainStreamVideoFormat(formatCode))
                {
                    if (mVideoDictionary.ContainsKey(key)) { continue; }
                    mVideoDictionary.Add(key, video);
                }
            }
            compileResolutionAndVideoLists();
        }

        private void compileResolutionAndVideoLists()
        {
            ResolutionList.Clear();
            mSortedVideoList.Clear();

            foreach (var videoItem in mVideoDictionary)
            {
                ResolutionList.Add(videoItem.Key);
                mSortedVideoList.Add(videoItem.Value);
            }
        }

        private string fileNameBuilder(string _name)
        {
            string pattern = @"[^\.]+";
            var filter = new Regex(pattern);
            var newName = filter.Match(_name).Groups[0].Value;

            newName += FileExtention;

            return newName;
        }

        private void addDataToDB()
        {
            throw new NotImplementedException();
        }

        private bool isMainStreamVideoFormat(int _formatCode)
        {
            if (_formatCode <= 46) { return true; }
            return false;
        }

        private bool is3DVideoFormat(int _formatCode)
        {
            if (((_formatCode >= 82) && (_formatCode <= 85)) ||
                ((_formatCode >= 100) && (_formatCode <= 102)))
            { return true; }
            return false;
        }

        private bool isDashMP4Video(int _formatCode)
        {
            if (((_formatCode >= 133) && (_formatCode <= 138)) ||
                ((_formatCode >= 298) && (_formatCode <= 299)) ||
                (_formatCode == 160) ||
                (_formatCode == 264) ||
                (_formatCode == 266))
            { return true; }
            return false;
        }

        private bool isDashWEBMVideo(int _formatCode)
        {
            if (((_formatCode >= 167) && (_formatCode <= 170)) ||
                ((_formatCode >= 218) && (_formatCode <= 219)) ||
                ((_formatCode >= 242) && (_formatCode <= 248)) ||
                ((_formatCode >= 271) && (_formatCode <= 272)) ||
                ((_formatCode >= 302) && (_formatCode <= 303)) ||
                (_formatCode == 308) ||
                (_formatCode == 313) ||
                (_formatCode == 315))
            { return true; }
            return false;
        }
    }
}
