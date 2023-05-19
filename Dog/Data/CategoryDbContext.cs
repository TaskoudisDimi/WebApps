using Dog.Models;
using Microsoft.EntityFrameworkCore;

namespace Dog.Data
{
    public class CategoryDbContext : DbContext
    {
        public DbSet<CatViewModel> Categories { get; set; }

        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options) 
        { 

        }


    }
}
