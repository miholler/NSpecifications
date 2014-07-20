using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Data
{
    public abstract class Page<TCollection, TEntity> : IEnumerable<TEntity>
        where TCollection : IEnumerable<TEntity>
    {
        protected Page(IPageRequest settings, TCollection collection)
        {
            this.PageSize = settings.PageSize;
            this.PageNumber = settings.PageNumber;
            this.Collection = collection;
        }

        protected IEnumerable<TEntity> Collection { get; private set; }         
        
        public long PageSize { get; private set; }

        public long PageNumber { get; private set; }

        #region IEnumerable<TEntity>
		
        public IEnumerator<TEntity> GetEnumerator()
        {
 	        return Collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
 	        return Collection.GetEnumerator();
        } 

	    #endregion
    }

    internal class MinimalPaginationPage<TCollection, TEntity> : Page<TCollection, TEntity>, IMinimalPagination
    where TCollection : IEnumerable<TEntity>
    {
        long _totalCount;
        long _count;
        long _pageCount;

        private MinimalPaginationPage(IPageRequest settings, TCollection collection, long totalCount)
            : base(settings, collection)
        {
            _count = Collection.Count();
            _totalCount = totalCount;
            _pageCount = CountPages();
        }

        #region IMinimalPagination

        public IPageRequest First()
        {
            return new PageSettings(PageSize, 0);
        }

        bool IsPageWithinRange(long pageNumber)
        {
            return (pageNumber - 1) * PageSize <= _totalCount && pageNumber >= 1;
        }

        long CountPages()
        {
            return (long)Math.Ceiling((decimal)_totalCount / (decimal)PageSize);
        }

        public IPageRequest Next()
        {
            var newPageNumber = PageNumber + 1;
            if (!IsPageWithinRange(newPageNumber))
                throw new ArgumentOutOfRangeException(string.Format("Page {0} is out of range.", newPageNumber));
            return new PageSettings(PageSize, newPageNumber);
        }


        public IPageRequest Previous()
        {
            var newPageNumber = PageNumber - 1;
            if (!IsPageWithinRange(newPageNumber))
                throw new ArgumentOutOfRangeException(string.Format("Page {0} is out of range.", newPageNumber));
            return new PageSettings(PageSize, newPageNumber);
        }

        public IPageRequest Last()
        {
            return new PageSettings(PageSize, _pageCount);
        }

        public bool IsFirstPage()
        {
            return PageNumber == 1;
        }

        public bool IsLastPage()
        {
            return PageNumber == _pageCount;
        }

        #endregion

    }

}
