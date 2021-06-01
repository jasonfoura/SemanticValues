using System;
using SemanticValues.Exceptions;
using Xunit;

namespace SemanticValues.Tests
{
    public class SemanticGuidTests
    {
        private record NonEmptyGuid : SemanticValue<Guid>
        {
            private static Func<Guid, bool> TestValidationFunc => value => value != Guid.Empty;

            public NonEmptyGuid(Guid value) : base(value, TestValidationFunc)
            {
            }
        }
        
        [Fact]
        public void ValidValuesCanBeConstructed()
        {
            _ = new NonEmptyGuid(Guid.NewGuid());
        }
        
        [Fact]
        public void InvalidValuesThrowException()
        {
            Assert.Throws<InvalidSemanticValueException<Guid>>(() =>
            {
                _ = new NonEmptyGuid(Guid.Empty);
            });
        }
    }
}