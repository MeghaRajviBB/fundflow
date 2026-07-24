using FundFlowNXT.API.Models;
using FundFlowNXT.API.Repositories;

namespace FundFlowNXT.API.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _repository;

        public BudgetService(IBudgetRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Budget>> GetAllBudgetsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Budget?> GetBudgetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Budget> CreateBudgetAsync(Budget budget)
        {
            return await _repository.AddAsync(budget);
        }

        public async Task<Budget?> UpdateBudgetAsync(int id, Budget budget)
        {
            return await _repository.UpdateAsync(id, budget);
        }

        public async Task<bool> DeleteBudgetAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<object> GetComparisonAsync()
        {
            var budgets = await _repository.GetAllAsync();
            return budgets.Select(b => new
            {
                b.Department,
                b.Category,
                b.BudgetedAmount,
                b.ActualAmount,
                Variance = b.BudgetedAmount - b.ActualAmount,
                Status = b.ActualAmount > b.BudgetedAmount ? "Over Budget" : "Within Budget"
            }).ToList();
        }

        public async Task<object> GetSummaryAsync()
        {
            var budgets = await _repository.GetAllAsync();
            return new
            {
                TotalBudgeted = budgets.Sum(b => b.BudgetedAmount),
                TotalActual = budgets.Sum(b => b.ActualAmount),
                TotalVariance = budgets.Sum(b => b.BudgetedAmount - b.ActualAmount),
                DepartmentsOverBudget = budgets.Count(b => b.ActualAmount > b.BudgetedAmount),
                DepartmentsWithinBudget = budgets.Count(b => b.ActualAmount <= b.BudgetedAmount)
            };
        }
    }
}
