using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProjectUD
{
    class VideoData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Link { get; set; }
        public int Progress { get; set; }
        public string Date { get; set; }
    }

    class DataContext : DbContext
    {
        public DataContext(): base("DbConnection"){}
        public void addDataToDB(YouTubeContext _youTubeContext, int _progress = 100)
        {
            VideoData data = new VideoData();
            data.Link = _youTubeContext.Link;
            data.Name = _youTubeContext.Name;
            data.Path = _youTubeContext.Path;
            data.Date = _youTubeContext.Date.ToString("yyyy-MM-dd HH:mm:ss");
            data.Progress = _progress;

            this.VideoDatas.Add(data);
            this.SaveChanges();
        }

        public void removeDataFromDB(YouTubeContext _youTubeContext)
        {
            //удаление по дате не очень работает.
            //Нужно додумывать
            //var infoToRemove = _youTubeContext.Date.ToString("yyyy-MM-dd HH:mm:ss");
            //var itemToRemove = this.VideoDatas.Where(row => row.Date == infoToRemove);

            var infoToRemove = _youTubeContext.Name;
            var itemToRemove = this.VideoDatas.Where(row => row.Name == infoToRemove);

            if (itemToRemove != null)
            {
                this.VideoDatas.RemoveRange(itemToRemove);
                this.SaveChanges();
            }
        }

        public DbSet<VideoData> getDataFromDB()
        {
            return VideoDatas;
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
