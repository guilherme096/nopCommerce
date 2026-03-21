using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Nop.Web.Infrastructure.OpenTelemetry;

public static class ProductDetailsTelemetry
{
    public const string ActivitySourceName = "Nop.Web.Catalog";
    public const string MeterName = "Nop.Web.Catalog";
    public const string ProductDetailsActivityName = "catalog.product_details";

    public const string ProductIdTagName = "product.id";
    public const string ProductTypeTagName = "product.type";
    public const string ProductIsAssociatedTagName = "product.is_associated";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
    public static readonly Meter Meter = new(MeterName);
    public static readonly Histogram<double> BuildDurationHistogram = Meter.CreateHistogram<double>(
        "catalog_product_details_model_build_duration",
        unit: "ms",
        description: "Time to build the product details view model");

    // Product overview metrics
    public const string ProductOverviewActivityName = "catalog.product_overview";
    public const string ProductCountTagName = "product.count";
    public const string PreparePriceTagName = "product.prepare_price";

    public static readonly Histogram<double> OverviewBuildDurationHistogram = Meter.CreateHistogram<double>(
        "catalog_product_overview_models_build_duration",
        unit: "ms",
        description: "Time to build product overview models for a listing page");
}
