using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sup.Plugin.Merchant.Areas.Admin.Models
{
    public class ContractModel
    {
        public int ID { get; set; }
        public string Contract { get; set; }
        public string WH { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string SKU { get; set; }
        public string StockLV1 { get; set; }

    }
}
