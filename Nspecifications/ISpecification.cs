using System;
using System.Linq.Expressions;

namespace NSpecifications
{
    public interface ISpecification
    {
    }

    public interface ISpecification<T> : ISpecification
    {
        bool IsSatisfiedBy(T candidate);

        Expression<Func<T, bool>> Expression { get; }
    }

    public interface IAndSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Spec1 { get; }

        ISpecification<T> Spec2 { get; }
    }

    public interface IOrSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Spec1 { get; }

        ISpecification<T> Spec2 { get; }
    }

    public interface INotSpecification<T> : ISpecification<T>
    {
        ISpecification<T> Inner { get; }
    }
}
