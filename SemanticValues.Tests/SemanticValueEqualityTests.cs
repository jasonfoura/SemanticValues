using System;
using FsCheck;
using FsCheck.Xunit;

namespace SemanticValues.Tests
{
    public class SemanticValueEqualityTests
    {
        [Property]
        public Property SameTypeWithSameValueAreEqual(object? value)
        {
            var first = new SemanticObject(value);
            var second = new SemanticObject(value);

            return Prop.OfTestable(first == second)
                .And(Prop.OfTestable(first.Equals(second)))
                .And(Prop.OfTestable(second.Equals(first)))
                .Collect(value?.GetType());
        }

        [Property]
        public Property DifferentTypesAreNeverEqual(object? value, object? otherValue, bool useSameValue)
        {
            var first = new SemanticObject(value);
            var second = useSameValue ? new DifferentSemanticObject(value) : new DifferentSemanticObject(otherValue);

            return Prop.OfTestable(first != second)
                .And(Prop.OfTestable(!first.Equals(second)))
                .And(Prop.OfTestable(!second.Equals(first)))
                .Collect($"{nameof(useSameValue)}: {useSameValue}");
        }

        [Property]
        public Property SameTypeWithDifferentValuesAreOnlyEqualWhenValuesAreEqual(object? value, object? otherValue, bool useSameValue)
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