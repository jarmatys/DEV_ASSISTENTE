using System.Text;

namespace ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;

public abstract class GetRequestBase : RequestBase
{
    public string QueryString()
    {
        var properties = GetType().GetProperties();
        var sb = new StringBuilder();

        foreach (var property in properties)
        {
            var value = property.GetValue(this);
            if (value != null)
            {
                sb.Append($"{property.Name}={value}&");
            }
        }

        return sb.ToString();
    }
}