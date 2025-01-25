using System.Net.NetworkInformation;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck.Server.Models;

// Documentation: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks

public class ICMPHealthCheck : IHealthCheck
{
    private readonly string Host = $"10.0.0.0"; // always unreachable so always returns Unhealthy for demo purposes 
    private readonly int HealthyRoundTripTime = 300;
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(Host);
            switch (reply.Status)
            {
                case IPStatus.Success:
                    return (reply.RoundtripTime > HealthyRoundTripTime)
                        ? HealthCheckResult.Degraded()
                        : HealthCheckResult.Healthy();
                default:
                    return HealthCheckResult.Unhealthy();
            }
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}