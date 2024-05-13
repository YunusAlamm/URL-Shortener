using Microsoft.EntityFrameworkCore;
using Models.Url;

namespace Data.UrlContext
{
    public class UrlContext: DbContext
    {
        public UrlContext(DbContextOptions<UrlContext> options): base(options)
        {

        }

        public DbSet<Url> Urls{ get; set; }
    }
    
}