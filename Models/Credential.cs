using System;
using System.Collections.Generic;

namespace HiringPortalWebAPI.Models;

public partial class Credential
{
    public int Id { get; set; }

    public int PanelistId { get; set; }

    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Panelist Panelist { get; set; } = null!;
}
