using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Mvc.Models
{
    public class SearchResult<T>
    {
        private readonly IEnumerable<T> data;
        private readonly int totalPages;
        private readonly int current;
        public SearchResult(IEnumerable<T> results, int pageCount, int currentPage)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            data = results;

            if (currentPage - 1 > pageCount)
                throw new ArgumentOutOfRangeException(nameof(currentPage));

            totalPages = pageCount;
            current = currentPage;
        }

        public IEnumerable<T> SearchResults => data;
        public int PageCount => totalPages;
        public int CurrentPage => current;
    }
}