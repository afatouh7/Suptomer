using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Web.Models.Contract
{
    public partial class SupplierContract : BaseEntity, ILocalizedEntity, ISlugSupported, IStoreMappingSupported, ISoftDeletedEntity
    {
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
        /// Gets or sets a collection of products associated with this supplier contract
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }
    }
}
