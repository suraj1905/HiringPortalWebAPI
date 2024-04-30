using System;
using System.Collections.Generic;
using HiringPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace HiringPortalWebAPI.Data;

public partial class HiringPortalDbContext : DbContext
{

    private readonly IConfiguration _configuration;

    public HiringPortalDbContext(DbContextOptions<HiringPortalDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Credential> Credentials { get; set; }

    public virtual DbSet<Interview> Interviews { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Panelist> Panelists { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ProdDB"),builder =>
        {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });

     } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("candidates_pkey");

            entity.ToTable("candidates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNo).HasColumnName("phone_no");
            entity.Property(e => e.Resume).HasColumnName("resume");
            entity.Property(e => e.Skills)
                .HasMaxLength(100)
                .HasColumnName("skills");
            entity.Property(e => e.YearsOfExperience)
                .HasPrecision(3, 1)
                .HasColumnName("years_of_experience");
        });

        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("credentials_pkey");

            entity.ToTable("credentials");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PanelistId).HasColumnName("panelist_id");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Panelist).WithMany(p => p.Credentials)
                .HasForeignKey(d => d.PanelistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credentials_panelist_id_fkey");
        });

        modelBuilder.Entity<Interview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("interviews_pkey");

            entity.ToTable("interviews");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CandidateId).HasColumnName("candidate_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Feedback).HasColumnName("feedback");
            entity.Property(e => e.InterviewDate).HasColumnName("interview_date");
            entity.Property(e => e.InterviewTime).HasColumnName("interview_time");
            entity.Property(e => e.InterviewType)
                .HasMaxLength(10)
                .HasColumnName("interview_type");
            entity.Property(e => e.JobId).HasColumnName("job_id");
            entity.Property(e => e.PanelistId).HasColumnName("panelist_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Candidate).WithMany(p => p.Interviews)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("interviews_candidate_id_fkey");

            entity.HasOne(d => d.Job).WithMany(p => p.Interviews)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("interviews_job_id_fkey");

            entity.HasOne(d => d.Panelist).WithMany(p => p.Interviews)
                .HasForeignKey(d => d.PanelistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("interviews_panelist_id_fkey");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jobs_pkey");

            entity.ToTable("jobs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastApplyDate).HasColumnName("last_apply_date");
        });

        modelBuilder.Entity<Panelist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("panelists_pkey");

            entity.ToTable("panelists");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNo).HasColumnName("phone_no");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Skills)
                .HasMaxLength(100)
                .HasColumnName("skills");
            entity.Property(e => e.YearsOfExperience)
                .HasPrecision(3, 1)
                .HasColumnName("years_of_experience");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("slots_pkey");

            entity.ToTable("slots");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateAvailable).HasColumnName("date_available");
            entity.Property(e => e.PanelistId).HasColumnName("panelist_id");
            entity.Property(e => e.TimeAvailable).HasColumnName("time_available");
            entity.Property(e => e.IsBooked).HasColumnName("is_booked");


            entity.HasOne(d => d.Panelist).WithMany(p => p.Slots)
                .HasForeignKey(d => d.PanelistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("slots_panelist_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
