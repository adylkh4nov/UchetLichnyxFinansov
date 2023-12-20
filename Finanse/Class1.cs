using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace Finanse
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }

        public string Category { get; set; }
        public string Comment { get; set; }
        public Guid id { get; set; }
    }

    public class Bank
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

            newTransaction.id = Guid.NewGuid();

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
            Console.WriteLine($"Доходы: {Dohod} KZT\tРасход: {Rashod} KZT");
            Console.WriteLine($"Общий баланс: {balance} KZT");
        }

        public static void LoadData()
        {
            Console.WriteLine("Укажите путь до файла (без расширения)");
            string Path = Console.ReadLine() + ".txt";
            if (File.Exists(@Path))
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
                            id = Guid.Parse(data[1]),
                            Sum = decimal.Parse(data[2]),
                            Category = data[3],
                            Comment = data[4]
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
                    sw.WriteLine($"{transaction.Date}|{transaction.id}|{transaction.Sum}|{transaction.Category}|{transaction.Comment}");
                }
            }
        }
    }
}
