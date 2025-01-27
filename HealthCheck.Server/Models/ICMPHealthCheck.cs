using System.Net.NetworkInformation;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck.Server.Models;

// Documentation: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks

public class ICMPHealthCheck : IHealthCheck
{
    private readonly string _host; 
    private readonly int _healthyRoundTripTime;
    
    public ICMPHealthCheck(string host, int healthyRoundTripTime) => (_host, _healthyRoundTripTime) = (host, healthyRoundTripTime);

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(_host);
            switch (reply.Status)
            {
                case IPStatus.Success:
                    var message = $"ICMP to {_host} took {reply.RoundtripTime} ms.";
                    return (reply.RoundtripTime > _healthyRoundTripTime)
                        ? HealthCheckResult.Degraded(message)
                        : HealthCheckResult.Healthy(message);
                default:
                    var error = $"ICMP to {_host} failed: {reply.Status}";
                    return HealthCheckResult.Unhealthy(error);
            }
        }
        catch (Exception e)
        {
            var error = $"ICMP failed: {e.Message}";
            return HealthCheckResult.Unhealthy(error);
        }
    }
}