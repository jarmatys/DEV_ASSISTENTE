using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace ASSISTENTE.UI.Common.Extensions;

public static class NavigationManagerExtensions
{
    public static string? GetQueryString<T>(this NavigationManager navManager, string key)
    {
        var uri = navManager.ToAbsoluteUri(navManager.Uri);
        var parsedUri = new Uri(uri.AbsoluteUri.Replace("#", "?"));
        
        if (!QueryHelpers.ParseQuery(parsedUri.Query).TryGetValue(key, out var valueFromQueryString)) 
            return default;
        
        if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
        {
            return valueAsInt.ToString();
        }

        if (typeof(T) == typeof(string))
        {
            return valueFromQueryString.ToString();
        }

        if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
        {
            return valueAsDecimal.ToString(CultureInfo.InvariantCulture);
        }

        return default;
    }
}