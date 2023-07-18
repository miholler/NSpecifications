using NSpecifications.Internal;
using System.Diagnostics.CodeAnalysis;

namespace NSpecifications;

/// <summary>
/// Provides a base class for implementations of the <see cref="ISpecification{T}"/>
/// generic interface.
/// </summary>
/// <typeparam name="T">The type of the candidate object.</typeparam>
public abstract class ASpec<T> : ISpecification<T>
{
    private Func<T, bool>? _compiledPredicate;

    /// <summary>
    /// Gets the predicate that defines the current specification.
    /// </summary>
    public abstract Expression<Func<T, bool>> Predicate { get; }

    /// <summary>
    /// Gets the compiled predicate that defines the current specification.
    /// </summary>
    protected Func<T, bool> CompiledPredicate => _compiledPredicate ??= Predicate.Compile();

    /// <summary>
    /// Casts the candidate type of the current specification to a specified derived candidate type.
    /// </summary>
    /// <typeparam name="TDerived">The derived candidate type to cast the current candidate type to.</typeparam>
    /// <returns>
    /// An <see cref="ASpec{TDerived}"/> whose candidate type is of type <typeparamref name="TDerived"/>.
    /// </returns>
    public ASpec<TDerived> CastUp<TDerived>() where TDerived : T
        => new CastSpec<T, TDerived>(Predicate);

    /// <summary>
    /// Determines whether the current specification is satisfied by a specified value.
    /// </summary>
    /// <param name="candidate">The value to check.</param>
    /// <returns>
    /// <see langword="true"/> if the specification is satisfied by the specified value;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public virtual bool IsSatisfiedBy(T candidate)
        => CompiledPredicate(candidate);

    /// <summary>
    /// Determines whether the current specification is satisfied by a specified value.
    /// </summary>
    /// <param name="candidate">The value to check.</param>
    /// <returns>
    /// <see langword="true"/> if the specification is satisfied by the specified value;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public virtual bool IsSatisfiedBy(object candidate)
        => IsSatisfiedBy((T)candidate);

    /// <summary>
    /// Determines whether the specified object is equal to the current specification.
    /// </summary>
    /// <param name="other">The object to compare with the current specification.</param>
    /// <returns>
    /// <see langword="true"/> if the specified object is equal to the current specification; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals([NotNullWhen(true)]object? other)
        => other is ASpec<T> spec && EqualityComparer<Expression<Func<T, bool>>>.Default.Equals(Predicate, spec.Predicate);

    /// <summary>
    /// Returns a hash code for the current specification.
    /// </summary>
    /// <returns>A hash code for the current specification.</returns>
    public override int GetHashCode()
        => Predicate.GetHashCode();

    /// <summary>
    /// Returns a string that represents the current specification.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents the current specification.</returns>
    public override string ToString()
        => Predicate.ToString();

    /// <summary>
    /// Implicitly converts a specification into a predicate expression.
    /// </summary>
    /// <param name="spec">The specification to convert into a predicate expression.</param>
    [return: NotNullIfNotNull(nameof(spec))]
    public static implicit operator Expression<Func<T, bool>>?(ASpec<T>? spec)
        => spec?.Predicate;

    /// <summary>
    /// Implicitly converts a specification into a predicate function.
    /// </summary>
    /// <param name="spec">The specification to convert into a predicate function.</param>
    [return: NotNullIfNotNull(nameof(spec))]
    public static implicit operator Func<T, bool>?(ASpec<T>? spec)
        => spec?.CompiledPredicate;

    /// <summary>
    /// Creates a specification that negates a specified specification.
    /// </summary>
    /// <param name="spec">The specification to negate.</param>
    /// <returns>
    /// An <see cref="ASpec{T}"/> representing the negation of the
    /// specified specification.
    /// </returns>
    public static ASpec<T> operator !(ASpec<T> spec)
        => new NotSpec<T>(spec);

    /// <summary>
    /// Creates a specification that combines two specified specifications
    /// with a conditional AND operator.
    /// </summary>
    /// <param name="left">The first specification to combine.</param>
    /// <param name="right">The second specification to combine.</param>
    /// <returns>
    /// An <see cref="ASpec{T}"/> representing the combination of both
    /// specifications with a conditional AND operator.
    /// </returns>
    public static ASpec<T> operator &(ASpec<T> left, ASpec<T> right)
        => new AndSpec<T>(left, right);

    /// <summary>
    /// Creates a specification that combines two specified specifications
    /// with a conditional OR operator.
    /// </summary>
    /// <param name="left">The first specification to combine.</param>
    /// <param name="right">The second specification to combine.</param>
    /// <returns>
    /// An <see cref="ASpec{T}"/> representing the combination of both
    /// specifications with a conditional OR operator.
    /// </returns>
    public static ASpec<T> operator |(ASpec<T> left, ASpec<T> right)
        => new OrSpec<T>(left, right);

    /// <summary>
    /// Compares a specified specification to a specified boolean value
    /// and negates it if the boolean value is false.
    /// </summary>
    /// <param name="spec">The specification to compare.</param>
    /// <param name="value">The boolean value to compare.</param>
    /// <returns>
    /// An <see cref="ASpec{T}"/> representing the negation of the
    /// specified specification if the specified boolean value was false;
    /// otherwise, the original specification.
    /// </returns>
    public static ASpec<T> operator ==(ASpec<T> spec, bool value)
        => value ? spec : !spec;

    /// <summary>
    /// Compares a specified specification to a specified boolean value
    /// and negates it if the boolean value is false.
    /// </summary>
    /// <param name="value">The boolean value to compare.</param>
    /// <param name="spec">The specification to compare.</param>
    /// <returns>
    /// An <see cref="ASpec{T}"/> representing the negation of the
    /// specified specification if the specified boolean value was false;
    /// otherwise, the original specification.
    /// </returns>
    public static ASpec<T> operator ==(bool value, ASpec<T> spec)
        => value ? spec : !spec;

    /// <summary>
    /// Compares a specified specification to a specified boolean value
    /// and negates it if the boolean value is true.
    /// </summary>
    /// <param name="spec">The specification to compare.</param>
    /// <param name="value">The boolean value to compare.</param>
    /// <returns>
    /// An <see cref="ASpec{T}"/> representing the negation of the
    /// specified specification if the specified boolean value was true;
    /// otherwise, the original specification.
    /// </returns>
    public static ASpec<T> operator !=(ASpec<T> spec, bool value)
        => value ? !spec : spec;

    /// <summary>
    /// Compares a specified specification to a specified boolean value
    /// and negates it if the boolean value is true.
    /// </summary>
    /// <param name="value">The boolean value to compare.</param>
    /// <param name="spec">The specification to compare.</param>
    /// <returns>
    /// An <see cref="ASpec{T}"/> representing the negation of the
    /// specified specification if the specified boolean value was true;
    /// otherwise, the original specification.
    /// </returns>
    public static ASpec<T> operator !=(bool value, ASpec<T> spec)
        => value ? !spec : spec;
}

file sealed class CastSpec<TFrom, TTo> : ASpec<TTo> where TTo : TFrom
{
    public CastSpec(Expression<Func<TFrom, bool>> predicate)
    {
        Predicate = PredicateBuilder.Cast<TFrom, TTo>(predicate);
    }

    public override Expression<Func<TTo, bool>> Predicate { get; }
}

file sealed class NotSpec<T> : ASpec<T>
{
    public NotSpec(Expression<Func<T, bool>> predicate)
    {
        Predicate = predicate.Not();
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
