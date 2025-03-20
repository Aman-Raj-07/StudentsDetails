using First.Models;
using Microsoft.EntityFrameworkCore;

namespace First.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Student> students { get; set; }
    }
}
