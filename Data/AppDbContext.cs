using Microsoft.EntityFrameworkCore;
using ReLogin.Models;


namespace ReLogin.Data
{
    public class AppDbContext : DbContext
    {
       

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Books> Books { set; get; }
        public DbSet<User> Users { set; get; }
        public DbSet<Token> Tokens { set; get; }
    }
}
