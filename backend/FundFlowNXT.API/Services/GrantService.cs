using FundFlowNXT.API.Models;
using FundFlowNXT.API.Repositories;

namespace FundFlowNXT.API.Services
{
    public class GrantService : IGrantService
    {
        private readonly IGrantRepository _repository;

        public GrantService(IGrantRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Grant>> GetAllGrantsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Grant?> GetGrantByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Grant> CreateGrantAsync(Grant grant)
        {
            // Business rule: spent amount cannot start higher than the total
            if (grant.SpentAmount > grant.TotalAmount)
            {
                throw new InvalidOperationException(
                    "Spent amount cannot exceed the total grant amount.");
            }
            return await _repository.AddAsync(grant);
        }

        public async Task<Grant?> UpdateGrantAsync(int id, Grant grant)
        {
            return await _repository.UpdateAsync(id, grant);
        }

        public async Task<bool> DeleteGrantAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<object> GetSummaryAsync()
        {
            var grants = await _repository.GetAllAsync();
            return new
            {
                TotalGrants = grants.Count,
                ActiveGrants = grants.Count(g => g.Status == "Active"),
                OverspentGrants = grants.Count(g => g.SpentAmount > g.TotalAmount),
                TotalFunding = grants.Sum(g => g.TotalAmount),
                TotalSpent = grants.Sum(g => g.SpentAmount),
                TotalRemaining = grants.Sum(g => g.TotalAmount - g.SpentAmount)
            };
        }
    }
}