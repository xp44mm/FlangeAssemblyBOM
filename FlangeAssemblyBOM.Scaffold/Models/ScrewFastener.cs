using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlangeAssemblyBOM.Scaffold.Models;

public partial class ScrewFastener
{
    [Key]
    public double M { get; set; }

    public double t { get; set; }

    public double z { get; set; }

    public double p { get; set; }
}
