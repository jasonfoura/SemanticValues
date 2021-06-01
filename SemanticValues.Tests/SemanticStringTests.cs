using System;
using SemanticValues.Exceptions;
using Xunit;

namespace SemanticValues.Tests
{
    public class SemanticStringTests
    {
        private record TestSemanticStringValue : SemanticValue<string>
        {
            private static Func<string, bool> TestValidationFunc => value => value == "valid value";

            public TestSemanticStringValue(string value) : base(value, TestValidationFunc)
            {
            }
        }
        
        [Fact]
        public void ValidValuesCanBeConstructed()
        {
            _ = new TestSemanticStringValue("valid value");
        }
        
        [Fact]
        public void InvalidValuesThrowException()
        {
            Assert.Throws<InvalidSemanticValueException<string>>(() =>
            {
                _ = new TestSemanticStringValue("invalid value");
            });
        }
    }
}