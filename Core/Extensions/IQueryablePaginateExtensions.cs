using Core.Utilities.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class IQueryablePaginateExtensions
    {
        public static Paginate<T> ToPaginate<T>(
            this IQueryable<T> source,
            int index,
            int size)
        {

            int count = source.Count();
            List<T> items = source.Skip(index * size).Take(size).ToList();

            Paginate<T> list = new()
            {
                Index = index,
                Count = count,
                Items = items,
                Size = size,
                Pages = (int)Math.Ceiling(count / (double)size)
            };
            return list;
        }
    }
}
