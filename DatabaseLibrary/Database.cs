using System.Data.SQLite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DatabaseLibrary
{

    public class PlayedVideoGames
    {
        public int Id { get; set; }
        public int HoursPlayed { get; set; }
        public DateTime DatePlayed { get; set; }
    }
    public class Database
    {
        public SQLiteConnection Connection;

        private static readonly string databaseName = "database";

        string connection;

        List<PlayedVideoGames> dbData;
        public Database()
        {
            if (!File.Exists($"./{databaseName}.db"))
            {
                SQLiteConnection.CreateFile($"{databaseName}.db");
                GetConnection();

                using (SQLiteConnection con = new SQLiteConnection(connection))
                {
                    SQLiteCommand command = new SQLiteCommand();
                    con.Open();

                    string query = @"CREATE TABLE played_videogames(id INTEGER PRIMARY KEY AUTOINCREMENT, hours_played INTEGER, date TEXT)";
                    command.CommandText = query;
                    command.Connection = con;
                    command.ExecuteNonQuery();

                    con.Close();
                }
            }

        }

        private int GetHoursPlayed()
        {
            string? input = "";
            int hoursPlayed = 0;

            Console.WriteLine("Please enter how long you have played.");
            input = Console.ReadLine();
            while (!int.TryParse(input, out hoursPlayed) || hoursPlayed < 1)
            {
                Console.WriteLine("Invalid input. Please try again.");
                input = Console.ReadLine();
            }
            return hoursPlayed;
        }

        private string GetDatePlayed()
        {
            string? input = "";
            string date;

            Console.WriteLine("Please enter the date. Enter in this format 'MM/dd/yyyy'.");
            input = Console.ReadLine();
            while (!DateTime.TryParseExact(input, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None,out _))
            {
                Console.WriteLine("Invalid date input. Please input the date in this format 'MM/dd/yyyy");
                input = Console.ReadLine();
            }
            date = input;
            return date;
        }
        public void InsertNewRecord()
        {
            Console.Clear();
            int hoursPlayedInput = GetHoursPlayed();

            string dateInput = GetDatePlayed();

            Add(hoursPlayedInput, dateInput);

            Console.WriteLine("Record successfully added");
        }
        private void Add(int hoursPlayed, string date)
        {
            GetConnection();

            using (SQLiteConnection con = new SQLiteConnection(connection))
            {
                SQLiteCommand command = new SQLiteCommand();
                con.Open();

                string query = $@"INSERT INTO played_videogames (hours_played, date)
                                VALUES ({hoursPlayed},'{date}')";
                command.CommandText = query;
                command.Connection = con;
                command.ExecuteNonQuery();

                con.Close();
            }
        }
        public void ReadAllRecords()
        {
            GetConnection();

            using (SQLiteConnection con = new SQLiteConnection(connection))
            {
                SQLiteCommand command = new SQLiteCommand();
                con.Open();

                string query = @"SELECT * FROM played_videogames";
                command.CommandText = query;
                command.Connection = con;

                SQLiteDataReader reader = command.ExecuteReader();
                dbData = new List<PlayedVideoGames>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dbData.Add(
                            new PlayedVideoGames()
                            {
                                Id = reader.GetInt32(0),
                                HoursPlayed = reader.GetInt32(1),
                                DatePlayed = DateTime.ParseExact(reader.GetString(2), "MM/dd/yyyy", new CultureInfo("en-US"))
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                con.Close();
            }

            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("ID\t\thours played\t\tdate");
            for (int i = 0; i < dbData.Count; i++)
            {
                Console.WriteLine($"{dbData[i].Id}\t\t{dbData[i].HoursPlayed}\t\t\t{dbData[i].DatePlayed.ToString("MM/dd/yyyy")}");
                if (i < dbData.Count - 1)
                {
                    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                }
            }
            Console.WriteLine("-----------------------------------------------------------------");
            Console.ReadKey();
        }

        private int GetID()
        {
            int id = 0;
            string? choice = "";
            ReadAllRecords();
            Console.WriteLine("Type the id of the record you wish to update.");
            choice = Console.ReadLine();
            while (!int.TryParse(choice, out id) || (id > dbData.Count || id < 1))
            {
                Console.WriteLine("Input is invalid. Please try again.");
                choice = Console.ReadLine();
            }
            return id;
        }

        private bool DoesIDExist(int id)
        {
            bool recordExists = false;

            for (int i = 0; i < dbData.Count; i++)
            {
                if (dbData[i].Id == id)
                {
                    recordExists = true;
                }
            }
            if (recordExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateRecord()
        {
            int id = GetID();
            if (!DoesIDExist(id))
            {
                Console.Clear();
                Console.WriteLine("Record not found. Please try again.");
                id = GetID();
            }

            int hoursPlayed = GetHoursPlayed();
            string date = GetDatePlayed();

            GetConnection();

            using (SQLiteConnection con = new SQLiteConnection(connection))
            {
                SQLiteCommand command = new SQLiteCommand();
                con.Open();

                string query = $@"UPDATE played_videogames SET hours_played = {hoursPlayed}, date = '{date}' WHERE id = {id}";
                command.CommandText = query;
                command.Connection = con;
                command.ExecuteNonQuery();

                con.Close();
            }
            Console.Clear();
            Console.WriteLine("Record updated successfully\n");
        }

        public void DeleteRecord()
        {

            int id = GetID();
            if (!DoesIDExist(id))
            {
                Console.Clear();
                Console.WriteLine("Record not found. Please try again.");
                id = GetID();
            }
            GetConnection();

            using (SQLiteConnection con = new SQLiteConnection(connection))
            {
                SQLiteCommand command = new SQLiteCommand();
                con.Open();

                string query = $@"DELETE FROM played_videogames WHERE id = {id}";
                command.CommandText = query;
                command.Connection = con;
                command.ExecuteNonQuery();

                
                con.Close();
            }
            Console.Clear();
            Console.WriteLine("Record deleted successfully\n");
        }
        public void GetConnection()
        {
            connection = @$"Data Source={databaseName}.db; Version=3";
        }

        

    }
}
