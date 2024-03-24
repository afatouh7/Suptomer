using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.SuptomerBranches;

namespace Nop.Services.SuptomerBranshes;
public partial interface IBranchService
{
    Task<List<Branch>> GetAllBranchesAsync(int userId);
    Task<Branch> GetBrancheByIdAsync(int branchId);
    Task AddBranchAsync(Branch branch);
    Task UpdateBranchAsync(Branch branch);
    Task DeleteBranchAsync(int branchId);
    Task<bool> FindByNameAsync(string branchName, int branchId);
}

