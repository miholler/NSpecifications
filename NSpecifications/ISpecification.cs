namespace NSpecifications;

/// <summary>
/// Defines methods to determine whether an
/// object meets a defined set of criteria.
/// </summary>
public interface ISpecification
{
    /// <summary>
    /// Determines whether the specification is satisfied by a specified value.
    /// </summary>
    /// <param name="candidate">The value to check.</param>
    /// <returns>
    /// <see langword="true"/> if the specification is satisfied by the specified value;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSatisfiedBy(object? candidate);
}

/// <summary>
/// Defines methods to determine whether an
/// object meets a defined set of criteria.
/// </summary>
/// <typeparam name="T">The type of the candidate object.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Determines whether the specification is satisfied by a specified value.
    /// </summary>
    /// <param name="candidate">The value to check.</param>
    /// <returns>
    /// <see langword="true"/> if the specification is satisfied by the specified value;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSatisfiedBy(T candidate);
}
