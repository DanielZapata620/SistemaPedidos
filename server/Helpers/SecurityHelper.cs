using System.Text.RegularExpressions;

namespace PedidoApi.Helpers;

public static partial class SecurityHelper
{
    public static string Clean(string value)
    {
        var withoutTags = HtmlTagRegex().Replace(value.Trim(), string.Empty);
        return withoutTags.Replace("<", string.Empty).Replace(">", string.Empty);
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex HtmlTagRegex();
}
