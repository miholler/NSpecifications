using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSpecifications
{
    public static class Spec
    {
        public static Specification<T> For<T>(Expression<Func<T, bool>> expression)
        {
            return new GenericSpecification<T>(expression);
        }

        public static Specification<T> All<T>()
        {
            return GenericSpecification<T>.All;
        }

        public static Specification<T> None<T>()
        {
            return GenericSpecification<T>.None;
        }
    }
}
