namespace NSpecifications;

public interface ISpecification
{
    bool IsSatisfiedBy(object candidate);
}

public interface ISpecification<T> : ISpecification
{
    bool IsSatisfiedBy(T candidate);
}

public interface IAndSpecification<T> : ISpecification<T>
{
    ISpecification<T> Left { get; }

    ISpecification<T> Right { get; }
}

public interface IOrSpecification<T> : ISpecification<T>
{
    ISpecification<T> Left { get; }

    ISpecification<T> Right { get; }
}

public interface INotSpecification<T> : ISpecification<T>
{
    ISpecification<T> Inner { get; }
}
