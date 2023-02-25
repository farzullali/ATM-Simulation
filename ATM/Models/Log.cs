using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int SenderCardId { get; set; }
        public DateTime OperationTime { get; set; }
        public int ReceiverCardId { get; set; }
        public decimal Amount { get; set; }
        public int ReceiverUserId { get; set; }
        public bool isReceive { get; set; }

    }
}
