using System;
using System.Text.Json;

namespace kwd.BoxOBlazor.Browser.Services.HostSettings
{
    public class HostSettingsError : Exception
    {
        private static string GetMessage(Exception inner)
        {
            if (inner is JsonException)
            {
                return "Invalid json data from host";
            }

            return "Failed to read host app settings";
        }

        public HostSettingsError(string message)
        :base(message){ }

        public HostSettingsError(Exception inner)
            : base(GetMessage(inner), inner) { }
    }
}
