using System.Data.SQLite;
namespace DatabaseLibrary
{
    public class Database
    {
        public SQLiteConnection Connection;

        private static readonly string databaseName = "database";

        public string connectionString {  get; set; }

        string connection;

        public void GetConnection()
        {
            connection = @$"Data Source={databaseName}.db; Version=3";
            connectionString = connection;
        }

        public Database()
        {
            if (!File.Exists($"./{databaseName}.db"))
            {
                Console.WriteLine("File not found. Creating database...");
                SQLiteConnection.CreateFile($"{databaseName}.db");
                GetConnection();

                using (SQLiteConnection con = new SQLiteConnection(connection))
                {
                    SQLiteCommand command = new SQLiteCommand();
                    con.Open();

                    string query = @"CREATE TABLE played_videogames(id INTEGER PRIMARY KEY AUTOINCREMENT, hours_played INTEGER, data TEXT)";
                    command.CommandText = query;
                    command.Connection = con;
                    command.ExecuteNonQuery();

                    con.Close();
                }
            }
            else
            {
                Console.WriteLine("Database found!");
            }

        }
    }
}
