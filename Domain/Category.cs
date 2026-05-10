using System;
using FinanceManager.Validation;

namespace FinanceManager.Domain
{
    public class Category : BaseEntity
    {
        [RequiredString("The category name cannot be empty.")]
        public string Name { get; set; }

        [PositiveValue("The budget must be greater than zero.")]
        public decimal? BudgetLimit { get; set; }

        public Category(string name, decimal? budgetLimit = null)
        {
            Name = name;
            BudgetLimit = budgetLimit;
        }
    }
}