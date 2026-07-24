using Microsoft.EntityFrameworkCore;
using FundFlowNXT.API.Data;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Repositories
{
    public class AccountsPayableRepository : IAccountsPayableRepository
    {
        private readonly AppDbContext _context;

        public AccountsPayableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            return await _context.Invoices.ToListAsync();
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoices.FindAsync(id);
        }

        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice?> UpdateAsync(int id, Invoice invoice)
        {
            var existing = await _context.Invoices.FindAsync(id);
            if (existing == null) return null;

            existing.VendorName = invoice.VendorName;
            existing.Description = invoice.Description;
            existing.Amount = invoice.Amount;
            existing.InvoiceDate = invoice.InvoiceDate;
            existing.DueDate = invoice.DueDate;
            existing.Status = invoice.Status;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Invoices.FindAsync(id);
            if (existing == null) return false;

            _context.Invoices.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
