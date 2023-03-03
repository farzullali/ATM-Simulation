using ATM.DBContext;
using ATM.Models;
using ATM.Models.LoggerModels;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Services
{
    public class UserServices
    {
        #region Card To Card
        public void CardToCard(User user)
        {
            int cardId = -1;
            try
            {
                Console.WriteLine("Your card: ");
                user.ShowAmount(user.Id);
                //choose card for send money
                int userCardId = ChooseUserCard(user);
                cardId = userCardId;

                // select user for send money
                int selectedUserId = SelectUserForSend(user);
                //show choiiced user cards
                int receiverCardId = ReceiverCardIdHandler(selectedUserId, userCardId);

                decimal forSendMoneyAmount = InputAmountForSend();
                //check limit
                decimal checkLimit = CheckOverLimit(user.Id, forSendMoneyAmount);
                if (checkLimit < 0)
                {
                    Console.WriteLine("Your limit is overed and your card was blocked. Please contact Admin");
                    BlockCardOperation(userCardId);
                    return;
                }

                //check card is Active or is block
                bool isActive = CheckCardIsBlocked(userCardId);
                if (!isActive)
                {
                    Console.WriteLine();
                    Console.WriteLine("                 Your current Card r blocked... Please contact with support.");
                    return;
                }

                CardRepository cr = new CardRepository();

                // check balance out for aomunt if have not enough money
                bool checkBalanceForProcess = CheckBalanceForSendMoney(userCardId, forSendMoneyAmount);
                if (checkBalanceForProcess)
                {
                    Console.WriteLine($"U have not for money for send {forSendMoneyAmount}man.");
                    return;
                }

                Log newLog = CreateLog(selectedUserId, forSendMoneyAmount, userCardId, receiverCardId);
                //log transaction
                CreateLogOnTransaction(user.Id, newLog);

                // userCardId send money to selectedCard
                decimal percent = cr.CardToCard(userCardId, receiverCardId, forSendMoneyAmount);
                if (percent > 0)
                {
                    Console.WriteLine($"From Master Card send money other bank cards then 1% taxes...");
                    Console.WriteLine($"Transfer taxes: {percent} man");
                }

                UserLog.CreateUserLoggerInformation(user.Id);
                CardLog.CreateLoggerInformation(userCardId);
            }
            catch (Exception ex)
            {
                UserLog.CreateUserLoggerError(user.Id, ex);
                CardLog.CreateLoggerError(cardId, ex);
            }
        }

        private bool CheckCardIsBlocked(int cardId)
        {
            CardRepository cr = new CardRepository();
            var currCard = cr.FindByIdAsync(cardId).Result;
            return currCard.IsActive;
        }

        private bool CheckBalanceForSendMoney(int userCardId, decimal forSendMoneyAmount)
        {
            CardRepository cr = new CardRepository();
            var card = cr.FindByIdAsync(userCardId).Result;
            decimal a = card.Amount - forSendMoneyAmount;

            if (a <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateLogOnTransaction(int userId, Log newLog)
        {
            TransactionRepository tr = new TransactionRepository();
            var newTrLogOfUser = tr.NewLog(userId, newLog).Result;

            Console.WriteLine($"Your limit left: {newTrLogOfUser.Limit}");
        }
        public decimal CheckOverLimit(int userId, decimal amount)
        {
            TransactionRepository tr = new TransactionRepository();
            decimal newLimit = tr.CheckLimit(userId, amount).Result;

            return newLimit;
        }
        public Log CreateLog(int receiverUserId, decimal amount, int senderCardId, int receiverCardId)
        {
            Log tempLog = new Log()
            {
                Amount = amount,
                ReceiverCardId = receiverCardId,
                SenderCardId = senderCardId,
                OperationTime = DateTime.Now,
                ReceiverUserId = receiverUserId
            };

            return tempLog;
        }

        public int ChooseUserCard(User user)
        {
            var cardListCurrUser = user.GetUserCards(user.CardsId);
            if (cardListCurrUser.Count > 1)
            {
                Console.WriteLine("Please choose your card for send money: ");

                foreach (Card curr in cardListCurrUser)
                {
                    Console.WriteLine($"Id: {curr.Id} Card number: {curr.ShowCardLastFourNumber()}     Amount: {curr.Amount} man.");
                }
                int cardId = int.Parse(Console.ReadLine());

                return cardId;
            }
            else if (cardListCurrUser.Count == 1)
            {
                return cardListCurrUser[0].Id;
            }
            else
            {
                Console.WriteLine("User have not a card");
                return 0;
            }
        }
        public int SelectUserForSend(User operationUser)
        {
            Console.WriteLine("please choose User for send money( type id )");
            Console.WriteLine();

            UserRepository userRepository = new UserRepository();

            var usersList = userRepository.GetAll().Result;
            foreach (User curr in usersList)
            {
                if (curr.PIN == "0000")
                {
                }
                else if (curr.Id == operationUser.Id)
                {
                    if (curr.CardsId.Count > 1)
                    {
                        Console.WriteLine($"Id: {curr.Id}   Name: {curr.Name}   Surname: {curr.Surname}");
                    }
                }
                else
                {
                    Console.WriteLine($"Id: {curr.Id}   Name: {curr.Name}   Surname: {curr.Surname}");
                }
            }
            int selectedUser = int.Parse(Console.ReadLine());

            return selectedUser;
        }
        public int ReceiverCardIdHandler(int userId, int selectedCurrCardId)
        {
            UserRepository userRepository = new UserRepository();
            User user = userRepository.FindByIdAsync(userId).Result;

            CardRepository cardRepository = new CardRepository();
            List<Card> cards = cardRepository.GetUserCards(user.CardsId).Result;

            int selectedCardId;

            if (cards.Count == 1)
            {
                selectedCardId = cards[0].Id;
            }
            else
            {
                ShowCards(cards, selectedCurrCardId);
                Console.WriteLine($"Please select {cards[0].CardHolderName}'s card ");
                selectedCardId = int.Parse(Console.ReadLine());
            }


            return selectedCardId;
        }
        public void ShowCards(List<Card> cardList, int selectedCurrCardId = -1)
        {
            foreach (Card card in cardList)
            {
                if (card.Id == selectedCurrCardId)
                {
                    continue;
                }
                Console.WriteLine($"Id: {card.Id}   Card Number: {card.ShowCardLastFourNumber()}     Card Type: {card.ShowCardType()}");
            }
        }
        public decimal InputAmountForSend()
        {
            Console.WriteLine("How much u wanna send? enter amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());
            return amount;
        }

        public void BlockCardOperation(int cardId)
        {
            CardRepository cardRepository = new CardRepository();
            cardRepository.BlockUnblockCard(cardId);
            EmailServices.SendLimitWarMail(cardId);
        }

        #endregion

        #region Payments


        #endregion
        public void ShowTransactions(int userId)
        {
            TransactionRepository tr = new TransactionRepository();
            var currTr = tr.GetWithId(userId).Result;
            if (currTr == null)
            {
                Console.WriteLine("Some errors happens. please try again later");
                return;
            }
            else
            {
                RenderLogs(currTr.Log);
            }
        }

        public void RenderLogs(List<Log> currLog)
        {
            Console.Clear();
            foreach (Log tr in currLog)
            {
                if (tr.isReceive)
                {
                    Console.WriteLine("         Income....      :)  ");
                    Console.WriteLine($"From: {tr.ReceiverCardId}   amount: {tr.Amount}");
                }
                else
                {
                    Console.WriteLine("         Excome....      :(  ");
                    Console.WriteLine($"To: {tr.SenderCardId}   amount: {tr.Amount}");
                }
            }
        }

        internal void GetUserLimit(User user)
        {
            TransactionRepository tr = new TransactionRepository();

            var currUserTransaction = tr.GetWithId(user.Id).Result;

            Console.WriteLine($"Your Limit: {currUserTransaction.Limit} man");
        }
    }
}
