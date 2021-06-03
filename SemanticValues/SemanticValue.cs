using System;
using SemanticValues.Exceptions;

namespace SemanticValues
{
    /// <summary>
    /// Represents a "semantic" (or perhaps "domain") value
    /// </summary>
    /// <typeparam name="TValue">The lower-level concrete type of the value (e.g. string, int, Guid, etc.)</typeparam>
    public abstract record SemanticValue<TValue> where TValue : notnull
    {
        /// <summary>
        /// By default, no validation is provided. The value is always valid.
        /// Inheritors are free to provide their own validation, as necessary and appropriate.
        /// </summary>
        /// <param name="_">The value to be checked, which is ignored in this case.</param>
        /// <returns>Always returns true.</returns>
        private static bool DefaultValidationFunc(TValue _) => true;

        /// <summary>
        /// The underlying value.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Check for validity and construct a semantic value with the provided underlying value.
        /// </summary>
        /// <param name="value">The underlying value.</param>
        /// <param name="validationFunc">A function to check whether or not the underlying value is valid.</param>
        /// <exception>Thrown when the underlying value is null.
        ///     <cref>ArgumentNullException</cref>
        /// </exception>
        /// <exception>Thrown when the underlying value is not valid according to the validation function.
        ///     <cref>InvalidSemanticValueException[TValue]</cref>
        /// </exception>
        protected SemanticValue(TValue value, Func<TValue, bool> validationFunc)
        {
            // This should not happen when using non-nullable reference types,
            // but guard against cases when a null value is provided anyway.
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            if (!validationFunc(value))
            {
                throw new InvalidSemanticValueException<TValue>(value);
            }
            
            Value = value;
        }

        /// <summary>
        /// Construct a semantic value with the provided underlying value.
        /// </summary>
        /// <param name="value">The underlying value.</param>
        protected SemanticValue(TValue value) : this(value, DefaultValidationFunc)
        {
        }
    }
}