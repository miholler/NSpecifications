using DDD.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD
{
    public abstract class Page
    {
        static public IPageRequest First(long size)
        {
            return new PageSettings(size, 0);
        }
    }
    
    internal class PageSettings : IPageRequest
    {
        public PageSettings(long pageSize, long pageNumber)
        {
            this.PageSize = pageSize;
            this.PageNumber = PageNumber;
        }

        public long PageSize { get; private set; }

        public long PageNumber { get; private set; }
    }
}
