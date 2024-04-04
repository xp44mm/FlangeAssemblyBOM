using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlangeAssemblyBOM.Scaffold.Models;

public partial class washer
{
    [Key]
    public double M { get; set; }

    public double Di { get; set; }

    public double Dw { get; set; }

    public double t { get; set; }
}
