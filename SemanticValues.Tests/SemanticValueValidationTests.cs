using System;
using FsCheck;
using FsCheck.Xunit;
using SemanticValues.Exceptions;

namespace SemanticValues.Tests
{
    public class SemanticValueValidationTests
    {
        private record MockValidatedSemanticObject : SemanticValue<object>
        {
            public MockValidatedSemanticObject(object value, bool isValid) : base(value, _ => isValid)
            {
            }
        }
        
        [Property]
        public Property WorksForChar(char value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForString(NonNull<string> value, bool isValid) => WorksForType(value.Item, isValid);

        [Property]
        public Property WorksForGuid(Guid value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForShort(short value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForInt(int value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForLong(long value, bool isValid) => WorksForType(value, isValid);

        [Property]
        public Property WorksForFloat(float value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForDouble(double value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForDecimal(decimal value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForBool(bool value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForByte(byte value, bool isValid) => WorksForType(value, isValid);
        
        [Property]
        public Property WorksForObject(NonNull<object> value, bool isValid) => WorksForType(value.Item, isValid);

        [Property]
        public Property WorksForDynamic(NonNull<dynamic> value, bool isValid) => WorksForType(value.Item, isValid);
        
        private static Property WorksForType<TValue>(TValue value, bool isValid) where TValue : notnull
        {
            return ValidValuesCanBeConstructedWhileInvalidValuesThrowInvalidSemanticValueException(value, isValid)
                .And(InvalidSemanticValueExceptionIncludesAttemptedValue(value));
        }

        private static Property ValidValuesCanBeConstructedWhileInvalidValuesThrowInvalidSemanticValueException(object value, bool isValid)
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
        
        private static Property InvalidSemanticValueExceptionIncludesAttemptedValue(object value)
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