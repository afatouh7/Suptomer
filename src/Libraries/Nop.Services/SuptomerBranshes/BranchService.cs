using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.SuptomerBranches;
using Nop.Data;

namespace Nop.Services.SuptomerBranshes;

public partial class BranchService : IBranchService
{
    private readonly IRepository<Branch> _repository;

    public BranchService(IRepository<Branch> repository)
    {
        _repository = repository;
    }
    public async Task<List<Branch>> GetAllBranchesAsync(int userId)
        => await _repository.Table.Where(b => b.BranchUserId == userId && b.Deleted == false).ToListAsync();


    public async Task<Branch> GetBrancheByIdAsync(int branchId)
        => await _repository.Table.FirstOrDefaultAsync(b => b.Id == branchId && b.Deleted == false);


    public async Task AddBranchAsync(Branch branch)
        => await _repository.InsertAsync(branch);


    public async Task UpdateBranchAsync(Branch branch)
     => await _repository.UpdateAsync(branch);


    public async Task DeleteBranchAsync(int branchId)
     => await _repository.DeleteAsync(b => b.Id == branchId);

    public async Task<bool> FindByNameAsync(string branchName, int branchId)
        => await _repository.Table
        .AnyAsync(b => b.BranchName == branchName &&
                       (branchId == 0 || b.Id != branchId));
}

