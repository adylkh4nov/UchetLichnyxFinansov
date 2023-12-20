using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PersonalFinanceTracker
{
    class Transaction
    {
        public string id { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public string Category { get; set; }
        public string Comment { get; set; }
    }

    public class Dengi
    {
        static List<Transaction> transactions = new List<Transaction>();

        public static void AddTransaction()
        {
            Transaction newTransaction = new Transaction();

            newTransaction.Date = DateTime.Now;

            Console.WriteLine("Укажите сумму:");
            newTransaction.Sum = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Введите категорию:");
            newTransaction.Category = Console.ReadLine();

            Console.WriteLine("Введите комментарий:");
            newTransaction.Comment = Console.ReadLine();

            transactions.Add(newTransaction);
            Console.WriteLine("Создана новая транзакция");
        }

        public static void ShowTransactionHistory()
        {
            Console.WriteLine("История транзакций:");

            foreach (var transaction in transactions)
            {
                Console.WriteLine($"Дата: {transaction.Date} | Id: {transaction.id} | Сумма: {transaction.Sum} | Категория: {transaction.Category} | Комментарий: {transaction.Comment}");
            }
        }

        public static void ShowBalance()
        {
            decimal Dohod = transactions.Where(t => t.Sum > 0).Sum(t => t.Sum);
            decimal Rashod = transactions.Where(t => t.Sum < 0).Sum(t => t.Sum);

            decimal balance = Dohod + Rashod;
            Console.WriteLine($"Доходы: {Dohod}\tРасход: {Rashod}");
            Console.WriteLine($"Общий баланс: {balance}");
        }

        public static void LoadData()
        {
            string Path = Console.ReadLine();
            if (File.Exists(Path))
            {
                using (StreamReader sr = new StreamReader(Path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split('|');
                        Transaction transaction = new Transaction()
                        {
                            Date = DateTime.Parse(data[0]),
                            Sum = decimal.Parse(data[1]),
                            Category = data[2],
                            Comment = data[3]
                        };
                        transactions.Add(transaction);
                    }
                }
            }
        }

        public static void SaveData()
        {
            Console.WriteLine("Укажите путь файла (без расширения)");
            string path = Console.ReadLine();
            using (StreamWriter sw = new StreamWriter(path + ".txt"))
            {
                foreach (var transaction in transactions)
                {
                    sw.WriteLine($"{transaction.Date}|{transaction.Sum}|{transaction.Category}|{transaction.Comment}");
                }
            }
        }
    }
}
