using System;

namespace ReLogin.Models
{
    public class UserTokenService
    {
    
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldToken { get; set; }
        public string TokenValue { get; set; }
        public DateTime LiveTime { get; set; }
        public bool IsRevoked { get; set; }
        public string ReplacedBy { get; set; }

    }
}
