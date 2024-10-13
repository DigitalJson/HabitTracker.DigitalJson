using DatabaseLibrary;

namespace HabitTrackerProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database();
            bool endApp = false;
            string? input = "";
            int choice = -1;

            while (!endApp)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Type the associated number with what you wish to do. Type 0 if you wish to exit.");
                Console.WriteLine("1 - View All Records");
                Console.WriteLine("2 - Add New Record");
                Console.WriteLine("3 - Update Record");
                Console.WriteLine("4 - Delete Record");

                input = Console.ReadLine();
                while (!int.TryParse(input, out choice) || (choice > 4 || choice < 0))
                {
                    Console.WriteLine("Invalid input. Please type the associated number of what you wish to do.");
                    input = Console.ReadLine();
                }

                switch (choice)
                {
                    case 1:
                        // View all records
                        database.ReadAllRecords();
                        break;
                    case 2:
                        // Add new record
                        database.InsertNewRecord();
                        break;
                    case 3:
                        // Update record
                        break;
                    case 4:
                        // Delete record
                        break;
                    case 0:
                        Console.WriteLine("Closing app...");
                        endApp = true;
                        break;
                }
            }
            
        }
    }
}