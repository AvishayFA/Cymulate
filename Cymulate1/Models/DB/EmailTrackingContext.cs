using Infrastructrure.Models.DB.Entities;
using Infrastructure.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cymulate1.Models.DB;

public class EmailTrackingContext : DbContext
{
    public EmailTrackingContext(DbContextOptions<EmailTrackingContext> options) : base(options)
    {
    }

    public DbSet<SentEmails> SentEmails { get; }
    public DbSet<LinkClick> LinkClicks { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LinkClick>(entity =>
        {
            entity.ToTable("link_clicks");
            entity.Property(e => e.Timestamp).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<SentEmails>(entity =>
        {
            entity.ToTable("sent_emails");
        });
    }
}