using System;
using FsCheck;
using FsCheck.Xunit;
using SemanticValues.Exceptions;

namespace SemanticValues.Tests
{
    public class SemanticValueValidationTests
    {
        private record MockValidatedSemanticObject : SemanticValue<object?>
        {
            public MockValidatedSemanticObject(object? value, bool isValid) : base(value, _ => isValid)
            {
            }
        }

        [Property]
        public Property ValidValuesCanBeConstructedWhileInvalidValuesThrowInvalidSemanticValueException(object? value, bool isValid)
        {
            var threwExpectedException = false;
            
            try
            {
                _ = new MockValidatedSemanticObject(value, isValid);
            }
            catch (InvalidSemanticValueException<object>)
            {
                threwExpectedException = true;
            }

            return Prop.Given(isValid, !threwExpectedException, threwExpectedException)
                .Classify(isValid, "Value considered valid")
                .Classify(!isValid, "Value considered invalid")
                .Collect(value?.GetType());
        }
        
        [Property]
        public Property InvalidSemanticValueExceptionIncludesAttemptedValue(object? value)
        {
            try
            {
                _ = new MockValidatedSemanticObject(value, isValid: false);

                throw new InvalidOperationException();
            }
            catch (InvalidSemanticValueException<object> e)
            {
                return Prop.OfTestable(e.Value == value)
                    .Collect(value?.GetType());
            }
        }
    }
}