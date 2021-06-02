using FsCheck;
using FsCheck.Xunit;
using SemanticValues.Exceptions;

namespace SemanticValues.Tests
{
    public class SemanticValueAccessorTests
    {
        [Property]
        public Property PropertyValueIsUnchanged(object value)
        {
            var s = new SemanticObject(value);

            return Prop.OfTestable(s.Value == value);
        }
    }
}