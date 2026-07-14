using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Services
{
    public class AnomalyResult
    {
        public bool HasAnomaly { get; set; }
        public string Severity { get; set; } = "None";
        public string Explanation { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
    }

    public class AnomalyDetectionService
    {
        // Restricted funds that can only be used for their designated purpose
        private readonly List<string> _restrictedFunds = new()
        {
            "Education Fund", "Grants Account", "Restricted Fund"
        };

        public AnomalyResult AnalyzeJournalEntry(JournalEntry entry)
        {
            // Check 1 — is the entry balanced?
            if (entry.DebitAmount != entry.CreditAmount)
            {
                var difference = Math.Abs(entry.DebitAmount - entry.CreditAmount);
                return new AnomalyResult
                {
                    HasAnomaly = true,
                    Severity = "High",
                    Explanation = $"This entry is unbalanced. The debit amount (₹{entry.DebitAmount}) does not equal the credit amount (₹{entry.CreditAmount}), a difference of ₹{difference}.",
                    Recommendation = "In double-entry accounting, every debit must have an equal credit. Review the entry and correct the mismatched amount before posting."
                };
            }

            // Check 2 — large transaction on a restricted fund
            if (_restrictedFunds.Contains(entry.Fund) && entry.DebitAmount > 100000)
            {
                return new AnomalyResult
                {
                    HasAnomaly = true,
                    Severity = "Medium",
                    Explanation = $"This is a large transaction (₹{entry.DebitAmount}) against '{entry.Fund}', which is a donor-restricted fund.",
                    Recommendation = "Large movements on restricted funds should be verified against the grant agreement to ensure the spend is permitted. Confirm with your grants officer."
                };
            }

            // Check 3 — zero-value entry
            if (entry.DebitAmount == 0 && entry.CreditAmount == 0)
            {
                return new AnomalyResult
                {
                    HasAnomaly = true,
                    Severity = "Low",
                    Explanation = "This entry has zero value on both debit and credit sides.",
                    Recommendation = "Zero-value entries are usually data-entry errors. Verify whether this entry is intentional."
                };
            }

            // No anomaly
            return new AnomalyResult
            {
                HasAnomaly = false,
                Severity = "None",
                Explanation = "This entry is balanced and follows standard accounting rules. No anomalies detected.",
                Recommendation = "No action needed."
            };
        }
    }
}