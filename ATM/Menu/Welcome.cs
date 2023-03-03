using ATM.Models;
using ATM.Models.LoggerModels;
using ATM.Services;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Menu
{
    public class Welcome
    {
        public static void WelcomeScreen()
        {
            Guid guid = Guid.NewGuid();
            UserLog.GetCorillationId(guid);
            CardLog.GetCorillationId(guid);
            TransactionLog.GetCorillationId(guid);

            UserRepository userRepository = new UserRepository();
            AdminServices AdminServices = new AdminServices();

            if (userRepository.GetAll().Result.Count == 0)
            {
                AdminServices.CreateUser();
            }
            Console.Write("Hello. Welcome our ATM... ");

            string currentPIN = ATMServices.GetPin();

            User foundedUser = ATMServices.CheckWhichUserPin(currentPIN);

            if (foundedUser == null)
            {
                return;
            }

            else if (foundedUser.IsAdmin)
            {
                
                Console.ForegroundColor = ConsoleColor.Red;
                UserLog.CreateUserLoggerInformation(foundedUser.Id);
                AdminMenu AdminMenu = new AdminMenu();
            }
            else
            {
                
                UserLog.CreateUserLoggerInformation(foundedUser.Id);
                UserMenu UserMenu = new UserMenu(foundedUser);
            }

        }

        //public static KeyValuePair<bool, string> CheckValue()
        //{
        //    string PIN = "";
        //    var something = Console.ReadKey();
        //    if ((int)something.KeyChar > 47 && (int)something.KeyChar < 59)
        //    {
        //        PIN += something.KeyChar.ToString();

        //        var res = new KeyValuePair<bool, string>(true, PIN);


        //        return res;


        //    }
        //    throw new Exception("PIN is not valid");
        //}


    }
}
