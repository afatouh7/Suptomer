using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Controllers;
using Nop.Web.Factories;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Models.Catalog;
using Sup.Plugin.Merchant.Areas.Admin.Models;

namespace Sup.Plugin.Merchant.Controllers
{
    public class ContractController : BasePublicController
    {
       
        private readonly CatalogSettings _catalogSettings;
        private readonly IAclService _aclService;
        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IManufacturerService _manufacturerService;
        private readonly INopUrlHelper _nopUrlHelper;
        private readonly IPermissionService _permissionService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IProductTagService _productTagService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IVendorService _vendorService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly VendorSettings _vendorSettings;

        public ContractController(CatalogSettings catalogSettings,
        IAclService aclService,
        ICatalogModelFactory catalogModelFactory,
        ICategoryService categoryService,
        ICustomerActivityService customerActivityService,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        IManufacturerService manufacturerService,
        INopUrlHelper nopUrlHelper,
        IPermissionService permissionService,
        IProductModelFactory productModelFactory,
        IProductService productService,
        IProductTagService productTagService,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        IUrlRecordService urlRecordService,
        IVendorService vendorService,
        IWebHelper webHelper,
        IWorkContext workContext,
        MediaSettings mediaSettings,
        VendorSettings vendorSettings)
        {
            _catalogSettings = catalogSettings;
            _aclService = aclService;
            _catalogModelFactory = catalogModelFactory;
            _categoryService = categoryService;
            _customerActivityService = customerActivityService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _manufacturerService = manufacturerService;
            _nopUrlHelper = nopUrlHelper;
            _permissionService = permissionService;
            _productModelFactory = productModelFactory;
            _productService = productService;
            _productTagService = productTagService;
            _storeContext = storeContext;
            _storeMappingService = storeMappingService;
            _urlRecordService = urlRecordService;
            _vendorService = vendorService;
            _webHelper = webHelper;
            _workContext = workContext;
            _mediaSettings = mediaSettings;
            _vendorSettings = vendorSettings;
        }


      

        #region Product 
      
        public virtual async Task<IActionResult> AllProducts(CatalogProductsCommand command)
        {
            var model = new NewProductsModel
            {
                CatalogProductsModel = await _catalogModelFactory.PrepareAllProductsModelAsync(command)
            };

            return View(model);
        }

        [CheckLanguageSeoCode(ignore: true)]
        public virtual async Task<IActionResult> GetAllProducts(CatalogProductsCommand command)
        {
            if (!_catalogSettings.NewProductsEnabled)
                return NotFound();

            var model = await _catalogModelFactory.PrepareAllProductsModelAsync(command);

            return PartialView("_ProductsInGridOrLines", model);
        }
    
        public async Task<IActionResult> AltenativeProducts(int productId)
        {
            var producatCategory = await _productService.GetProductCategoryByProductIdAsync(productId);
            if (producatCategory == null)
                return NotFound();

            var model = await _productService.GetCategoryFeaturedProductsAsync(producatCategory.CategoryId, 0);

            return View(model);
        }

        [CheckLanguageSeoCode(ignore: true)]
        public async Task<IActionResult> GetAltenativeProducts(int productId)
        {
            var producatCategory = await _productService.GetProductCategoryByProductIdAsync(productId);
            if (producatCategory == null)
                return NotFound();

            var model = await _productService.GetCategoryFeaturedProductsAsync(producatCategory.CategoryId, 0);

            return PartialView("_AlternativeProducts", model);
        }
        #endregion

        #region Contract

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }


        public virtual async Task<IActionResult> List()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var email = customer.Email;
            var model = _productService.GetAllContractForMerchantByEmail(email);

            return View("~/Plugins/Sup.Plugin.Merchant/Views/List.cshtml", model);
        }

   
   

        [HttpPost]
        public virtual IActionResult List(ContractModel searchModel)
        {

            //prepare model
            var model = new ContractModel();

            return View("~/Plugins/Sup.Plugin.Merchant/Views/List.cshtml", model);
        } 

        //public virtual IActionResult PreCreateStep()
        //{
        //    return View("~/Plugins/Sup.Plugin.Merchant/Views/PreCreateStep.cshtml");
        //}

       
        public virtual IActionResult PreCreateStep()
        {

            return View("~/Plugins/Sup.Plugin.Merchant/Views/PreCreateStep.cshtml");
        }
        public virtual IActionResult CreateContract()
        {

            return View("~/Plugins/Sup.Plugin.Merchant/Views/CreateContract.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract(Nop.Core.Domain.Contract.SupplierContract supplierContract)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            supplierContract.Email = customer.Email;
            var model = _productService.AddSuplierContractAsync(supplierContract);
            return Json(model);
        }
        #endregion

        #region Categories

        public virtual async Task<IActionResult> Category(int categoryId, CatalogProductsCommand command)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (!await CheckCategoryAvailabilityAsync(category))
                return InvokeHttp404();

            var store = await _storeContext.GetCurrentStoreAsync();

            //'Continue shopping' URL
            await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.LastContinueShoppingPageAttribute,
                _webHelper.GetThisPageUrl(false),
                store.Id);

            //display "edit" (manage) link
            if (await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel) && await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                DisplayEditLink(Url.Action("Edit", "Category", new { id = category.Id, area = AreaNames.Admin }));

            //activity log
            await _customerActivityService.InsertActivityAsync("PublicStore.ViewCategory",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.ViewCategory"), category.Name), category);

            //model
            var model = await _catalogModelFactory.PrepareCategoryModelAsync(category, command);

            //template
            var templateViewPath = await _catalogModelFactory.PrepareCategoryTemplateViewPathAsync(category.CategoryTemplateId);
            return View(templateViewPath, model);
        }

        //ignore SEO friendly URLs checks
        [CheckLanguageSeoCode(ignore: true)]
        public virtual async Task<IActionResult> GetCategoryProducts(int categoryId, CatalogProductsCommand command)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (!await CheckCategoryAvailabilityAsync(category))
                return NotFound();

            var model = await _catalogModelFactory.PrepareCategoryProductsModelAsync(category, command);

            return PartialView("_ProductsInGridOrLines", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetCatalogRoot()
        {
            var model = await _catalogModelFactory.PrepareRootCategoriesAsync();

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetCatalogSubCategories(int id)
        {
            var model = await _catalogModelFactory.PrepareSubCategoriesAsync(id);

            return Json(model);
        }

        #endregion

        #region Manufacturers

        public virtual async Task<IActionResult> Manufacturer(int manufacturerId, CatalogProductsCommand command)
        {
            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(manufacturerId);

            if (!await CheckManufacturerAvailabilityAsync(manufacturer))
                return InvokeHttp404();

            var store = await _storeContext.GetCurrentStoreAsync();

            //'Continue shopping' URL
            await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.LastContinueShoppingPageAttribute,
                _webHelper.GetThisPageUrl(false),
                store.Id);

            //display "edit" (manage) link
            if (await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel) && await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageManufacturers))
                DisplayEditLink(Url.Action("Edit", "Manufacturer", new { id = manufacturer.Id, area = AreaNames.Admin }));

            //activity log
            await _customerActivityService.InsertActivityAsync("PublicStore.ViewManufacturer",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.ViewManufacturer"), manufacturer.Name), manufacturer);

            //model
            var model = await _catalogModelFactory.PrepareManufacturerModelAsync(manufacturer, command);

            //template
            var templateViewPath = await _catalogModelFactory.PrepareManufacturerTemplateViewPathAsync(manufacturer.ManufacturerTemplateId);

            return View(templateViewPath, model);
        }

        //ignore SEO friendly URLs checks
        [CheckLanguageSeoCode(ignore: true)]
        public virtual async Task<IActionResult> GetManufacturerProducts(int manufacturerId, CatalogProductsCommand command)
        {
            var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(manufacturerId);

            if (!await CheckManufacturerAvailabilityAsync(manufacturer))
                return NotFound();

            var model = await _catalogModelFactory.PrepareManufacturerProductsModelAsync(manufacturer, command);

            return PartialView("_ProductsInGridOrLines", model);
        }

        public virtual async Task<IActionResult> ManufacturerAll()
        {
            var model = await _catalogModelFactory.PrepareManufacturerAllModelsAsync();

            return View(model);
        }

        #endregion

        #region Searching

        public virtual async Task<IActionResult> Search(SearchModel model, CatalogProductsCommand command)
        {
            var store = await _storeContext.GetCurrentStoreAsync();

            //'Continue shopping' URL
            await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(),
                NopCustomerDefaults.LastContinueShoppingPageAttribute,
                _webHelper.GetThisPageUrl(true),
                store.Id);

            if (model == null)
                model = new SearchModel();

            model = await _catalogModelFactory.PrepareSearchModelAsync(model, command);

            return View(model);
        }

        [CheckLanguageSeoCode(ignore: true)]
        public virtual async Task<IActionResult> SearchTermAutoComplete(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Content("");

            term = term.Trim();

            if (string.IsNullOrWhiteSpace(term) || term.Length < _catalogSettings.ProductSearchTermMinimumLength)
                return Content("");

            //products
            var productNumber = _catalogSettings.ProductSearchAutoCompleteNumberOfProducts > 0 ?
                _catalogSettings.ProductSearchAutoCompleteNumberOfProducts : 10;
            var store = await _storeContext.GetCurrentStoreAsync();
            var products = await _productService.SearchProductsAsync(0,
                storeId: store.Id,
                keywords: term,
                languageId: (await _workContext.GetWorkingLanguageAsync()).Id,
                visibleIndividuallyOnly: true,
                pageSize: productNumber);

            var showLinkToResultSearch = _catalogSettings.ShowLinkToAllResultInSearchAutoComplete && (products.TotalCount > productNumber);

            var models = (await _productModelFactory.PrepareProductOverviewModelsAsync(products, false, _catalogSettings.ShowProductImagesInSearchAutoComplete, _mediaSettings.AutoCompleteSearchThumbPictureSize)).ToList();
            var result = (from p in models
                          select new
                          {
                              label = p.Name,
                              producturl = Url.RouteUrl<Product>(new { SeName = p.SeName }),
                              productpictureurl = p.PictureModels.FirstOrDefault()?.ImageUrl,
                              showlinktoresultsearch = showLinkToResultSearch
                          })
                .ToList();
            return Json(result);
        }

        //ignore SEO friendly URLs checks
        [CheckLanguageSeoCode(ignore: true)]
        public virtual async Task<IActionResult> SearchProducts(SearchModel searchModel, CatalogProductsCommand command)
        {
            if (searchModel == null)
                searchModel = new SearchModel();

            var model = await _catalogModelFactory.PrepareSearchProductsModelAsync(searchModel, command);

            return PartialView("_ProductsInGridOrLines", model);
        }

        #endregion

        #region Utilities

        private async Task<bool> CheckCategoryAvailabilityAsync(Category category)
        {
            if (category is null)
                return false;

            var isAvailable = true;

            if (category.Deleted)
                isAvailable = false;

            var notAvailable =
            //published?
                !category.Published ||
                //ACL (access control list) 
                !await _aclService.AuthorizeAsync(category) ||
                //Store mapping
                !await _storeMappingService.AuthorizeAsync(category);
            //Check whether the current user has a "Manage categories" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel) && await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories);
            if (notAvailable && !hasAdminAccess)
                isAvailable = false;

            return isAvailable;
        }

        private async Task<bool> CheckManufacturerAvailabilityAsync(Manufacturer manufacturer)
        {
            var isAvailable = true;

            if (manufacturer == null || manufacturer.Deleted)
                isAvailable = false;

            var notAvailable =
            //published?
                !manufacturer.Published ||
            //ACL (access control list) 
                !await _aclService.AuthorizeAsync(manufacturer) ||
                //Store mapping
                !await _storeMappingService.AuthorizeAsync(manufacturer);
            //Check whether the current user has a "Manage categories" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            var hasAdminAccess = await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel) && await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageManufacturers);
            if (notAvailable && !hasAdminAccess)
                isAvailable = false;

            return isAvailable;
        }

        private Task<bool> CheckVendorAvailabilityAsync(Vendor vendor)
        {
            var isAvailable = true;

            if (vendor == null || vendor.Deleted || !vendor.Active)
                isAvailable = false;

            return Task.FromResult(isAvailable);
        }

        #endregion
    }
}
