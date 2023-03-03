
using ATM.Menu;
using ATM.Models;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Services
{
    public class ATMServices
    {
        public static string GetPin()
        {
            Console.WriteLine("Please enter your pin: ");
            string enteredValue = Console.ReadLine();

            string PINValue = "";
            if (enteredValue.Length == 4)
            {
                foreach (var item in enteredValue)
                {
                    if ((int)item > 47 && (int)item < 59)
                    {
                        PINValue += item;
                    }
                }
            }
            else
            {
                throw new Exception("PIN does required 4");
            }


            return PINValue;
        }
        public static User CheckWhichUserPin(string PIN)
        {
            UserRepository userRepository = new UserRepository();
            User entryValidUser = userRepository.FindUserAfterPin(PIN);

            return entryValidUser;
        }

    }
}
