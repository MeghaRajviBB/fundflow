using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Grant> Grants { get; set; }
        public DbSet<Budget> Budgets { get; set; }
    }
}