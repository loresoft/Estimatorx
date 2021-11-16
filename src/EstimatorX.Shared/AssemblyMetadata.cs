using System;
using System.Reflection;

namespace EstimatorX.Shared;

public static class AssemblyMetadata
{
    private static readonly Lazy<string> _fileVersion = new(() =>
    {
        var assembly = typeof(AssemblyMetadata).Assembly;
        var attribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
        return attribute?.Version ?? "1.0.0.0";
    });

    private static readonly Lazy<string> _assemblyVersion = new(() =>
    {
        var assembly = typeof(AssemblyMetadata).Assembly;
        var attribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
        return attribute?.Version ?? "1.0.0.0";
    });

    private static readonly Lazy<string> _informationVersion = new(() =>
    {
        var assembly = typeof(AssemblyMetadata).Assembly;
        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return attribute?.InformationalVersion ?? "1.0.0.0";
    });

    public static string FileVersion => _fileVersion.Value;

    public static string AssemblyVersion => _assemblyVersion.Value;

    public static string InformationalVersion => _informationVersion.Value;
}
