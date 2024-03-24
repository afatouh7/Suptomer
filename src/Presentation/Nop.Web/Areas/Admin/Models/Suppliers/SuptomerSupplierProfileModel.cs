using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Suppliers;

public partial record SuptomerSupplierProfileModel : BaseNopModel
{
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.Fields.Email")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [NopResourceDisplayName("Account.Changepassword.Fields.OldPassword")]
    public string OldPassword { get; set; }

    [DataType(DataType.Password)]
    [NopResourceDisplayName("account.passwordrecovery.newpassword")]
    public string NewPassword { get; set; }

    [NopResourceDisplayName("Account.Fields.FullName")]
    public string FullName { get; set; }

    [DataType(DataType.PhoneNumber)]
    [NopResourceDisplayName("Account.Fields.Phone")]
    public string Phone { get; set; }

    [NopResourceDisplayName("admin.customers.customers.fields.company")]
    public string CompanyName { get; set; }

    [NopResourceDisplayName("Suptomer.Common.MobileNumber")]
    public string MobileNumber { get; set; }

    [NopResourceDisplayName("Account.Fields.ShipmentEmail")]
    public string ShipmentEmail { get; set; }

    public IFormFile AvatarFile { get; set; }

    public int? WorkDayFrom { get; set; }
    public int? WorkDayTo { get; set; }
    public int? DayOff { get; set; }
    public string TimeFrameFrom { get; set; }
    public string TimeFrameTo { get; set; }
    public string TimeZone { get; set; } = "Arab Standard Time";
    public IList<IFormFile> SupplierDocs { get; set; }
    public IList<IFormFile> SupplierNationalId { get; set; }
    public IFormFile Signature { get; set; }
    public List<UploadedFilesModel> SupplierDocFiles { get; set; }
    public List<UploadedFilesModel> SupplierNationalIdFiles { get; set; }
    //******************* prepare page data ***********************
    public string AvatarUrl { get; set; }
    public string SignatureUrl { get; set; }
    public List<WeekDaysOptionModel> WeekDaysOptions { get; set; } = new();
    public List<SelectListItem> TimeZones { get; set; } = new();
}

public partial record WeekDaysOptionModel : BaseNopEntityModel
{
    public string Name { get; set; }
}

public partial record UploadedFilesModel
{
    public string FileName { get; set; }
    public int FileId { get; set; }
}
