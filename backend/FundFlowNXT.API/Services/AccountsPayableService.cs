using FundFlowNXT.API.Models;
using FundFlowNXT.API.Repositories;

namespace FundFlowNXT.API.Services
{
    public class AccountsPayableService : IAccountsPayableService
    {
        private readonly IAccountsPayableRepository _repository;

        public AccountsPayableService(IAccountsPayableRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<Invoice>> GetByStatusAsync(string status)
        {
            var invoices = await _repository.GetAllAsync();
            return invoices
                .Where(i => i.Status.ToLower() == status.ToLower())
                .ToList();
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            // Business rule: new invoices always start in the Pending state
            invoice.Status = "Pending";
            return await _repository.AddAsync(invoice);
        }

        public async Task<Invoice?> UpdateInvoiceAsync(int id, Invoice invoice)
        {
            return await _repository.UpdateAsync(id, invoice);
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<Invoice?> ApproveInvoiceAsync(int id)
        {
            var invoice = await _repository.GetByIdAsync(id);
            if (invoice == null) return null;

            // Business rule: a paid invoice cannot be approved again
            if (invoice.Status == "Paid")
                throw new InvalidOperationException("Invoice is already paid");

            invoice.Status = "Approved";
            await _repository.UpdateAsync(id, invoice);
            return invoice;
        }

        public async Task<Invoice?> PayInvoiceAsync(int id)
        {
            var invoice = await _repository.GetByIdAsync(id);
            if (invoice == null) return null;

            // Business rule: only approved invoices can be paid
            if (invoice.Status != "Approved")
                throw new InvalidOperationException("Invoice must be approved before payment");

            invoice.Status = "Paid";
            await _repository.UpdateAsync(id, invoice);
            return invoice;
        }

        public async Task<object> GetAgingReportAsync()
        {
            var invoices = await _repository.GetAllAsync();
            return new
            {
                TotalInvoices = invoices.Count,
                TotalPending = invoices.Count(i => i.Status == "Pending"),
                TotalApproved = invoices.Count(i => i.Status == "Approved"),
                TotalPaid = invoices.Count(i => i.Status == "Paid"),
                TotalAmountPending = invoices
                    .Where(i => i.Status != "Paid")
                    .Sum(i => i.Amount)
            };
        }
    }
}
