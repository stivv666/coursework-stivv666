using System;
using FinanceManager.Domain;
using FinanceManager.Exceptions;
using FinanceManager.DataAccess;
using FinanceManager.Validation;

namespace FinanceManager.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IRepository<Transaction> _transactionRepo;
        private readonly IRepository<Category> _categoryRepo;

        public event Action<string> OnBudgetExceeded;

        public FinanceService(IRepository<Transaction> transactionRepo, IRepository<Category> categoryRepo)
        {
            _transactionRepo = transactionRepo;
            _categoryRepo = categoryRepo;
        }

        public void AddCategory(Category category)
        {
            var validationErrors = Validator.Validate(category);
            if (validationErrors.Any())
            {
                throw new FinanceException(string.Join("\n", validationErrors));
            }

            _categoryRepo.Add(category);
        }

        public IEnumerable<Category> GetAllCategories() => _categoryRepo.GetAll();

        public void AddTransaction(Transaction transaction)
        {
            var validationErrors = Validator.Validate(transaction);
            if (validationErrors.Any())
            {
                throw new FinanceException(string.Join("\n", validationErrors));
            }

            if (transaction.Type == TransactionType.Expense)
            {
                CheckBudgetLimit(transaction);
            }

            _transactionRepo.Add(transaction);
        }

        public void DeleteTransaction(Guid id)
        {
            _transactionRepo.Delete(id);
        }

        public IEnumerable<Transaction> GetAllTransactions() => _transactionRepo.GetAll();

        public IEnumerable<Transaction> GetTransactionsByCategory(Guid categoryId)
        {
            return _transactionRepo.GetAll().Where(t => t.CategoryId == categoryId);
        }

        public decimal GetTotalBalance()
        {
            var transactions = _transactionRepo.GetAll();
            var income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var expense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

            return income - expense;
        }

        private void CheckBudgetLimit(Transaction newExpense)
        {
            var category = _categoryRepo.GetAll().FirstOrDefault(c => c.Id == newExpense.CategoryId);

            if (category != null && category.BudgetLimit.HasValue)
            {
                var currentExpenses = GetTransactionsByCategory(category.Id)
                    .Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);

                if (currentExpenses + newExpense.Amount > category.BudgetLimit.Value)
                {
                    OnBudgetExceeded?.Invoke($"Budget exceeded for category '{category.Name}'. Limit: {category.BudgetLimit.Value:C}, Current: {currentExpenses:C}, New Expense: {newExpense.Amount:C}");
                }
            }
        }
    }
}