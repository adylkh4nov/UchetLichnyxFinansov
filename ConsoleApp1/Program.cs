using Finanse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


            Console.WriteLine("\tЗдравствуйте, хотите загрузить данные из файла? (Yes, либо что угодно если нет)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "yes")
            {
                Bank.LoadData();
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Учет личных финансов");
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Ввести новую транзакцию");
                Console.WriteLine("2. Просмотреть историю транзакций");
                Console.WriteLine("3. Показать общий баланс");
                Console.WriteLine("4. Выход");
                Console.WriteLine("5. Сохранить транзакции в файл (txt)");
                Console.WriteLine("6. Очистить консоль");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Bank.AddTransaction();
                        break;
                    case 2:
                        Bank.ShowTransactionHistory();
                        break;
                    case 3:
                        Bank.ShowBalance();
                        break;
                    case 4:
                        exit = true;
                        break;
                    case 5:
                        Bank.SaveData();
                        break;
                    case 6:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор");
                        break;
                }
            }
        }
    }

}
