using System;
namespace FinanceManager.Exceptions
{
    public class FinanceException : Exception
    {
        public FinanceException(string message) : base(message) { }
    }
}