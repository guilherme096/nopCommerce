using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Plugins;

namespace Nop.Web.Framework.Infrastructure.OpenTelemetry;

public class TracingSearchPluginManager : SearchPluginManager
{
    public TracingSearchPluginManager(CatalogSettings catalogSettings,
        ICustomerService customerService,
        IPluginService pluginService)
        : base(catalogSettings, customerService, pluginService)
    {
    }

    public override async Task<IList<ISearchProvider>> LoadActivePluginsAsync(List<string> systemNames, Customer customer = null, int storeId = 0)
    {
        var providers = await base.LoadActivePluginsAsync(systemNames, customer, storeId);

        return providers.Select(Wrap).ToList();
    }

    public override async Task<IList<ISearchProvider>> LoadAllPluginsAsync(Customer customer = null, int storeId = 0)
    {
        var providers = await base.LoadAllPluginsAsync(customer, storeId);

        return providers.Select(Wrap).ToList();
    }

    public override bool IsPluginActive(ISearchProvider searchProvider)
        => base.IsPluginActive(Unwrap(searchProvider));

    public override async Task<bool> IsPluginActiveAsync(string systemName, Customer customer = null, int storeId = 0)
        => await base.IsPluginActiveAsync(systemName, customer, storeId);

    public override async Task<ISearchProvider> LoadPluginBySystemNameAsync(string systemName, Customer customer = null, int storeId = 0)
    {
        var provider = await base.LoadPluginBySystemNameAsync(systemName, customer, storeId);

        return Wrap(provider);
    }

    public override async Task<ISearchProvider> LoadPrimaryPluginAsync(Customer customer = null, int storeId = 0)
    {
        var provider = await base.LoadPrimaryPluginAsync(customer, storeId);

        return Wrap(provider);
    }

    private static ISearchProvider Wrap(ISearchProvider searchProvider)
    {
        if (searchProvider == null || searchProvider is TracingSearchProvider)
            return searchProvider;

        return new TracingSearchProvider(searchProvider);
    }

    private static ISearchProvider Unwrap(ISearchProvider searchProvider)
        => searchProvider is TracingSearchProvider tracingSearchProvider ? tracingSearchProvider.InnerProvider : searchProvider;
}
