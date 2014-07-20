using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD
{
    public interface IMinimalPagination : IPageRequest
    {
        IPageRequest First();

        IPageRequest Next();

        IPageRequest Previous();

        IPageRequest Last();

        bool IsFirstPage();

        bool IsLastPage();
    }

    public interface IPagination : IMinimalPagination
    {
        long PageCount { get; }

        long TotalItems { get; }
    }
}
