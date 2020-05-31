using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo.util.RicherException
{
    /// <summary>Standard JS Error data.</summary>
    public class StandardError : JSException
    {
        public StandardError(EnrichError.ErrorData data, JSException inner)
            : base(data.Message, inner)
        {
            Name = data.Name;
        }

        public string Name { get; }

        /// <summary>
        /// Name constants for standard error types.
        /// </summary>
        /// <remarks>
        /// See https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Error
        /// </remarks>
        public static class StandardNames
        {
            /// <summary> Error that occurs regarding the global function eval(). </summary>
            public const string EvalError = nameof(EvalError);

            /// <summary> Internal error in the JavaScript engine is thrown.
            /// E.g. "too much recursion" </summary>
            public const string InternalError = nameof(InternalError);

            /// <summary> Error that occurs when a numeric variable or
            /// parameter is outside of its valid range.</summary>
            public const string RangeError = nameof(RangeError);

            /// <summary>
            /// Error that occurs when de-referencing an invalid reference.
            /// </summary>
            public const string ReferenceError = nameof(ReferenceError);

            /// <summary>Syntax error.</summary>
            public const string SyntaxError = nameof(SyntaxError);

            /// <summary>Error that occurs when a variable or
            /// parameter is not of a valid type.</summary>
            public const string TypeError = nameof(TypeError);

            /// <summary>Error that occurs when
            /// encodeURI() or decodeURI() are passed invalid parameters</summary>
            public const string URIError = nameof(URIError);
        }
    }
}