using FundFlowNXT.API.Models;
using FundFlowNXT.API.Repositories;

namespace FundFlowNXT.API.Services
{
    public class TreasuryService : ITreasuryService
    {
        private readonly ITreasuryRepository _repository;

        public TreasuryService(ITreasuryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BankTransaction>> GetAllTransactionsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<BankTransaction>> GetUnclearedAsync()
        {
            var transactions = await _repository.GetAllAsync();
            return transactions.Where(t => !t.IsCleared).ToList();
        }

        public async Task<BankTransaction> CreateTransactionAsync(BankTransaction transaction)
        {
            return await _repository.AddAsync(transaction);
        }

        public async Task<BankTransaction?> UpdateTransactionAsync(int id, BankTransaction transaction)
        {
            return await _repository.UpdateAsync(id, transaction);
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<int> BulkClearAsync(List<int> ids)
        {
            var cleared = 0;
            foreach (var id in ids)
            {
                var transaction = await _repository.GetByIdAsync(id);
                if (transaction != null)
                {
                    transaction.IsCleared = true;
                    await _repository.UpdateAsync(id, transaction);
                    cleared++;
                }
            }
            return cleared;
        }

        public async Task<object> GetBalanceAsync()
        {
            var transactions = await _repository.GetAllAsync();
            var totalCredits = transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount);
            var totalDebits = transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount);

            return new
            {
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                Balance = totalCredits - totalDebits,
                UnclearedTransactions = transactions.Count(t => !t.IsCleared)
            };
        }
    }
}
