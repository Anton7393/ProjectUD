using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProjectUD
{
    public enum DownloadStatus
    {
        Active,
        Failure,
        Completed
    }

    public class VideoData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Link { get; set; }
        public string Date { get; set; }
        public int FormatCode { get; set; }
        public string DirectLink { get; set; }
        public DownloadStatus Status { get; set; }
    }

    class DataContext : DbContext
    {
        public DataContext()
            : base("DbConnection")
        {

        }

        public void addDataToDB(VideoData _videoData)
        {
            this.VideoDatas.Add(_videoData);
            this.SaveChanges();
        }

        public void removeDataFromDB(VideoData _videoData)
        {
            var infoToRemove = _videoData.Date;
            var itemToRemove = this.VideoDatas.Where(row => row.Date == infoToRemove);

            if (itemToRemove != null)
            {
                this.VideoDatas.RemoveRange(itemToRemove);
                this.SaveChanges();
            }
        }

        public List<VideoData> getDataFromDB()
        {
            List<VideoData> tempList = new List<VideoData>();
            var datas = this.VideoDatas.OrderByDescending(p => p.Date);

            foreach (var data in datas)
            {
                tempList.Add(data);
            }
            return tempList;
        }

        public void clearDB()
        {
            var rows = this.VideoDatas;

            foreach (var row in rows)
            {
                this.VideoDatas.Remove(row);
            }
            this.SaveChanges();
        }

        public DbSet<VideoData> VideoDatas { get; set; }
    }

}
