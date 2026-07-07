using Microsoft.AspNetCore.Mvc;
using FundFlowNXT.API.Models;

namespace FundFlowNXT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreasuryController : ControllerBase
    {
        private static List<BankTransaction> _transactions = new List<BankTransaction>
        {
            new BankTransaction { Id=1, Description="Donor Payment", Amount=50000, Date=DateTime.Now, Type="Credit", IsCleared=false, BankAccount="Main Account" },
            new BankTransaction { Id=2, Description="Office Rent", Amount=15000, Date=DateTime.Now, Type="Debit", IsCleared=false, BankAccount="Main Account" },
            new BankTransaction { Id=3, Description="Grant Received", Amount=200000, Date=DateTime.Now, Type="Credit", IsCleared=false, BankAccount="Grants Account" },
            new BankTransaction { Id=4, Description="Staff Salary", Amount=75000, Date=DateTime.Now, Type="Debit", IsCleared=false, BankAccount="Main Account" },
            new BankTransaction { Id=5, Description="Equipment Purchase", Amount=25000, Date=DateTime.Now, Type="Debit", IsCleared=false, BankAccount="Main Account" },
        };

        // GET all transactions
        [HttpGet]
        public ActionResult<List<BankTransaction>> GetAll()
        {
            return Ok(_transactions);
        }

        // GET uncleared transactions only
        [HttpGet("uncleared")]
        public ActionResult<List<BankTransaction>> GetUncleared()
        {
            var uncleared = _transactions.Where(t => !t.IsCleared).ToList();
            return Ok(uncleared);
        }

        // BULK CLEAR — this fixes the real FE NXT pain point
        [HttpPut("bulkclear")]
        public ActionResult BulkClear([FromBody] List<int> ids)
        {
            var cleared = 0;
            foreach (var id in ids)
            {
                var transaction = _transactions.FirstOrDefault(t => t.Id == id);
                if (transaction != null)
                {
                    transaction.IsCleared = true;
                    cleared++;
                }
            }
            return Ok($"{cleared} transactions cleared successfully.");
        }

        // GET balance summary
        [HttpGet("balance")]
        public ActionResult GetBalance()
        {
            var totalCredits = _transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount);
            var totalDebits = _transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount);
            var balance = totalCredits - totalDebits;
            var unclearedCount = _transactions.Count(t => !t.IsCleared);

            return Ok(new
            {
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                Balance = balance,
                UnclearedTransactions = unclearedCount
            });
        }
    }
}