namespace FundFlowNXT.API.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public DateTime Date { get; set; }
        public string Fund { get; set; } = string.Empty;
        public bool IsBalanced => DebitAmount == CreditAmount;
    }
}