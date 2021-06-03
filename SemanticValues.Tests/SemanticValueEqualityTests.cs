using System;
using FsCheck;
using FsCheck.Xunit;

namespace SemanticValues.Tests
{
    public class SemanticValueEqualityTests
    {
        [Property]
        public Property WorksForChar(char value, char otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForString(string value, string otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);

        [Property]
        public Property WorksForGuid(Guid value, Guid otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForShort(short value, short otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForInt(int value, int otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForLong(long value, long otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);

        [Property]
        public Property WorksForFloat(float value, float otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForDouble(double value, double otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForDecimal(decimal value, decimal otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForBool(bool value, bool otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForByte(byte value, byte otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);
        
        [Property]
        public Property WorksForObject(object value, object otherValue, bool useSameValue) => WorksForType(value, otherValue, useSameValue);

        [Property]
        public Property WorksForDynamic(dynamic value, dynamic otherValue, bool useSameValue) => WorksForType<dynamic>(value, otherValue, useSameValue);
        
        private static Property WorksForType<TValue>(TValue value, TValue otherValue, bool useSameValue) where TValue : notnull
        {
            return SameTypeWithSameValueAreEqual(value)
                .And(DifferentTypesAreNeverEqual(value, otherValue, useSameValue)
                    .And(SameTypeWithDifferentValuesAreOnlyEqualWhenValuesAreEqual(value, otherValue, useSameValue)));
        }
        
        private static Property SameTypeWithSameValueAreEqual(object value)
        {
            var first = new SemanticObject(value);
            var second = new SemanticObject(value);

            return Prop.OfTestable(first == second)
                .And(Prop.OfTestable(first.Equals(second)))
                .And(Prop.OfTestable(second.Equals(first)))
                .Collect(value?.GetType());
        }

        private static Property DifferentTypesAreNeverEqual(object value, object otherValue, bool useSameValue)
        {
            var first = new SemanticObject(value);
            var second = useSameValue ? new DifferentSemanticObject(value) : new DifferentSemanticObject(otherValue);

            return Prop.OfTestable(first != second)
                .And(Prop.OfTestable(!first.Equals(second)))
                .And(Prop.OfTestable(!second.Equals(first)))
                .Collect($"{nameof(useSameValue)}: {useSameValue}");
        }

        private static Property SameTypeWithDifferentValuesAreOnlyEqualWhenValuesAreEqual(object value, object otherValue, bool useSameValue)
        {
            var first = new SemanticObject(value);
            var second = useSameValue ? new SemanticObject(value) : new SemanticObject(otherValue);

            var valuesEqual = useSameValue || Object.Equals(value, otherValue);

            return Prop.Given(valuesEqual,
                    Prop.OfTestable(first == second).Label("==")
                        .And(Prop.OfTestable(first.Equals(second))).Label("first.Equals(second)")
                        .And(Prop.OfTestable(second.Equals(first))).Label("second.Equals(first)"),
                    Prop.OfTestable(first != second).Label("!=")
                        .And(Prop.OfTestable(!first.Equals(second))).Label("!first.Equals(second)")
                        .And(Prop.OfTestable(!second.Equals(first)))).Label("!second.Equals(first)")
                .Classify(valuesEqual, "Values are equal")
                .Classify(!valuesEqual, "Values are not equal")
                .Collect($"{nameof(useSameValue)}: {useSameValue}");
        }
    }
}