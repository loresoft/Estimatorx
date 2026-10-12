using System.Reflection;

namespace EstimatorX.Templates;

public static class TemplateNames
{
    public static readonly Assembly TemplateAssembly = typeof(TemplateNames).Assembly;
    public static readonly string TemplateResourceFormat =  $"{typeof(TemplateNames).Namespace}.{{0}}.yml";

    public const string InviteTemplate = "InviteEmail";

    public static string GetResourceName(string templateName)
    {
        return string.Format(TemplateResourceFormat, templateName);
    }
}
