using NUnit.Framework;
using FinanceManager.Domain;
using FinanceManager.Services;
using FinanceManager.Exceptions;
using FinanceManager.DataAccess;

namespace FinanceManager.Tests
{
    public class TransactionTests
    {
        private FinanceService _financeService;
        private FileRepository<Category> _categoryRepo;
        private readonly string _catFile = "test_categories.json";
        private readonly string _transFile = "test_transactions.json";

        [SetUp]
        public void Setup()
        {
            CleanUpFiles();
            _categoryRepo = new FileRepository<Category>(_catFile);
            _financeService = new FinanceService(new FileRepository<Transaction>(_transFile), _categoryRepo);
        }

        [TearDown]
        public void TearDown() => CleanUpFiles();

        private void CleanUpFiles()
        {
            if (File.Exists(_catFile)) File.Delete(_catFile);
            if (File.Exists(_transFile)) File.Delete(_transFile);
        }

        [Test]
        public void GetTotalBalance_CalculatesIncomeAndExpensesCorrectly()
        {
            var cat = new Category("Стипендія", null);
            _categoryRepo.Add(cat);

            _financeService.AddTransaction(new Transaction(cat.Id, TransactionType.Income, 2000m, "Прийшли гроші"));
            _financeService.AddTransaction(new Transaction(cat.Id, TransactionType.Expense, 500m, "Шаурма"));

            Assert.That(_financeService.GetTotalBalance(), Is.EqualTo(1500m));
        }

        [Test]
        public void DeleteTransaction_UpdatesBalanceCorrectly()
        {
            var cat = new Category("Різне", null);
            _categoryRepo.Add(cat);

            var transaction = new Transaction(cat.Id, TransactionType.Income, 1000m, "Подарунок");
            _financeService.AddTransaction(transaction);

            _financeService.DeleteTransaction(transaction.Id);

            Assert.That(_financeService.GetTotalBalance(), Is.EqualTo(0m));
        }

        [Test]
        public void AddTransaction_ExceedsBudget_TriggersEvent()
        {
            var category = new Category("Розваги", 500m);
            _categoryRepo.Add(category);

            bool eventTriggered = false;
            _financeService.OnBudgetExceeded += (msg) => eventTriggered = true;

            _financeService.AddTransaction(new Transaction(category.Id, TransactionType.Expense, 600m, "Кіно"));

            Assert.That(eventTriggered, Is.True);
        }

        [Test]
        public void AddTransaction_ExactlyAtBudgetLimit_DoesNotTriggerEvent()
        {
            var category = new Category("Одяг", 1000m);
            _categoryRepo.Add(category);

            bool eventTriggered = false;
            _financeService.OnBudgetExceeded += (msg) => eventTriggered = true;

            _financeService.AddTransaction(new Transaction(category.Id, TransactionType.Expense, 1000m, "Кросівки"));

            Assert.That(eventTriggered, Is.False);
        }

        [Test]
        public void AddTransaction_Income_DoesNotTriggerBudgetEvent()
        {
            var category = new Category("Підробіток", 500m);
            _categoryRepo.Add(category);

            bool eventTriggered = false;
            _financeService.OnBudgetExceeded += (msg) => eventTriggered = true;

            _financeService.AddTransaction(new Transaction(category.Id, TransactionType.Income, 5000m, "Зарплата"));

            Assert.That(eventTriggered, Is.False);
        }

        [Test]
        public void AddTransaction_NegativeAmount_ThrowsFinanceException()
        {
            var category = new Category("Транспорт", 500m);
            _categoryRepo.Add(category);

            Assert.Throws<FinanceException>(() =>
                _financeService.AddTransaction(new Transaction(category.Id, TransactionType.Expense, -50m, "Метро")));
        }
    }
}