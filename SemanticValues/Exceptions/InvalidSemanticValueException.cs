using System;

namespace SemanticValues.Exceptions
{
    public class InvalidSemanticValueException : Exception
    {
        protected InvalidSemanticValueException() : base("Provided value is not valid for this type")
        {
        }
    }
    
    public class InvalidSemanticValueException<TValue> : InvalidSemanticValueException
    {
        public TValue Value { get; }

        public InvalidSemanticValueException(TValue value)
        {
            Value = value;
        }
    }
}