using ATM.Models;
using ATM.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DBContext

{
    public interface IlogRepository
    {
        Task<int> CreateAsync(Card card);
        Task DeleteAsync(int id);
        Task<Card> FindByIdAsync(int id);
        Task<List<Card>> GetAll();
    }
    public class CardRepository : IlogRepository
    {
        public Task<int> CreateAsync(Card card)
        {
            var cards = GetAll().Result;

            if (cards.Count > 0)
            {
                int maxId = cards.Max(x => x.Id);
                card.Id = ++maxId;
            }
            else
            {
                card.Id = 1;
            }

            cards.Add(card);
            JsonWriterReader<Card>.WriteToJson(cards, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Cards.json");

            return Task.FromResult(card.Id);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Card> FindByIdAsync(int id)
        {
            var cards = GetAll().Result;

            Card findedCard = cards.Find(x => x.Id == id);

            return Task.FromResult(findedCard);
        }

        public Task<List<Card>> GetAll()
        {
            var cards = JsonWriterReader<Card>.DeserializeFromJson(@"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Cards.json");

            return Task.FromResult(cards);
        }

        public Task<List<Card>> GetUserCards(List<int> cardIds)
        {
            var cards = GetAll().Result;

            List<Card> userCards = new List<Card>();

            foreach (int id in cardIds)
            {
                Card currentFindedCard = FindByIdAsync(id).Result;
                userCards.Add(currentFindedCard);
            }

            return Task.FromResult(userCards);
        }

        public Task UpdateCard(Card cardForEdited)
        {
            var cards = GetAll().Result;

            int indexCurrCard = cards.FindIndex(x => x.Id == cardForEdited.Id);

            cards[indexCurrCard] = cardForEdited;

            JsonWriterReader<Card>.WriteToJson(cards, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Cards.json");


            return Task.CompletedTask;
        }

        public Task<List<Card>> GetNoneOwnerCards()
        {
            var cards = GetAll().Result;
            List<Card> noneOwnerCards = new List<Card>();

            foreach (Card curr in cards)
            {
                if (curr.CardHolderName == null)
                {
                    noneOwnerCards.Add(curr);
                }
            }

            return Task.FromResult(noneOwnerCards);
        }

        public decimal CardToCard(int outputCardId, int inputCardId, decimal amount)
        {
            var cardsList = GetAll().Result;

            int outputCardIndex = cardsList.FindIndex(x => x.Id == outputCardId);
            int inputCardIndex = cardsList.FindIndex(x => x.Id == inputCardId);

            decimal percent = MasterCardPercentTexas(cardsList[outputCardIndex], amount);

            if (percent > 0)
            {
                cardsList[outputCardIndex].Amount -= percent;
                cardsList[0].Amount += percent;
            }

            cardsList[outputCardIndex].Amount -= amount;
            cardsList[inputCardIndex].Amount += amount;

            JsonWriterReader<Card>.WriteToJson(cardsList, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Cards.json");
            return percent;
        }

        public decimal MasterCardPercentTexas(Card card, decimal amount)
        {
            decimal percent = 0;
            if (card.CardType == 1)
            {
                percent = amount * 1 / 100;
                if (percent <= 1)
                {
                    percent = 1;
                }
            }
            return percent;
        }

        public void BlockUnblockCard(int cardId)
        {
            var cardList = GetAll().Result;
            int index = cardList.FindIndex(x => x.Id == cardId);

            if (cardList[index].IsActive == true)
            {
                cardList[index].IsActive = false;
            }else if(cardList[index].IsActive == false)
            {
                cardList[index].IsActive = true;
            }

            JsonWriterReader<Card>.WriteToJson(cardList, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Cards.json");
        }

        public void AddCardHolderName(int cardId, User user)
        {
            var cardList = GetAll().Result;
            int index = cardList.FindIndex(x => x.Id == cardId);

            cardList[index].CardHolderName = user.Name + " " + user.Surname;
            JsonWriterReader<Card>.WriteToJson(cardList, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Cards.json");
        }

        public void AddCardOwnerId(int cardId,int existingUserId)
        {
            var cardList = GetAll().Result;
            int index = cardList.FindIndex(x => x.Id == cardId);

            cardList[index].OwnerUserId = existingUserId;
            JsonWriterReader<Card>.WriteToJson(cardList, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Cards.json");

        }
    }
}
