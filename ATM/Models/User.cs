using ATM.DBContext;
using ATM.Models.LoggerModels;
using ATM.Services;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAdmin { get; set; }
        public string PIN { get; set; }
        public List<int> CardsId { get; set; }

        public List<Card> GetUserCards(List<int> cardsId)
        {
            CardRepository cardRepository = new CardRepository();
            List<Card> userCards = cardRepository.GetUserCards(cardsId).Result;

            return userCards;
        }

        public void ShowAmount(int userId)
        {
            try
            {
                var userCards = GetUserCards(CardsId);
                decimal summAmount = 0;

                Console.WriteLine($"Hello, dear {Name}! Your Cards and amount list is there: ");
                Console.WriteLine();

                foreach (Card curr in userCards)
                {
                    summAmount += curr.Amount;
                    Console.WriteLine($"Card number: {curr.ShowCardLastFourNumber()}     Card Type: {curr.ShowCardType()}     Amount: {curr.Amount} man.");
                }

                Console.WriteLine();
                Console.WriteLine($"Your bank account summary of amount is: {summAmount}");

                UserLog.CreateUserLoggerInformation(userId);
            }
            catch (Exception ex)
            {
                UserLog.CreateUserLoggerError(userId, ex);
            }
        }

        public void UserCardDetailsHandler(int userId)
        {
            try
            {
                var userCards = GetUserCards(CardsId);

                foreach (Card card in userCards)
                {
                    Console.WriteLine($"Card Number: {card.ShowCardLastFourNumber()}   Card Type: {card.ShowCardType()}   Amount: {card.Amount}");
                }
                UserLog.CreateUserLoggerInformation(userId);
            }
            catch (Exception ex)
            {
                UserLog.CreateUserLoggerError(userId, ex);
                throw;
            }
        }
        public void ChangePin(User user)
        {
            Console.WriteLine("please enter your new pin");
            string newPin = Console.ReadLine();
            Console.WriteLine("Please enter new pin again");
            string checkNewPin = Console.ReadLine();

            if (newPin == checkNewPin)
            {
                user.PIN = newPin;

                UserRepository ur = new UserRepository();
                ur.UpdateAsync(user);

            }
            else
            {
                Console.WriteLine("your new pin does not true with second time what is u wrote");
            }
        }
    }
}
