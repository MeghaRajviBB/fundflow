using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Data;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _context;

        public BudgetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Budget>> GetAllAsync()
        {
            return await _context.Budgets.ToListAsync();
        }

        public async Task<Budget?> GetByIdAsync(int id)
        {
            return await _context.Budgets.FindAsync(id);
        }

        public async Task<Budget> AddAsync(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return budget;
        }

        public async Task<Budget?> UpdateAsync(int id, Budget budget)
        {
            var existing = await _context.Budgets.FindAsync(id);
            if (existing == null) return null;

            existing.Department = budget.Department;
            existing.Category = budget.Category;
            existing.BudgetedAmount = budget.BudgetedAmount;
            existing.ActualAmount = budget.ActualAmount;
            existing.FiscalYear = budget.FiscalYear;
            existing.Quarter = budget.Quarter;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Budgets.FindAsync(id);
            if (existing == null) return false;

            _context.Budgets.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
