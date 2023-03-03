using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    public enum Level { Information, Error }
    public class Logger
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string UserId { get; set; }
        public string CardId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Level Level { get; set; }
        public string Transaction { get; set; }
        public string User { get; set; }
        public string Card { get; set; }
    }
}
