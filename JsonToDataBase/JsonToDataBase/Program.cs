using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Web;
using System.Text.Json;

namespace JsonToDataBase
{
    class jsonconvert
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string History { get; set; }
    }

    internal class Program
    {

        static void Main(string[] args)
        {

            string file = File.ReadAllText("Data.json");
            var js = JsonSerializer.Deserialize<List<jsonconvert>>(file);

            string connectionString = "Data source = DataBase.db";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string tableS = @"CREATE TABLE Tales(
                    Id INTEGER NOT NULL, 
                    Category TEXT NOT NULL, 
                    Title TEXT NOT NULL, 
                    History TEXT NOT NULL)";
                using (var command = new SQLiteCommand(tableS, connection))
                {
                    command.ExecuteNonQuery();
                }
                string insert = "INSERT INTO Tales (Id, Category, Title, History) VALUES (@Id, @Category, @Title, @History)";
                foreach (var item in js)
                {
                    using (var com = new SQLiteCommand(insert, connection))
                    {
                        com.Parameters.AddWithValue("@Id", item.Id);
                        com.Parameters.AddWithValue("@Category", item.Category);
                        com.Parameters.AddWithValue("@Title", item.Title);
                        com.Parameters.AddWithValue("@History", item.History);
                        com.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Ready");
                Console.ReadLine();
            }
        }
    }
}
