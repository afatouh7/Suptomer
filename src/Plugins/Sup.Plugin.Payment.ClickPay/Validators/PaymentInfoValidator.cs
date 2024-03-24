using System;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Nop.Plugin.Payments.Manual.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Sup.Plugin.Payment.ClickPay.Models;

namespace Sup.Plugin.Payment.ClickPay.Validators
{
    public class PaymentValidator : BaseNopValidator<PaymentInfo>
    {
        public PaymentValidator(ILocalizationService localizationService)
        {
            //useful links:
            //http://fluentvalidation.codeplex.com/wikipage?title=Custom&referringTitle=Documentation&ANCHOR#CustomValidator
            //http://benjii.me/2010/11/credit-card-validator-attribute-for-asp-net-mvc-3/

            //RuleFor(x => x.CardNumber).NotEmpty().WithMessage(localizationService.GetResource("Payment.CardNumber.Required"));
            //RuleFor(x => x.CardCode).NotEmpty().WithMessage(localizationService.GetResource("Payment.CardCode.Required"));

            RuleFor(x => x.card_scheme).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Payment.CardholderName.Required"));
            RuleFor(x => x.card_type).IsCreditCard().WithMessageAwait(localizationService.GetResourceAsync("Payment.SelectCreditCard.Required"));
            RuleFor(x => x.payment_description).IsCreditCard().WithMessageAwait(localizationService.GetResourceAsync("Payment.Description.Required"));
            RuleFor(x => x.expiryMonth).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Payment.ExpireMonth.Required"));
            RuleFor(x => x.expiryYear).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Payment.ExpireYear.Required"));
            RuleFor(x => x.expiryMonth).Must((x, context) =>
            {
                //not specified yet
                if (string.IsNullOrEmpty(x.expiryYear) || string.IsNullOrEmpty(x.expiryMonth))
                    return true;
                
                //the cards remain valid until the last calendar day of that month
                //If, for example, an expiration date reads 06/15, this means it can be used until midnight on June 30, 2015
                var enteredDate = new DateTime(int.Parse(x.expiryYear), int.Parse(x.expiryMonth), 1).AddMonths(1);

                if (enteredDate < DateTime.Now)
                    return false;

                return true;
            }).WithMessageAwait(localizationService.GetResourceAsync("Payment.ExpirationDate.Expired"));
        }
    }
}