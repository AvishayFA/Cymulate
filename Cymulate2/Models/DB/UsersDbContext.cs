using Cymulate2.Models.Entities;
using Infrastructrure.Models.DB.Entities;
using Infrastructure.DB.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cymulate2.Models.DB;

public class UsersDbContext : IdentityDbContext<ApplicationUser>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    public DbSet<LinkClick> LinkClicks { get; set; }
    public DbSet<SentEmails> SentEmails { get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
        });

        builder.Entity<LinkClick>(entity =>
        {
            entity.ToTable("link_clicks");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.Timestamp);
        });

        builder.Entity<SentEmails>(entity =>
        {
            entity.ToTable("sent_emails");
        });
    }
}