using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Data.Paggination
{
    public class PagiData<T>
    {
        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public IEnumerable<T> Items { get; set; }

        public List<int> Pages { get; set; }

        public int TotalPages { get; set; }
    }

    public static class Pagination<T>
    {
        public static PagiData<T> GetData(int currentPage = -1, int limit = -1, IEnumerable<T> itemsData = null)
        {
            if (itemsData == null) return null;
            if (currentPage <= 0) return new PagiData<T>() { Items = itemsData, EndPage = 1, StartPage = 1, Pages = new List<int>() { 1 }, TotalPages = 1 };
            var itemsCntOnPage = limit >0 ? limit : itemsData.Count();

            var totalItems = itemsData.Count();

            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsCntOnPage);

            var startInedx = (currentPage - 1) * itemsCntOnPage;
            var endIndex = (int)Math.Min(startInedx + itemsCntOnPage - 1, totalPages - 1);
            var startPage = 0;
            var endPage = 0;

            if (currentPage >= 10)
            {
                startPage = currentPage - 5;

                if (currentPage > totalPages)
                {
                    endPage = totalPages;
                }
                else
                {
                    if (currentPage + 5 <= totalPages)
                    {
                        endPage = currentPage + 5;
                    }
                    else
                    {
                        endPage = totalPages;
                    }
                }
            }
            else
            {
                startPage = 1;
                endPage = (int)Math.Ceiling(totalItems / (double)itemsCntOnPage) > 10 ? 10 : (int)Math.Ceiling(totalItems / (double)itemsCntOnPage);
            }

            if (endPage <= 0) endPage = 1;


            //let pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);
            var pages = new List<int>();

            for (int i = 0; i < endPage + 1 - startPage; i++)
            {
                pages.Add(startPage + i);
            }

            var items = itemsData.Skip(startInedx).Take(itemsCntOnPage);


            return new PagiData<T>() { Items = items, EndPage = endPage, StartPage = startPage, Pages = pages, TotalPages = totalPages };

        }
    }
}
