using System.Data.SQLite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DatabaseLibrary;


public class NumberOfSteps
{
    public int Id { get; set; }
    public int Steps { get; set; }
    public DateTime Date { get; set; }
}
public class Database
{
    public SQLiteConnection Connection;

    private static readonly string databaseName = "database";

    string connection;

    List<NumberOfSteps> dbData;
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

                string query = @"CREATE TABLE dbTable(id INTEGER PRIMARY KEY AUTOINCREMENT, numberofSteps INTEGER, date TEXT)";
                command.CommandText = query;
                command.Connection = con;
                command.ExecuteNonQuery();

                con.Close();
            }
        }
    }

    private int GetStepCount()
    {
        string? input = "";
        int stepCount = 0;

        Console.WriteLine("Please enter how many steps you've done.");
        input = Console.ReadLine();
        while (!int.TryParse(input, out stepCount) || stepCount < 1)
        {
            Console.WriteLine("Invalid input. Please try again.");
            input = Console.ReadLine();
        }
        return stepCount;
    }

    private string GetDate()
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
        int stepCountInput = GetStepCount();

        string dateInput = GetDate();

        Add(stepCountInput, dateInput);

        Console.WriteLine("Record successfully added");
    }
    private void Add(int stepCount, string date)
    {
        GetConnection();

        using (SQLiteConnection con = new SQLiteConnection(connection))
        {
            SQLiteCommand command = new SQLiteCommand();
            con.Open();

            string query = $@"INSERT INTO dbTable (numberofSteps, date)
                                VALUES ({stepCount},'{date}')";
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

            string query = @"SELECT * FROM dbTable";
            command.CommandText = query;
            command.Connection = con;

            SQLiteDataReader reader = command.ExecuteReader();
            dbData = new List<NumberOfSteps>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    dbData.Add(
                        new NumberOfSteps()
                        {
                            Id = reader.GetInt32(0),
                            Steps = reader.GetInt32(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "MM/dd/yyyy", new CultureInfo("en-US"))
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
        Console.WriteLine("ID\t\tnumber of steps\t\tdate");
        for (int i = 0; i < dbData.Count; i++)
        {
            Console.WriteLine($"{dbData[i].Id}\t\t{dbData[i].Steps}\t\t\t{dbData[i].Date.ToString("MM/dd/yyyy")}");
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
        while (!int.TryParse(choice, out id) || id < 1)
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
        while (!DoesIDExist(id))
        {
            Console.Clear();
            Console.WriteLine("Record not found. Please try again.");
            id = GetID();
        }

        int stepCount = GetStepCount();
        string date = GetDate();

        GetConnection();

        using (SQLiteConnection con = new SQLiteConnection(connection))
        {
            SQLiteCommand command = new SQLiteCommand();
            con.Open();

            string query = $@"UPDATE dbTable SET numberofSteps = {stepCount}, date = '{date}' WHERE id = {id}";
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
        while (!DoesIDExist(id))
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

            string query = $@"DELETE FROM dbTable WHERE id = {id}";
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
