using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models.LoggerModels
{
    public interface ILogger
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public DateTime TimeStamp { get; set; }
        public Level Level { get; set; }
        public string ErrorMessage { get; set; }
        public Guid CorilationId { get; set; }
    }
}
