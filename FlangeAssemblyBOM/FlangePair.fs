module FlangeAssemblyBOM.FlangePair
open System

//垫片厚度
let gasket = 3.0

//法兰表，和紧固件表,连接(join)
let getTbl pn dn =
    let flange =
        FlangeRow.fromTsv()
        |> Array.find(fun x -> x.PN = pn && x.DN = dn)

    let m = flange.M

    let screw =
        ScrewFastener.fromTsv()
        |> Array.find(fun x -> x.M = m)
    flange,screw

//计算用于连接法兰的螺栓长度
let boltLength pn dn =    
    let flange, screw = getTbl pn dn
    let y =
        flange.C * 2.0 + gasket + screw.t + screw.p * 3.0 + screw.z

    Math.Ceiling(y/5.0)*5.0
