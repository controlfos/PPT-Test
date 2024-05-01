using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace PPT_Test.Model
{
    public class ImagesContext : DbContext
    {
        public ImagesContext(DbContextOptions<ImagesContext> options) : base(options)
        {
        }

        public DbSet<Images> Images { get; set; }
    }
}
