using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public interface ITreasuryRepository
    {
        Task<List<BankTransaction>> GetAllAsync();
        Task<BankTransaction?> GetByIdAsync(int id);
        Task<BankTransaction> AddAsync(BankTransaction transaction);
        Task<BankTransaction?> UpdateAsync(int id, BankTransaction transaction);
        Task<bool> DeleteAsync(int id);
    }
}
