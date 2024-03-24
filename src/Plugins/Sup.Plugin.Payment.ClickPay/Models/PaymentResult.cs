using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sup.Plugin.Payment.ClickPay.Models
{
    public class PaymentResult
    {
        public string response_status { get; set; }
        public string response_code { get; set; }
        public string response_message { get; set; }

        public string acquirer_message { get; set; }
        public string acquirer_rrn
        {
            get; set;
        }
        public DateTime transaction_time { get; set; }
    }
}
