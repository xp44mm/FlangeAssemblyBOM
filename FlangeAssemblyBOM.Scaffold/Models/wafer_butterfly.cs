using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlangeAssemblyBOM.Scaffold.Models;

[Table("wafer butterfly")]
public partial class wafer_butterfly
{
    [Key]
    public double DN { get; set; }

    public double Length { get; set; }
}
