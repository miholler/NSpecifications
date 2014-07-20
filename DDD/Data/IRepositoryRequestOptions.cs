using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Data
{
    //public interface IRepositoryRequestOptions
    //{
    //}

    public interface IPageRequest
    {
    }

    public interface IPageNumberRequest : IPageRequest
    {
        long PageSize { get; }

        long PageNumber { get; }
    }

    public interface ILastPageRequest : IPageRequest
    {
    }

    public interface ISortedPageRequest : IPageRequest
    {
        string SorteBy { get; }
    }
}
