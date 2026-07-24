using FundFlowNXT.API.Models;
using FundFlowNXT.API.Repositories;

namespace FundFlowNXT.API.Services
{
    public class JournalEntryService : IJournalEntryService
    {
        private readonly IJournalEntryRepository _repository;
        private readonly AnomalyDetectionService _anomalyService;

        public JournalEntryService(IJournalEntryRepository repository, AnomalyDetectionService anomalyService)
        {
            _repository = repository;
            _anomalyService = anomalyService;
        }

        public async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<JournalEntry?> GetEntryByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<JournalEntry> CreateEntryAsync(JournalEntry entry)
        {
            // Business rule: entry must be balanced (debit equals credit)
            if (entry.DebitAmount != entry.CreditAmount)
                throw new InvalidOperationException("Entry is not balanced. Debit must equal Credit.");

            return await _repository.AddAsync(entry);
        }

        public async Task<JournalEntry?> UpdateEntryAsync(int id, JournalEntry entry)
        {
            return await _repository.UpdateAsync(id, entry);
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public AnomalyResult AnalyzeEntry(JournalEntry entry)
        {
            return _anomalyService.AnalyzeJournalEntry(entry);
        }

        public async Task<object> ScanAnomaliesAsync()
        {
            var entries = await _repository.GetAllAsync();
            var flagged = new List<object>();

            foreach (var entry in entries)
            {
                var result = _anomalyService.AnalyzeJournalEntry(entry);
                if (result.HasAnomaly)
                {
                    flagged.Add(new
                    {
                        entry.Id,
                        entry.Description,
                        entry.Fund,
                        entry.DebitAmount,
                        entry.CreditAmount,
                        result.Severity,
                        result.Explanation,
                        result.Recommendation
                    });
                }
            }

            return new
            {
                TotalScanned = entries.Count,
                AnomaliesFound = flagged.Count,
                Anomalies = flagged
            };
        }
    }
}
