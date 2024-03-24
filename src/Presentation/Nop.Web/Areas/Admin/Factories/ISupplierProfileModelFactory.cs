using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Suppliers;

namespace Nop.Web.Areas.Admin.Factories;

public interface ISupplierProfileModelFactory
{
    Task<SuptomerSupplierProfileModel> PrepareProfileModel(Customer customer);
}
