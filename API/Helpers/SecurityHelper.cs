using System.Net;

namespace API.Helpers
{
    public class SecurityHelper
    {
        public static string Clean(string? value)
        {
            return WebUtility.HtmlEncode((value ?? "").Trim());
        }
    }
}
