using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Nop.Web.Infrastructure.OpenTelemetry;

public static class CatalogSearchTelemetry
{
    public const string ActivitySourceName = "Nop.Web.Catalog";
    public const string MeterName = "Nop.Web.Catalog";
    public const string SearchActivityName = "catalog.search";

    public const string ResultCountTagName = "catalog.search.result_count";
    public const string HasSearchTermTagName = "catalog.search.has_search_term";
    public const string IsAdvancedTagName = "catalog.search.is_advanced";
    public const string HasCategoryFilterTagName = "catalog.search.has_category_filter";
    public const string HasManufacturerFilterTagName = "catalog.search.has_manufacturer_filter";
    public const string HasVendorFilterTagName = "catalog.search.has_vendor_filter";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
    public static readonly Meter Meter = new(MeterName);
    public static readonly Counter<long> SearchZeroResultsCounter = Meter.CreateCounter<long>(
        "catalog_search_zero_results_total",
        unit: "{search}",
        description: "Total number of catalog searches that returned zero products.");

    // Browse metrics
    public const string BrowseTypeTagName = "catalog.browse.type";
    public const string BrowseResultCountTagName = "catalog.browse.result_count";

    public static readonly Histogram<double> BrowseModelBuildDurationHistogram = Meter.CreateHistogram<double>(
        "catalog_browse_model_build_duration",
        unit: "ms",
        description: "Time to build catalog browse model (category/manufacturer/vendor)");

    public static readonly Counter<long> BrowseZeroResultsCounter = Meter.CreateCounter<long>(
        "catalog_browse_zero_results_total",
        unit: "{browse}",
        description: "Total catalog browse requests that returned zero products.");

    public static readonly Counter<long> SearchTotalCounter = Meter.CreateCounter<long>(
        "catalog_search_total",
        unit: "{search}",
        description: "Total number of catalog search requests.");

    public static readonly Counter<long> BrowseTotalCounter = Meter.CreateCounter<long>(
        "catalog_browse_total",
        unit: "{browse}",
        description: "Total number of catalog browse requests.");
}
