using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DBContext
{
    public interface ITransactionRepository
    {
        Task<int> CreateAsync(int userId);

    }

    public class TransactionRepository : ITransactionRepository
    {
        public Task<int> CreateAsync(int userId)
        {
            var list = GetAll().Result;
            list.Add(new Transaction() { UserID = userId, Limit = 10000, Log = new List<Log>() });
            JsonWriterReader<Transaction>.WriteToJson(list, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Transactions.json");

            return Task.FromResult(list.Count - 1);
        }

        public Task<decimal> CheckLimit(int userId, decimal amount)
        {
            var list = GetAll().Result;

            if (list.Count > 0)
            {
                var finded = list.Find(x => x.UserID == userId);
                decimal newLimit = finded.Limit - amount;

                return Task.FromResult(newLimit);
            }
            else
            {
                decimal newLimit = 10000 - amount;
                return Task.FromResult(newLimit);
            }
        }


        public Task<Transaction> NewLog(int userId, Log newLog)
        {
            CreateLogForReceiverUser(newLog);

            var list = GetAll().Result;
            int index = list.FindIndex(x => x.UserID == userId);

            // if user's transaction have not in db
            if (index < 0)
            {
                CreateAsync(userId);
                index = 0;
                list = GetAll().Result;
            }

            //last log id
            if (list[index].Log.Count > 0)
            {
                int maxId = list[index].Log.Max(x => x.Id);
                newLog.Id = ++maxId;
            }
            else
            {
                newLog.Id = 0;
            }

            newLog.isReceive = false;


            list[index].Log.Add(newLog);

            list[index].Limit -= newLog.Amount;
            JsonWriterReader<Transaction>.WriteToJson(list, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Transactions.json");

            return Task.FromResult(list[index]);
        }



        private void CreateLogForReceiverUser(Log log)
        {
            var list = GetAll().Result;
            int index = list.FindIndex(x => x.UserID == log.ReceiverUserId);

            log.isReceive = true;

            if (index < 0)
            {
                index = CreateAsync(log.ReceiverUserId).Result;
                list = GetAll().Result;
            }

            if (list[index].Log.Count > 0)
            {
                int maxId = list[index].Log.Max(x => x.Id);
                log.Id = ++maxId;
            }
            else
            {
                log.Id = 0;
            }

            list[index].Log.Add(log);

            JsonWriterReader<Transaction>.WriteToJson(list, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Transactions.json");

        }

        private Task<List<Transaction>> GetAll()
        {
            var transactions = JsonWriterReader<Transaction>.DeserializeFromJson(@"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Transactions.json");

            return Task.FromResult(transactions);
        }

        public Task<Transaction> GetWithId(int userId)
        {
            var list = GetAll().Result;

            Transaction currTr = list.Find(x => x.UserID == userId);

            return Task.FromResult(currTr);
        }
    }
}
