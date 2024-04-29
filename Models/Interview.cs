using System;
using System.Collections.Generic;

namespace HiringPortalWebAPI.Models;

public partial class Interview
{
    public int Id { get; set; }

    public int PanelistId { get; set; }

    public int JobId { get; set; }

    public int CandidateId { get; set; }

    public DateOnly InterviewDate { get; set; }

    public TimeOnly InterviewTime { get; set; }

    public string InterviewType { get; set; } = null!;

    public int Duration { get; set; }

    public string Status { get; set; } = null!;

    public string Feedback { get; set; } = null!;

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual Job Job { get; set; } = null!;

    public virtual Panelist Panelist { get; set; } = null!;
}
