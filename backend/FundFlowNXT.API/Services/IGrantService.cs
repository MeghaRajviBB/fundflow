using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Services
{
    public interface IGrantService
    {
        Task<List<Grant>> GetAllGrantsAsync();
        Task<Grant?> GetGrantByIdAsync(int id);
        Task<Grant> CreateGrantAsync(Grant grant);
        Task<Grant?> UpdateGrantAsync(int id, Grant grant);
        Task<bool> DeleteGrantAsync(int id);
        Task<object> GetSummaryAsync();
    }
}