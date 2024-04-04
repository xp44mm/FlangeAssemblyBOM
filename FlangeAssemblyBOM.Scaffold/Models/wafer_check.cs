using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlangeAssemblyBOM.Scaffold.Models;

[Table("wafer check")]
public partial class wafer_check
{
    [Key]
    public double DN { get; set; }

    public double Length { get; set; }
}
