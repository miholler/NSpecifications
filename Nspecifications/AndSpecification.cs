using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSpecifications
{
    internal class AndSpecification<T> : IAndSpecification<T>
    {
        public ISpecification<T> Spec1 { get; private set; }

        public ISpecification<T> Spec2 { get; private set; }

        internal AndSpecification(ISpecification<T> spec1, ISpecification<T> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            Spec1 = spec1;
            Spec2 = spec2;
        }

        public Expression<Func<T, bool>> Expression
        {
            get { return Spec1.Expression.And(Spec2.Expression); }
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate);
        }
    }
}
