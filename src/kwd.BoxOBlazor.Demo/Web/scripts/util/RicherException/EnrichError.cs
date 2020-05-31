using System.Linq;
using System.Text.Json;
using Microsoft.JSInterop;

namespace kwd.BoxOBlazor.Demo.util.RicherException
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
}