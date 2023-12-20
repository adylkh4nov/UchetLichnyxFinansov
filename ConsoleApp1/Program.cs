using Finanse;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker
{


    public class Program
    {
        static void Main(string[] args)
        {
            StartProgramm();
        }

        public static void StartProgramm()
        {
            string connectionString = @"Data Source=OLZHAS;Initial Catalog=Finanse;User id=sa;Password=123;";

            UserInterface userInterface = new UserInterface(connectionString);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Учет личных финансов");
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Ввести новую транзакцию");
                Console.WriteLine("2. Просмотреть историю транзакций");
                Console.WriteLine("3. Показать общий баланс");
                Console.WriteLine("4. Выход");
                Console.WriteLine("5 Очистить консоль");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Transaction newTransaction = NewTransaction();
                        if(newTransaction.Category == "Error")
                        {
                            Console.WriteLine("Транзакция не проведена: " + newTransaction.Comment);
                            break;
                        }
                        userInterface.AddTransaction(newTransaction);
                        break;
                    case 2:
                        ShowTrasactions(userInterface);
                        break;
                    case 3:
                        userInterface.CalculateAndShowBalance();
                        break;
                    case 4:
                        exit = true;
                        break;
                    case 5:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор");
                        break;
                }
            }
        }
        public static Transaction NewTransaction()
        {
            Transaction newTransaction = new Transaction();
            newTransaction.Date = DateTime.Now.ToUniversalTime();
            try
            {
                Console.WriteLine("Введите сумму:");
                newTransaction.Amount = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Введите категорию:");
                newTransaction.Category = Console.ReadLine();

                Console.WriteLine("Введите комментарий:");
                newTransaction.Comment = Console.ReadLine();

            }
            catch (Exception ex)
            {
                newTransaction.Amount = 0;
                newTransaction.Category = "Error";
                newTransaction.Comment = ex.Message;

                
            }
            return newTransaction;

        }

        public static void ShowTrasactions(UserInterface userInterface)
        {
            List<Transaction> transactions = userInterface.ShowTransactionsForUser();

            foreach (Transaction transaction in transactions)
            {
                Console.WriteLine($"ID: {transaction.id}| Date: {transaction.Date} | Sum: {transaction.Amount} | Category: {transaction.Category} |  Comment: {transaction.Comment}"); 
            }
        }
    }

}
