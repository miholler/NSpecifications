using System.Diagnostics.CodeAnalysis;
using static NSpecifications.Internal.PredicateBuilder;

namespace NSpecifications;

/// <summary>
/// Provides a set of static methods for working
/// with <see cref="Spec{T}"/> instances.
/// </summary>
public static class Spec
{
    /// <summary>
    /// Creates a specification that is satisfied by any candidate object.
    /// </summary>
    /// <typeparam name="T">The type of the candidate object to which the specification applies.</typeparam>
    /// <returns>A <see cref="Spec{T}"/> that is satisfied by any candidate object.</returns>
    public static Spec<T> Any<T>()
        => Spec<T>.Any;

    /// <summary>
    /// Creates a specification that is not satisfied by any candidate object.
    /// </summary>
    /// <typeparam name="T">The type of the candidate object to which the specification applies.</typeparam>
    /// <returns>A <see cref="Spec{T}"/> that is not satisfied by any candidate object.</returns>
    public static Spec<T> None<T>()
        => Spec<T>.None;

    /// <summary>
    /// Creates a specification that is defined by a specified predicate expression.
    /// </summary>
    /// <typeparam name="T">The type of the candidate object to which the specification applies.</typeparam>
    /// <param name="predicate">The predicate expression that defines the specification.</param>
    /// <returns>A <see cref="Spec{T}"/> that is defined by <paramref name="predicate"/>.</returns>
    public static Spec<T> Create<T>(Expression<Func<T, bool>> predicate)
        => Spec<T>.Create(predicate);
}

/// <summary>
/// Represents a generic specification that is defined by a predicate expression.
/// </summary>
/// <typeparam name="T">The type of the candidate object to which the specification applies.</typeparam>
/// <param name="Predicate">The predicate expression that defines the specification.</param>
public record Spec<T>(Expression<Func<T, bool>> Predicate) : ISpecification<T>, ISpecification
{
    /// <summary>
    /// Represents a specification that is satisfied by any candidate object.
    /// </summary>
    public static readonly Spec<T> Any = new(_ => true);

    /// <summary>
    /// Represents a specification that is not satisfied by any candidate object.
    /// </summary>
    public static readonly Spec<T> None = new(_ => false);

    /// <summary>
    /// Gets the compiled predicate that defines the current specification.
    /// </summary>
    protected Func<T, bool> CompiledPredicate => field ??= Predicate.Compile();

    /// <summary>
    /// Creates a specification that is defined by a specified predicate expression.
    /// </summary>
    /// <param name="predicate">The predicate expression that defines the specification.</param>
    /// <returns>A <see cref="Spec{T}"/> that is defined by <paramref name="predicate"/>.</returns>
    public static Spec<T> Create(Expression<Func<T, bool>> predicate)
        => new(predicate);

    /// <summary>
    /// Casts the current specification to a specification of a specified derived candidate type.
    /// </summary>
    /// <typeparam name="TDerived">The type of the derived candidate object.</typeparam>
    /// <returns>
    /// A <see cref="Spec{TDerived}"/> whose candidate type is of type <typeparamref name="TDerived"/>.
    /// </returns>
    public Spec<TDerived> CastUp<TDerived>() where TDerived : T
        => CastUp<T, TDerived>(Predicate);

    /// <summary>
    /// Determines whether a specified candidate object satisfies the current specification.
    /// </summary>
    /// <param name="candidate">The candidate object to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if the candidate object satisfies the specification;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsSatisfiedBy(T candidate)
        => CompiledPredicate(candidate);

    /// <summary>
    /// Determines whether a specified candidate object satisfies the current specification.
    /// </summary>
    /// <param name="candidate">The candidate object to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if the candidate object satisfies the specification;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool ISpecification.IsSatisfiedBy(object? candidate)
        => IsSatisfiedBy((T)candidate!);

    /// <summary>
    /// Returns a string that represents the current specification.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents the current specification.</returns>
    public override string ToString()
        => Predicate.ToString();

    /// <summary>
    /// Implicitly converts a predicate expression to a specification.
    /// </summary>
    /// <param name="predicate">The predicate expression to convert.</param>
    public static implicit operator Spec<T>(Expression<Func<T, bool>> predicate)
        => new(predicate);

    /// <summary>
    /// Implicitly converts a specification to its underlying predicate expression.
    /// </summary>
    /// <param name="spec">The specification to convert.</param>
    [return: NotNullIfNotNull(nameof(spec))]
    public static implicit operator Expression<Func<T, bool>>?(Spec<T>? spec)
        => spec?.Predicate;

    /// <summary>
    /// Implicitly converts a specification to a predicate function.
    /// </summary>
    /// <param name="spec">The specification to convert.</param>
    [return: NotNullIfNotNull(nameof(spec))]
    public static implicit operator Func<T, bool>?(Spec<T>? spec)
        => spec?.CompiledPredicate;

    /// <summary>
    /// Creates a specification that represents the logical negation
    /// of a specified specification.
    /// </summary>
    /// <param name="spec">The specification to negate.</param>
    /// <returns>
    /// A <see cref="Spec{T}"/> that represents the logical negation
    /// of the specified specification.
    /// </returns>
    public static Spec<T> operator !(Spec<T> spec)
        => Not<T>(spec);

    /// <summary>
    /// Creates a specification that combines two specified specifications
    /// with a conditional AND operator.
    /// </summary>
    /// <param name="left">The first specification to combine.</param>
    /// <param name="right">The second specification to combine.</param>
    /// <returns>
    /// A <see cref="Spec{T}"/> that represents the combination of both
    /// specified specifications with a conditional AND operator.
    /// </returns>
    public static Spec<T> operator &(Spec<T> left, Spec<T> right)
        => AndAlso<T>(left, right);

    /// <summary>
    /// Creates a specification that combines two specified specifications
    /// with a conditional OR operator.
    /// </summary>
    /// <param name="left">The first specification to combine.</param>
    /// <param name="right">The second specification to combine.</param>
    /// <returns>
    /// A <see cref="Spec{T}"/> that represents the combination of both
    /// specified specifications with a conditional OR operator.
    /// </returns>
    public static Spec<T> operator |(Spec<T> left, Spec<T> right)
        => OrElse<T>(left, right);

    /// <summary>
    /// Compares a specified specification to a specified boolean condition
    /// and negates it if the boolean condition is false.
    /// </summary>
    /// <param name="spec">The specification to compare.</param>
    /// <param name="condition">The boolean condition to compare against.</param>
    /// <returns>
    /// A <see cref="Spec{T}"/> that represents the negation of the
    /// specified specification if the boolean condition is false;
    /// otherwise, the original specification.
    /// </returns>
    public static Spec<T> operator ==(Spec<T> spec, bool condition)
        => condition ? spec : !spec;

    /// <summary>
    /// Compares a specified specification to a specified boolean condition
    /// and negates it if the boolean condition is false.
    /// </summary>
    /// <param name="condition">The boolean condition to compare against.</param>
    /// <param name="spec">The specification to compare.</param>
    /// <returns>
    /// A <see cref="Spec{T}"/> that represents the negation of the
    /// specified specification if the boolean condition is false;
    /// otherwise, the original specification.
    /// </returns>
    public static Spec<T> operator ==(bool condition, Spec<T> spec)
        => condition ? spec : !spec;

    /// <summary>
    /// Compares a specified specification to a specified boolean condition
    /// and negates it if the boolean condition is true.
    /// </summary>
    /// <param name="spec">The specification to compare.</param>
    /// <param name="condition">The boolean condition to compare against.</param>
    /// <returns>
    /// A <see cref="Spec{T}"/> that represents the negation of the
    /// specified specification if the boolean condition is true;
    /// otherwise, the original specification.
    /// </returns>
    public static Spec<T> operator !=(Spec<T> spec, bool condition)
        => condition ? !spec : spec;

    /// <summary>
    /// Compares a specified specification to a specified boolean condition
    /// and negates it if the boolean condition is true.
    /// </summary>
    /// <param name="condition">The boolean condition to compare against.</param>
    /// <param name="spec">The specification to compare.</param>
    /// <returns>
    /// A <see cref="Spec{T}"/> that represents the negation of the
    /// specified specification if the boolean condition is true;
    /// otherwise, the original specification.
    /// </returns>
    public static Spec<T> operator !=(bool condition, Spec<T> spec)
        => condition ? !spec : spec;
}
