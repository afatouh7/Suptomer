using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.Contract
{
    public partial class SupplierContract : BaseEntity, ISoftDeletedEntity
    {
        public string Email { get; set; } 
        public string Contract { get; set; }
        public string WH { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string SKU { get; set; }
        public string StockLV1 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EnterDate { get; set; }
        public string Interval { get; set; }
       
        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }
        /// <summary>
        /// Gets or sets the supplier contract identifier
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets a  of Order associated with this supplier contract
        /// </summary>
        public virtual Order  Order { get; set; }
    }
}
