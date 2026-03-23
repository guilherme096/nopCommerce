using System.Diagnostics;

namespace Nop.Web.Framework.Infrastructure.OpenTelemetry;

public static class CatalogTracingHelper
{
    public const string ActivitySourceName = "Nop.Web.Catalog";
    public const string SearchExecutionActivityName = "catalog.search.execution";
    public const string SearchProviderActivityName = "catalog.search.provider";
    public const string ProductPriceActivityName = "catalog.product_price";
    public const string ProductDetailsMediaActivityName = "catalog.product_details.media";
    public const string ProductDetailsAddToCartActivityName = "catalog.product_details.add_to_cart";
    public const string ProductDetailsAttributesActivityName = "catalog.product_details.attributes";
    public const string ProductDetailsReviewOverviewActivityName = "catalog.product_details.review_overview";
    public const string ProductDetailsReviewsActivityName = "catalog.product_details.reviews";
    public const string EventPublishActivityName = "catalog.event.publish";

    public const string SearchHasKeywordsTagName = "catalog.search.has_keywords";
    public const string SearchHasCategoryFilterTagName = "catalog.search.has_category_filters";
    public const string SearchHasManufacturerFilterTagName = "catalog.search.has_manufacturer_filters";
    public const string SearchHasSpecificationFiltersTagName = "catalog.search.has_specification_filters";
    public const string SearchHasProductTagFilterTagName = "catalog.search.has_product_tag_filter";
    public const string SearchShowHiddenTagName = "catalog.search.show_hidden";
    public const string SearchPageSizeTagName = "catalog.search.page_size_bucket";
    public const string SearchResultCountTagName = "catalog.search.result_count";
    public const string SearchProviderSystemNameTagName = "catalog.search.provider.system";
    public const string SearchIsLocalizedTagName = "catalog.search.is_localized";
    public const string ProductTypeTagName = "catalog.product.type";
    public const string AddPriceRangeFromTagName = "catalog.product_price.add_price_range_from";
    public const string ForceRedirectionAfterAddingToCartTagName = "catalog.product_price.force_redirection_after_adding_to_cart";
    public const string IsAssociatedProductTagName = "catalog.product.is_associated";
    public const string EventTypeTagName = "catalog.event.type";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    public static string GetPageSizeBucket(int pageSize)
    {
        if (pageSize == int.MaxValue)
            return "all";

        if (pageSize <= 1)
            return "1";

        if (pageSize <= 10)
            return "2-10";

        if (pageSize <= 25)
            return "11-25";

        if (pageSize <= 50)
            return "26-50";

        if (pageSize <= 100)
            return "51-100";

        return "101+";
    }

    public static void MarkException(Activity activity, Exception exception)
    {
        if (activity == null || exception == null)
            return;

        activity.SetStatus(ActivityStatusCode.Error, exception.Message);
        activity.SetTag("exception.type", exception.GetType().FullName);
        activity.SetTag("exception.message", exception.Message);
    }
}
