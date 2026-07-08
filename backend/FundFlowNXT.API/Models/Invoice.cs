namespace FundFlowNXT.API.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public bool IsOverdue => !Status.Equals("Paid") && DueDate < DateTime.Now;
    }
}