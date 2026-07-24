using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Data;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public class GrantRepository : IGrantRepository
    {
        private readonly AppDbContext _context;

        public GrantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Grant>> GetAllAsync()
        {
            return await _context.Grants.ToListAsync();
        }

        public async Task<Grant?> GetByIdAsync(int id)
        {
            return await _context.Grants.FindAsync(id);
        }

        public async Task<Grant> AddAsync(Grant grant)
        {
            _context.Grants.Add(grant);
            await _context.SaveChangesAsync();
            return grant;
        }

        public async Task<Grant?> UpdateAsync(int id, Grant grant)
        {
            var existing = await _context.Grants.FindAsync(id);
            if (existing == null) return null;

            existing.GrantName = grant.GrantName;
            existing.FunderName = grant.FunderName;
            existing.TotalAmount = grant.TotalAmount;
            existing.SpentAmount = grant.SpentAmount;
            existing.StartDate = grant.StartDate;
            existing.EndDate = grant.EndDate;
            existing.Status = grant.Status;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Grants.FindAsync(id);
            if (existing == null) return false;

            _context.Grants.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}