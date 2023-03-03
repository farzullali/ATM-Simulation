using ATM.DBContext;
using ATM.UserContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models.LoggerModels
{

    public class UserLog : ILogger
    {
        static Guid CorIdVar;
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Path { get; set; }
        public DateTime TimeStamp { get; set; }
        public Level Level { get; set; }
        public string User { get; set; }
        public string ErrorMessage { get; set; }
        public Guid CorilationId { get; set; }

        public static void CreateUserLoggerInformation(int UserId)
        {
            UserRepository ur = new UserRepository();
            User findedUser = ur.FindByIdAsync(UserId).Result;

            UserLogRepository ulr = new UserLogRepository();

            Level level = Level.Information;

            UserLog userLog = new UserLog()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                User = UserSerialize(findedUser),
                Level = level,
                Path = MethodName(),
                UserId = findedUser.Id,
                CorilationId = CorIdVar
            };

            ulr.CreateAsync(userLog);
        }
        public static void CreateUserLoggerError(int UserId, Exception ex)
        {
            UserRepository ur = new UserRepository();
            User findedUser = ur.FindByIdAsync(UserId).Result;

            UserLogRepository ulr = new UserLogRepository();

            Level level = Level.Error;

            UserLog userLog = new UserLog()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                User = UserSerialize(findedUser),
                Level = level,
                Path = MethodName(),
                UserId = findedUser.Id,
                ErrorMessage = ex.Message,
                CorilationId = CorIdVar
            };

            ulr.CreateAsync(userLog);
        }

        public static string UserSerialize(User user)
        {
            return JsonConvert.SerializeObject(user);
        }
        public static User UserDeserialize(string user)
        {
            return JsonConvert.DeserializeObject<User>(user);
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
