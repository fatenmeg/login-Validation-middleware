using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReLogin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLogin.Models.Repo

{
    public class BookService : IBookService
    {
        private readonly AppDbContext _appDbContext;

        public BookService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Books> Create(Books book)
        {
            _appDbContext.Books.Add(book);
            await _appDbContext.SaveChangesAsync();
            return book;
        }

        public async Task Delete(int id)
        {
            var DeletedBook = await _appDbContext.Books.FindAsync(id);
            _appDbContext.Books.Remove(DeletedBook);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Books>> Get()
        {
            return await _appDbContext.Books.ToListAsync();
        }

        public async Task<Books> Get(int id)
        {
            return await _appDbContext.Books.FindAsync(id);
        }

        public async Task Update(Books book)
        {
            _appDbContext.Entry(book).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

        }

        public Task<PagedList<Books>> CustomSearch(SearchFilterPaging searchFilterPaging)
        {

            var dbBookQuery = _appDbContext.Books.Select(a => a);
            if (!string.IsNullOrEmpty(searchFilterPaging.Title))
            {
                dbBookQuery = dbBookQuery.Where(a => a.Title == searchFilterPaging.Title);


            }
            if (!string.IsNullOrEmpty(searchFilterPaging.Description))
            {
                dbBookQuery = dbBookQuery.Where(a => a.Description == searchFilterPaging.Description);

            }

            return Task.FromResult(PagedList<Books>.ToPagedList(dbBookQuery, searchFilterPaging.PageIndex, searchFilterPaging.PageSize));
        }

        public async Task<PagedList<Books>> GetPagedBooksAsync(BaseFilterPaging filterPaging)
        {

            // var validFilter = new FilterPaging(filterPaging.PageIndex, filterPaging.PageSize);
            //if (filterPaging.PageIndex >= 0 && filterPaging.PageSize >= 0)
            //{
            //    var pagedData = await _appDbContext.Books
            //        .Skip((filterPaging.PageIndex - 1) * filterPaging.PageSize)
            //        .Take(filterPaging.PageSize)
            //        .ToListAsync();

            //    var totalRecords = await _appDbContext.Books.CountAsync();

            //    var pagedReponse = new GetPageBooksResponse
            //    {
            //        Data = pagedData, 
            //        PageNo = filterPaging.PageIndex, 
            //        TotalData = totalRecords
            //    };

            //    return pagedReponse;
            //}
            //return new GetPageBooksResponse();

            var dbQuery = _appDbContext
                .Books
                .Include(x => x.users)
                .Select(a => a);


            return PagedList<Books>.ToPagedList(dbQuery, filterPaging.PageIndex, filterPaging.PageSize);
              
        }

     

    }

    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}