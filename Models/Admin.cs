using System;

namespace HiringPortalWebAPI.Models;

public partial class Admin
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

}
