using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Services
{
    public interface IBudgetService
    {
        Task<List<Budget>> GetAllBudgetsAsync();
        Task<Budget?> GetBudgetByIdAsync(int id);
        Task<Budget> CreateBudgetAsync(Budget budget);
        Task<Budget?> UpdateBudgetAsync(int id, Budget budget);
        Task<bool> DeleteBudgetAsync(int id);
        Task<object> GetComparisonAsync();
        Task<object> GetSummaryAsync();
    }
}
