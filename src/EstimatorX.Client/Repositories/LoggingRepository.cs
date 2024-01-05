using System.Reflection.PortableExecutable;
using System.Text.Json;
using EstimatorX.Client.Services;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using FluentRest;

namespace EstimatorX.Client.Repositories;

[RegisterScoped]
public class LoggingRepository
{
    protected GatewayClient Gateway { get; }

    public LoggingRepository(GatewayClient gateway)
    {
        Gateway = gateway;
    }

    public async Task<List<string>> List()
    {

        var result = await Gateway.GetAsync<string[]>(b => b
            .AppendPath("/api/administrative/logging")
        );

        return result.ToList();
    }

    public async Task<List<LogEvent>> Read(string file)
    {

        var result = await Gateway.GetAsync(b => b
            .AppendPath("/api/administrative/logging")
            .AppendPath(file)
        );

        var logs = new List<LogEvent>();

        await using var stream = await result.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var json = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(json))
                continue;

            var logEvent = JsonSerializer.Deserialize(json, JsonContext.Default.LogEvent);
            logs.Add(logEvent);
        }

        return logs;
    }
}
