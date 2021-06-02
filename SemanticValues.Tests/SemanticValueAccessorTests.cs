using FsCheck;
using FsCheck.Xunit;

namespace SemanticValues.Tests
{
    public class SemanticValueAccessorTests
    {
        [Property]
        public Property PropertyValueIsUnchanged(object? value)
        {
            var s = new SemanticObject(value);

            return Prop.OfTestable(s.Value == value)
                .Collect(value?.GetType());
        }
    }
}