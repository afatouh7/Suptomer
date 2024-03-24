using System.ComponentModel.DataAnnotations;
using Nop.Core.Domain.Customers;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Supplier;

public partial record SuptomerRegisterModel : BaseNopModel
{
    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.Fields.Email")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [NopResourceDisplayName("Account.Fields.Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [NopResourceDisplayName("Account.Fields.ConfirmPassword")]
    public string ConfirmPassword { get; set; }

    [NopResourceDisplayName("Account.Fields.FullName")]
    public string FullName { get; set; }

    [DataType(DataType.PhoneNumber)]
    [NopResourceDisplayName("Account.Fields.Phone")]
    public string Phone { get; set; }

    public UserType UserType { get; set; }
}