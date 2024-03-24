using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Suppliers;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Suppliers;

public class SupplierProfileModelValidator : BaseNopValidator<SuptomerSupplierProfileModel>
{
    public SupplierProfileModelValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.Email.Required"));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Common.WrongEmail"));

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.FirstName.Required"));

        RuleFor(x => x.ShipmentEmail)
            .EmailAddress()
            .When(x => x.ShipmentEmail is not null)
            .WithName(localizationService.GetResourceAsync("Account.Fields.ShipmentEmail").Result);
    }
}
