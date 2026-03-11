using large_fantasy_model.Models;
using Microsoft.EntityFrameworkCore;

namespace large_fantasy_model.Data
{
    public class LargeFantasyModelContext : DbContext
    {
        public LargeFantasyModelContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }


        // Relacja jeden-do-wielu pomiędzy Conversation-User (jedna gra może mieć wielu użytkowników)
        public DbSet<Conversation> Conversation { get; set; }

        // Relacja jeden-do-wielu pomiędzy User-Message (jeden użytkownik może być autorem wielu wiadomości)
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacja wiele-do-wielu pomiędzy User-User (wiele użytkowników może być przyjaciółmi wielu użytkowników)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany(u => u.FriendOf);

            // Relacja wiele-do-wielu pomiędzy Game-User (wielu użytkowników może brać udział w wielu grach)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Games)
                .WithMany(g => g.Users);

            // Relacja wiele-do-wielu pomiędzy Conversation-User (wielu użytkowników może uczestniczyć w wielu konwersacjach)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Conversations)
                .WithMany(c => c.Users);
        }
    }   
}
