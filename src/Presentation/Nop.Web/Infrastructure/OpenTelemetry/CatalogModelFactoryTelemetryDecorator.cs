using System.Diagnostics;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Web.Framework.Infrastructure.OpenTelemetry;
using Nop.Web.Factories;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Infrastructure.OpenTelemetry;

public class CatalogModelFactoryTelemetryDecorator : ICatalogModelFactory
{
    private readonly ICatalogModelFactory _inner;

    public CatalogModelFactoryTelemetryDecorator(ICatalogModelFactory inner)
    {
        _inner = inner;
    }

    public async Task<CategoryModel> PrepareCategoryModelAsync(Category category, CatalogProductsCommand command)
    {
        using var activity = CatalogSearchTelemetry.ActivitySource.StartActivity("catalog.category_products");
        var stopwatch = Stopwatch.StartNew();
        try
        {
            CatalogSearchTelemetry.BrowseTotalCounter.Add(1,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "category"));

            var result = await _inner.PrepareCategoryModelAsync(category, command);

            activity?.SetTag(CatalogSearchTelemetry.BrowseResultCountTagName, result.CatalogProductsModel.TotalItems);
            if (result.CatalogProductsModel.TotalItems == 0)
                CatalogSearchTelemetry.BrowseZeroResultsCounter.Add(1,
                    new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "category"));

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            CatalogSearchTelemetry.BrowseModelBuildDurationHistogram.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "category"));
        }
    }

    public Task<string> PrepareCategoryTemplateViewPathAsync(int templateId)
        => _inner.PrepareCategoryTemplateViewPathAsync(templateId);

    public Task<CategoryNavigationModel> PrepareCategoryNavigationModelAsync(int currentCategoryId, int currentProductId)
        => _inner.PrepareCategoryNavigationModelAsync(currentCategoryId, currentProductId);

    public Task<List<CategoryModel>> PrepareHomepageCategoryModelsAsync()
        => _inner.PrepareHomepageCategoryModelsAsync();

    public async Task<CatalogProductsModel> PrepareCategoryProductsModelAsync(Category category, CatalogProductsCommand command)
    {
        using var activity = CatalogSearchTelemetry.ActivitySource.StartActivity("catalog.category_products");
        var stopwatch = Stopwatch.StartNew();
        try
        {
            CatalogSearchTelemetry.BrowseTotalCounter.Add(1,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "category"));

            var result = await _inner.PrepareCategoryProductsModelAsync(category, command);

            activity?.SetTag(CatalogSearchTelemetry.BrowseResultCountTagName, result.TotalItems);
            if (result.TotalItems == 0)
                CatalogSearchTelemetry.BrowseZeroResultsCounter.Add(1,
                    new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "category"));

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            CatalogSearchTelemetry.BrowseModelBuildDurationHistogram.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "category"));
        }
    }

    public async Task<ManufacturerModel> PrepareManufacturerModelAsync(Manufacturer manufacturer, CatalogProductsCommand command)
    {
        using var activity = CatalogSearchTelemetry.ActivitySource.StartActivity("catalog.manufacturer_products");
        var stopwatch = Stopwatch.StartNew();
        try
        {
            CatalogSearchTelemetry.BrowseTotalCounter.Add(1,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "manufacturer"));

            var result = await _inner.PrepareManufacturerModelAsync(manufacturer, command);

            activity?.SetTag(CatalogSearchTelemetry.BrowseResultCountTagName, result.CatalogProductsModel.TotalItems);
            if (result.CatalogProductsModel.TotalItems == 0)
                CatalogSearchTelemetry.BrowseZeroResultsCounter.Add(1,
                    new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "manufacturer"));

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            CatalogSearchTelemetry.BrowseModelBuildDurationHistogram.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "manufacturer"));
        }
    }

    public async Task<CatalogProductsModel> PrepareManufacturerProductsModelAsync(Manufacturer manufacturer, CatalogProductsCommand command)
    {
        using var activity = CatalogSearchTelemetry.ActivitySource.StartActivity("catalog.manufacturer_products");
        var stopwatch = Stopwatch.StartNew();
        try
        {
            CatalogSearchTelemetry.BrowseTotalCounter.Add(1,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "manufacturer"));

            var result = await _inner.PrepareManufacturerProductsModelAsync(manufacturer, command);

            activity?.SetTag(CatalogSearchTelemetry.BrowseResultCountTagName, result.TotalItems);
            if (result.TotalItems == 0)
                CatalogSearchTelemetry.BrowseZeroResultsCounter.Add(1,
                    new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "manufacturer"));

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            CatalogSearchTelemetry.BrowseModelBuildDurationHistogram.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "manufacturer"));
        }
    }

    public Task<string> PrepareManufacturerTemplateViewPathAsync(int templateId)
        => _inner.PrepareManufacturerTemplateViewPathAsync(templateId);

    public Task<List<ManufacturerModel>> PrepareManufacturerAllModelsAsync()
        => _inner.PrepareManufacturerAllModelsAsync();

    public Task<ManufacturerNavigationModel> PrepareManufacturerNavigationModelAsync(int currentManufacturerId)
        => _inner.PrepareManufacturerNavigationModelAsync(currentManufacturerId);

    public Task<VendorModel> PrepareVendorModelAsync(Vendor vendor, CatalogProductsCommand command)
        => _inner.PrepareVendorModelAsync(vendor, command);

    public async Task<CatalogProductsModel> PrepareVendorProductsModelAsync(Vendor vendor, CatalogProductsCommand command)
    {
        using var activity = CatalogSearchTelemetry.ActivitySource.StartActivity("catalog.vendor_products");
        var stopwatch = Stopwatch.StartNew();
        try
        {
            CatalogSearchTelemetry.BrowseTotalCounter.Add(1,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "vendor"));

            var result = await _inner.PrepareVendorProductsModelAsync(vendor, command);

            activity?.SetTag(CatalogSearchTelemetry.BrowseResultCountTagName, result.TotalItems);
            if (result.TotalItems == 0)
                CatalogSearchTelemetry.BrowseZeroResultsCounter.Add(1,
                    new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "vendor"));

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            CatalogSearchTelemetry.BrowseModelBuildDurationHistogram.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(CatalogSearchTelemetry.BrowseTypeTagName, "vendor"));
        }
    }

    public Task<List<VendorModel>> PrepareVendorAllModelsAsync()
        => _inner.PrepareVendorAllModelsAsync();

    public Task<VendorNavigationModel> PrepareVendorNavigationModelAsync()
        => _inner.PrepareVendorNavigationModelAsync();

    public Task<VendorProductReviewsListModel> PrepareVendorProductReviewsModelAsync(Vendor vendor, VendorReviewsPagingFilteringModel pagingModel)
        => _inner.PrepareVendorProductReviewsModelAsync(vendor, pagingModel);

    public Task<PopularProductTagsModel> PreparePopularProductTagsModelAsync(int numberTagsToReturn = 0)
        => _inner.PreparePopularProductTagsModelAsync(numberTagsToReturn);

    public Task<ProductsByTagModel> PrepareProductsByTagModelAsync(ProductTag productTag, CatalogProductsCommand command)
        => _inner.PrepareProductsByTagModelAsync(productTag, command);

    public Task<CatalogProductsModel> PrepareTagProductsModelAsync(ProductTag productTag, CatalogProductsCommand command)
        => _inner.PrepareTagProductsModelAsync(productTag, command);

    public Task<CatalogProductsModel> PrepareNewProductsModelAsync(CatalogProductsCommand command)
        => _inner.PrepareNewProductsModelAsync(command);

    public async Task<SearchModel> PrepareSearchModelAsync(SearchModel model, CatalogProductsCommand command)
    {
        var normalizedSearchTerm = NormalizeSearchTerm(model.q);
        var hasSearchTerm = !string.IsNullOrEmpty(normalizedSearchTerm);
        var isAdvancedSearch = model.advs;
        var hasCategoryFilter = model.cid > 0;
        var hasManufacturerFilter = model.mid > 0;
        var hasVendorFilter = model.vid > 0 && model.asv;

        var metricTags = new System.Diagnostics.TagList
        {
            { CatalogSearchTelemetry.HasSearchTermTagName, hasSearchTerm },
            { CatalogSearchTelemetry.IsAdvancedTagName, isAdvancedSearch },
            { CatalogSearchTelemetry.HasCategoryFilterTagName, hasCategoryFilter },
            { CatalogSearchTelemetry.HasManufacturerFilterTagName, hasManufacturerFilter },
            { CatalogSearchTelemetry.HasVendorFilterTagName, hasVendorFilter }
        };

        using var searchActivity = CatalogSearchTelemetry.ActivitySource.StartActivity(CatalogSearchTelemetry.SearchActivityName);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasSearchTermTagName, hasSearchTerm);
        searchActivity?.SetTag(CatalogSearchTelemetry.IsAdvancedTagName, isAdvancedSearch);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasCategoryFilterTagName, hasCategoryFilter);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasManufacturerFilterTagName, hasManufacturerFilter);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasVendorFilterTagName, hasVendorFilter);

        CatalogSearchTelemetry.SearchTotalCounter.Add(1, metricTags);

        try
        {
            var result = await _inner.PrepareSearchModelAsync(model, command);

            searchActivity?.SetTag(CatalogSearchTelemetry.ResultCountTagName, result.CatalogProductsModel.TotalItems);

            if (result.CatalogProductsModel.TotalItems == 0)
                CatalogSearchTelemetry.SearchZeroResultsCounter.Add(1, metricTags);

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(searchActivity, exception);
            throw;
        }
    }

    public async Task<CatalogProductsModel> PrepareSearchProductsModelAsync(SearchModel searchModel, CatalogProductsCommand command)
    {
        var normalizedSearchTerm = NormalizeSearchTerm(searchModel.q);
        var hasSearchTerm = !string.IsNullOrEmpty(normalizedSearchTerm);
        var isAdvancedSearch = searchModel.advs;
        var hasCategoryFilter = searchModel.cid > 0;
        var hasManufacturerFilter = searchModel.mid > 0;
        var hasVendorFilter = searchModel.vid > 0 && searchModel.asv;

        var metricTags = new System.Diagnostics.TagList
        {
            { CatalogSearchTelemetry.HasSearchTermTagName, hasSearchTerm },
            { CatalogSearchTelemetry.IsAdvancedTagName, isAdvancedSearch },
            { CatalogSearchTelemetry.HasCategoryFilterTagName, hasCategoryFilter },
            { CatalogSearchTelemetry.HasManufacturerFilterTagName, hasManufacturerFilter },
            { CatalogSearchTelemetry.HasVendorFilterTagName, hasVendorFilter }
        };

        using var searchActivity = CatalogSearchTelemetry.ActivitySource.StartActivity(CatalogSearchTelemetry.SearchActivityName);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasSearchTermTagName, hasSearchTerm);
        searchActivity?.SetTag(CatalogSearchTelemetry.IsAdvancedTagName, isAdvancedSearch);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasCategoryFilterTagName, hasCategoryFilter);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasManufacturerFilterTagName, hasManufacturerFilter);
        searchActivity?.SetTag(CatalogSearchTelemetry.HasVendorFilterTagName, hasVendorFilter);

        CatalogSearchTelemetry.SearchTotalCounter.Add(1, metricTags);

        try
        {
            var result = await _inner.PrepareSearchProductsModelAsync(searchModel, command);

            searchActivity?.SetTag(CatalogSearchTelemetry.ResultCountTagName, result.TotalItems);

            if (result.TotalItems == 0)
                CatalogSearchTelemetry.SearchZeroResultsCounter.Add(1, metricTags);

            return result;
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(searchActivity, exception);
            throw;
        }
    }

    private static string NormalizeSearchTerm(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return string.Empty;

        return searchTerm.Trim();
    }

    public Task<CatalogProductsModel> PrepareSearchProductsByFilterLevelValuesModelAsync(SearchFilterLevelValueModel searchModel, CatalogProductsCommand command)
        => _inner.PrepareSearchProductsByFilterLevelValuesModelAsync(searchModel, command);

    public Task<SearchBoxModel> PrepareSearchBoxModelAsync()
        => _inner.PrepareSearchBoxModelAsync();

    public Task PrepareSortingOptionsAsync(CatalogProductsModel pagingFilteringModel, CatalogProductsCommand command)
        => _inner.PrepareSortingOptionsAsync(pagingFilteringModel, command);

    public Task PrepareViewModesAsync(CatalogProductsModel pagingFilteringModel, CatalogProductsCommand command)
        => _inner.PrepareViewModesAsync(pagingFilteringModel, command);

    public Task PreparePageSizeOptionsAsync(CatalogProductsModel pagingFilteringModel, CatalogProductsCommand command, bool allowCustomersToSelectPageSize, string pageSizeOptions, int fixedPageSize)
        => _inner.PreparePageSizeOptionsAsync(pagingFilteringModel, command, allowCustomersToSelectPageSize, pageSizeOptions, fixedPageSize);
}
