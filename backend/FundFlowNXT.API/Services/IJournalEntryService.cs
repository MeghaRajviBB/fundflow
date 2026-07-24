using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Services
{
    public interface IJournalEntryService
    {
        Task<List<JournalEntry>> GetAllEntriesAsync();
        Task<JournalEntry?> GetEntryByIdAsync(int id);
        Task<JournalEntry> CreateEntryAsync(JournalEntry entry);
        Task<JournalEntry?> UpdateEntryAsync(int id, JournalEntry entry);
        Task<bool> DeleteEntryAsync(int id);
        AnomalyResult AnalyzeEntry(JournalEntry entry);
        Task<object> ScanAnomaliesAsync();
    }
}
