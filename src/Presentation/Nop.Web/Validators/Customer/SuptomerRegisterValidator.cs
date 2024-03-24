using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Supplier;

namespace Nop.Web.Validators.Customer;

public class SuptomerRegisterValidator : BaseNopValidator<SuptomerRegisterModel>
{
    public SuptomerRegisterValidator(ILocalizationService localizationService,
            CustomerSettings customerSettings)
    {
        RuleFor(x => x.Email).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.Email.Required"));
        RuleFor(x => x.Email).EmailAddress().WithMessageAwait(localizationService.GetResourceAsync("Common.WrongEmail"));
        RuleFor(x => x.FullName).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.FirstName.Required"));
        RuleFor(x => x.Password).IsPassword(localizationService, customerSettings);
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.ConfirmPassword.Required"));
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.Password.EnteredPasswordsDoNotMatch"));
    }
}
