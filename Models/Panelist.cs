using System;
using System.Collections.Generic;

namespace HiringPortalWebAPI.Models;

public partial class Panelist
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long PhoneNo { get; set; }

    public string Role { get; set; } = null!;

    public string Department { get; set; } = null!;

    public decimal YearsOfExperience { get; set; }

    public string Skills { get; set; } = null!;

    public string Category { get; set; } = null!;

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
