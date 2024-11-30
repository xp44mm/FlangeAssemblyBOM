namespace FlangeAssemblyBOM.Scaffold;

using System;
using System.IO;

using Microsoft.EntityFrameworkCore;

using UnquotedJson;

public partial class FlangeDbContext : DbContext
{
    public FlangeDbContext()
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
            var connectionString = "Data Source=D:/Application Data/flange.db;Mode=ReadOnly;Cache=Private";
            optionsBuilder.UseSqlite(connectionString);
        }
    }

    /// 使用配置文件读取字符串
    private string GetConnectionString()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "app.config");
        var text = File.ReadAllText(path);
        var json = Json.parse.Invoke(text);
        return json["connectionString"].stringText;
    }
}
