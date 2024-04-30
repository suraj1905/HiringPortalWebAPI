using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HiringPortalWebAPI.Models;

public partial class Slot
{
    public int Id { get; set; }

    public int PanelistId { get; set; }

    public DateOnly DateAvailable { get; set; }

    public TimeOnly TimeAvailable { get; set; }

    public bool IsBooked { get; set; }

    [JsonIgnore]
    public virtual Panelist? Panelist { get; set; }
}
