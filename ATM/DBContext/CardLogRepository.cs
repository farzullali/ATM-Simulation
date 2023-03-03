using ATM.Models.LoggerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DBContext
{
    public interface ICardLogRepository
    {
        Task<int> CreateAsync(CardLog cardLog);
        Task<List<CardLog>> GetAll();
    }
    public class CardLogRepository : ICardLogRepository
    {
        public Task<int> CreateAsync(CardLog cardLog)
        {
            var list = GetAll().Result;
            list.Add(cardLog);

            JsonWriterReader<CardLog>.WriteToJson(list, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\CardLogs.json");

            return Task.FromResult(1);
        }

        public Task<List<CardLog>> GetAll()
        {
            var cardLogList = JsonWriterReader<CardLog>.DeserializeFromJson(@"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\CardLogs.json");
            return Task.FromResult(cardLogList);
        }
    }
}
