using System;

namespace FinanceManager.Exceptions
{
    public class BudgetExceededException : FinanceException
    {
        public BudgetExceededException(string message) : base(message) { }
    }
}