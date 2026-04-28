using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManager.Domain
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public decimal? BudgetLimit { get; set; }

        public Category(string name, decimal? budgetLimit = null)
        {
            Name = name;
            BudgetLimit = budgetLimit;
        }
    }
}
