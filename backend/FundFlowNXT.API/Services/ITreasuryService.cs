using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Services
{
    public interface ITreasuryService
    {
        Task<List<BankTransaction>> GetAllTransactionsAsync();
        Task<List<BankTransaction>> GetUnclearedAsync();
        Task<BankTransaction> CreateTransactionAsync(BankTransaction transaction);
        Task<BankTransaction?> UpdateTransactionAsync(int id, BankTransaction transaction);
        Task<bool> DeleteTransactionAsync(int id);
        Task<int> BulkClearAsync(List<int> ids);
        Task<object> GetBalanceAsync();
    }
}
