using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sup.Plugin.Payment.ClickPay.Models
{
    public class RefundResult
    {
        public bool Success { get; set; }
        public string RefundTransactionId { get; set; }
        public string Error { get; set; }
    }
}
