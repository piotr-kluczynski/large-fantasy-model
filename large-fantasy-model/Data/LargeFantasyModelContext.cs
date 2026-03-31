using large_fantasy_model.Models;
using large_fantasy_model.Models.Character;
using large_fantasy_model.Models.Character.Additional;
using large_fantasy_model.Models.Character.References;
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

        // Postacie gracza
        public DbSet<Character> Characters { get; set; }

        public DbSet<CharacterProficiency> Proficiencies { get; set; }
        public DbSet<CharacterRace> Races { get; set; }
        public DbSet<CharacterClass> Classes { get; set; }
        public DbSet<CharacterEquipment> Equipment { get; set; }
        public DbSet<CharacterWeapon> Weapons { get; set; }
        public DbSet<CharacterSpell> Spells { get; set; }
        public DbSet<CharacterFeat> Feats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacja jeden-do-wielu pomiędzy User-Message (jeden użytkownik może być autorem wielu wiadomości)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // Relacja jeden-do-wielu pomiędzy User-Game (jeden użytkownik może być właścicielem wielu gier)
            modelBuilder.Entity<Game>()
                .HasOne(g => g.User)
                .WithMany(u => u.OwnedGames)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Konfiguracja modelu Postaci
            ConfigureCharacter(modelBuilder);
        }

        private void ConfigureCharacter(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Character>();

            // Pomocnicze tabele (istnieją niezależnie od tabeli Character)
            entity.HasOne(c => c.Race)
                 .WithOne(cr => cr.Character)
                 .HasForeignKey<CharacterRace>(cr => cr.CharacterId)
                 .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Classes)
                .WithOne()
                .HasForeignKey(cc => cc.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Equipment)
                .WithOne()
                .HasForeignKey(ce => ce.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Weapons)
                .WithOne()
                .HasForeignKey(cw => cw.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Spells)
                .WithOne()
                .HasForeignKey(s => s.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Feats)
                .WithOne()
                .HasForeignKey(f => f.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Proficiencies)
                .WithOne(p => p.Character)
                .HasForeignKey(p => p.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.DamageTypes)
                .WithOne(d => d.Character)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Owned Entity - uwzględniane w tabeli Character
            entity.OwnsOne(c => c.Currency, cc =>
            {
                cc.Property(c => c.Copper).HasColumnName("Currency_Copper");
                cc.Property(c => c.Silver).HasColumnName("Currency_Silver");
                cc.Property(c => c.Electrum).HasColumnName("Currency_Electrum");
                cc.Property(c => c.Gold).HasColumnName("Currency_Gold");
                cc.Property(c => c.Platinum).HasColumnName("Currency_Platinum");
            });
            entity.OwnsOne(c => c.Background, bg =>
            {
                bg.Property(b => b.Name).HasColumnName("Background_Name");
                bg.Property(b => b.Option).HasColumnName("Background_Option");
                bg.Property(b => b.Description).HasColumnName("Background_Description");
            });
            entity.OwnsOne(c => c.Details, d =>
            {
                d.Property(x => x.Age)
                    .HasColumnName("Details_Age");

                d.Property(x => x.Eyes)
                    .HasColumnName("Details_Eyes");

                d.Property(x => x.Hair)
                    .HasColumnName("Details_Hair");

                d.Property(x => x.Skin)
                    .HasColumnName("Details_Skin");

                d.Property(x => x.Weight)
                    .HasColumnName("Details_Weight");

                d.Property(x => x.Height)
                    .HasColumnName("Details_Height");

                d.Property(x => x.Personality)
                    .HasColumnName("Details_Personality");

                d.Property(x => x.Ideal)
                    .HasColumnName("Details_Ideal");

                d.Property(x => x.Bond)
                    .HasColumnName("Details_Bond");

                d.Property(x => x.Flaw)
                    .HasColumnName("Details_Flaw");

                d.Property(x => x.Backstory)
                    .HasColumnName("Details_Backstory");

                d.Property(x => x.Physical)
                    .HasColumnName("Details_Physical");
            });
            entity.OwnsOne(c => c.Creature, creature =>
            {
                creature.Property(x => x.Name);
                creature.Property(x => x.Alignment);
                creature.Property(x => x.Inspiration);
                creature.Property(x => x.Shield);

                creature.OwnsOne(x => x.Speed);
                creature.OwnsOne(x => x.HitPoints);
                creature.OwnsOne(x => x.Skills);
                creature.OwnsOne(x => x.AbilityScores);
                creature.OwnsOne(x => x.SavingThrows);
                creature.OwnsOne(x => x.Senses);
                creature.OwnsOne(x => x.ArmorClass);
                creature.OwnsOne(x => x.Conditions);

                creature.OwnsMany(x => x.Languages, b =>
                {
                    b.Property(p => p).HasColumnName("Language");
                });

                creature.OwnsMany(x => x.ConditionImmunities, b =>
                {
                    b.Property(p => p).HasColumnName("ConditionImmunity");
                });

                creature.OwnsMany(x => x.DamageImmunities, b =>
                {
                    b.Property(p => p).HasColumnName("DamageImmunity");
                });

                creature.OwnsMany(x => x.DamageResistances, b =>
                {
                    b.Property(p => p).HasColumnName("DamageResistance");
                });

                creature.OwnsMany(x => x.Vulnerabilities, b =>
                {
                    b.Property(p => p).HasColumnName("Vulnerability");
                });
            });
        }
    }   
}
