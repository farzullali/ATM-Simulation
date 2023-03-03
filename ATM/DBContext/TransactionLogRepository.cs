using ATM.Models.LoggerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DBContext
{
    public interface ITransactionLogRepository
    {
        Task<int> CreateAsync(TransactionLog trLog);
        Task<List<TransactionLog>> GetAll();
    }
    public class TransactionLogRepository : ITransactionLogRepository
    {
        public Task<int> CreateAsync(TransactionLog trLog)
        {
            var logList = GetAll().Result;

            logList.Add(trLog);
            JsonWriterReader<TransactionLog>.WriteToJson(logList, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\TransactionLogs.json");

            return Task.FromResult(1);
        }
        public Task<List<TransactionLog>> GetAll()
        {
            var userLogList = JsonWriterReader<TransactionLog>.DeserializeFromJson(@"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\TransactionLogs.json");
            return Task.FromResult(userLogList);
        }
    }
}
