using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HiringPortalWebAPI.Models;

public partial class Candidate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int PhoneNo { get; set; }

    public decimal YearsOfExperience { get; set; }

    public string Skills { get; set; } = null!;

    public string Resume { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
}
