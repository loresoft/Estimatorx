using Microsoft.AspNetCore.Components;

namespace EstimatorX.Web;

public class SeoOptions
{
    public const string SectionName = "Seo";

    public const string DefaultCanonicalUrl = "https://estimatorx.com/";

    public const string DefaultSiteName = "EstimatorX";

    public const string DefaultDescription = "Fast, consistent project estimates that account for uncertainty.";

    public const string ImageAssetPath = "web-app-manifest-512x512.png";


    private string? _imageUrl;


    public string CanonicalUrl { get; set; } = DefaultCanonicalUrl;

    public string SiteName { get; set; } = DefaultSiteName;

    public string Description { get; set; } = DefaultDescription;


    // computed once and cached since the fingerprinted asset path doesn't change for the lifetime of the app
    public string GetImageUrl(ResourceAssetCollection assets) => _imageUrl ??= $"{CanonicalUrl.TrimEnd('/')}{assets[ImageAssetPath]}";
}
