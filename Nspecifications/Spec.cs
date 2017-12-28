using System;
using System.Linq.Expressions;

namespace NSpecifications
{
    /// <summary>
    /// Facade for a fluent language.
    /// </summary>
    public static class Spec
    {
        /// <summary>
        /// Satisfied by any candidates.
        /// </summary>
        public static ASpec<T> Any<T>()
        {
            return Spec<T>.Any;
        }

        /// <summary>
        /// Not satisfied by any candidate.
        /// </summary>
        public static ASpec<T> Not<T>()
        {
            return Spec<T>.None;
        }
    }

    /// <summary>
    /// Generic implementation of ASpecification abstract class.
    /// </summary>
    /// <typeparam name="T">The type of candidate.</typeparam>
    /// <remarks>If your specification relies on properties that are changed after instantiating it
    /// this implementation might fail because the IsSatisfiedBy is compiled and cached on it's first 
    /// usage.</remarks>
    /// <example><code>var blackSpec = new <see cref="Spec{T}"/>(c => c.Name.ToLower() == "black");></code></example>
    public class Spec<T> : ASpec<T>
    {
        readonly Expression<Func<T, bool>> _expression;

        /// <summary>
        /// Satisfied by any candidates.
        /// </summary>
        public static readonly ASpec<T> Any = new Spec<T>(x => true);

        /// <summary>
        /// Not satisfied by any candidate.
        /// </summary>
        public static readonly ASpec<T> None = new Spec<T>(x => false);

        /// <summary>
        /// Caches the compiled Expression so that it doesn't have to compile every time IsSatisfiedBy is
        /// invoked.
        /// </summary>
        readonly Lazy<Func<T, bool>> _compiledExpression;

        public Spec(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
            _compiledExpression = new Lazy<Func<T, bool>>(() => _expression.Compile());
        }

        public override Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return _compiledExpression.Value(candidate);
        }

    }
}
