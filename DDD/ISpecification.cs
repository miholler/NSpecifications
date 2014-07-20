using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD
{
    public interface ISpecification<TEntity>
    {
        bool IsSatisfiedBy(TEntity entity);
    }

    public static class ISpecificationExtensionMethods
    {
        public static ISpecification<TEntity> And<TEntity>(this ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            return new AndSpecification<TEntity>(spec1, spec2);
        }

        public static ISpecification<TEntity> Or<TEntity>(this ISpecification<TEntity> spec1, ISpecification<TEntity> spec2)
        {
            return new OrSpecification<TEntity>(spec1, spec2);
        }

        public static ISpecification<TEntity> Not<TEntity>(this ISpecification<TEntity> spec)
        {
            return new NotSpecification<TEntity>(spec);
        }

        public static ISpecification<TEntity> Paged<TEntity>(this ISpecification<TEntity> spec, long pageSize, long page)
        {
            return new NotSpecification<TEntity>(spec);
        }

        public static IEnumerable<TEntity> Satisfies<TEntity, TSpecification>(this IEnumerable<TEntity> collection, TSpecification spec) where TSpecification : ISpecification<TEntity>
        {
            return collection.Where(spec.IsSatisfiedBy);
        }

    }
}
