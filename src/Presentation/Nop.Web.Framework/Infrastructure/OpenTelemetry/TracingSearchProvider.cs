using Nop.Services.Catalog;
using Nop.Services.Plugins;

namespace Nop.Web.Framework.Infrastructure.OpenTelemetry;

public class TracingSearchProvider : ISearchProvider
{
    public TracingSearchProvider(ISearchProvider innerProvider)
    {
        InnerProvider = innerProvider;
    }

    public ISearchProvider InnerProvider { get; }

    public PluginDescriptor PluginDescriptor
    {
        get => InnerProvider.PluginDescriptor;
        set => InnerProvider.PluginDescriptor = value;
    }

    public string GetConfigurationPageUrl()
        => InnerProvider.GetConfigurationPageUrl();

    public Task InstallAsync()
        => InnerProvider.InstallAsync();

    public Task InstallSampleDataAsync()
        => InnerProvider.InstallSampleDataAsync();

    public async Task<List<int>> SearchProductsAsync(string keywords, bool isLocalized)
    {
        using var activity = CatalogTracingHelper.ActivitySource.StartActivity(CatalogTracingHelper.SearchProviderActivityName);
        activity?.SetTag(CatalogTracingHelper.SearchProviderSystemNameTagName, PluginDescriptor?.SystemName ?? InnerProvider.GetType().Name);
        activity?.SetTag(CatalogTracingHelper.SearchHasKeywordsTagName, !string.IsNullOrWhiteSpace(keywords));
        activity?.SetTag(CatalogTracingHelper.SearchIsLocalizedTagName, isLocalized);

        try
        {
            var result = await InnerProvider.SearchProductsAsync(keywords, isLocalized);
            activity?.SetTag(CatalogTracingHelper.SearchResultCountTagName, result.Count);

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }

    public Task UninstallAsync()
        => InnerProvider.UninstallAsync();

    public Task UpdateAsync(string currentVersion, string targetVersion)
        => InnerProvider.UpdateAsync(currentVersion, targetVersion);

    public Task PreparePluginToUninstallAsync()
        => InnerProvider.PreparePluginToUninstallAsync();
}
