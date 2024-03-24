using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Profile;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.SuptomerBranshes;
using Nop.Web.Models.Merchant;

namespace Nop.Web.Factories;

public class MerchantProfileModelFactory : IMerchantProfileModelFactory
{
    #region Fields

    private readonly CustomerSettings _customerSettings;
    private readonly ICountryService _countryService;
    private readonly ICustomerService _customerService;
    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly IGenericAttributeService _genericAttributeService;
    private readonly ILocalizationService _localizationService;
    private readonly IPictureService _pictureService;
    private readonly MediaSettings _mediaSettings;
    private readonly IDownloadService _downloadService;
    private readonly IBranchService _branchService;

    #endregion

    #region Ctor

    public MerchantProfileModelFactory(
        CustomerSettings customerSettings,
        ICountryService countryService,
        ICustomerService customerService,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        IPictureService pictureService,
        MediaSettings mediaSettings,
        IDownloadService downloadService,
        IBranchService branchService)
    {
        _customerSettings = customerSettings;
        _countryService = countryService;
        _customerService = customerService;
        _dateTimeHelper = dateTimeHelper;
        _genericAttributeService = genericAttributeService;
        _localizationService = localizationService;
        _pictureService = pictureService;
        _mediaSettings = mediaSettings;
        _downloadService = downloadService;
        _branchService = branchService;
    }

    #endregion
    public async Task<SuptomerMerchantProfileModel> PrepareProfileModel(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        var budgetLimitIntervalOptions = await Enum.GetValues(typeof(BudgetLimitIntervalEnum)).OfType<BudgetLimitIntervalEnum>()
            .SelectAwait(async e => new BudgetLimitIntervalsOptionModel
            {
                Id = (int)e,
                Name = await _localizationService.GetLocalizedEnumAsync(e)
            }).ToListAsync();

        var availableTimeZones = _dateTimeHelper.GetSystemTimeZones();

        var customerAttributes = await _genericAttributeService.GetAttributesForEntityAsync(customer.Id, customer.GetType().Name);
        var contactPersonName = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.ContactPersonName);
        var budgetValue = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BudgetValue);
        var budgetLimitInterval = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BudgetLimitInterval);
        var branchName = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BranchName);
        var branchManager = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BranchManager);
        var branchMobileNumber = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BranchMobileNumber);
        var branchTimeFrameFrom = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BranchTimeFrameFrom);
        var branchTimeFrameTo = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BranchTimeFrameTo);
        var branchTimeZone = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.BranchTimeZone);

        var workDayTo = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.WorkingDaysTo);
        var dayoff = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.DayOff);
        var avatar = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.AvatarPictureIdAttribute);
        var avatarUrl = await _pictureService.GetPictureUrlAsync(avatar is null ? 0 : int.Parse(avatar.Value),
                 _mediaSettings.AvatarPictureSize, true, defaultPictureType: PictureType.Avatar);
        var shipmentEmail = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.ShipmentEmail);
        var timeFrameFrom = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.TimeFrameFrom);
        var timeFrameTo = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.TimeFrameTo);

        var signature = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.Signature);
        var signatureUrl = signature is null ? null : await _pictureService.GetPictureUrlAsync(signature is null ? 0 : int.Parse(signature.Value),
                 _mediaSettings.AvatarPictureSize, true, defaultPictureType: PictureType.Entity);

        var supplierDocs = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.SupplierDocs);
        var supplierDocsIds = supplierDocs?.Value.Split(',');
        var supplierDocsFiles = supplierDocsIds?.SelectAwait(async d =>
        {
            var file = await _downloadService.GetDownloadByIdAsync(int.Parse(d));

            return new UploadedFilesModel
            {
                FileId = file.Id,
                FileName = file.Filename
            };
        }).ToListAsync();

        var supplierNationalId = customerAttributes.FirstOrDefault(a => a.Key == NopCustomerDefaults.NationalId);
        var supplierNationalIds = supplierNationalId?.Value.Split(',');
        var supplierNationalIdFiles = supplierNationalIds?.SelectAwait(async d =>
        {
            var file = await _downloadService.GetDownloadByIdAsync(int.Parse(d));

            return new UploadedFilesModel
            {
                FileId = file.Id,
                FileName = file.Filename
            };
        }).ToListAsync();

        var branches = await _branchService.GetAllBranchesAsync(customer.Id);

        return new()
        {
            FullName = customer.FirstName,
            Email = customer.Email,
            Phone = customer.Phone,
            TimeZones = availableTimeZones.Select(tz => new SelectListItem
            {
                Value = tz.Id,
                Text = tz.DisplayName
            }).ToList(),
            CompanyName = customer.Company,
            ContactPersonName = contactPersonName?.Value,
            BudgetValue = budgetValue?.Value,
            BudgetLimitInterval = budgetLimitInterval is null ? null : int.Parse(budgetLimitInterval.Value),
            AvatarUrl = avatarUrl,
            ShipmentEmail = shipmentEmail?.Value,
            TimeZone = customer.TimeZoneId ?? "Arab Standard Time",
            TimeFrameFrom = timeFrameFrom?.Value,
            TimeFrameTo = timeFrameTo?.Value,
            SignatureUrl = signatureUrl,
            SupplierDocFiles = supplierDocsFiles.HasValue ? await supplierDocsFiles.Value : new(),
            SupplierNationalIdFiles = supplierNationalIdFiles.HasValue ? await supplierNationalIdFiles.Value : new(),
            BudgetLimitIntervalsOptions = budgetLimitIntervalOptions,
            Branches = branches?.ToList(),
        };
    }
}
