using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Models
{
    public class Token
    {
        public int Id { get; set; }

        public string TokenValue { get; set; }
        public DateTime LiveTime { get; set; }
        public bool IsRevoked { get; set; }
        public string ReplacedBy { get; set; }
    }
}
