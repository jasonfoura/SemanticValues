using System;
using SemanticValues.Exceptions;
using Xunit;

namespace SemanticValues.Tests
{
    public class SemanticIntTests
    {
        private record PositiveIntValue : SemanticValue<int>
        {
            private static Func<int, bool> TestValidationFunc => value => value >= 0;

            public PositiveIntValue(int value) : base(value, TestValidationFunc)
            {
            }
        }
        
        [Fact]
        public void ValidValuesCanBeConstructed()
        {
            _ = new PositiveIntValue(42);
        }
        
        [Fact]
        public void InvalidValuesThrowException()
        {
            Assert.Throws<InvalidSemanticValueException<int>>(() =>
            {
                _ = new PositiveIntValue(-1);
            });
        }
    }
}