using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
namespace Finanse
{
    public class Transaction
    {
        public int id {  get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public string Comment { get; set; }
    }

    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddTransaction(Transaction transaction)
        {
            string query = "INSERT INTO Bank (creation_date, summary, category, comment) " +
                           "VALUES (@CreationDate, @Summary, @Category, @Comment)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CreationDate", transaction.Date);
                command.Parameters.AddWithValue("@Summary", transaction.Amount);
                command.Parameters.AddWithValue("@Category", transaction.Category);
                command.Parameters.AddWithValue("@Comment", transaction.Comment);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public decimal CalculateBalance()
        {

            decimal balance = 0;
            string query = "SELECT SUM(CASE WHEN summary > 0 THEN summary ELSE 0 END) - " +
                           "SUM(CASE WHEN summary < 0 THEN summary ELSE 0 END) " +
                           "FROM Bank";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            { 

                connection.Open();
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    balance = Convert.ToDecimal(result);
                }
            }

            return balance;
        }

        public List<Transaction> ShowTransactionsByUserId()
        { 
            string query = "SELECT * FROM Bank";
            List<Transaction> transactionList = new List<Transaction>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            { 

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Transaction t = new Transaction();
                        t.id = reader.GetInt32(0);
                        t.Date = reader.GetDateTime(1);
                        t.Amount = reader.GetDecimal(2);
                        t.Category= reader.GetString(3);
                        t.Comment = reader.GetString(4);
                        
                        transactionList.Add(t);
                    }
                }
            }
            return transactionList;
        }
    }
    public class UserInterface
    {
        private DatabaseManager dbManager;

        public UserInterface(string connectionString)
        {
            dbManager = new DatabaseManager(connectionString);
        }

        public List<Transaction> ShowTransactionsForUser()
        { 
            return dbManager.ShowTransactionsByUserId();
        }
        public void AddTransaction(Transaction transaction)
        {
            dbManager.AddTransaction(transaction);
        }

        public void CalculateAndShowBalance()
        {
            decimal balance = dbManager.CalculateBalance();
            Console.WriteLine(balance + " KZT");
        }

    }
}
