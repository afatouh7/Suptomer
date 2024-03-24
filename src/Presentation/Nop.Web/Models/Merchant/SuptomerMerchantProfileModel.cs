using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.SuptomerBranches;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Merchant;

public partial record SuptomerMerchantProfileModel : BaseNopModel
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

    [NopResourceDisplayName("Suptomer.Merchant.Profile.ContactPersonName")]
    public string ContactPersonName { get; set; }

    [NopResourceDisplayName("Suptomer.Merchant.Profile.BudgetValue")]
    public string BudgetValue { get; set; }

    [NopResourceDisplayName("Suptomer.Merchant.Profile.BudgetLimitInterval")]
    public int? BudgetLimitInterval { get; set; }

    [NopResourceDisplayName("Account.Fields.ShipmentEmail")]
    public string ShipmentEmail { get; set; }

    public IFormFile AvatarFile { get; set; }

    public string TimeFrameFrom { get; set; }
    public string TimeFrameTo { get; set; }
    public string TimeZone { get; set; } = "Arab Standard Time";
    public string BranchesTimeZone { get; set; } = "Arab Standard Time";
    public IList<IFormFile> SupplierDocs { get; set; }
    public IList<IFormFile> SupplierNationalId { get; set; }
    public IFormFile Signature { get; set; }
    public List<UploadedFilesModel> SupplierDocFiles { get; set; }
    public List<UploadedFilesModel> SupplierNationalIdFiles { get; set; }
    //******************* prepare page data ***********************
    public string AvatarUrl { get; set; }
    public string SignatureUrl { get; set; }
    public List<BudgetLimitIntervalsOptionModel> BudgetLimitIntervalsOptions { get; set; } = new();
    public List<SelectListItem> TimeZones { get; set; } = new();
    public List<Branch> Branches { get; set; }
}

public partial record UploadedFilesModel
{
    public string FileName { get; set; }
    public int FileId { get; set; }
}

public partial record BudgetLimitIntervalsOptionModel : BaseNopEntityModel
{
    public string Name { get; set; }
}
