namespace FundFlowNXT.API.Models
{
    public class BankTransaction
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsCleared { get; set; } = false;
        public string BankAccount { get; set; } = string.Empty;
    }
}