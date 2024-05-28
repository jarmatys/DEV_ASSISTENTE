using ASSISTENTE.Common.Correlation.Consts;
using ASSISTENTE.Common.Correlation.Generators;

namespace ASSISTENTE.UI.Common.Middlewares;

internal class RequestContextLoggingMiddleware : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var correlationId = CorrelationGenerator.Generate();

        request.Headers.Add(CorrelationConsts.CorrelationHeader, correlationId);
        
        return await base.SendAsync(request, cancellationToken);
    }
}