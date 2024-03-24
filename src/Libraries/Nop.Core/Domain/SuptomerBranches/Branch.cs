using System;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.SuptomerBranches;
public partial class Branch : BaseEntity, ISoftDeletedEntity
{
    public Branch()
    {
        BranchGuid = Guid.NewGuid();
    }
    public string BranchName { get; set; }
    public string BranchManager { get; set; }
    public string BranchMobileNumber { get; set; }
    public string BranchTimeframeFrom { get; set; }
    public string BranchTimeframeTo { get; set; }
    public string BranchTimeZoneId { get; set; }
    public string BranchLocation { get; set; }
    public Guid BranchGuid { get; }
    public bool Deleted { get; set; }
    public int BranchUserId { get; set; }
}
