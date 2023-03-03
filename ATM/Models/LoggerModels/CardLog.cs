using ATM.DBContext;
using ATM.UserContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models.LoggerModels
{
    public class CardLog :ILogger
    {
        static Guid CorIdVar;
        public Guid Id { get; set; }
        public int CardId { get; set; }
        public string Path { get; set; }
        public DateTime TimeStamp { get; set; }
        public Level Level { get; set; }
        public string Card { get; set; }
        public string ErrorMessage { get; set; }
        public Guid CorilationId { get; set; }


        public static void CreateLoggerInformation(int cardId)
        {
            CardRepository cr = new CardRepository();
            Card findedCard = cr.FindByIdAsync(cardId).Result;

            CardLogRepository clr = new CardLogRepository();

            Level level = Level.Information;

            CardLog cardLog = new CardLog()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                Card = CardSerialize(findedCard),
                Level = level,
                Path = MethodName(),
                CardId = cardId,
                CorilationId = CorIdVar
            };

            clr.CreateAsync(cardLog);
        }
        public static void CreateLoggerError(int cardId, Exception ex)
        {
            CardRepository cr = new CardRepository();
            Card findedCard = cr.FindByIdAsync(cardId).Result;

            CardLogRepository clr = new CardLogRepository();

            Level level = Level.Error;

            CardLog cardLog = new CardLog()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                Card = CardSerialize(findedCard),
                Level = level,
                Path = MethodName(),
                CardId = cardId,
                ErrorMessage = ex.Message,
                CorilationId = CorIdVar

            };

            clr.CreateAsync(cardLog);
        }

        public static string CardSerialize(Card card)
        {
            return JsonConvert.SerializeObject(card);
        }

        public static Card CardDeserialize(string card)
        {
            return JsonConvert.DeserializeObject<Card>(card);
        }

        private static string MethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(2);

            string path = stackFrame.GetMethod().Name;

            return path;
        }

        public static void GetCorillationId(Guid CorId)
        {
            CorIdVar = CorId;
        }
    }
}
