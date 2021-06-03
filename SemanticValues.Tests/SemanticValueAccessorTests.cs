using System;
using FsCheck;
using FsCheck.Xunit;

namespace SemanticValues.Tests
{
    public class SemanticValueAccessorTests
    {
        [Property]
        public Property WorksForChar(char value) => WorksForType(value);
        
        [Property]
        public Property WorksForString(string value) => WorksForType(value);

        [Property]
        public Property WorksForGuid(Guid value) => WorksForType(value);
        
        [Property]
        public Property WorksForShort(short value) => WorksForType(value);
        
        [Property]
        public Property WorksForInt(int value) => WorksForType(value);
        
        [Property]
        public Property WorksForLong(long value) => WorksForType(value);

        [Property]
        public Property WorksForFloat(float value) => WorksForType(value);
        
        [Property]
        public Property WorksForDouble(double value) => WorksForType(value);
        
        [Property]
        public Property WorksForDecimal(decimal value) => WorksForType(value);
        
        [Property]
        public Property WorksForBool(bool value) => WorksForType(value);
        
        [Property]
        public Property WorksForByte(byte value) => WorksForType(value);
        
        [Property]
        public Property WorksForObject(object value) => WorksForType(value);

        [Property]
        public Property WorksForDynamic(dynamic value) => WorksForType<dynamic>(value);
        
        private static Property WorksForType<TValue>(TValue value) where TValue : notnull
        {
            return PropertyValueIsUnchanged(value);
        }
        
        private static Property PropertyValueIsUnchanged(object value)
        {
            var s = new SemanticObject(value);

            return Prop.OfTestable(s.Value == value)
                .Collect(value?.GetType());
        }
    }
}