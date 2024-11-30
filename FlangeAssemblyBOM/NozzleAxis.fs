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

type NozzleAxis = 
    {
        root:float*float*float // sw m
        face:float*float*float // sw m

    }
    /// 标高m
    member this.elevation = 
        let _,y,_ = this.root
        y

    /// 生根点水平方向角
    member this.direction = 
        let x,_,z = this.root
        let c = Complex(x,z)
        c.Phase / Math.PI * 180.

    member this.rootToFace = 
        let x0,y0,z0 = this.root
        let x1,y1,z1 = this.face
        x1-x0,y1-y0,z1-z0

    /// 倾斜角
    member this.slope =
        let x,y,z = this.rootToFace
        if y = 0. then
            0.
        elif x = 0. && z = 0. then
            90.
        else
            let proj = Complex(x,z).Magnitude // 水平投影长度
            let c = Complex(proj,y)
            c.Phase / Math.PI * 180.

    /// 法兰轴线水平方向角
    member this.AxisDirection =
        let x,y,z = this.rootToFace
        if x = 0. && z = 0. then
            this.direction
        else
            let c = Complex(x,z)
            c.Phase / Math.PI * 180.


