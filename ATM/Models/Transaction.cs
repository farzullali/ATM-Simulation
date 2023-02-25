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

    }
}
