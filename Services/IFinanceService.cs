using System;
using System.Collections.Generic;
using System.Text;
using FinanceManager.Domain;

namespace FinanceManager.Services
{ 
    public interface IFinanceService
    {
        event Action<string> OnBudgetExceeded;

        void AddTransaction(Transaction transaction);
        IEnumerable<Transaction> GetAllTransactions();
        IEnumerable <Transaction> GetTransactionsByCategory(Guid categoryId);
        decimal GetTotalBalance();
        void AddCategory(Category category);
        IEnumerable<Category> GetAllCategories();
    }
}
