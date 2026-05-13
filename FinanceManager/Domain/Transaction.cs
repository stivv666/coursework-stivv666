using System;
using FinanceManager.Validation;

namespace FinanceManager.Domain
{
    public class Transaction : BaseEntity
    {
        public Guid CategoryId { get; set; }

        public TransactionType Type { get; set; }

        [PositiveValue("The transaction amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        [RequiredString("Add a description for the transaction.")]
        public string Description { get; set; }

        public Transaction(Guid categoryId, TransactionType type, decimal amount, string description)
        {
            CategoryId = categoryId;
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
            Description = description;
        }
    }
}