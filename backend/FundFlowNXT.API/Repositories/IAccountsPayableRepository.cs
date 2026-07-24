using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public interface IAccountsPayableRepository
    {
        Task<List<Invoice>> GetAllAsync();
        Task<Invoice?> GetByIdAsync(int id);
        Task<Invoice> AddAsync(Invoice invoice);
        Task<Invoice?> UpdateAsync(int id, Invoice invoice);
        Task<bool> DeleteAsync(int id);
    }
}
