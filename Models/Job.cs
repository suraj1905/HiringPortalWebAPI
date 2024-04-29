using System;
using System.Collections.Generic;

namespace HiringPortalWebAPI.Models;

public partial class Job
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateOnly LastApplyDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
}
