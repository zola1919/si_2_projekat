using Microsoft.EntityFrameworkCore;
using DatabaseEntityLib;
using System.Reflection.Emit;

namespace DataBaseContext
{
    public class DB_Context_Class : DbContext
    {
        public DbSet<Predmet> Predmet { get; set; }
        public DbSet<Pitanje> Pitanje { get; set; }
        public DbSet<Oblast> Oblast { get; set; }
        public DB_Context_Class(DbContextOptions<DB_Context_Class> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Predmet>()
                .Property(p => p.Name)
                .IsRequired();


            modelBuilder.Entity<Pitanje>()
                .Property(u => u.Difficulty)
                .IsRequired();
            modelBuilder.Entity<Pitanje>()
                .Property(u => u.Question)
                .IsRequired();
            modelBuilder.Entity<Pitanje>()
                .Property(u => u.Answer)
                .IsRequired();
            modelBuilder.Entity<Pitanje>()
              .Property(u => u.OblastID)
              .IsRequired();
            modelBuilder.Entity<Pitanje>()
               .Property(u => u.PredmetID)
               .IsRequired();


            modelBuilder.Entity<Oblast>()
               .Property(o => o.Name)
               .IsRequired();
            modelBuilder.Entity<Oblast>()
               .Property(o => o.PredmetID)
               .IsRequired();


            modelBuilder.Entity<Predmet>()
                .HasMany(s => s.PredmetnaPitanja).WithOne(p => p.Predmet).HasForeignKey(p => p.PredmetID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Predmet>()
                .HasMany(s => s.OblastPoPredmetima).WithOne(p => p.Predmet).HasForeignKey(p => p.PredmetID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Oblast>()
                .HasMany(s => s.PredmetnaOblast).WithOne(p => p.Oblast).HasForeignKey(p => p.OblastID)
                .OnDelete(DeleteBehavior.Cascade);

            /* modelBuilder.Entity<Ispit>()
                 .HasMany(i => i.Prijave).WithOne(p => p.Ispit).HasForeignKey(p => p.IspitID)
                 .OnDelete(DeleteBehavior.Cascade);

             modelBuilder.Entity<IspitniRok>()
                 .HasMany(i => i.IspitnePrijave).WithOne(p => p.IspitniRok).HasForeignKey(p => p.IspitniRokID)
                 .OnDelete(DeleteBehavior.Cascade);
             */
        }
    }
}