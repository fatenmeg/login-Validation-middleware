using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Models.Repo
{
    public interface IBookService
    {
        Task<IEnumerable<Books>> Get();
        Task<Books> Get(int id);
        Task<Books> Create(Books book);
        Task Update(Books book);
        Task Delete(int id);
        Task<PagedList<Books>> CustomSearch(SearchFilterPaging searchFilterPaging);
        Task<PagedList<Books>> GetPagedBooksAsync(BaseFilterPaging filterPaging);

    }
}
