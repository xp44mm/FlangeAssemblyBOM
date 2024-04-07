module FlangeAssemblyBOM.Screw

open System
open FlangeAssemblyBOM.Scaffold
open FlangeAssemblyBOM.Scaffold.Models

// 垫片厚度
let gasket = 3.0

let ceiling5 y = Math.Ceiling(y/5.0)*5.0

//计算用于连接法兰的螺栓长度
let boltLength (flange:Flange) (screw:ScrewFastener) =

    flange.C * 2.0 + gasket + screw.t + screw.p * 3.0 + screw.z
    |> ceiling5

///螺柱长度
let studLength (flange:Flange) (screw:ScrewFastener) ( washer:Washer) =

    //单侧螺柱伸出长度
    let extensionlength =
        gasket + flange.C + screw.t + washer.t + screw.p * 3.0 + screw.z

    //中间对夹的长度
    fun wafer ->
        wafer + extensionlength * 2.0
        |> ceiling5
