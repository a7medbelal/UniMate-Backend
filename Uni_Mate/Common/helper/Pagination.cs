
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;

namespace Uni_Mate.Common.Helper
{
    public class Pagination<T> :List<T>
    {
        public int  CurrentPage { get; set; }

        public int TotalPages { get; set; } = 0;

        public int pageSize { get; set; }

        public int TotalCount { get; set; }

        public Pagination(List<T> Items, int Count, int PageNumber, int PageSize) {
           
            TotalCount = Count;
            TotalPages = PageNumber;
            pageSize = PageSize;
            TotalPages = (int) Math.Ceiling(Count / (double)pageSize);
            
            AddRange(Items);  
        }

        public static async Task<Pagination<T>> ToPagedList(IQueryable<T> source, int PageNumber, int pageSize)
        {
            var Count = await source.CountAsync();
            var Items = await source.Skip((PageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new Pagination<T>(Items, Count, PageNumber, pageSize);
        }


    }
}
