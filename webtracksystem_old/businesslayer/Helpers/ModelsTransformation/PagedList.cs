using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace BusinessLayer.Transformation
{
    public class PagedList<T> : List<T>{

        public PagedList(List<T> items, int count, int pageNumber, int pageSize){
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            AddRange(items);
        }
        public static async Task<PagedList<T>> CreateAsync<TSource>(IQueryable<TSource> source, int pageNumber, int pageSize){
            int count = source.Count();
            var items = await Task.Run(() => source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList());
            var res = Mapper.Map<List<T>>(items);
            return new PagedList<T>(res, count, pageNumber, pageSize);
        }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious {
            get {
                return CurrentPage > 1;
            }
        }
        public bool HasNext {
            get {
                return CurrentPage < TotalPages;
            }
        }
    }
}