﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Models
{
    public class SearchFilterPaging:BaseFilterPaging
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
    }
}
