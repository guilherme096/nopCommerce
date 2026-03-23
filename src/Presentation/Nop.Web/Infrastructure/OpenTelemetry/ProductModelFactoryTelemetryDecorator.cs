#nullable enable
using System.Diagnostics;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Web.Framework.Infrastructure.OpenTelemetry;
using Nop.Web.Factories;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Infrastructure.OpenTelemetry;

public class ProductModelFactoryTelemetryDecorator : IProductModelFactory
{
    private readonly IProductModelFactory _inner;

    public ProductModelFactoryTelemetryDecorator(IProductModelFactory inner)
    {
        _inner = inner;
    }

    public Task<string> PrepareProductTemplateViewPathAsync(Product product)
        => _inner.PrepareProductTemplateViewPathAsync(product);

    public async Task<IEnumerable<ProductOverviewModel>> PrepareProductOverviewModelsAsync(IEnumerable<Product> products,
        bool preparePriceModel = true, bool preparePictureModel = true,
        int? productThumbPictureSize = null, bool prepareSpecificationAttributes = false,
        bool forceRedirectionAfterAddingToCart = false)
    {
        var productList = products as IList<Product> ?? products.ToList();
        using var activity = ProductDetailsTelemetry.ActivitySource.StartActivity(
            ProductDetailsTelemetry.ProductOverviewActivityName);
        activity?.SetTag(ProductDetailsTelemetry.ProductCountTagName, productList.Count);
        activity?.SetTag(ProductDetailsTelemetry.PreparePriceTagName, preparePriceModel);

        var stopwatch = Stopwatch.StartNew();
        try
        {
            return await _inner.PrepareProductOverviewModelsAsync(productList, preparePriceModel,
                preparePictureModel, productThumbPictureSize, prepareSpecificationAttributes,
                forceRedirectionAfterAddingToCart);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            ProductDetailsTelemetry.OverviewBuildDurationHistogram.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(ProductDetailsTelemetry.ProductCountTagName, productList.Count),
                new KeyValuePair<string, object?>(ProductDetailsTelemetry.PreparePriceTagName, preparePriceModel));
        }
    }

    public Task<IList<ProductCombinationModel>> PrepareProductCombinationModelsAsync(Product product)
        => _inner.PrepareProductCombinationModelsAsync(product);

    public async Task<ProductDetailsModel> PrepareProductDetailsModelAsync(Product product,
        ShoppingCartItem updatecartitem = null, bool isAssociatedProduct = false)
    {
        using var activity = ProductDetailsTelemetry.ActivitySource.StartActivity(
            ProductDetailsTelemetry.ProductDetailsActivityName);
        activity?.SetTag(ProductDetailsTelemetry.ProductIdTagName, product.Id);
        activity?.SetTag(ProductDetailsTelemetry.ProductTypeTagName, product.ProductType.ToString());
        activity?.SetTag(ProductDetailsTelemetry.ProductIsAssociatedTagName, isAssociatedProduct);

        var stopwatch = Stopwatch.StartNew();
        try
        {
            return await _inner.PrepareProductDetailsModelAsync(product, updatecartitem, isAssociatedProduct);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            ProductDetailsTelemetry.BuildDurationHistogram.Record(
                stopwatch.Elapsed.TotalMilliseconds,
                new KeyValuePair<string, object?>(ProductDetailsTelemetry.ProductIdTagName, product.Id),
                new KeyValuePair<string, object?>(ProductDetailsTelemetry.ProductTypeTagName, product.ProductType.ToString()),
                new KeyValuePair<string, object?>(ProductDetailsTelemetry.ProductIsAssociatedTagName, isAssociatedProduct));
        }
    }

    public Task<ProductReviewsModel> PrepareProductReviewsModelAsync(Product product)
        => _inner.PrepareProductReviewsModelAsync(product);

    public Task<CustomerProductReviewsModel> PrepareCustomerProductReviewsModelAsync(int? page)
        => _inner.PrepareCustomerProductReviewsModelAsync(page);

    public Task<ProductEmailAFriendModel> PrepareProductEmailAFriendModelAsync(ProductEmailAFriendModel model, Product product, bool excludeProperties)
        => _inner.PrepareProductEmailAFriendModelAsync(model, product, excludeProperties);

    public Task<ProductSpecificationModel> PrepareProductSpecificationModelAsync(Product product)
        => _inner.PrepareProductSpecificationModelAsync(product);
}
