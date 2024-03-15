module FlangeAssemblyBOM.Wafer
open System

// 垫片厚度
let gasket = 3.0

//join data table
let getTbl pn dn =
    let flange =
        FlangeRow.fromTsv()
        |> Array.find(fun x -> x.PN = pn && x.DN = dn)
    let m = flange.M

    let fasteners =
        ScrewFastener.fromTsv()
        |> Array.find(fun x -> x.M = m)

    let washer =
        Washer.fromTsv()
        |> Array.find(fun x -> x.M = m)

    flange,fasteners,washer

///螺柱长度
let studLength pn dn =
    let flange,thread,washer = getTbl pn dn
    //单侧螺柱伸出长度
    let extensionlength =
        gasket + flange.C + thread.t + washer.t + thread.p * 3.0 + thread.z
    //中间对夹的长度
    fun wafer ->
        let y =
            wafer + extensionlength * 2.0

        Math.Ceiling(y/5.0)*5.0
