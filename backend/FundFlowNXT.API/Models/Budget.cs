namespace FundFlowNXT.API.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal BudgetedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public int FiscalYear { get; set; }
        public string Quarter { get; set; } = string.Empty;
        public decimal Variance => BudgetedAmount - ActualAmount;
        public bool IsOverBudget => ActualAmount > BudgetedAmount;
        public decimal UtilizationPercent => BudgetedAmount == 0 ? 0
            : Math.Round((ActualAmount / BudgetedAmount) * 100, 2);
    }
}