using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimpleServer
{
    public class SerContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public SerContext() { }
        public SerContext(DbContextOptions<SerContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Message>()
             .HasKey(m => m.Id);

            modelBuilder.Entity<Message>()
                .Property(m => m.Text)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .Property(m => m.TimeSend)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Message>()
                .Property(m => m.UserIP)
                .IsRequired();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=ep-icy-frost-a455v4s9-pooler.us-east-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_okyhW5lf0wJE");
        }
    }
}
