using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Profile;
using Nop.Core.Events;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Suppliers;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Controllers;

[AutoValidateAntiforgeryToken]
[ValidateIpAddress]
[AuthorizeAdmin]
[ValidateVendor]
[SaveSelectedTab]
[NotNullValidationMessage]
// [Authorize(Policy = "supplier")]
public class SupplierProfileController : BaseController
{
    private readonly ICustomerService _customerService;
    private readonly ISupplierProfileModelFactory _supplierProfileModelFactory;
    private readonly IWorkContext _workContext;
    private readonly ICustomerRegistrationService _customerRegistrationService;
    private readonly CustomerSettings _customerSettings;
    private readonly ILocalizationService _localizationService;
    private readonly IGenericAttributeService _genericAttributeService;
    private readonly IPictureService _pictureService;
    private readonly IDownloadService _downloadService;
    private readonly IEventPublisher _eventPublisher;
    private readonly IEncryptionService _encryptionService;
    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly MediaSettings _mediaSettings;
    private readonly INotificationService _notificationService;

    public SupplierProfileController(ICustomerService customerService,
        ISupplierProfileModelFactory supplierProfileModelFactory,
        IWorkContext workContext,
        ICustomerRegistrationService customerRegistrationService,
        CustomerSettings customerSettings,
        ILocalizationService localizationService,
        IGenericAttributeService genericAttributeService,
        IPictureService pictureService,
        IDownloadService downloadService,
        IEventPublisher eventPublisher,
        IEncryptionService encryptionService,
        IDateTimeHelper dateTimeHelper,
        MediaSettings mediaSettings,
        INotificationService notificationService)
    {
        _customerService = customerService;
        _supplierProfileModelFactory = supplierProfileModelFactory;
        _workContext = workContext;
        _customerRegistrationService = customerRegistrationService;
        _customerSettings = customerSettings;
        _localizationService = localizationService;
        _genericAttributeService = genericAttributeService;
        _pictureService = pictureService;
        _downloadService = downloadService;
        _eventPublisher = eventPublisher;
        _encryptionService = encryptionService;
        _dateTimeHelper = dateTimeHelper;
        _mediaSettings = mediaSettings;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();

        var model = await _supplierProfileModelFactory.PrepareProfileModel(customer);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(SuptomerSupplierProfileModel model)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var changePassword = !string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword);

        try
        {
            if (changePassword)
            {
                var loginResult = await _customerRegistrationService.ValidateCustomerAsync(customer.Email, model.OldPassword);

                if (loginResult == CustomerLoginResults.WrongPassword)
                    throw new NopException(await _localizationService.GetResourceAsync("topic.wrongpassword"));
            }

            if (model.AvatarFile is not null)
                await ValidateUploadImageAsync(model.AvatarFile, "Account.Avatar.MaximumUploadedFileSize");

            if (model.Signature is not null)
                await ValidateUploadImageAsync(model.Signature, "Suptomer.Common.MaximumUploadedFileSize");

            if (model.SupplierDocs is not null)
            {
                foreach (var file in model.SupplierDocs)
                    await ValidateUploadFileAsync(file);
            }

            if (model.SupplierNationalId is not null)
            {
                foreach (var file in model.SupplierNationalId)
                    await ValidateUploadFileAsync(file);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        if (ModelState.IsValid)
        {
            customer.FirstName = model.FullName;
            customer.Email = model.Email;
            customer.Phone = model.Phone;
            customer.Company = model.CompanyName;
            customer.TimeZoneId = model.TimeZone;

            await _customerService.UpdateCustomerAsync(customer);

            await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.WorkingDaysFrom, model.WorkDayFrom);
            await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.WorkingDaysTo, model.WorkDayTo);
            await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.DayOff, model.DayOff);
            await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.TimeFrameFrom, model.TimeFrameFrom);
            await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.TimeFrameTo, model.TimeFrameTo);
            await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.MobileNumber, model.MobileNumber);
            await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.ShipmentEmail, model.ShipmentEmail);
            await UploadAvatar(customer, model.AvatarFile);
            await UploadSignatureAsync(customer, model.Signature);
            if (changePassword)
                await ChangePasswordAsync(customer, model.NewPassword);

            if (model.SupplierDocs is not null)
            {
                var ids = new List<int>();

                foreach (var file in model.SupplierDocs)
                    ids.Add(await UploadFileAsync(file));

                var supplierDocs = await _genericAttributeService.GetAttributeAsync<string>(customer,
                    NopCustomerDefaults.SupplierDocs);

                supplierDocs = (string.IsNullOrWhiteSpace(supplierDocs) ? string.Empty : supplierDocs + ",")
                    + string.Join(",", ids);

                await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.SupplierDocs, supplierDocs);
            }

            if (model.SupplierNationalId is not null)
            {
                var ids = new List<int>();

                foreach (var file in model.SupplierNationalId)
                    ids.Add(await UploadFileAsync(file));

                var nationalId = await _genericAttributeService.GetAttributeAsync<string>(customer,
                    NopCustomerDefaults.NationalId);

                nationalId = (string.IsNullOrWhiteSpace(nationalId) ? string.Empty : nationalId + ",")
                    + string.Join(",", ids);

                await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.NationalId, nationalId);
            }

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("admin.common.alert.save.ok"));

            return RedirectToAction(nameof(Index));
        }

        var weekDaysOptions = await Enum.GetValues(typeof(WeekDaysEnum)).OfType<WeekDaysEnum>()
            .SelectAwait(async e => new WeekDaysOptionModel
            {
                Id = (int)e,
                Name = await _localizationService.GetLocalizedEnumAsync(e)
            }).ToListAsync();

        var availableTimeZones = _dateTimeHelper.GetSystemTimeZones();
        var avatar = await _genericAttributeService.GetAttributeAsync<string>(customer,
                    NopCustomerDefaults.AvatarPictureIdAttribute);
        var avatarUrl = await _pictureService.GetPictureUrlAsync(avatar is null ? 0 : int.Parse(avatar),
                 _mediaSettings.AvatarPictureSize, true, defaultPictureType: PictureType.Avatar);
        var signature = await _genericAttributeService.GetAttributeAsync<string>(customer,
                    NopCustomerDefaults.Signature);
        var signatureUrl = signature is null ? null : await _pictureService.GetPictureUrlAsync(signature is null ? 0 : int.Parse(signature),
                 _mediaSettings.AvatarPictureSize, true, defaultPictureType: PictureType.Entity);
        var docs = await _genericAttributeService.GetAttributeAsync<string>(customer,
                    NopCustomerDefaults.SupplierDocs);
        var supplierDocsIds = docs?.Split(',');
        var supplierDocsFiles = supplierDocsIds?.SelectAwait(async d =>
        {
            var file = await _downloadService.GetDownloadByIdAsync(int.Parse(d));

            return new UploadedFilesModel
            {
                FileId = file.Id,
                FileName = file.Filename
            };
        }).ToListAsync();

        var supplierNationalId = await _genericAttributeService.GetAttributeAsync<string>(customer,
                    NopCustomerDefaults.NationalId);
        var supplierNationalIds = supplierNationalId?.Split(',');
        var supplierNationalIdFiles = supplierNationalIds?.SelectAwait(async d =>
        {
            var file = await _downloadService.GetDownloadByIdAsync(int.Parse(d));

            return new UploadedFilesModel
            {
                FileId = file.Id,
                FileName = file.Filename
            };
        }).ToListAsync();

        model.TimeZones = availableTimeZones.Select(tz => new SelectListItem
        {
            Value = tz.Id,
            Text = tz.DisplayName
        }).ToList();
        model.WeekDaysOptions = weekDaysOptions;
        model.AvatarUrl = avatarUrl;
        model.SignatureUrl = signatureUrl;
        model.SupplierDocFiles = supplierDocsFiles.HasValue ? await supplierDocsFiles.Value : new();
        model.SupplierNationalIdFiles = supplierNationalIdFiles.HasValue ? await supplierNationalIdFiles.Value : new();
        return View(model);
    }


    public async Task<IActionResult> DownloadFile(int id)
    {
        var file = await _downloadService.GetDownloadByIdAsync(id);

        return File(file.DownloadBinary, file.ContentType, file.Filename);
    }

    private async Task ChangePasswordAsync(Customer customer, string password)
    {
        var customerPassword = new CustomerPassword
        {
            CustomerId = customer.Id,
            PasswordFormat = _customerSettings.DefaultPasswordFormat,
            CreatedOnUtc = DateTime.UtcNow
        };

        var saltKey = _encryptionService.CreateSaltKey(NopCustomerServicesDefaults.PasswordSaltKeySize);
        customerPassword.PasswordSalt = saltKey;
        customerPassword.Password = _encryptionService.CreatePasswordHash(password, saltKey, _customerSettings.HashedPasswordFormat);

        await _customerService.InsertCustomerPasswordAsync(customerPassword);

        await _eventPublisher.PublishAsync(new CustomerPasswordChangedEvent(customerPassword));
    }

    private async Task UploadAvatar(Customer customer, IFormFile uploadedFile)
    {
        var contentType = uploadedFile?.ContentType.ToLowerInvariant();
        var customerAvatar = await _pictureService.GetPictureByIdAsync(await _genericAttributeService.GetAttributeAsync<int>(customer, NopCustomerDefaults.AvatarPictureIdAttribute));

        if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
        {
            var customerPictureBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);

            if (customerAvatar != null)
                await _pictureService.DeletePictureAsync(customerAvatar);

            var newAvatar = await _pictureService.InsertPictureAsync(customerPictureBinary, contentType, null);

            await _genericAttributeService.SaveAttributeAsync(customer,
                NopCustomerDefaults.AvatarPictureIdAttribute, newAvatar?.Id);
        }
    }

    private async Task UploadSignatureAsync(Customer customer, IFormFile uploadedFile)
    {
        var contentType = uploadedFile?.ContentType.ToLowerInvariant();
        var customerSignature = await _pictureService.GetPictureByIdAsync(
            await _genericAttributeService.GetAttributeAsync<int>(customer, NopCustomerDefaults.Signature));

        if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
        {
            var customerPictureBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);

            if (customerSignature != null)
                await _pictureService.DeletePictureAsync(customerSignature);

            var newSignature = await _pictureService.InsertPictureAsync(customerPictureBinary, contentType, null);

            await _genericAttributeService.SaveAttributeAsync(customer,
                NopCustomerDefaults.Signature, newSignature?.Id);
        }
    }

    private async Task<int> UploadFileAsync(IFormFile uploadedFile)
    {
        var contentType = uploadedFile?.ContentType.ToLowerInvariant();

        var fileBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);

        var downlad = new Download
        {
            DownloadBinary = fileBinary,
            ContentType = contentType,
            Extension = Path.GetExtension(uploadedFile.FileName).Replace(".", ""),
            DownloadGuid = Guid.NewGuid(),
            Filename = uploadedFile.FileName,
            IsNew = true
        };

        await _downloadService.InsertDownloadAsync(downlad);
        return downlad.Id;
    }

    private async Task ValidateUploadFileAsync(IFormFile uploadedFile)
    {
        var contentType = uploadedFile?.ContentType.ToLowerInvariant();

        if (contentType != null &&
            !contentType.Equals("image/jpeg") &&
            !contentType.Equals("image/png") &&
            !contentType.Equals("application/pdf") &&
            !contentType.Equals("application/msword") &&
            !contentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            throw new NopException(await _localizationService.GetResourceAsync("Account.Avatar.UploadRules"));

        if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
        {
            var maxSize = _customerSettings.AvatarMaximumSizeBytes;

            if (uploadedFile.Length > maxSize)
                throw new NopException(string.Format(await _localizationService
                    .GetResourceAsync("admin.catalog.attributes.checkoutattributes.fields.filemaximumsize"), maxSize));
        }
    }

    private async Task ValidateUploadImageAsync(IFormFile imageFile, string errorMessage)
    {
        var contentType = imageFile?.ContentType.ToLowerInvariant();

        if (contentType != null && !contentType.Equals("image/jpeg") && !contentType.Equals("image/png"))
            throw new NopException(await _localizationService.GetResourceAsync("Account.Avatar.UploadRules"));

        if (imageFile != null && !string.IsNullOrEmpty(imageFile.FileName))
        {
            var avatarMaxSize = _customerSettings.AvatarMaximumSizeBytes;
            if (imageFile.Length > avatarMaxSize)
                throw new NopException(
                    string.Format(await _localizationService.GetResourceAsync("Account.Avatar.MaximumUploadedFileSize"),
                    avatarMaxSize));
        }
    }
}
