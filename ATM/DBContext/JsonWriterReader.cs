using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.DBContext
{
    public static class JsonWriterReader<T> where T : class
    {
        public static List<T> DeserializeFromJson(string path)
        {
            string json = File.ReadAllText(path);
            var UsersList = JsonConvert.DeserializeObject<List<T>>(json);
            return UsersList;
        }
        public static void WriteToJson(List<T> UsersList, string path)
        {
            var newJson = JsonConvert.SerializeObject(UsersList);
            File.WriteAllText(path, newJson);
        }

    }
}
