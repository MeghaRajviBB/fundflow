using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Data;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public class TreasuryRepository : ITreasuryRepository
    {
        private readonly AppDbContext _context;

        public TreasuryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BankTransaction>> GetAllAsync()
        {
            return await _context.BankTransactions.ToListAsync();
        }

        public async Task<BankTransaction?> GetByIdAsync(int id)
        {
            return await _context.BankTransactions.FindAsync(id);
        }

        public async Task<BankTransaction> AddAsync(BankTransaction transaction)
        {
            _context.BankTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<BankTransaction?> UpdateAsync(int id, BankTransaction transaction)
        {
            var existing = await _context.BankTransactions.FindAsync(id);
            if (existing == null) return null;

            existing.Description = transaction.Description;
            existing.Amount = transaction.Amount;
            existing.Date = transaction.Date;
            existing.Type = transaction.Type;
            existing.IsCleared = transaction.IsCleared;
            existing.BankAccount = transaction.BankAccount;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.BankTransactions.FindAsync(id);
            if (existing == null) return false;

            _context.BankTransactions.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
