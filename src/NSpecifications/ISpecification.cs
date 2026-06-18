namespace NSpecifications;

/// <summary>
/// Defines methods to determine whether an
/// object meets a defined set of criteria.
/// </summary>
public interface ISpecification
{
    /// <summary>
    /// Determines whether a specified candidate object satisfies the specification.
    /// </summary>
    /// <param name="candidate">The candidate object to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if the candidate object satisfies the specification;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSatisfiedBy(object? candidate);
}

/// <summary>
/// Defines methods to determine whether an
/// object meets a defined set of criteria.
/// </summary>
/// <typeparam name="T">The type of the candidate object to which the specification applies.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Determines whether a specified candidate object satisfies the specification.
    /// </summary>
    /// <param name="candidate">The candidate object to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if the candidate object satisfies the specification;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSatisfiedBy(T candidate);
}
