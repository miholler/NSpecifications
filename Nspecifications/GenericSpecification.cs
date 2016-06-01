using System;
using System.Linq.Expressions;

namespace NSpecifications
{
    /// <summary>
    /// Generic implementation of ASpecification abstract class.
    /// </summary>
    /// <typeparam name="T">The type of candidate.</typeparam>
    /// <remarks>If your specification relies on properties that are changed after instatiating it
    /// this implementation might fail because the IsSatisfiedBy is compiled and cached on it's first 
    /// usage.</remarks>
    /// <example><code>var blackSpec = new <see cref="GenericSpecification{T}"/>(c => c.Name.ToLower() == "black");></code></example>
    internal class GenericSpecification<T> : Specification<T>
    {
        readonly Expression<Func<T, bool>> _expression;

        internal static readonly Specification<T> All = new GenericSpecification<T>(x => true);

        internal static readonly Specification<T> None = new GenericSpecification<T>(x => false);

        /// <summary>
        /// Caches the compiled Expression so that it doesn't have to compile everytime IsSatisfiedBy is
        /// invoked.
        /// </summary>
        Func<T, bool> _compiledFunc;

        public GenericSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public override Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }


        public override bool IsSatisfiedBy(T candidate)
        {
            _compiledFunc = _compiledFunc ?? Expression.Compile();
            return _compiledFunc(candidate);
        }

    }
}
