﻿using Domain.Common;
using Domain.Exceptions;
using Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public sealed class Number : ValueObject
    {
        public int Value { get; private set; }
        public Number(int value)
        {
            var result = CheckNumber(value);
            if (result.IsSuccess == true)
            {
                Value = value;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        private OperationResult CheckNumber(int value)
        {
            var result = OperationResult.CreateValidator(value)
                .Validate(x => x <= 0, string.Format(ConstMessages.NotNegativeOrZero, nameof(Number)));
            return result;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Number(int value)
        => new Number(value);

        public static implicit operator int(Number number)
            => number.Value;
    }
}
