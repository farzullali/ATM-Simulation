using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string Cvv { get; set; }
        public string CardHolderName { get; set; }
        // 1 - master 2 - visa
        public int CardType { get; set; }
        public decimal Amount { get; set; }
        public int OwnerUserId { get; set; } // ???????? what r u thinking do something about this shit, bro?!
        public bool IsActive { get; set; } = true;
    }
}
