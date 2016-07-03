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
    }

    class DataContext : DbContext
    {
        public DataContext()
            : base("DbConnection")
        {

        }

        public DbSet<VideoData> VideoDatas { get; set; }
    }
}
