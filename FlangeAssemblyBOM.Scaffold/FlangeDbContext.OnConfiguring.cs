namespace FlangeAssemblyBOM.Scaffold;

using System;
using System.IO;

using Microsoft.EntityFrameworkCore;

using UnquotedJson;

public partial class FlangeDbContext : DbContext
{
    public FlangeDbContext()
    {
        //使用配置文件的代码
        //var path = Path.Combine(Environment.CurrentDirectory, "app.config");
        //var text = File.ReadAllText(path);
        //var json = Json.parse.Invoke(text);
        //var connectionString = json["connectionString"].stringText;
        this.ConnectionString = "Data Source=D:/Application Data/flange.db;Mode=ReadOnly;Cache=Private"; //
    }

    public string ConnectionString { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlite(this.ConnectionString);
        }
    }
}
