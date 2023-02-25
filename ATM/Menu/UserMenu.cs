using ATM.DBContext;
using ATM.Models;
using ATM.Services;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ATM.Menu
{
    public class UserMenu
    {
        public UserMenu(User foundedUser)
        {
            bool check = true;
            int checkErr = 0;

            do
            {
                //BaseService.StartTask().ConfigureAwait(true);
                Console.Clear();

                Console.WriteLine($"Hello, dear {foundedUser.Name} {foundedUser.Surname}");
                Console.WriteLine();

                UserMenuScreen();

                string selectedOption = Console.ReadLine();
                UserSelectOptionHandler(selectedOption, foundedUser);
            } while (check);

        }

        public void UserMenuScreen()
        {
            Console.WriteLine("                 User Menu");
            Console.WriteLine();
            Console.WriteLine("1. Show Amount");
            Console.WriteLine("2. Card details");
            Console.WriteLine("3. Operations");
            Console.WriteLine("4. Card Settings");
            Console.WriteLine("5. Transactions");
            Console.WriteLine("6. Settings");
            Console.WriteLine();
            Console.WriteLine(" 0. Sign out");
            Console.WriteLine();
            Console.WriteLine("Please enter your chooice and press enter");
        }

        private void WaitScreen()
        {
            Console.WriteLine("");
            Console.WriteLine("Please press ENTER for back Admin Menu...");
            Console.ReadLine();

        }

        public void UserSelectOptionHandler(string selectedOption, User currUser)
        {
            UserServices userServices = new UserServices();


            switch (selectedOption)
            {

                case "1":
                    Console.Clear();
                    Console.WriteLine("User's amount: ");
                    Console.WriteLine();
                    userServices.ShowAmount(currUser.Id);
                    WaitScreen();
                    break;
                case "2":
                    Console.Clear();
                    userServices.UserCardDetailsHandler(currUser.CardsId);
                    WaitScreen();
                    break;
                case "3":
                    Console.Clear();
                    string chooice = OperationsMenu();
                    UserMenuOperations(chooice, currUser);
                    WaitScreen();
                    break;
                case "4":
                    // cardsettings
                    Console.Clear();

                    CardSettingsHandler(currUser);
                    WaitScreen();
                    break;
                case "5":
                    Console.Clear();

                    ShowTransactions(currUser.Id);
                    WaitScreen();

                    break;
                case "6":
                    Console.Clear();
                    SettingsMenu(currUser);
                    WaitScreen();
                    break;
                case "0":
                    Console.Clear();
                    Welcome.WelcomeScreen();
                    break;
                default:
                    Console.WriteLine("Wrong selection... Try again.");
                    break;
            }

        }

        private void SettingsMenu(User user)
        {
            Console.Clear();
            Console.WriteLine("Settings Menu");
            Console.WriteLine("1. Change Pin");
            Console.WriteLine("2. Change Email-adress");
            Console.WriteLine("3. Change Color App");
            Console.WriteLine("4. Show decimals"); // ?????

            string chooice = Console.ReadLine();

            switch (chooice)
            {
                case "1":
                    //change pin
                    ChangePinHandler(user);
                    break;
                case "2":
                    //change email
                    ChangeEmailHandler(user);
                    break;
                case "3":
                    ChangeColorHandler();
                    break;
                case "4":
                    //show decimals i dont know what is it
                    break;
                default:
                    Console.WriteLine("Wrong chooice. Please type menu id(number)");
                    break;
            }
        }
        private void ChangePinHandler(User user)
        {
            Console.WriteLine("Please enter your Current Pin");
            string currPin = Console.ReadLine();

            if (currPin == user.PIN)
            {
                UserServices ur = new UserServices();
                ur.ChangePin(user);
            }
            else
            {
                Console.WriteLine("Your current pin is not correct");
            }
        }
        private void ShowTransactions(int userId)
        {
            UserServices us = new UserServices();
            us.ShowTransactions(userId);
        }

        #region Operations Handlers

        public string OperationsMenu()
        {
            Console.WriteLine("Operaions with money $$$ Please select menu what u want to do");
            Console.WriteLine();
            Console.WriteLine("1. Card To Card");
            Console.WriteLine("2. Payments");

            return Console.ReadLine();
        }

        public void UserMenuOperations(string chooice, User currUser)
        {
            UserServices userServices = new UserServices();

            switch (chooice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Card to Card Process");
                    Console.WriteLine();
                    //card to card
                    userServices.CardToCard(currUser);
                    break;
                case "2":
                    //payments
                    PaymentMethod(PaymentMenu());
                    break;
                default:
                    Console.WriteLine("Wrong selection... Try again.");
                    break;
            }
        }

        #region Payment handlers
        public string PaymentMenu()
        {
            Console.WriteLine("What u wanna payment?");
            Console.WriteLine("1. AzerEnerji");
            Console.WriteLine("2. AzerGaz");
            Console.WriteLine("3. AzerSu");

            return Console.ReadLine();
        }
        public void PaymentMethod(string selectedProcess)
        {
            switch (selectedProcess)
            {
                case "1":
                    //azerenerji
                    break;
                case "2":
                    //azerqaz
                    break;
                case "3":
                    //azersu
                    break;
                default:
                    Console.WriteLine("Wrong selection... Try again.");
                    break;
            }
        }
        #endregion

        #endregion

        #region CardSettingsHandlers
        private void CardSettingsHandler(User currUser)
        {
            string selected = CardSettingsUserMenu();

            switch (selected)
            {
                case "1":
                    //block card
                    CardToBlock(currUser);
                    break;
                case "2":
                    // limit of card
                    ShowLimit(currUser);
                    break;
                default:
                    break;
            }
        }

        private void ChangeEmailHandler(User user)
        {
            Console.WriteLine("Please write new email:");
            string newMail = Console.ReadLine();

            UserRepository ur = new UserRepository();
            ur.UpdateAsync(user);

            Console.WriteLine("Your Mail is changed");
        }
        private void ShowLimit(User user)
        {
            UserServices ur = new UserServices();

            ur.GetUserLimit(user);
        }

        #region card to block
        private void CardToBlock(User currUser)
        {
            Console.WriteLine("Write card id for block card");
            CardRepository cardRepository = new CardRepository();
            var cardsId = cardRepository.GetUserCards(currUser.CardsId).Result;

            UserServices userServices = new UserServices();
            userServices.ShowCards(cardsId);

            int blockCardId = int.Parse(Console.ReadLine());
            userServices.BlockCardOperation(blockCardId);

            Console.WriteLine("Done!");

        }

        #endregion

        public string CardSettingsUserMenu()
        {
            Console.WriteLine("1. Block Card");
            Console.WriteLine("2. Limit of Card");

            return Console.ReadLine();
        }

        #endregion

        #region change color console
        public void ChangeColorHandler()
        {
            string[] array = new string[] {
                "black",
                "white",
                "red",
                "yellow",
                "green"
            };
            Console.WriteLine("Please choose color for set");
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine($"{i + 1} {array[i]}");
            }

            string chooice = Console.ReadLine();
            var selectedColor = GenerateColor(chooice);

            Console.WriteLine("Where u want to set color: ");
            Console.WriteLine("1. Background");
            Console.WriteLine("2. Text Color");

            string chooiceFunction = Console.ReadLine();

            if (chooiceFunction == "1")
            {
                changeBackgroundColorHandler(selectedColor);
            }
            else if (chooiceFunction == "2")
            {
                changeTextColorHandler(selectedColor);
            }
            else
            {
                Console.WriteLine("wrong chooice");
            }
        }

        public ConsoleColor GenerateColor(string chooice)
        {
            switch (chooice)
            {
                case "1":
                    return ConsoleColor.Black;
                case "2":
                    return ConsoleColor.White;
                case "3":
                    return ConsoleColor.Red;
                case "4":
                    return ConsoleColor.Yellow;
                case "5":
                    return ConsoleColor.Green;
                default:
                    return 0;
            }
        }
        private void changeBackgroundColorHandler(ConsoleColor color)
        {
            Console.BackgroundColor = color;
        }
        private void changeTextColorHandler(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
        #endregion
    }
}
