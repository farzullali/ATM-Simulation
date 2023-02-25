using ATM.DBContext;
using ATM.Models;
using ATM.Services;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Menu
{
    public class AdminMenu
    {
        public AdminMenu()
        {

            //BaseService.StartTask();
            bool check = true;
            int checkErr = 0;

            do
            {
                if (checkErr < 0)
                {
                    InteractionServices.Message("Wrong chooice, please write service id");
                    break;
                }
                else if (checkErr == 0)
                {
                    Console.Clear();

                    AdminMenuScreen();
                    string selectedOption = Console.ReadLine();
                    checkErr = UserSelectOptionHandler(selectedOption);

                }
                else if (checkErr > 0)
                {
                    checkErr = 0;
                }
            } while (check);
        }

        private void AdminMenuScreen()
        {

            Console.WriteLine("                                                 Admin Menu");
            Console.WriteLine();

            Console.WriteLine("     Choose what u wanna do");
            Console.WriteLine("                             1. Create a User");
            Console.WriteLine("                             2. Create a card");
            Console.WriteLine("                             3. User Account");
            Console.WriteLine("                             4. Add card to existing User");
            Console.WriteLine("                             5. Show All User List");
            Console.WriteLine("                             6. Show All Cards");
            Console.WriteLine("                             7. BLocked or active cards");

            Console.WriteLine();
            Console.WriteLine("                             0. Sign out");

        }

        public int UserSelectOptionHandler(string selectedOption)
        {
            AdminServices adminServices = new AdminServices();
            switch (selectedOption)
            {
                case "1":
                    Console.Clear();
                    adminServices.CreateUser();
                    WaitScreen();
                    return 1;
                case "2":
                    Console.Clear();
                    // get user name, surname, card type
                    adminServices.CreateCard();
                    WaitScreen();
                    return 1;
                case "3":
                    adminServices.ShowUserAccount();
                    WaitScreen();
                    return 1;
                case "4":
                    adminServices.AddCardExistingUser();
                    WaitScreen();
                    return 1;
                case "5":
                    // all users List
                    adminServices.ShowAllUserList();
                    WaitScreen();
                    return 1;
                case "6":
                    //all cards list
                    Console.Clear();
                    adminServices.ShowAllCardList();
                    WaitScreen();
                    return 1;
                case "7":
                    BlockedActiveCardsHandler();
                    WaitScreen();
                    return 1;
                case "0":
                    Console.Clear();
                    Welcome.WelcomeScreen();
                    return 1;
                default:
                    return -1;
            }
        }

        private void BlockedActiveCardsHandler()
        {
            Console.Clear();
            //blocked cards
            //active cards
            Console.WriteLine("1. Blocked cards");
            Console.WriteLine("2. Active cards");
            Console.WriteLine();
            string chooice = Console.ReadLine();

            switch (chooice)
            {
                case "1":
                    BlockedCardsHandler(1);
                    break;
                case "2":
                    //active cards
                    BlockedCardsHandler(0);
                    break;
                default:
                    break;
            }

        }

        private void BlockedCardsHandler(int v)
        {
            AdminServices ass = new AdminServices();
            ass.ShowActiveOrBlockedCards(v);
        }

        private void WaitScreen()
        {
            Console.WriteLine("");
            Console.WriteLine("Please press ENTER for back Admin Menu...");
            Console.ReadLine();

        }

        ////private string getCardType()
        ////{
        ////    bool check = true;
        ////    string choose = null;

        ////    do
        ////    {


        ////        Console.WriteLine("Choose card:     1 - Master Card     2 - Visa Card");
        ////        choose = Console.ReadLine();

        ////        if (choose == "1" || choose == "2")
        ////        {
        ////            check = false;
        ////            return choose;
        ////        }
        ////        else
        ////        {
        ////            Console.WriteLine("Invalid input, ");



        ////        }

        ////    } while (check);

        ////}     
    }
}
