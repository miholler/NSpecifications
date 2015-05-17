using System.Collections.Generic;
using System.Linq;

namespace NSpecifications
{
    public static class ISpecificationExtensions
    {
        public static IAndSpecification<T> And<T>(this ISpecification<T> spec1, ISpecification<T> spec2)
        {
            return new AndSpecification<T>(spec1, spec2);
        }

        public static IOrSpecification<T> Or<T>(this ISpecification<T> spec1, ISpecification<T> spec2)
        {
            return new OrSpecification<T>(spec1, spec2);
        }

        public static INotSpecification<T> Not<T>(this ISpecification<T> inner)
        {
            return new NotSpecification<T>(inner);
        }

        public static IEnumerable<T> Satisfy<T>(this IEnumerable<T> collection, ISpecification<T> spec)
        {
            return collection.Where(spec.IsSatisfiedBy);
        }
    }
}
