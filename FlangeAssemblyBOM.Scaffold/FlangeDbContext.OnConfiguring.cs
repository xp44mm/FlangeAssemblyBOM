namespace FlangeAssemblyBOM.Scaffold;

using System;
using System.IO;

using Microsoft.EntityFrameworkCore;

using UnquotedJson;

public partial class FlangeDbContext : DbContext
{
    public FlangeDbContext()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "app.config");
        //var text = File.ReadAllText(path);
        //var json = Json.parse.Invoke(text);
        //var connectionString = json["connectionString"].stringText;
        this.ConnectionString = "Data Source=D:/Application Data/flange.db";

    }
    public string ConnectionString { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (optionsBuilder != null) {
            optionsBuilder.UseSqlite(this.ConnectionString);
        }
    }
}
