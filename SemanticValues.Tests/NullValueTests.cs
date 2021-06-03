using System;
using System.Reflection;
using FsCheck;
using FsCheck.Xunit;

namespace SemanticValues.Tests
{
    public class NullValueTests
    {
        private record BrokenRecord : SemanticValue<object>
        {
            public static BrokenRecord CreateWithValue(object? value)
            {
                // Uses reflection to "force" the input value to `null`, even though this is not "legal" with nullable reference types.
                // throws `TargetInvocationException` if an error occurs (which we actually expect to happen when the value is null).
                try
                {
                    var result = Activator.CreateInstance(typeof(BrokenRecord), value);

                    if (result is null)
                    {
                        throw new Exception();
                    }

                    return (BrokenRecord) result;
                }
                catch (TargetInvocationException tie)
                {
                    if (tie.InnerException is ArgumentNullException {ParamName: "value"})
                    {
                        throw tie.InnerException;
                    }

                    throw;
                }
            }

            public BrokenRecord(object value) : base(value)
            {
            }
        }

        [Property]
        public Property ThrowsNullSemanticValueExceptionWhenValueIsNull(bool useNullValue)
        {
            var threwExpectedException = false;

            try
            {
                _ = BrokenRecord.CreateWithValue(useNullValue ? null : 42);
            }
            catch (ArgumentNullException)
            {
                threwExpectedException = true;
            }

            return Prop.Given(useNullValue, threwExpectedException, !threwExpectedException)
                .Classify(useNullValue, "null")
                .Classify(!useNullValue, "not null");
        }
    }
}