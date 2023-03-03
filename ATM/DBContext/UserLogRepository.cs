using ATM.Models;
using ATM.Models.LoggerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DBContext
{
    public interface IUserLogRepository
    {
        Task<int> CreateAsync(UserLog userLog);
        Task<List<UserLog>> GetAll();
    }
    public class UserLogRepository : IUserLogRepository
    {
        public Task<int> CreateAsync(UserLog userLog)
        {
            var logList = GetAll().Result;

            logList.Add(userLog);
            JsonWriterReader<UserLog>.WriteToJson(logList, @"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\UserLogs.json");

            return Task.FromResult(1);
        }

        public Task<List<UserLog>> GetAll()
        {
            var userLogList = JsonWriterReader<UserLog>.DeserializeFromJson(@"C:\Users\Line\Desktop\ATM\ATM\ATM\ATM\DBContext\Data\UserLogs.json");
            return Task.FromResult(userLogList);
        }
    }
}
