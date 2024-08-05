using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.DB
{
    public class AppDb : IdentityDbContext<User>
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>()
                .HasMany(a => a.SenderTransactions)
                .WithOne(t => t.SenderAccount)
                .HasForeignKey(t => t.SenderAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Account>()
                .HasMany(a => a.ReceiverTransactions)
                .WithOne(t => t.ReceiverAccount)
                .HasForeignKey(t => t.ReceiverAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .Property(t => t.PaymentChannel)
                .HasConversion(
                    v => v.ToString(),
                    v => (PaymentChannel)Enum.Parse(typeof(PaymentChannel), v)
                );
        }
    }
}
