using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Models
{
    public class GetPageBooksResponse
    {
        public  int PageNo { get; set; }
        public List<Books> Data { get; set; }
        public int TotalData { get; set; }
    }
}
