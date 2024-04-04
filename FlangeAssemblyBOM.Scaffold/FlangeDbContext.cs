using System;
using System.Collections.Generic;
using FlangeAssemblyBOM.Scaffold.Models;
using Microsoft.EntityFrameworkCore;

namespace FlangeAssemblyBOM.Scaffold;

public partial class FlangeDbContext : DbContext
{
    public FlangeDbContext(DbContextOptions<FlangeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<flange> flange { get; set; }

    public virtual DbSet<screw_fastener> screw_fastener { get; set; }

    public virtual DbSet<wafer_butterfly> wafer_butterfly { get; set; }

    public virtual DbSet<wafer_check> wafer_check { get; set; }

    public virtual DbSet<washer> washer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
