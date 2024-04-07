using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlangeAssemblyBOM.Scaffold.Models;

[PrimaryKey("PN", "DN")]
public partial class Flange
{
    [Key]
    public double PN { get; set; }

    [Key]
    public double DN { get; set; }

    public double Di1 { get; set; }

    public double Di2 { get; set; }

    public double Dw { get; set; }

    public double DK { get; set; }

    public double C { get; set; }

    public double DS { get; set; }

    public double HS { get; set; }

    public double H { get; set; }

    public double M { get; set; }

    public int N { get; set; }
}
