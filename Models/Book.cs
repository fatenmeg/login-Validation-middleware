using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Models
{
    public class Books
    {
        public Books()
        {
            users = new HashSet<User>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> users { get; set; }
    }
}
