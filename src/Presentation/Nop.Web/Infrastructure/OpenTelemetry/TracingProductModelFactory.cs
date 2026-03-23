using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Vendors;
using Nop.Core.Http;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Infrastructure.OpenTelemetry;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Media;

namespace Nop.Web.Infrastructure.OpenTelemetry;

public class TracingProductModelFactory : ProductModelFactory
{
    public TracingProductModelFactory(CaptchaSettings captchaSettings,
        CatalogSettings catalogSettings,
        CustomerSettings customerSettings,
        GpsrSettings gpsrSettings,
        ICategoryService categoryService,
        ICurrencyService currencyService,
        ICustomerService customerService,
        ICustomWishlistService customWishlistService,
        IDateRangeService dateRangeService,
        IDateTimeHelper dateTimeHelper,
        IDownloadService downloadService,
        IGenericAttributeService genericAttributeService,
        IJsonLdModelFactory jsonLdModelFactory,
        ILocalizationService localizationService,
        IManufacturerService manufacturerService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IPriceCalculationService priceCalculationService,
        IPriceFormatter priceFormatter,
        IProductAttributeParser productAttributeParser,
        IProductAttributeService productAttributeService,
        IProductReviewService productReviewService,
        IProductService productService,
        IProductTagService productTagService,
        IProductTemplateService productTemplateService,
        IReviewTypeService reviewTypeService,
        IShoppingCartService shoppingCartService,
        ISpecificationAttributeService specificationAttributeService,
        IStaticCacheManager staticCacheManager,
        IStoreContext storeContext,
        IStoreService storeService,
        IShoppingCartModelFactory shoppingCartModelFactory,
        ITaxService taxService,
        IUrlRecordService urlRecordService,
        IVendorService vendorService,
        IVideoService videoService,
        IWebHelper webHelper,
        IWorkContext workContext,
        MediaSettings mediaSettings,
        OrderSettings orderSettings,
        SeoSettings seoSettings,
        ShippingSettings shippingSettings,
        VendorSettings vendorSettings)
        : base(captchaSettings, catalogSettings, customerSettings, gpsrSettings, categoryService, currencyService,
            customerService, customWishlistService, dateRangeService, dateTimeHelper, downloadService,
            genericAttributeService, jsonLdModelFactory, localizationService, manufacturerService,
            permissionService, pictureService, priceCalculationService, priceFormatter, productAttributeParser,
            productAttributeService, productReviewService, productService, productTagService,
            productTemplateService, reviewTypeService, shoppingCartService, specificationAttributeService,
            staticCacheManager, storeContext, storeService, shoppingCartModelFactory, taxService,
            urlRecordService, vendorService, videoService, webHelper, workContext, mediaSettings,
            orderSettings, seoSettings, shippingSettings, vendorSettings)
    {
    }

    protected override async Task<ProductPriceModel> PrepareProductPriceModelAsync(Product product,
        bool addPriceRangeFrom = false, bool forceRedirectionAfterAddingToCart = false)
    {
        using var activity = CatalogTracingHelper.ActivitySource.StartActivity(CatalogTracingHelper.ProductPriceActivityName);
        activity?.SetTag(CatalogTracingHelper.ProductTypeTagName, product.ProductType.ToString());
        activity?.SetTag(CatalogTracingHelper.AddPriceRangeFromTagName, addPriceRangeFrom);
        activity?.SetTag(CatalogTracingHelper.ForceRedirectionAfterAddingToCartTagName, forceRedirectionAfterAddingToCart);

        try
        {
            return await base.PrepareProductPriceModelAsync(product, addPriceRangeFrom, forceRedirectionAfterAddingToCart);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }

    protected override async Task<(PictureModel pictureModel, IList<PictureModel> allPictureModels, IList<VideoModel> allVideoModels)> PrepareProductDetailsPictureModelAsync(Product product, bool isAssociatedProduct)
    {
        using var activity = CatalogTracingHelper.ActivitySource.StartActivity(CatalogTracingHelper.ProductDetailsMediaActivityName);
        activity?.SetTag(CatalogTracingHelper.ProductTypeTagName, product.ProductType.ToString());
        activity?.SetTag(CatalogTracingHelper.IsAssociatedProductTagName, isAssociatedProduct);

        try
        {
            return await base.PrepareProductDetailsPictureModelAsync(product, isAssociatedProduct);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }

    protected override async Task<ProductDetailsModel.AddToCartModel> PrepareProductAddToCartModelAsync(Product product, ShoppingCartItem updatecartitem)
    {
        using var activity = CatalogTracingHelper.ActivitySource.StartActivity(CatalogTracingHelper.ProductDetailsAddToCartActivityName);
        activity?.SetTag(CatalogTracingHelper.ProductTypeTagName, product.ProductType.ToString());

        try
        {
            return await base.PrepareProductAddToCartModelAsync(product, updatecartitem);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }

    protected override async Task<IList<ProductDetailsModel.ProductAttributeModel>> PrepareProductAttributeModelsAsync(Product product, ShoppingCartItem updatecartitem)
    {
        using var activity = CatalogTracingHelper.ActivitySource.StartActivity(CatalogTracingHelper.ProductDetailsAttributesActivityName);
        activity?.SetTag(CatalogTracingHelper.ProductTypeTagName, product.ProductType.ToString());

        try
        {
            return await base.PrepareProductAttributeModelsAsync(product, updatecartitem);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }

    protected override async Task<ProductReviewOverviewModel> PrepareProductReviewOverviewModelAsync(Product product)
    {
        using var activity = CatalogTracingHelper.ActivitySource.StartActivity(CatalogTracingHelper.ProductDetailsReviewOverviewActivityName);
        activity?.SetTag(CatalogTracingHelper.ProductTypeTagName, product.ProductType.ToString());

        try
        {
            return await base.PrepareProductReviewOverviewModelAsync(product);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }

    public override async Task<ProductReviewsModel> PrepareProductReviewsModelAsync(Product product)
    {
        using var activity = CatalogTracingHelper.ActivitySource.StartActivity(CatalogTracingHelper.ProductDetailsReviewsActivityName);
        activity?.SetTag(CatalogTracingHelper.ProductTypeTagName, product.ProductType.ToString());

        try
        {
            return await base.PrepareProductReviewsModelAsync(product);
        }
        catch (Exception exception)
        {
            CatalogTracingHelper.MarkException(activity, exception);
            throw;
        }
    }
}
