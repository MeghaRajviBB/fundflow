using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public interface IBudgetRepository
    {
        Task<List<Budget>> GetAllAsync();
        Task<Budget?> GetByIdAsync(int id);
        Task<Budget> AddAsync(Budget budget);
        Task<Budget?> UpdateAsync(int id, Budget budget);
        Task<bool> DeleteAsync(int id);
    }
}
