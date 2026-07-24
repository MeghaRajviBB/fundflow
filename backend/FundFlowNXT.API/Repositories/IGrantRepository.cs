using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public interface IGrantRepository
    {
        Task<List<Grant>> GetAllAsync();
        Task<Grant?> GetByIdAsync(int id);
        Task<Grant> AddAsync(Grant grant);
        Task<Grant?> UpdateAsync(int id, Grant grant);
        Task<bool> DeleteAsync(int id);
    }
}