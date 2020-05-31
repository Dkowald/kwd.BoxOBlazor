using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo.util.RicherException
{
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