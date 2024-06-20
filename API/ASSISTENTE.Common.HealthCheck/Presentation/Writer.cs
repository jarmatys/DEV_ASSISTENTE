using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ASSISTENTE.Common.HealthCheck.Presentation;

internal static class Writer
{
    internal static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("Status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("Results");

            foreach (var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString("Status", healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString("Description", healthReportEntry.Value.Description);
                
                if (healthReportEntry.Value.Data.Count > 0)
                {
                    jsonWriter.WriteStartObject("Information");
                    
                    foreach(var data in healthReportEntry.Value.Data)
                        jsonWriter.WriteString(data.Key, data.Value.ToString());
                    
                    jsonWriter.WriteEndObject();
                }
                
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}