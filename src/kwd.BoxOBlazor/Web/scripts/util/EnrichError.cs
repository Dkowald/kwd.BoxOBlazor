using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Web.util
{
    /*
     * Collect more JavaScript error details.
     *
     * Blazor transfers exception data via:
     * if e instanceof Error then e.message + '\n' + e.stack
     * else e not null e.toString()
     * else 'null'
     *
     * So if we pack e.message with json info; can then extract this in C#
     * to get easier to read exceptions across the wire.
     */

    public static class EnrichError
    {
        /// <summary>
        /// Try extract more details, return richer error data os <paramref name="error"/>.
        /// </summary>
        public static JSException TryUnwrap(JSException error)
        {
            var idxLine1 = error.Message.IndexOf('\n');
            if (idxLine1 <= 0) return null;

            var line1 = error.Message.Substring(0, idxLine1);

            if (line1.First() != '{' || line1.Last() != '}') 
                return null;

            try
            {
                var data = JsonSerializer.Deserialize<ErrorData>(line1);

                if(data.Name == nameof(DOMException))
                    return new DOMException(data, error);

                return new StandardError(data, error);
            }
            catch (JsonException)
            {
                /*not json data*/
            }

            return null;
        }

        public class ErrorData
        {
            public string Name { get; set; }
            public string DomErrorName { get; set; }
            public string Message { get; set; }
        }
    }

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

    /// <summary>JS Dom exception data</summary>
    public class DOMException : JSException
    {
        public DOMException(EnrichError.ErrorData data, JSException inner)
            :base(data.Message, inner)
        {
            Name = data.DomErrorName;
        }

        /// <summary>
        /// The Dom exception name see
        /// (https://developer.mozilla.org/en-US/docs/Web/API/DOMError)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Well known dom exception name's
        /// </summary>
        /// <remarks>
        /// https://developer.mozilla.org/en-US/docs/Web/API/DOMException#Error_names
        /// </remarks>
        public static class StandardNames
        {
            /// <summary>
            /// The index is not in the allowed range.
            /// For example, this can be thrown by the Range object.
            /// </summary>
            public const string IndexSizeError = nameof(IndexSizeError);

            /// <summary>The node tree hierarchy is not correct.</summary>
            public const string HierarchyRequestError = nameof(HierarchyRequestError);

            /// <summary>The object is in the wrong Document.</summary>
            public const string WrongDocumentError = nameof(WrongDocumentError);

            /// <summary>The string contains invalid characters.</summary>
            public const string InvalidCharacterError = nameof(InvalidCharacterError);

            /// <summary>The object cannot be modified.</summary>
            public const string NoModificationAllowedError = nameof(NoModificationAllowedError);

            /// <summary>The object cannot be found here.</summary>
            public const string NotFoundError = nameof(NotFoundError);

            /// <summary>The operation is not supported.</summary>
            public const string NotSupportedError = nameof(NotSupportedError);

            /// <summary>The object is in an invalid state.</summary>
            public const string InvalidStateError = nameof(InvalidStateError);

            /// <summary>The string did not match the expected pattern.</summary>
            public const string SyntaxError = nameof(SyntaxError);

            /// <summary>The object cannot be modified in this way.</summary>
            public const string InvalidModificationError = nameof(InvalidModificationError);

            /// <summary>The operation is not allowed by Namespaces in XML.</summary>
            public const string NamespaceError = nameof(NamespaceError);

            /// <summary>The object does not support the operation or argument.</summary>
            public const string InvalidAccessError = nameof(InvalidAccessError);

            /// <summary>The operation is insecure.</summary>
            public const string SecurityError = nameof(SecurityError);

            /// <summary>The operation timed out.</summary>
            public const string TimeoutError = nameof(TimeoutError);

            /// <summary>
            /// The request is not allowed by the user agent.
            /// (may be because user denied access)
            /// </summary>
            public const string NotAllowedError = nameof(NotAllowedError);
        }
    }
}