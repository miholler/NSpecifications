using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSpecifications
{
    /// <summary>
    /// CategorySpec defined by an Expressions that can be used by IQueryables.
    /// Implements !, &amp; and | operators.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public abstract class ASpecification<T> : ISpecification<T>
    {

        /// <summary>
        /// Holds the compiled expression so that it doesn't need to compile it everytime.
        /// </summary>
        Func<T, bool> _compiledFunc;

        public virtual bool IsSatisfiedBy(T entity)
        {
            _compiledFunc = _compiledFunc ?? this.Expression.Compile();
            return _compiledFunc(entity);
        }

        public abstract Expression<Func<T, bool>> Expression { get; }

        public static And<T> operator &(ASpecification<T> spec1, ASpecification<T> spec2)
        {
            return new And<T>(spec1, spec2);
        }

        public static Or<T> operator |(ASpecification<T> spec1, ASpecification<T> spec2)
        {
            return new Or<T>(spec1, spec2);
        }

        public static Not<T> operator !(ASpecification<T> spec)
        {
            return new Not<T>(spec);
        }

        public static implicit operator Expression<Func<T, bool>>(ASpecification<T> specification)
        {
            return specification.Expression;
        }

        public static implicit operator Func<T, bool>(ASpecification<T> specification)
        {
            return specification.IsSatisfiedBy;
        }

        public override string ToString()
        {
            return Expression.ToString();
        }

        public sealed class And<T> : ASpecification<T>, IOrSpecification<T>
        {
            public ASpecification<T> Spec1 { get; private set; }

            public ASpecification<T> Spec2 { get; private set; }

            ISpecification<T> IOrSpecification<T>.Spec1 { get { return Spec1; } }

            ISpecification<T> IOrSpecification<T>.Spec2 { get { return Spec1; } }

            internal And(ASpecification<T> spec1, ASpecification<T> spec2)
            {
                if (spec1 == null)
                    throw new ArgumentNullException("spec1");

                if (spec2 == null)
                    throw new ArgumentNullException("spec2");

                Spec1 = spec1;
                Spec2 = spec2;
            }

            public override Expression<Func<T, bool>> Expression
            {
                get { return Spec1.Expression.And(Spec2.Expression); }
            }

            public new bool IsSatisfiedBy(T candidate)
            {
                return Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate);
            }
        }

        public sealed class Or<T> : ASpecification<T>, IOrSpecification<T>
        {
            public ASpecification<T> Spec1 { get; private set; }

            public ASpecification<T> Spec2 { get; private set; }

            ISpecification<T> IOrSpecification<T>.Spec1 { get { return Spec1; } }

            ISpecification<T> IOrSpecification<T>.Spec2 { get { return Spec1; } }

            internal Or(ASpecification<T> spec1, ASpecification<T> spec2)
            {
                if (spec1 == null)
                    throw new ArgumentNullException("spec1");

                if (spec2 == null)
                    throw new ArgumentNullException("spec2");

                Spec1 = spec1;
                Spec2 = spec2;
            }

            public override Expression<Func<T, bool>> Expression
            {
                get { return Spec1.Expression.Or(Spec2.Expression); }
            }

            public new bool IsSatisfiedBy(T candidate)
            {
                return Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
            }
        }

        public sealed class Not<T> : ASpecification<T>, INotSpecification<T>
        {
            public ASpecification<T> Inner { get; private set; }

            ISpecification<T> INotSpecification<T>.Inner { get { return Inner; } }

            internal Not(ASpecification<T> spec)
            {
                if (spec == null)
                    throw new ArgumentNullException("spec");

                Inner = spec;
            }

            public override Expression<Func<T, bool>> Expression
            {
                get { return Inner.Expression.Not(); }
            }

            public new bool IsSatisfiedBy(T candidate)
            {
                return !Inner.IsSatisfiedBy(candidate);
            }
        }

    }
}
