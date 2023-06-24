using System.Diagnostics.CodeAnalysis;

namespace NSpecifications;

public abstract class ASpec<T> : ISpecification<T>
{
    private Func<T, bool>? _compiledPredicate;

    public abstract Expression<Func<T, bool>> Predicate { get; }

    protected Func<T, bool> CompiledPredicate => _compiledPredicate ??= Predicate.Compile();

    public virtual bool IsSatisfiedBy(T candidate)
        => CompiledPredicate(candidate);

    public virtual bool IsSatisfiedBy(object candidate)
        => IsSatisfiedBy((T)candidate);

    public override bool Equals(object other)
        => other is ASpec<T> spec && Equals(Predicate, spec.Predicate);

    public override int GetHashCode()
        => Predicate.GetHashCode();

    public override string ToString()
        => Predicate.ToString();

    [return: NotNullIfNotNull(nameof(spec))]
    public static implicit operator Expression<Func<T, bool>>?(ASpec<T>? spec)
        => spec?.Predicate;

    [return: NotNullIfNotNull(nameof(spec))]
    public static implicit operator Func<T, bool>?(ASpec<T>? spec)
        => spec?.CompiledPredicate;

    public static ASpec<T> operator !(ASpec<T> spec)
        => new NotSpec<T>(spec);

    public static ASpec<T> operator &(ASpec<T> left, ASpec<T> right)
        => new AndSpec<T>(left, right);

    public static ASpec<T> operator |(ASpec<T> left, ASpec<T> right)
        => new OrSpec<T>(left, right);

    public static ASpec<T> operator ==(ASpec<T> spec, bool value)
        => value ? spec : !spec;

    public static ASpec<T> operator ==(bool value, ASpec<T> spec)
        => value ? spec : !spec;

    public static ASpec<T> operator !=(ASpec<T> spec, bool value)
        => value ? !spec : spec;

    public static ASpec<T> operator !=(bool value, ASpec<T> spec)
        => value ? !spec : spec;
}

file sealed class NotSpec<T> : ASpec<T>
{
    public NotSpec(Expression<Func<T, bool>> inner)
    {
        Predicate = inner.Not();
    }

    public override Expression<Func<T, bool>> Predicate { get; }
}

file sealed class AndSpec<T> : ASpec<T>
{
    public AndSpec(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        Predicate = left.And(right);
    }

    public override Expression<Func<T, bool>> Predicate { get; }
}

file sealed class OrSpec<T> : ASpec<T>
{
    public OrSpec(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        Predicate = left.Or(right);
    }

    public override Expression<Func<T, bool>> Predicate { get; }
}
