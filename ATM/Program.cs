using ATM.Menu;
using ATM.Models;
using ATM.Services;
using ATM.UserContext;

bool check = true;

do
{
    try
    {
        Console.Clear();

        Welcome.WelcomeScreen();
        

        if (Console.ReadKey().Key == ConsoleKey.Escape)
        {
            Console.WriteLine("salaaaam");
        }
    }
    catch (Exception ex)
    {
        if (ex.Message == "Object reference not set to an instance of an object.")
        {
            Console.WriteLine("Input is not valid. Please try again...");
        }
    }
} while (check);
