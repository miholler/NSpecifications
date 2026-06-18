namespace NSpecifications;

/// <summary>
/// Provides extension members for <see cref="ISpecification{T}"/> objects.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Determines whether a candidate object satisfies a specification.
    /// </summary>
    /// <typeparam name="T">The type of the candidate object to which the specification applies.</typeparam>
    /// <param name="candidate">The candidate object to evaluate.</param>
    /// <param name="spec">The specification to evaluate against.</param>
    /// <returns>
    /// <see langword="true"/> if the candidate object satisfies the specification;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Is<T>(this T candidate, ISpecification<T> spec)
    {
        ArgumentNullException.ThrowIfNull(spec);

        return spec.IsSatisfiedBy(candidate);
    }

    /// <summary>
    /// Determines whether multiple candidate objects all satisfy a specification.
    /// </summary>
    /// <typeparam name="T">The type of the candidate objects to which the specification applies.</typeparam>
    /// <param name="candidates">The candidate objects to evaluate.</param>
    /// <param name="spec">The specification to evaluate against.</param>
    /// <returns>
    /// <see langword="true"/> if all candidate objects satisfy the specification;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Are<T>(this IEnumerable<T> candidates, ISpecification<T> spec)
    {
        ArgumentNullException.ThrowIfNull(candidates);
        ArgumentNullException.ThrowIfNull(spec);

        return candidates.All(spec.IsSatisfiedBy);
    }

    /// <summary>
    /// Creates a specification that represents the logical negation
    /// of a specification.
    /// </summary>
    /// <typeparam name="T">The type of the candidate object to which the specification applies.</typeparam>
    /// <param name="spec">The specification to negate.</param>
    /// <returns>
    /// An <see cref="ISpecification{T}"/> that represents the logical negation
    /// of the original specification.
    /// </returns>
    public static ISpecification<T> Not<T>(this ISpecification<T> spec)
    {
        ArgumentNullException.ThrowIfNull(spec);

        return new NotSpecification<T>(spec);
    }

    /// <summary>
    /// Creates a specification that combines two specifications
    /// with a conditional AND operator.
    /// </summary>
    /// <typeparam name="T">The type of the candidate object to which the specifications apply.</typeparam>
    /// <param name="left">The first specification to combine.</param>
    /// <param name="right">The second specification to combine.</param>
    /// <returns>
    /// An <see cref="ISpecification{T}"/> that represents the combination of
    /// both specifications with a conditional AND operator.
    /// </returns>
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new AndSpecification<T>(left, right);
    }

    /// <summary>
    /// Creates a specification that combines two specifications
    /// with a conditional OR operator.
    /// </summary>
    /// <typeparam name="T">The type of the candidate object to which the specifications apply.</typeparam>
    /// <param name="left">The first specification to combine.</param>
    /// <param name="right">The second specification to combine.</param>
    /// <returns>
    /// An <see cref="ISpecification{T}"/> that represents the combination of
    /// both specifications with a conditional OR operator.
    /// </returns>
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new OrSpecification<T>(left, right);
    }

    extension<T>(ISpecification<T>)
    {
        /// <summary>
        /// Creates a specification that represents the logical negation
        /// of a specified specification.
        /// </summary>
        /// <param name="spec">The specification to negate.</param>
        /// <returns>
        /// An <see cref="ISpecification{T}"/> that represents the logical negation
        /// of the specified specification.
        /// </returns>
        public static ISpecification<T> operator !(ISpecification<T> spec)
            => spec.Not();

        /// <summary>
        /// Creates a specification that combines two specified specifications
        /// with a conditional AND operator.
        /// </summary>
        /// <param name="left">The first specification to combine.</param>
        /// <param name="right">The second specification to combine.</param>
        /// <returns>
        /// An <see cref="ISpecification{T}"/> that represents the combination of both
        /// specified specifications with a conditional AND operator.
        /// </returns>
        public static ISpecification<T> operator &(ISpecification<T> left, ISpecification<T> right)
            => left.And(right);

        /// <summary>
        /// Creates a specification that combines two specified specifications
        /// with a conditional OR operator.
        /// </summary>
        /// <param name="left">The first specification to combine.</param>
        /// <param name="right">The second specification to combine.</param>
        /// <returns>
        /// An <see cref="ISpecification{T}"/> that represents the combination of both
        /// specified specifications with a conditional OR operator.
        /// </returns>
        public static ISpecification<T> operator |(ISpecification<T> left, ISpecification<T> right)
            => left.Or(right);

        /// <summary>
        /// Compares a specified specification to a specified boolean condition
        /// and negates it if the boolean condition is false.
        /// </summary>
        /// <param name="spec">The specification to compare.</param>
        /// <param name="condition">The boolean condition to compare against.</param>
        /// <returns>
        /// An <see cref="ISpecification{T}"/> that represents the negation of the
        /// specified specification if the boolean condition is false;
        /// otherwise, the original specification.
        /// </returns>
        public static ISpecification<T> operator ==(ISpecification<T> spec, bool condition)
            => condition ? spec : !spec;

        /// <summary>
        /// Compares a specified specification to a specified boolean condition
        /// and negates it if the boolean condition is false.
        /// </summary>
        /// <param name="condition">The boolean condition to compare against.</param>
        /// <param name="spec">The specification to compare.</param>
        /// <returns>
        /// An <see cref="ISpecification{T}"/> that represents the negation of the
        /// specified specification if the boolean condition is false;
        /// otherwise, the original specification.
        /// </returns>
        public static ISpecification<T> operator ==(bool condition, ISpecification<T> spec)
            => condition ? spec : !spec;

        /// <summary>
        /// Compares a specified specification to a specified boolean condition
        /// and negates it if the boolean condition is true.
        /// </summary>
        /// <param name="spec">The specification to compare.</param>
        /// <param name="condition">The boolean condition to compare against.</param>
        /// <returns>
        /// An <see cref="ISpecification{T}"/> that represents the negation of the
        /// specified specification if the boolean condition is true;
        /// otherwise, the original specification.
        /// </returns>
        public static ISpecification<T> operator !=(ISpecification<T> spec, bool condition)
            => condition ? !spec : spec;

        /// <summary>
        /// Compares a specified specification to a specified boolean condition
        /// and negates it if the boolean condition is true.
        /// </summary>
        /// <param name="condition">The boolean condition to compare against.</param>
        /// <param name="spec">The specification to compare.</param>
        /// <returns>
        /// An <see cref="ISpecification{T}"/> that represents the negation of the
        /// specified specification if the boolean condition is true;
        /// otherwise, the original specification.
        /// </returns>
        public static ISpecification<T> operator !=(bool condition, ISpecification<T> spec)
            => condition ? !spec : spec;
    }
}

file sealed record NotSpecification<T>(ISpecification<T> Operand) : ISpecification<T>
{
    public bool IsSatisfiedBy(T candidate)
        => !Operand.IsSatisfiedBy(candidate);
}

file sealed record AndSpecification<T>(ISpecification<T> Left, ISpecification<T> Right) : ISpecification<T>
{
    public bool IsSatisfiedBy(T candidate)
        => Left.IsSatisfiedBy(candidate) && Right.IsSatisfiedBy(candidate);
}

file sealed record OrSpecification<T>(ISpecification<T> Left, ISpecification<T> Right) : ISpecification<T>
{
    public bool IsSatisfiedBy(T candidate)
        => Left.IsSatisfiedBy(candidate) || Right.IsSatisfiedBy(candidate);
}
