namespace NSpecifications;

public static class SpecificationExtensions
{
    public static bool Is<T>(this T candidate, ISpecification<T> spec)
        => spec.IsSatisfiedBy(candidate);

    public static  bool Are<T>(this IEnumerable<T> candidates, ISpecification<T> spec)
        => candidates.All(spec.IsSatisfiedBy);

    public static INotSpecification<T> Not<T>(this ISpecification<T> inner)
        => new NotSpecification<T>(inner);

    public static IAndSpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        => new AndSpecification<T>(left, right);

    public static IOrSpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        => new OrSpecification<T>(left, right);
}

file sealed class NotSpecification<T> : INotSpecification<T>
{
    public NotSpecification(ISpecification<T> inner)
    {
        Inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public ISpecification<T> Inner { get; }

    public bool IsSatisfiedBy(T candidate)
        => !Inner.IsSatisfiedBy(candidate);

    public bool IsSatisfiedBy(object candidate)
        => !Inner.IsSatisfiedBy(candidate);
}

file sealed class AndSpecification<T> : IAndSpecification<T>
{
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Right = right ?? throw new ArgumentNullException(nameof(right));
    }

    public ISpecification<T> Left { get; }

    public ISpecification<T> Right { get; }

    public bool IsSatisfiedBy(T candidate)
        => Left.IsSatisfiedBy(candidate) && Right.IsSatisfiedBy(candidate);

    public bool IsSatisfiedBy(object candidate)
        => Left.IsSatisfiedBy(candidate) && Right.IsSatisfiedBy(candidate);
}

file sealed class OrSpecification<T> : IOrSpecification<T>
{
    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Right = right ?? throw new ArgumentNullException(nameof(right));
    }

    public ISpecification<T> Left { get; }

    public ISpecification<T> Right { get; }

    public bool IsSatisfiedBy(T candidate)
        => Left.IsSatisfiedBy(candidate) || Right.IsSatisfiedBy(candidate);

    public bool IsSatisfiedBy(object candidate)
        => Left.IsSatisfiedBy(candidate) || Right.IsSatisfiedBy(candidate);
}
