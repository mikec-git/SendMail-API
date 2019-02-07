using Microsoft.EntityFrameworkCore;
using SendMail.Entities;

namespace SendMail.Data
{
    public class EmailDbContext : DbContext
    {
        public virtual DbSet<DeliveredEmail> DeliveredEmails { get; set; }
        public virtual DbSet<EmailAddress> EmailAddresses { get; set; }
        public virtual DbSet<Email> Emails { get; set; }

        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeds database with dummy data
            //modelBuilder.Seed();
        }
    }
}
