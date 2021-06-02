namespace SemanticValues.Tests
{
    internal record SemanticObject : SemanticValue<object?>
    {
        public SemanticObject(object? value) : base(value)
        {
        }
    }
    
    internal record DifferentSemanticObject : SemanticValue<object?>
    {
        public DifferentSemanticObject(object? value) : base(value)
        {
        }
    }
}