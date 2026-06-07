using large_fantasy_model.Models;
using large_fantasy_model.Models.CharacterModels;
using large_fantasy_model.Models.CharacterModels.Additional;
using large_fantasy_model.Models.CharacterModels.References;
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
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<CharacterRace> Races { get; set; }
        public DbSet<CharacterClass> Classes { get; set; }
        public DbSet<CharacterEquipment> Equipment { get; set; }
        public DbSet<CharacterWeapon> Weapons { get; set; }
        public DbSet<CharacterSpell> Spells { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<GameChatMessage> GameChatMessages { get; set; }
        public DbSet<CharacterFeature> Features { get; set; }

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

            // Relacja jeden-do-wielu pomiędzy User-Character (jeden użytkownik może być właścicielem wielu postaci)
            modelBuilder.Entity<Character>()
                .HasOne(c => c.User)
                .WithMany(u => u.Characters)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja modelu Postaci
            ConfigureCharacter(modelBuilder);
        }

        private void ConfigureCharacter(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Character>();

            // Pomocnicze tabele (istnieją niezależnie od tabeli Character)
            entity.HasOne(c => c.Race) // CharacterRace
                 .WithOne(cr => cr.Character)
                 .HasForeignKey<CharacterRace>(cr => cr.CharacterId)
                 .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Class) // CharacterClass
                .WithOne()
                .HasForeignKey<CharacterClass>(cc => cc.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Equipment) // Character Equipment
                .WithOne()
                .HasForeignKey(ce => ce.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Weapons) // Character Weapon
                .WithOne()
                .HasForeignKey(cw => cw.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Spells) // CharacterSpells
                .WithOne()
                .HasForeignKey(s => s.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Features) // Character Features
                .WithOne()
                .HasForeignKey(f => f.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Owned Entity - uwzględniane w tabeli Character
            entity.OwnsOne(c => c.Skills, sk =>
            {
                sk.Property(s => s.Athletics).HasColumnName("Skill_Athletics");
                sk.Property(s => s.Acrobatics).HasColumnName("Skill_Acrobatics");
                sk.Property(s => s.SleightOfHand).HasColumnName("Skill_SleightOfHand");
                sk.Property(s => s.Stealth).HasColumnName("Skill_Stealth");
                sk.Property(s => s.Arcana).HasColumnName("Skill_Arcana");
                sk.Property(s => s.History).HasColumnName("Skill_History");
                sk.Property(s => s.Investigation).HasColumnName("Skill_Investigation");
                sk.Property(s => s.Nature).HasColumnName("Skill_Nature");
                sk.Property(s => s.Religion).HasColumnName("Skill_Religion");
                sk.Property(s => s.AnimalHandling).HasColumnName("Skill_AnimalHandling");
                sk.Property(s => s.Insight).HasColumnName("Skill_Insight");
                sk.Property(s => s.Medicine).HasColumnName("Skill_Medicine");
                sk.Property(s => s.Perception).HasColumnName("Skill_Perception");
                sk.Property(s => s.Survival).HasColumnName("Skill_Survival");
                sk.Property(s => s.Deception).HasColumnName("Skill_Deception");
                sk.Property(s => s.Intimidation).HasColumnName("Skill_Intimidation");
                sk.Property(s => s.Performance).HasColumnName("Skill_Performance");
                sk.Property(s => s.Persuasion).HasColumnName("Skill_Persuasion");
            });
            entity.OwnsOne(c => c.SavingThrows, st =>
            {
                st.Property(s => s.Strength).HasColumnName("SavingThrow_Strength");
                st.Property(s => s.Dexterity).HasColumnName("SavingThrow_Dexterity");
                st.Property(s => s.Constitution).HasColumnName("SavingThrow_Constitution");
                st.Property(s => s.Intelligence).HasColumnName("SavingThrow_Intelligence");
                st.Property(s => s.Wisdom).HasColumnName("SavingThrow_Wisdom");
                st.Property(s => s.Charisma).HasColumnName("SavingThrow_Charisma");
            });
            entity.OwnsOne(c => c.AbilityScores, ab =>
            {
                ab.Property(a => a.Strength).HasColumnName("AbilityScores_Strength");
                ab.Property(a => a.Dexterity).HasColumnName("AbilityScores_Dexterity");
                ab.Property(a => a.Constitution).HasColumnName("AbilityScores_Constitution");
                ab.Property(a => a.Intelligence).HasColumnName("AbilityScores_Intelligence");
                ab.Property(a => a.Wisdom).HasColumnName("AbilityScores_Wisdom");
                ab.Property(a => a.Charisma).HasColumnName("AbilityScores_Charisma");
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
        }
    }   
}
