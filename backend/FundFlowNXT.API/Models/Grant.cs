namespace FundFlowNXT.API.Models
{
    public class Grant
    {
        public int Id { get; set; }
        public string GrantName { get; set; } = string.Empty;
        public string FunderName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";
        public bool IsOverspent => SpentAmount > TotalAmount;
        public decimal RemainingBalance => TotalAmount - SpentAmount;
        public bool IsExpired => EndDate < DateTime.Now;
    }
}