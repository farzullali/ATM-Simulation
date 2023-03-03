using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    public class Transaction
    {
        public int UserID { get; set; }
        public decimal Limit { get; set; }
        public List<Log> Log { get; set; }

        public void ShowLogOperations()
        {
            foreach(Log item in Log)
            {
                Console.WriteLine($"Sender CardId: {item.SenderCardId} " +
                    $"Transaction Amount: {item.Amount} " +
                    $"Receiver CardId: {item.ReceiverCardId}" +
                    $"Receiver UserId: {item.ReceiverUserId}" +
                    $"Transaction Time: {item.OperationTime}");
            }
        }

    }
}
