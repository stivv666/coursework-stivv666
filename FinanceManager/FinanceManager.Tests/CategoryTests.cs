using NUnit.Framework;
using FinanceManager.Domain;
using FinanceManager.Services;
using FinanceManager.Exceptions;
using FinanceManager.DataAccess;

namespace FinanceManager.Tests
{
    public class CategoryTests
    {
        private FinanceService _financeService;
        private readonly string _catFile = "test_categories.json";
        private readonly string _transFile = "test_transactions.json";

        [SetUp]
        public void Setup()
        {
            CleanUpFiles();
            _financeService = new FinanceService(
                new FileRepository<Transaction>(_transFile),
                new FileRepository<Category>(_catFile)
            );
        }

        [TearDown]
        public void TearDown() => CleanUpFiles();

        private void CleanUpFiles()
        {
            if (File.Exists(_catFile)) File.Delete(_catFile);
            if (File.Exists(_transFile)) File.Delete(_transFile);
        }

        [Test]
        public void AddCategory_ValidData_SavesSuccessfully()
        {
            _financeService.AddCategory(new Category("Продукти", 1000m));

            Assert.That(_financeService.GetAllCategories().Count(), Is.EqualTo(1));
            Assert.That(_financeService.GetAllCategories().First().Name, Is.EqualTo("Продукти"));
        }

        [Test]
        public void GetAllCategories_ReturnsMultipleSavedCategories()
        {
            _financeService.AddCategory(new Category("Продукти", 1000m));
            _financeService.AddCategory(new Category("Транспорт", 500m));

            Assert.That(_financeService.GetAllCategories().Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddCategory_EmptyName_ThrowsFinanceException()
        {
            Assert.Throws<FinanceException>(() => _financeService.AddCategory(new Category("", 1000m)));
        }

        [Test]
        public void AddCategory_NullName_ThrowsFinanceException()
        {
            Assert.Throws<FinanceException>(() => _financeService.AddCategory(new Category(null, 1000m)));
        }
    }
}