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

    public virtual DbSet<Flange> Flange { get; set; }

    public virtual DbSet<ScrewFastener> ScrewFastener { get; set; }

    public virtual DbSet<WaferButterflyValve> WaferButterflyValve { get; set; }

    public virtual DbSet<WaferCheckValve> WaferCheckValve { get; set; }

    public virtual DbSet<Washer> Washer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
