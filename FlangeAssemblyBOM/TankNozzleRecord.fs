namespace FlangeAssemblyBOM

open FlangeAssemblyBOM
open FlangeAssemblyBOM.Scaffold

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal
open FSharp.Idioms.OrdinalIgnoreCase

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions
open System.Numerics

type TankNozzleRecord =
    {    
        file:string
        name:string
        config:string
        rootx:float
        rooty:float
        rootz:float
        facex:float
        facey:float
        facez:float
        elev:float
        slope:float
        rootDirection: float
        axisDirection: float
    }

    static member Title = [|"FILE";"NAME";"DN";"RTX";"RTY";"RTZ";"FCX";"FCY";"FCZ";"ELEV";"SLP";"DRT";"AXIS"|]

    /// 圆整小数并且消去尾零
    member row.toStringArray() = 
        let sfloat (f:float) =
            let s = f.ToString("0.####").TrimEnd('.')
            if s = "-0" then
                "0"
            else s

        [|
            row.file;
            row.name;
            row.config;
            sfloat row.rootx;
            sfloat row.rooty;
            sfloat row.rootz;
            sfloat row.facex;
            sfloat row.facey;
            sfloat row.facez;
            sfloat row.elev;
            sfloat row.slope;
            sfloat row.rootDirection;
            sfloat row.axisDirection;
        |]
//        using System;
 
//public class Program
//{
//    public static void Main()
//    {
//        decimal number = 123.4500m;
//        Console.WriteLine(NormalizeDecimal(number));
//    }
 
//    public static string NormalizeDecimal(decimal value)
//    {
//        return value.ToString();
//    }
//}
