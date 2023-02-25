using ATM.DBContext;
using ATM.Models;
using ATM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.UserContext
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User> FindByIdAsync(int id);
        Task<List<User>> GetAll();
    }

    public class UserRepository : IUserRepository
    {
        public Task<int> CreateAsync(User user)
        {
            var users = JsonWriterReader<User>.DeserializeFromJson(@"C:\\Users\\Line\\Desktop\\ATM\\ATM\\ATM\\ATM\\DBContext\\Data\\Users.json");

            if (users.Count > 0)
            {
                int maxId = users.Max(x => x.Id);
                user.Id = ++maxId;
            }
            else
            {
                user.Id = 1;
            }

            TransactionRepository tr = new TransactionRepository();
            tr.CreateAsync(user.Id);

            users.Add(user);

            JsonWriterReader<User>.WriteToJson(users, @"C:\\Users\\Line\\Desktop\\ATM\\ATM\\ATM\\ATM\\DBContext\\Data\\Users.json");
            return Task.FromResult(1);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();

        }

        public Task<User> FindByIdAsync(int id)
        {
            List<User> usersList = GetAll().Result;

            User findedUser = usersList.Find(x => x.Id == id);

            return Task.FromResult(findedUser);
        }

        public Task<List<User>> GetAll()
        {
            var users = JsonWriterReader<User>.DeserializeFromJson(@"C:\\Users\\Line\\Desktop\\ATM\\ATM\\ATM\\ATM\\DBContext\\Data\\Users.json");

            return Task.FromResult<List<User>>(users);
        }

        public Task UpdateAsync(User user)
        {
            var list = GetAll().Result;

            int index = list.FindIndex(x => x.Id == user.Id);

            list[index] = user;

            JsonWriterReader<User>.WriteToJson(list, @"C:\\Users\\Line\\Desktop\\ATM\\ATM\\ATM\\ATM\\DBContext\\Data\\Users.json");

            return Task.FromResult(user);
        }

        public User FindUserAfterPin(string PIN)
        {
            var users = JsonWriterReader<User>.DeserializeFromJson(@"C:\\Users\\Line\\Desktop\\ATM\\ATM\\ATM\\ATM\\DBContext\\Data\\Users.json");

            User findedUser = null;

            foreach (User item in users)
            {
                if (item.PIN == PIN)
                {
                    findedUser = item;
                }
            }
            return findedUser;
        }

        public void AddCardToUser(int cardId, int userId)
        {
            var usersList = GetAll().Result;
            int indexOfUser = usersList.FindIndex(x => x.Id == userId);

            if (usersList[indexOfUser].CardsId.Count == 0)
            {
                usersList[indexOfUser].CardsId = new List<int>();
            }

            usersList[indexOfUser].CardsId.Add(cardId);

            JsonWriterReader<User>.WriteToJson(usersList, @"C:\\Users\\Line\\Desktop\\ATM\\ATM\\ATM\\ATM\\DBContext\\Data\\Users.json");

        }
    }

}
