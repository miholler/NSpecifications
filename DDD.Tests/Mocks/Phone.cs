using DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDDD.Tests.Mocks
{
    public class Phone : IValueObject<Phone>
    {
        public string Value { get; private set; }

        private Phone() { }

        static bool TryParse(string value, out Phone phone)
        {
            if (value.Length > 10)
            {
                phone = new Phone();
                return false;
            }
            phone = new Phone() { Value = value };
            return true;
        }

        public bool SameValueAs(Phone other)
        {
            return this.Value == other.Value;
        }
    }
}
