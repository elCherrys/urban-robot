
using Microsoft.EntityFrameworkCore;
using WebApplication3.Entities;
//Databse para las notas
namespace WebApplication3.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Note> Notes { get; set; }

    }
}
