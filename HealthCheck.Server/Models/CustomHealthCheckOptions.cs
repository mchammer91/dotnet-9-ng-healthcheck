using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace HealthCheck.Server.Models;

public class CustomHealthCheckOptions : HealthCheckOptions
{
    public CustomHealthCheckOptions() : base()
    {
        var jsonSerlializerOptions = new JsonSerializerOptions() { WriteIndented = true };
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status200OK; // always return 200 OK to make it easier for the UI to handle
            var result = JsonSerializer.Serialize(new
            {
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    responseTime = e.Value.Duration.TotalMilliseconds,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                }),
                totalStatus = report.Status,
                totalResponseTime = report.TotalDuration.TotalMilliseconds,
            }, jsonSerlializerOptions);
            await context.Response.WriteAsync(result);
        };
    }
}