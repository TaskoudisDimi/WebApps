using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //receive some options from ctor, and pass them to the base class which is DbContext

        }

        //Set name of table "categories"
        public DbSet<Category> Categories { get; set; }
    }
}
