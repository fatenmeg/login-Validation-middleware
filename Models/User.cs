using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Models
{
    public class User
    {
        public User()
        {
            books = new HashSet<Books>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Books> books { get; set; }

    }
}
