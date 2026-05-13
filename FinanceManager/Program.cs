using System;
using FinanceManager.DataAccess;
using FinanceManager.Domain;
using FinanceManager.Services;
using FinanceManager.UI;

namespace FinanceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var transactionRepo = new FileRepository<Transaction>("Data/transactions.json");
            var categoryRepo = new FileRepository<Category>("Data/categories.json");

            var financeService = new FinanceService(transactionRepo, categoryRepo);

            financeService.OnBudgetExceeded += (message) =>
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"\n[ЛІМІТ ПЕРЕВИЩЕНО]: {message}\n");
                Console.ResetColor();
            };

            var consoleManager = new ConsoleManager(financeService);
            consoleManager.Run();
        }
    }
}