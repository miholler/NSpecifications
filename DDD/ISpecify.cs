using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD
{
    public interface ISpecify<TEntity>
    {
        bool IsSatisfiedBy(TEntity entity);
    }

    public static class ISpecificationExtensionMethods
    {
        public static ISpecify<TEntity> And<TEntity>(this ISpecify<TEntity> spec1, ISpecify<TEntity> spec2)
        {
            return new AndSpecification<TEntity>(spec1, spec2);
        }

        public static ISpecify<TEntity> Or<TEntity>(this ISpecify<TEntity> spec1, ISpecify<TEntity> spec2)
        {
            return new OrSpecification<TEntity>(spec1, spec2);
        }

        public static ISpecify<TEntity> Not<TEntity>(this ISpecify<TEntity> spec)
        {
            return new NotSpecification<TEntity>(spec);
        }

        public static ISpecify<TEntity> Paged<TEntity>(this ISpecify<TEntity> spec, long pageSize, long page)
        {
            return new NotSpecification<TEntity>(spec);
        }

        public static IEnumerable<TEntity> Satisfies<TEntity, TSpecification>(this IEnumerable<TEntity> collection, TSpecification spec) where TSpecification : ISpecify<TEntity>
        {
            return collection.Where(spec.IsSatisfiedBy);
        }

    }
}
