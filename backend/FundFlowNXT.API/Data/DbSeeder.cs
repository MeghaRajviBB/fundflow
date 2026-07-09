using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Only seed if the database is empty
            if (context.JournalEntries.Any() || context.BankTransactions.Any())
                return;

            // Journal Entries
            context.JournalEntries.AddRange(
                new JournalEntry { Description = "Office Supplies", DebitAmount = 5000, CreditAmount = 5000, Date = DateTime.Now.AddDays(-10), Fund = "Operations Fund" },
                new JournalEntry { Description = "Donor Contribution", DebitAmount = 100000, CreditAmount = 100000, Date = DateTime.Now.AddDays(-8), Fund = "General Fund" },
                new JournalEntry { Description = "Education Grant Disbursement", DebitAmount = 50000, CreditAmount = 50000, Date = DateTime.Now.AddDays(-5), Fund = "Education Fund" }
            );

            // Bank Transactions
            context.BankTransactions.AddRange(
                new BankTransaction { Description = "Donor Payment", Amount = 50000, Date = DateTime.Now.AddDays(-9), Type = "Credit", IsCleared = false, BankAccount = "Main Account" },
                new BankTransaction { Description = "Office Rent", Amount = 15000, Date = DateTime.Now.AddDays(-7), Type = "Debit", IsCleared = false, BankAccount = "Main Account" },
                new BankTransaction { Description = "Grant Received", Amount = 200000, Date = DateTime.Now.AddDays(-6), Type = "Credit", IsCleared = false, BankAccount = "Grants Account" },
                new BankTransaction { Description = "Staff Salary", Amount = 75000, Date = DateTime.Now.AddDays(-3), Type = "Debit", IsCleared = false, BankAccount = "Main Account" },
                new BankTransaction { Description = "Equipment Purchase", Amount = 25000, Date = DateTime.Now.AddDays(-2), Type = "Debit", IsCleared = false, BankAccount = "Main Account" }
            );

            // Invoices
            context.Invoices.AddRange(
                new Invoice { VendorName = "Office Depot", Description = "Office Supplies", Amount = 15000, InvoiceDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(-5), Status = "Pending" },
                new Invoice { VendorName = "Azure Cloud", Description = "Cloud Services", Amount = 25000, InvoiceDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(10), Status = "Approved" },
                new Invoice { VendorName = "Cleaning Co", Description = "Office Cleaning", Amount = 8000, InvoiceDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(-20), Status = "Pending" },
                new Invoice { VendorName = "Tech Support Ltd", Description = "IT Services", Amount = 35000, InvoiceDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(15), Status = "Paid" }
            );

            // Grants
            context.Grants.AddRange(
                new Grant { GrantName = "Education Initiative", FunderName = "Gates Foundation", TotalAmount = 500000, SpentAmount = 320000, StartDate = DateTime.Now.AddMonths(-6), EndDate = DateTime.Now.AddMonths(6), Status = "Active" },
                new Grant { GrantName = "Healthcare Outreach", FunderName = "Government of India", TotalAmount = 200000, SpentAmount = 210000, StartDate = DateTime.Now.AddMonths(-12), EndDate = DateTime.Now.AddMonths(1), Status = "Active" },
                new Grant { GrantName = "Women Empowerment", FunderName = "UN Foundation", TotalAmount = 350000, SpentAmount = 100000, StartDate = DateTime.Now.AddMonths(-3), EndDate = DateTime.Now.AddMonths(9), Status = "Active" }
            );

            // Budgets
            context.Budgets.AddRange(
                new Budget { Department = "Education", Category = "Salaries", BudgetedAmount = 300000, ActualAmount = 280000, FiscalYear = 2026, Quarter = "Q1" },
                new Budget { Department = "Operations", Category = "Equipment", BudgetedAmount = 150000, ActualAmount = 175000, FiscalYear = 2026, Quarter = "Q1" },
                new Budget { Department = "Healthcare", Category = "Supplies", BudgetedAmount = 200000, ActualAmount = 120000, FiscalYear = 2026, Quarter = "Q1" }
            );

            context.SaveChanges();
        }
    }
}