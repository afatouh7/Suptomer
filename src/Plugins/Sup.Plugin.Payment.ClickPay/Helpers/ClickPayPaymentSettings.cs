using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Sup.Plugin.Payment.ClickPay.Helpers
{
    public class ClickPayPaymentSettings : ISettings
    {
        public string ProfileServerKey { get; set; }
        public int ProfileId { get; set; }
        public string CallbackUrl { get; set; }
        public string ReturnUrl { get; set; }

       
    }
}
