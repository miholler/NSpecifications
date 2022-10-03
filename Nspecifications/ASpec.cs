using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NSpecifications
{
    /// <summary>
    /// Abstract Specification defined by an Expressions that can be used on IQueryables.
    /// </summary>
    /// <typeparam name="T">The type of the candidate.</typeparam>
    public abstract class ASpec<T> : ISpecification<T>
    {
        /// <summary>
        /// Holds the compiled expression so that it doesn't need to compile it everytime.
        /// </summary>
        Func<T, bool> _compiledFunc;

        /// <summary>
        /// Checks if a certain candidate meets a given specification.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns>New specification</returns>
        public virtual bool IsSatisfiedBy(T candidate)
        {
            _compiledFunc = _compiledFunc ?? this.Expression.Compile();
            return _compiledFunc(candidate);
        }

        /// <summary>
        /// Expression that defines the specification.
        /// </summary>
        public abstract Expression<Func<T, bool>> Expression { get; }

        /// <summary>
        /// Composes two specifications with an And operator.
        /// </summary>
        /// <param name="spec1">Specification</param>
        /// <param name="spec2">Specification</param>
        /// <returns>New specification</returns>
        public static And<T> operator &(ASpec<T> spec1, ASpec<T> spec2)
        {
            return new And<T>(spec1, spec2);
        }

        /// <summary>
        /// Composes two specifications with an Or operator.
        /// </summary>
        /// <param name="spec1">Specification</param>
        /// <param name="spec2">Specification</param>
        /// <returns>New specification</returns>
        public static Or<T> operator |(ASpec<T> spec1, ASpec<T> spec2)
        {
            return new Or<T>(spec1, spec2);
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is true.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator ==(bool value, ASpec<T> spec)
        {
            return value ? spec : !spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is true.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator ==(ASpec<T> spec, bool value)
        {
            return value ? spec : !spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is false.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator !=(bool value, ASpec<T> spec)
        {
            return value ? !spec : spec;
        }

        /// <summary>
        /// Combines a specification with a boolean value. 
        /// The candidate meets the criteria only when the boolean is false.
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static ASpec<T> operator !=(ASpec<T> spec, bool value)
        {
            return value ? !spec : spec;
        }

        /// <summary>
        /// Creates a new specification that negates a given specification.
        /// </summary>
        /// <param name="spec">Specification</param>
        /// <returns>New specification</returns>
        public static Not<T> operator !(ASpec<T> spec)
        {
            return new Not<T>(spec);
        }

        /// <summary>
        /// Allows using ASpec[T] in place of a lambda expression.
        /// </summary>
        /// <param name="spec"></param>
        public static implicit operator Expression<Func<T, bool>>(ASpec<T> spec)
        {
            return spec.Expression;
        }

        /// <summary>
        /// Allows using ASpec[T] in place of Func[T, bool].
        /// </summary>
        /// <param name="spec"></param>
        public static implicit operator Func<T, bool>(ASpec<T> spec)
        {
            return spec.IsSatisfiedBy;
        }

        /// <summary>
        /// Converts the expression into a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Expression.ToString();
        }

        public sealed class And<T> : ASpec<T>, IAndSpecification<T>
        {
            public ASpec<T> Spec1 { get; private set; }

            public ASpec<T> Spec2 { get; private set; }

            ISpecification<T> IOrSpecification<T>.Spec1 { get { return Spec1; } }

            ISpecification<T> IOrSpecification<T>.Spec2 { get { return Spec1; } }

            internal And(ASpec<T> spec1, ASpec<T> spec2)
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

        public sealed class Or<T> : ASpec<T>, IOrSpecification<T>
        {
            public ASpec<T> Spec1 { get; private set; }

            public ASpec<T> Spec2 { get; private set; }

            ISpecification<T> IOrSpecification<T>.Spec1 { get { return Spec1; } }

            ISpecification<T> IOrSpecification<T>.Spec2 { get { return Spec1; } }

            internal Or(ASpec<T> spec1, ASpec<T> spec2)
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

            public new bool Is(T candidate)
            {
                return Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
            }
        }

        public sealed class Not<T> : ASpec<T>, INotSpecification<T>
        {
            public ASpec<T> Inner { get; private set; }

            ISpecification<T> INotSpecification<T>.Inner { get { return Inner; } }

            internal Not(ASpec<T> spec)
            {
                if (spec == null)
                    throw new ArgumentNullException("spec");

                Inner = spec;
            }

            public override Expression<Func<T, bool>> Expression
            {
                get { return Inner.Expression.Not(); }
            }

            public new bool Is(T candidate)
            {
                return !Inner.IsSatisfiedBy(candidate);
            }
        }
    }
}
