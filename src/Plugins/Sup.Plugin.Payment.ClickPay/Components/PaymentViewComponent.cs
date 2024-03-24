using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Components;
using Sup.Plugin.Payment.ClickPay.Models;

namespace Nop.Plugin.Payment.ClickPay.Components
{
    public class PaymentViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //var model = new PaymentInfo()
            //{
            //    CreditCardTypes = new List<SelectListItem>
            //    {
            //        new SelectListItem { Text = "Visa", Value = "visa" },
            //        new SelectListItem { Text = "Master card", Value = "MasterCard" },
            //        new SelectListItem { Text = "Discover", Value = "Discover" },
            //        new SelectListItem { Text = "Amex", Value = "Amex" },
            //    }
            //};

            ////years
            //for (var i = 0; i < 15; i++)
            //{
            //    var year = (DateTime.Now.Year + i).ToString();
            //    model.ExpireYears.Add(new SelectListItem { Text = year, Value = year, });
            //}

            ////months
            //for (var i = 1; i <= 12; i++)
            //{
            //    model.ExpireMonths.Add(new SelectListItem { Text = i.ToString("D2"), Value = i.ToString(), });
            //}

            ////set postback values (we cannot access "Form" with "GET" requests)
            //if (Request.Method != WebRequestMethods.Http.Get)
            //{
            //    var form = Request.Form;
            //    model.card_scheme = form["card_scheme"];
            //    model.payment_description = form["payment_description"];
            //   // model.CardCode = form["CardCode"];
            //    var selectedCcType = model.CreditCardTypes.FirstOrDefault(x => x.Value.Equals(form["CreditCardType"], StringComparison.InvariantCultureIgnoreCase));
            //    if (selectedCcType != null)
            //        selectedCcType.Selected = true;
            //    var selectedMonth = model.ExpireMonths.FirstOrDefault(x => x.Value.Equals(form["ExpireMonth"], StringComparison.InvariantCultureIgnoreCase));
            //    if (selectedMonth != null)
            //        selectedMonth.Selected = true;
            //    var selectedYear = model.ExpireYears.FirstOrDefault(x => x.Value.Equals(form["ExpireYear"], StringComparison.InvariantCultureIgnoreCase));
            //    if (selectedYear != null)
            //        selectedYear.Selected = true;
            //}

           // return View("~/Sup.Plugin.Payment.ClickPay/Views/PaymentInfo.cshtml", model);
            return View("~/Sup.Plugin.Payment.ClickPay/Views/PaymentInfo.cshtml");
        }
    }
}
