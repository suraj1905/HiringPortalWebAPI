using System;
using System.Collections.Generic;

namespace HiringPortalWebAPI.Models;

public partial class Slot
{
    public int Id { get; set; }

    public int PanelistId { get; set; }

    public DateOnly DateAvailable { get; set; }

    public TimeOnly TimeAvailable { get; set; }

    public virtual Panelist Panelist { get; set; } = null!;
}
