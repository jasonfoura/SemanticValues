using Xunit;

namespace SemanticValues.Tests
{
    public class SemanticValueEqualityTests
    {
        private record TestSemanticStringValueA : SemanticValue<string>
        {
            public TestSemanticStringValueA(string value) : base(value)
            {
            }
        }
        
        private record TestSemanticStringValueB : SemanticValue<string>
        {
            public TestSemanticStringValueB(string value) : base(value)
            {
            }
        }
        
        private record TestSemanticIntValue : SemanticValue<int>
        {
            public TestSemanticIntValue(int value) : base(value)
            {
            }
        }
        
        [Fact]
        public void SameTypeWithSameValueAreEqual()
        {
            var commonValue = "any value";
            
            var firstA = new TestSemanticStringValueA(commonValue);
            var otherA = new TestSemanticStringValueA(commonValue);

            Assert.True(firstA == otherA);
        }
        
        [Fact]
        public void SameTypeWithDifferentValueAreNotEqual()
        {
            var a = new TestSemanticStringValueA("a");
            var b = new TestSemanticStringValueA("b");

            Assert.False(a == b);
        }
        
        [Fact]
        public void DifferentTypesWithSameValueAreNotEqual()
        {
            var commonValue = "any value";
            
            var a = new TestSemanticStringValueA(commonValue);
            var b = new TestSemanticStringValueB(commonValue);

            Assert.False(a == b);
        }
        
        [Fact]
        public void DifferentTypesWithDifferentValueAreNotEqual()
        {
            var a = new TestSemanticStringValueA("a");
            var b = new TestSemanticStringValueB("b");

            Assert.False(a == b);
        }
    }
}