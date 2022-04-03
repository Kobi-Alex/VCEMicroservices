using System;
using System.Linq;
using System.Collections.Generic;


namespace Applicant.API.Application.Pagginations
{
    public class PagiData<T>
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public IEnumerable<T> Items { get; set; }
        public List<int> Pages { get; set; }
        public int TotalPages { get; set; }
    }


    public static class Paggination<T>
    {
        /// <summary>
        /// Gets data by pagination
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="limit">Qty of items on the page</param>
        /// <param name="cntBetween">Qty of pages before and after the middle</param>
        /// <param name="middleVal">Middle value. Must be more then cntBetween</param>
        /// <param name="itemsData">Data</param>
        /// <returns></returns>
        public static PagiData<T> GetData(int currentPage, int limit = 10, int cntBetween = 5, int middleVal = 10, IEnumerable<T> itemsData = null)
        {
            if (itemsData == null) throw new Exception("ItemsData was null");
            if (middleVal < cntBetween) throw new Exception("MiddleVal must be more than cntBetween");
            if (currentPage <= 0) return new PagiData<T>() { Items = itemsData, EndPage = 1, StartPage = 1, Pages = new List<int>() { 1 }, TotalPages = 1 };

            var itemsCntOnPage = limit > 0 ? limit : itemsData.Count();

            if (middleVal <= 0)
            {
                middleVal = 10; // default
            }

            if (cntBetween <= 0)
            {
                cntBetween = 5; // default
            }

            var totalItems = itemsData.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsCntOnPage);

            var startInedx = (currentPage - 1) * itemsCntOnPage;
            var endIndex = (int)Math.Min(startInedx + itemsCntOnPage - 1, totalPages - 1);
            var startPage = 0;
            var endPage = 0;

            if (currentPage >= middleVal)
            {
                startPage = currentPage - cntBetween;

                if (currentPage > totalPages)
                {
                    endPage = totalPages;
                }
                else
                {
                    if (currentPage + cntBetween <= totalPages)
                    {
                        endPage = currentPage + cntBetween;
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
                endPage = (int)Math.Ceiling(totalItems / (double)itemsCntOnPage) > middleVal ? middleVal : (int)Math.Ceiling(totalItems / (double)itemsCntOnPage);
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
