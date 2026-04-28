using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManager.Domain
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public TransactionType Type { get; set; }
        public Guid CategoryId { get; set; }

        public Transaction(decimal amount, DateTime date, string comment, TransactionType type, Guid categoryId)
        {
            Amount = amount;
            Date = date;
            Comment = comment;
            Type = type;
            CategoryId = categoryId;
        }
    }
}
