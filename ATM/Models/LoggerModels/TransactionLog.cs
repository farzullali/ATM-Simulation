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
    public class TransactionLog : ILogger
    {
        static Guid CorIdVar;
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Path { get; set; }
        public DateTime TimeStamp { get; set; }
        public Level Level { get; set; }
        public string Transaction { get; set; }
        public string ErrorMessage { get; set; }
        public Guid CorilationId { get; set; }

        public static void CreateTransactionLoggerInformation(int UserId)
        {
            TransactionRepository tr = new TransactionRepository();
            Transaction findedTr = tr.GetWithId(UserId).Result;

            TransactionLogRepository tlr = new TransactionLogRepository();

            Level level = Level.Information;

            TransactionLog TrLogLog = new TransactionLog()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                Transaction = UserSerialize(findedTr),
                Level = level,
                Path = MethodName(),
                UserId = findedTr.UserID,
                CorilationId = CorIdVar

            };

           tlr.CreateAsync(TrLogLog);
        }
        public static void CreateUserLoggerError(int UserId, Exception ex)
        {
            TransactionRepository tr = new TransactionRepository();
            Transaction findedTr = tr.GetWithId(UserId).Result;

            TransactionLogRepository tlr = new TransactionLogRepository();

            Level level = Level.Error;

            TransactionLog TrLogLog = new TransactionLog()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                Transaction = UserSerialize(findedTr),
                Level = level,
                Path = MethodName(),
                UserId = findedTr.UserID,
                ErrorMessage = ex.Message
            };

            tlr.CreateAsync(TrLogLog);
        }

        public static string UserSerialize(Transaction tr)
        {
            return JsonConvert.SerializeObject(tr);
        }
        public static Transaction UserDeserialize(string tr)
        {
            return JsonConvert.DeserializeObject<Transaction>(tr);
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
