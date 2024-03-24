using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Sup.Plugin.Payment.ClickPay.Models
{
    public class ClickPayConfiguration 
    {
        public int ActiveStoreScopeConfiguration { get; set; }
        [Display(Name = "Profile ID")]
        public int ProfileId { get; set; }
        public bool ProfileId_OverrideForStore { get; set; }

        [Display(Name = "Transaction Type")]
        public string TranType { get; set; }
        public bool TranType_OverrideForStore { get; set; }

        [Display(Name = "Transaction Class")]
        public string TranClass { get; set; }
        public bool TranClass_OverrideForStore { get; set; }

        [Display(Name = "Cart Description")]
        public string CartDescription { get; set; }
        public bool CartDescription_OverrideForStore { get; set; }

        [Display(Name = "Cart ID")]
        public string CartId { get; set; }
         public bool CartId_OverrideForStore { get; set; }

        [Display(Name = "Cart Currency")]
        public string CartCurrency { get; set; }
        public bool CartCurrency_OverrideForStore { get; set; }

        [Display(Name = "Cart Amount")]
        public decimal CartAmount { get; set; }
        public bool CartAmount_OverrideForStore { get; set; }

        [Display(Name = "Callback URL")]
        public string CallbackUrl { get; set; }
        public bool CallbackUrl_OverrideForStore { get; set; }

        [Display(Name = "Return URL")]
        public string ReturnUrl { get; set; }
        public bool ReturnUrl_OverrideForStore { get; set; }
        [Display(Name = "Server Key")]
        public string ProfileServerKey { get; set; }
        public bool ProfileServerKey_OverrideForStore { get; set; }

    }
}

