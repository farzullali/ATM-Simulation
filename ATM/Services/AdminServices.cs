using ATM.DBContext;
using ATM.Menu;
using ATM.Models;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ATM.Services.InteractionServices;

namespace ATM.Services
{
    public class AdminServices
    {
        #region Create New User
        public void CreateUser()
        {
            Message("Create a New User");
            Message("");
            // Name
            Message("Name?");
            string name = Console.ReadLine();
            // Surname
            Message("Surname?");
            string surname = Console.ReadLine();
            // Email
            Message("Email: ");
            string email = Console.ReadLine();
            // Phone number
            Message("Phone number: ");
            string phoneNumber = Console.ReadLine();
            // is admin?!
            bool isAdmin = false;
            bool check = true;
            do
            {
                Message("Is Admin:    [ Y / N ]");
                string chooice = Console.ReadLine().ToUpper();
                if (chooice == "Y")
                {
                    isAdmin = true;
                    check = false;
                }
                else if (chooice == "N")
                {
                    isAdmin = false;
                    check = false;
                }
                else
                {
                    Console.WriteLine("Wrong answer... Please write Y or N");
                }

            } while (check);
            //Pin
            string newPin = GeneratePin();
            // create Card
            int newUserCardId = CreateCard();

            GenerateCardHolderName(name + " " + surname, newUserCardId);

            // convert data to User model
            User newUser = new User()
            {
                Name = name,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber,
                CreatedDate = DateTime.Now,
                IsAdmin = isAdmin,
                PIN = newPin,
                CardsId = AddCardToNewUser(newUserCardId),
            };

            UserRepository userRepository = new UserRepository();

            userRepository.CreateAsync(newUser);
        }
        public List<int> AddCardToNewUser(int cardId)
        {
            List<int> ints = new List<int>();
            ints.Add(cardId);
            return ints;
        }
        #endregion

        #region CreateCard
        public int CreateCard()
        {
            Message("Please choose card type: 1 - MasterCard  2 - Visa");
            string chooseType = Console.ReadLine();

            string newCardNumber = CreateCardNumber();

            string newCvvNumber = CreateCvvNumber();

            Card newCard = new Card()
            {
                CardNumber = newCardNumber,
                CardType = int.Parse(chooseType),
                ExpirationTime = DateTime.Now.AddYears(4),
                Cvv = newCvvNumber,
            };

            CardRepository cardRepository = new CardRepository();
            int cardId = cardRepository.CreateAsync(newCard).Result;

            return cardId;
        }

        public string CreateCardNumber()
        {
            long baseNumbers = 4052502400000000;
            Random rnd = new Random();
            baseNumbers = baseNumbers + rnd.Next(1, 99999999);
            string cardNumber = baseNumbers.ToString();

            return cardNumber;
        }
        public string CreateCvvNumber()
        {
            Random rnd = new Random();
            string newCvv = (rnd.Next(1, 1000)).ToString();
            return newCvv;
        }

        public void GenerateCardHolderName(string fullName, int cardId)
        {
            CardRepository cardRepository = new CardRepository();
            var currentCard = cardRepository.FindByIdAsync(cardId).Result;

            currentCard.CardHolderName = fullName;

            cardRepository.UpdateCard(currentCard);
        }

        public string GeneratePin()
        {
            string newPin = "";
            Random rnd = new Random();
            for (int i = 0; i <= 3; i++)
            {
                int rndNumber = rnd.Next(0, 9);
                newPin += rndNumber.ToString();
            }

            return newPin;
        }
        #endregion

        public void AddCardExistingUser()
        {
            // empty card list
            CardRepository cardRepository = new CardRepository();
            var noneOwnerCards = cardRepository.GetNoneOwnerCards().Result;

            if (noneOwnerCards.Count == 0)
            {
                Console.WriteLine("We have not free owner card... But we can create new Card");
                CreateCard();
                noneOwnerCards = cardRepository.GetNoneOwnerCards().Result;
            }

            int existingUserId = GetUserIDForNewCard();

            Console.WriteLine("Please choose which one card. There are cards have not Owner. Write card index what is see you");
            ShowCardList(noneOwnerCards);
            int choosedCardId = int.Parse(Console.ReadLine());

            // add card to user
            UserRepository userRepository = new UserRepository();
            userRepository.AddCardToUser(choosedCardId, existingUserId);

            // add card Hodler Name
            var user = userRepository.FindByIdAsync(existingUserId).Result;
            cardRepository.AddCardHolderName(choosedCardId, user);
            cardRepository.AddCardOwnerId(choosedCardId ,existingUserId);
        }

        public void ShowCardList(List<Card> cardList)
        {
            foreach (Card curr in cardList)
            {
                string cardType = "";
                if (curr.CardType == 1)
                {
                    cardType = "MasterCard";
                }
                else
                {
                    cardType = "Visa";
                }

                Console.WriteLine($"{curr.Id} - {cardType}  Expiration date: {curr.ExpirationTime.Day}-{curr.ExpirationTime.Month}-{curr.ExpirationTime.Year}");
            }
        }

        public int GetUserIDForNewCard()
        {
            Message("New card who wanna ?!");
            var usersList = GetAllUsersList();

            foreach (User user in usersList)
            {
                Message($"{user.Id}  {user.Name}  {user.Surname}");
            }

            Message("Enter user id: ");
            string userId = Console.ReadLine();

            return int.Parse(userId);
        }

        public void ShowUserAccount()
        {
            var usersList = GetAllUsersList();
            Message("All Users List with amount");

            foreach (User item in usersList)
            {
                decimal currentUserAmount = GetUserCardsAmount(item.CardsId);
                Message($"{item.Id} -- {item.Name} {item.Surname} ----- {currentUserAmount} man.");
            }
        }
        public List<User> GetAllUsersList()
        {
            UserRepository userRepository = new UserRepository();

            var userList = userRepository.GetAll().Result;

            return userList;
        }

        public decimal GetUserCardsAmount(List<int> cardIDs)
        {
            if (cardIDs == null)
            {
                return 0;
            }

            decimal currentAmount = 0;
            var userCardIds = cardIDs;

            CardRepository cardRepository = new CardRepository();

            foreach (var card in userCardIds)
            {
                var currentCard = cardRepository.FindByIdAsync(card).Result;
                currentAmount += currentCard.Amount;
            }

            return currentAmount;
        }

        public void ShowAllUserList()
        {
            UserRepository ur = new UserRepository();
            var list = ur.GetAll().Result;

            foreach (User user in list)
            {
                Console.WriteLine($"Name: {user.Name}   Surname: {user.Surname}");
            }
        }

        public void ShowAllCardList()
        {
            CardRepository cr = new CardRepository();
            var cardList = cr.GetAll().Result;

            foreach (Card curr in cardList)
            {
                string cardType = "";
                if (curr.CardType == 1)
                {
                    cardType = "MasterCard";
                }
                else
                {
                    cardType = "Visa";
                }

                Console.WriteLine($"{curr.Id} - {cardType}  Expiration date: {curr.ExpirationTime.Day}-{curr.ExpirationTime.Month}-{curr.ExpirationTime.Year}   Card Owner: {curr.CardHolderName}");
            }
        }

        public void ShowActiveOrBlockedCards(int v)
        {
            Console.Clear();

            CardRepository cr = new CardRepository();
            var list = cr.GetAll().Result;

            UserServices us = new UserServices();

            // v=1 blocked cards
            // v=0 active cards

            if (v == 1)
            {
                Console.WriteLine("Blocked cards list");
                // filter blocked cards and send us.show cards
                list = list.FindAll(x => !x.IsActive);
                us.ShowCards(list);

                IfWantUnblockCard(list);
            }
            else if (v == 0)
            {
                Console.WriteLine("Active cards list");

                list = list.FindAll(x => x.IsActive);
                us.ShowCards(list);

                IfUWantBlockCard(list);
            }

        }

        private void IfWantUnblockCard(List<Card> blockedCardList)
        {
            Console.WriteLine("R u sure unblock card? [ y / n ]");
            string chooice = Console.ReadLine().ToLower();

            switch (chooice)
            {
                case "y":
                    UnblockCardHandler(blockedCardList);
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine("wrong chooice");
                    break;
            }
        }
        public void IfUWantBlockCard(List<Card> cards)
        {
            Console.WriteLine("R u sure block card? [ y / n ]");
            string chooice = Console.ReadLine().ToLower();

            switch (chooice)
            {
                case "y":
                    BlockCardHandler(cards);
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine("wrong chooice");
                    break;
            }
        }

        private void UnblockCardHandler(List<Card> cards)
        {
            Console.Clear();
            Console.WriteLine("Unblock card");
            Console.WriteLine();

            UserServices us = new UserServices();
            us.ShowCards(cards);

            Console.WriteLine();
            Console.WriteLine("Please choose card id");
            Console.WriteLine();

            int cardId = int.Parse(Console.ReadLine());

            CardRepository cr = new CardRepository();
            cr.BlockUnblockCard(cardId);

            Console.WriteLine("Card is unblocked....");
        }
        private void BlockCardHandler(List<Card> cards)
        {
            Console.Clear();
            Console.WriteLine("block card\n");
            Console.WriteLine();

            UserServices us = new UserServices();
            us.ShowCards(cards);

            Console.WriteLine();
            Console.WriteLine("Please choose card id");
            Console.WriteLine();

            int cardId = int.Parse(Console.ReadLine());

            CardRepository cr = new CardRepository();
            cr.BlockUnblockCard(cardId);

            EmailServices.SendLimitWarMail(cardId);

            Console.WriteLine("Card is blocked....");
        }
    }
}
