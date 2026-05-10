using System;
using System.Linq;
using FinanceManager.Domain;
using FinanceManager.Services;
using FinanceManager.Exceptions;

namespace FinanceManager.UI
{
    public class ConsoleManager
    {
        private readonly IFinanceService _financeService;

        public ConsoleManager(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================");
                Console.WriteLine("          ФІНАНСОВИЙ МЕНЕДЖЕР            ");
                Console.WriteLine("=========================================");
                Console.ResetColor();

                Console.WriteLine("1. Переглянути всі транзакції (Таблиця)");
                Console.WriteLine("2. Додати категорію");
                Console.WriteLine("3. Додати дохід");
                Console.WriteLine("4. Додати витрату");
                Console.WriteLine("5. Показати загальний баланс");
                Console.WriteLine("0. Вихід");
                Console.Write("\nОберіть опцію: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ShowTransactionsTable();
                            break;
                        case "2":
                            AddCategoryUi();
                            break;
                        case "3":
                            AddTransactionUi(TransactionType.Income);
                            break;
                        case "4":
                            AddTransactionUi(TransactionType.Expense);
                            break;
                        case "5":
                            ShowBalanceUi();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Невідома команда!");
                            break;
                    }
                }
                catch (FinanceException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[ПОМИЛКА]: {ex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[КРИТИЧНА ПОМИЛКА]: {ex.Message}");
                    Console.ResetColor();
                }

                Console.WriteLine("\nНатисни будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        private void ShowTransactionsTable()
        {
            var transactions = _financeService.GetAllTransactions().ToList();
            var categories = _financeService.GetAllCategories().ToList();

            Console.Clear();
            Console.WriteLine("╔════════════╦══════════════════════╦═══════════════╦════════════════════════════════╗");
            Console.WriteLine("║ Дата       ║ Категорія            ║ Сума          ║ Опис                           ║");
            Console.WriteLine("╠════════════╬══════════════════════╬═══════════════╬════════════════════════════════╣");

            if (!transactions.Any())
            {
                Console.WriteLine("║ ТРАНЗАКЦІЙ ПОКИ НЕМАЄ                                                              ║");
            }
            else
            {
                foreach (var t in transactions)
                {
                    var catName = categories.FirstOrDefault(c => c.Id == t.CategoryId)?.Name ?? "Невідомо";

                    string dateStr = t.Date.ToString("dd.MM.yyyy").PadRight(10);
                    string catStr = catName.Length > 20 ? catName.Substring(0, 17) + "..." : catName.PadRight(20);
                    string amountStr = (t.Type == TransactionType.Income ? "+" : "-") + t.Amount.ToString("0.00");
                    amountStr = amountStr.PadRight(13);
                    string descStr = t.Description.Length > 30 ? t.Description.Substring(0, 27) + "..." : t.Description.PadRight(30);

                    Console.Write($"║ {dateStr} ║ {catStr} ║ ");

                    if (t.Type == TransactionType.Income) Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(amountStr);
                    Console.ResetColor();

                    Console.WriteLine($" ║ {descStr} ║");
                }
            }
            Console.WriteLine("╚════════════╩══════════════════════╩═══════════════╩════════════════════════════════╝");
        }

        private void AddCategoryUi()
        {
            Console.Write("Введи назву категорії: ");
            string name = Console.ReadLine();

            Console.Write("Введи ліміт бюджету (або натисни Enter, якщо без ліміту): ");
            string limitInput = Console.ReadLine();
            decimal? limit = string.IsNullOrWhiteSpace(limitInput) ? (decimal?)null : decimal.Parse(limitInput);

            var category = new Category(name, limit);
            _financeService.AddCategory(category);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Категорію успішно додано!");
            Console.ResetColor();
        }

        private void AddTransactionUi(TransactionType type)
        {
            var categories = _financeService.GetAllCategories().ToList();
            if (!categories.Any())
            {
                throw new FinanceException("Спочатку створіть хоча б одну категорію!");
            }

            Console.WriteLine("Доступні категорії:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            }

            Console.Write("Оберіть номер категорії: ");
            int catIndex = int.Parse(Console.ReadLine()) - 1;
            var selectedCategory = categories[catIndex];

            Console.Write("Введіть суму: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.Write("Введіть опис: ");
            string desc = Console.ReadLine();

            var transaction = new Transaction(selectedCategory.Id, type, amount, desc);
            _financeService.AddTransaction(transaction);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Транзакцію успішно додано!");
            Console.ResetColor();
        }

        private void ShowBalanceUi()
        {
            decimal balance = _financeService.GetTotalBalance();
            Console.Write("\nПОТОЧНИЙ БАЛАНС: ");
            if (balance > 0) Console.ForegroundColor = ConsoleColor.Green;
            else if (balance < 0) Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"{balance:C}");
            Console.ResetColor();
        }
    }
}