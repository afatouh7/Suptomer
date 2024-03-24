using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Helpers;
using Nop.Web.Factories;
using Nop.Web.Framework.Factories;
using Nop.Web.Infrastructure.Installation;

namespace Nop.Web.Infrastructure;

/// <summary>
/// Represents the registering services on application startup
/// </summary>
public partial class NopStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //installation localization service
        services.AddScoped<IInstallationLocalizationService, InstallationLocalizationService>();

        //common factories
        services.AddScoped<IAclSupportedModelFactory, AclSupportedModelFactory>();
        services.AddScoped<IDiscountSupportedModelFactory, DiscountSupportedModelFactory>();
        services.AddScoped<ILocalizedModelFactory, LocalizedModelFactory>();
        services.AddScoped<IStoreMappingSupportedModelFactory, StoreMappingSupportedModelFactory>();

        //admin factories
        services.AddScoped<IBaseAdminModelFactory, BaseAdminModelFactory>();
        services.AddScoped<IActivityLogModelFactory, ActivityLogModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IAddressModelFactory, Areas.Admin.Factories.AddressModelFactory>();
        services.AddScoped<IAddressAttributeModelFactory, AddressAttributeModelFactory>();
        services.AddScoped<IAffiliateModelFactory, AffiliateModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IBlogModelFactory, Areas.Admin.Factories.BlogModelFactory>();
        services.AddScoped<ICampaignModelFactory, CampaignModelFactory>();
        services.AddScoped<ICategoryModelFactory, CategoryModelFactory>();
        services.AddScoped<ICheckoutAttributeModelFactory, CheckoutAttributeModelFactory>();
        services.AddScoped<Areas.Admin.Factories.ICommonModelFactory, Areas.Admin.Factories.CommonModelFactory>();
        services.AddScoped<Areas.Admin.Factories.ICountryModelFactory, Areas.Admin.Factories.CountryModelFactory>();
        services.AddScoped<ICurrencyModelFactory, CurrencyModelFactory>();
        services.AddScoped<ICustomerAttributeModelFactory, CustomerAttributeModelFactory>();
        services.AddScoped<Areas.Admin.Factories.ICustomerModelFactory, Areas.Admin.Factories.CustomerModelFactory>();
        services.AddScoped<ICustomerRoleModelFactory, CustomerRoleModelFactory>();
        services.AddScoped<IDiscountModelFactory, DiscountModelFactory>();
        services.AddScoped<IEmailAccountModelFactory, EmailAccountModelFactory>();
        services.AddScoped<IExternalAuthenticationMethodModelFactory, ExternalAuthenticationMethodModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IForumModelFactory, Areas.Admin.Factories.ForumModelFactory>();
        services.AddScoped<IGiftCardModelFactory, GiftCardModelFactory>();
        services.AddScoped<IHomeModelFactory, HomeModelFactory>();
        services.AddScoped<ILanguageModelFactory, LanguageModelFactory>();
        services.AddScoped<ILogModelFactory, LogModelFactory>();
        services.AddScoped<IManufacturerModelFactory, ManufacturerModelFactory>();
        services.AddScoped<IMeasureModelFactory, MeasureModelFactory>();
        services.AddScoped<IMessageTemplateModelFactory, MessageTemplateModelFactory>();
        services.AddScoped<IMultiFactorAuthenticationMethodModelFactory, MultiFactorAuthenticationMethodModelFactory>();
        services.AddScoped<INewsletterSubscriptionModelFactory, NewsletterSubscriptionModelFactory>();
        services.AddScoped<Areas.Admin.Factories.INewsModelFactory, Areas.Admin.Factories.NewsModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IOrderModelFactory, Areas.Admin.Factories.OrderModelFactory>();
        services.AddScoped<IPaymentModelFactory, PaymentModelFactory>();
        services.AddScoped<IPluginModelFactory, PluginModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IPollModelFactory, Areas.Admin.Factories.PollModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IProductModelFactory, Areas.Admin.Factories.ProductModelFactory>();
        services.AddScoped<IProductAttributeModelFactory, ProductAttributeModelFactory>();
        services.AddScoped<IProductReviewModelFactory, ProductReviewModelFactory>();
        services.AddScoped<IReportModelFactory, ReportModelFactory>();
        services.AddScoped<IQueuedEmailModelFactory, QueuedEmailModelFactory>();
        services.AddScoped<IRecurringPaymentModelFactory, RecurringPaymentModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IReturnRequestModelFactory, Areas.Admin.Factories.ReturnRequestModelFactory>();
        services.AddScoped<IReviewTypeModelFactory, ReviewTypeModelFactory>();
        services.AddScoped<IScheduleTaskModelFactory, ScheduleTaskModelFactory>();
        services.AddScoped<ISecurityModelFactory, SecurityModelFactory>();
        services.AddScoped<ISettingModelFactory, SettingModelFactory>();
        services.AddScoped<IShippingModelFactory, ShippingModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IShoppingCartModelFactory, Areas.Admin.Factories.ShoppingCartModelFactory>();
        services.AddScoped<ISpecificationAttributeModelFactory, SpecificationAttributeModelFactory>();
        services.AddScoped<IStoreModelFactory, StoreModelFactory>();
        services.AddScoped<ITaxModelFactory, TaxModelFactory>();
        services.AddScoped<ITemplateModelFactory, TemplateModelFactory>();
        services.AddScoped<Areas.Admin.Factories.ITopicModelFactory, Areas.Admin.Factories.TopicModelFactory>();
        services.AddScoped<IVendorAttributeModelFactory, VendorAttributeModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IVendorModelFactory, Areas.Admin.Factories.VendorModelFactory>();
        services.AddScoped<Areas.Admin.Factories.IWidgetModelFactory, Areas.Admin.Factories.WidgetModelFactory>();

        //supplier factories
        services.AddScoped<ISupplierProfileModelFactory, SupplierProfileModelFactory>();
        services.AddScoped<IMerchantProfileModelFactory, MerchantProfileModelFactory>();

        //factories
        services.AddScoped<Factories.IAddressModelFactory, Factories.AddressModelFactory>();
        services.AddScoped<Factories.IBlogModelFactory, Factories.BlogModelFactory>();
        services.AddScoped<Factories.ICatalogModelFactory, Factories.CatalogModelFactory>();
        services.AddScoped<Factories.ICheckoutModelFactory, Factories.CheckoutModelFactory>();
        services.AddScoped<Factories.ICommonModelFactory, Factories.CommonModelFactory>();
        services.AddScoped<Factories.ICountryModelFactory, Factories.CountryModelFactory>();
        services.AddScoped<Factories.ICustomerModelFactory, Factories.CustomerModelFactory>();
        services.AddScoped<Factories.IForumModelFactory, Factories.ForumModelFactory>();
        services.AddScoped<Factories.IExternalAuthenticationModelFactory, Factories.ExternalAuthenticationModelFactory>();
        services.AddScoped<Factories.INewsModelFactory, Factories.NewsModelFactory>();
        services.AddScoped<Factories.INewsletterModelFactory, Factories.NewsletterModelFactory>();
        services.AddScoped<Factories.IOrderModelFactory, Factories.OrderModelFactory>();
        services.AddScoped<Factories.IPollModelFactory, Factories.PollModelFactory>();
        services.AddScoped<Factories.IPrivateMessagesModelFactory, Factories.PrivateMessagesModelFactory>();
        services.AddScoped<Factories.IProductModelFactory, Factories.ProductModelFactory>();
        services.AddScoped<Factories.IProfileModelFactory, Factories.ProfileModelFactory>();
        services.AddScoped<Factories.IReturnRequestModelFactory, Factories.ReturnRequestModelFactory>();
        services.AddScoped<Factories.IShoppingCartModelFactory, Factories.ShoppingCartModelFactory>();
        services.AddScoped<Factories.ISitemapModelFactory, Factories.SitemapModelFactory>();
        services.AddScoped<Factories.ITopicModelFactory, Factories.TopicModelFactory>();
        services.AddScoped<Factories.IVendorModelFactory, Factories.VendorModelFactory>();
        services.AddScoped<Factories.IWidgetModelFactory, Factories.WidgetModelFactory>();

        //helpers classes
        services.AddScoped<ITinyMceHelper, TinyMceHelper>();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 2002;
}
