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
        public DateTime Date { get; set; }
    }

    class DataContext : DbContext
    {
        public DataContext()
            : base("DbConnection")
        {

        }

        public void addDataToDB(YouTubeContext _youTubeContext, int _progress = 100)
        {
            VideoData data = new VideoData();
            data.Link = _youTubeContext.Link;
            data.Name = _youTubeContext.Name;
            data.Path = _youTubeContext.Path;
            data.Date = _youTubeContext.Date;
            data.Progress = _progress;

            this.VideoDatas.Add(data);
            this.SaveChanges();
        }

        public void removeDataFromDB(YouTubeContext _youTubeContext)
        {
            var itemToRemove = this.VideoDatas.SingleOrDefault(row => row.Date == _youTubeContext.Date);

            if (itemToRemove != null)
            {
                this.VideoDatas.Remove(itemToRemove);
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
