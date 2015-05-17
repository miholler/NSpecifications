using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSpecifications
{
    internal class NotSpecification<T> : INotSpecification<T>
    {
        public ISpecification<T> Inner { get; private set; }

        internal NotSpecification(ISpecification<T> inner)
        {
            if (inner == null)
                throw new ArgumentNullException("spec");

            Inner = inner;
        }

        public Expression<Func<T, bool>> Expression
        {
            get { return Inner.Expression.Not(); }
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return !Inner.IsSatisfiedBy(candidate);
        }
    }

}
