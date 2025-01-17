using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize){
            CurrentPage = pageNumber;
            TotalCount = count;
            PageSize = pageSize;  
            TotalPages = (int)Math.Ceiling(count/ (double)pageSize);      
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumeber, int pageSize){
            var count =  await source.CountAsync();
            var items =  await source.Skip((pageNumeber-1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumeber, pageSize);
        }
    }
}