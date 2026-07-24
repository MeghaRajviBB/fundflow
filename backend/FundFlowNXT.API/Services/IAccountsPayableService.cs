using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Services
{
    public interface IAccountsPayableService
    {
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task<List<Invoice>> GetByStatusAsync(string status);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task<Invoice?> UpdateInvoiceAsync(int id, Invoice invoice);
        Task<bool> DeleteInvoiceAsync(int id);
        Task<Invoice?> ApproveInvoiceAsync(int id);
        Task<Invoice?> PayInvoiceAsync(int id);
        Task<object> GetAgingReportAsync();
    }
}
