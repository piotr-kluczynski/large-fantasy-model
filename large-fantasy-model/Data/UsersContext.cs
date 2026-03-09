using large_fantasy_model.Models;
using Microsoft.EntityFrameworkCore;

namespace large_fantasy_model.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
