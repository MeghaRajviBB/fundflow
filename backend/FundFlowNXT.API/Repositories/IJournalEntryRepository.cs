using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public interface IJournalEntryRepository
    {
        Task<List<JournalEntry>> GetAllAsync();
        Task<JournalEntry?> GetByIdAsync(int id);
        Task<JournalEntry> AddAsync(JournalEntry entry);
        Task<JournalEntry?> UpdateAsync(int id, JournalEntry entry);
        Task<bool> DeleteAsync(int id);
    }
}
