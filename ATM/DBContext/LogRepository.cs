using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DBContext
{
    public interface ILoggerRepository
    {
        Task<int> CreateAsync();
        Task DeleteAsync();
        Task<Logger> FindByIdAsync();
        Task<List<Logger>> GetAll();
    }

    public class LogRepository : ILoggerRepository
    {
        public Task<int> CreateAsync(Logger logger)
        {
            var logs = GetAll().Result;
            logs.Add(logger);

            JsonWriterReader<Logger>.WriteToJson(logs, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Logs.json");

            return Task.FromResult(1);
        }      

        public Task<List<Logger>> GetAll()
        {
            var logs = JsonWriterReader<Logger>.DeserializeFromJson(@"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\Logs.json");

            return Task.FromResult(logs);
        }
    }
}
