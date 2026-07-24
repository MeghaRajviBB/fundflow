using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Data;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public class JournalEntryRepository : IJournalEntryRepository
    {
        private readonly AppDbContext _context;

        public JournalEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<JournalEntry>> GetAllAsync()
        {
            return await _context.JournalEntries.ToListAsync();
        }

        public async Task<JournalEntry?> GetByIdAsync(int id)
        {
            return await _context.JournalEntries.FindAsync(id);
        }

        public async Task<JournalEntry> AddAsync(JournalEntry entry)
        {
            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<JournalEntry?> UpdateAsync(int id, JournalEntry entry)
        {
            var existing = await _context.JournalEntries.FindAsync(id);
            if (existing == null) return null;

            existing.Description = entry.Description;
            existing.DebitAmount = entry.DebitAmount;
            existing.CreditAmount = entry.CreditAmount;
            existing.Date = entry.Date;
            existing.Fund = entry.Fund;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.JournalEntries.FindAsync(id);
            if (existing == null) return false;

            _context.JournalEntries.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
