using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlangeAssemblyBOM.Scaffold.Models;

public partial class WaferCheckValve
{
    [Key]
    public double DN { get; set; }

    public double Length { get; set; }
}
