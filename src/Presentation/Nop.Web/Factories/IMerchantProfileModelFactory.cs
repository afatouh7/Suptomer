using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Web.Models.Merchant;

namespace Nop.Web.Factories;

public interface IMerchantProfileModelFactory
{
    Task<SuptomerMerchantProfileModel> PrepareProfileModel(Customer customer);
}
