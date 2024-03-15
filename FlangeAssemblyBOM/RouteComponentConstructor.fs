module FlangeAssemblyBOM.Routing.RouteComponentConstructor
open FlangeAssemblyBOM

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions

//todo:全局获取tsv数据表
let fastenersOfFlange (dn:float) (pn:float) (times:int) =
    let flange, screw = FlangePair.getTbl (pn*10.0) dn
    //垫片厚度
    let gasket = 3.0

    let y =
        flange.C * 2.0 + gasket + screw.t + screw.p * 3.0 + screw.z

    let boltLength = Math.Ceiling(y/5.0)*5.0

    [
        Gasket( dn, pn, times)
        Bolt (flange.M,boltLength,flange.N*times)
        Nut (flange.M,flange.N*times)
    ]

let toroute (node: Json) =
    let rec loop (node: Json) =
        match node.["kind"].stringText with
        | "Pipe" ->
            {
                refid = node.["refid"].stringText
                specific = Pipe(
                    node.["length"].floatValue,
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }
        | "Elbow" ->
            {
                refid = node.["refid"].stringText
                specific = Elbow(
                    node.["angle"].floatValue,
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                    )
            }

        | "Tee" ->
            {
                refid    = node.["refid"].stringText
                specific = Tee(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "Flange" ->
            {
                refid    = node.["refid"].stringText
                specific = Flange(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "Reducer" ->
            {
                refid = node.["refid"].stringText
                specific = Reducer(
                    node.["dn1"].floatValue,
                    node.["dn2"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "EccentricReducer" ->
            {
                refid = node.["refid"].stringText
                specific = EccentricReducer(
                    node.["dn1"].floatValue,
                    node.["dn2"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "ReducingTee" ->
            {
                refid     = node.["refid"].stringText
                specific = ReducingTee(
                    node.["dn1"].floatValue,
                    node.["dn2"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText

                )
            }

        | "BallValve" ->
            {
                refid = node.["refid"].stringText
                specific = BallValve(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "WaferButterflyValve" ->
            {
                refid = node.["refid"].stringText
                specific = WaferButterflyValve(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "WaferCheckValve" ->
            {
                refid = node.["refid"].stringText
                specific = WaferCheckValve(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }
        | "Expansion" ->
            {
                refid = node.["refid"].stringText
                specific = Expansion(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "Flowmeter" ->
            {
                refid     = node.["refid"].stringText
                specific = Flowmeter(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "MagneticFilter" ->
            {
                refid = node.["refid"].stringText
                specific = MagneticFilter(
                    node.["dn"].floatValue,
                    node.["pn"].floatValue,
                    node.["material"].stringText
                )
            }

        | "ComponentPart" ->
            {
                refid = node.["refid"].stringText
                specific = ComponentPart(
                    node.["title"].stringText,
                    node.["refconfig"].stringText
                )
            }

        | "SingleFlange"
        | "Flanges" 
            as tag ->
            let unionTag =
                match tag with
                | "SingleFlange" -> SingleFlange
                | "Flanges" -> Flanges
                | _ -> failwith ""

            let dn = node.["dn"].floatValue
            let pn = node.["pn"].floatValue
            let children =
                node.["children"].elements 
                |> List.map loop

            let fasteners = 
                fastenersOfFlange dn pn 1
                |> List.map (fun specific ->
                    {refid = ""; specific = specific;}
                )

            {
                refid = node.["refid"].stringText
                specific = unionTag [yield! children; yield! fasteners ]
            }

        | "BallValveFlanges"
        | "BallValveSolo"
        | "WaferButterflyValveFlanges"
        | "WaferButterflyValveSolo"
        | "WaferCheckValveFlanges"
        | "ExpansionFlanges"
        | "ExpansionSolo"
        | "FlowmeterFlanges"
        | "MagneticFilterFlanges"
            as tag ->
            let dn = node.["dn"].floatValue
            let pn = node.["pn"].floatValue
            let children =
                node.["children"].elements 
                |> List.map loop

            let fasteners = 
                fastenersOfFlange dn pn 2
                |> List.map (fun specific ->
                    {refid = ""; specific = specific;}
                )
            let unionTag =
                match tag with
                | "BallValveFlanges"            -> BallValveFlanges
                | "BallValveSolo"               -> BallValveSolo
                | "WaferButterflyValveFlanges"  -> WaferButterflyValveFlanges
                | "WaferButterflyValveSolo"     -> WaferButterflyValveSolo
                | "WaferCheckValveFlanges"      -> WaferCheckValveFlanges
                | "ExpansionFlanges"            -> ExpansionFlanges
                | "ExpansionSolo"               -> ExpansionSolo
                | "FlowmeterFlanges"            -> FlowmeterFlanges
                | "MagneticFilterFlanges"       -> MagneticFilterFlanges
                | _ -> failwith ""

            {
                refid = node.["refid"].stringText
                specific = unionTag [yield! children; yield! fasteners ]
            }
        | "ComponentAssembly" ->
            {
                refid    = node.["refid"].stringText
                specific = ComponentAssembly(
                    node.["title"].stringText,
                    node.["refconfig"].stringText,
                    node.["children"].elements |> List.map loop
                    )
            }

        | "RouteAssembly" ->
            {
                refid    = node.["refid"].stringText
                specific = RouteAssembly(
                    node.["title"].stringText,
                    node.["children"].elements |> List.map loop
                    )
            }

        | x -> failwith $"unimpl: {x}"
    loop node

let toLine (this:RouteComponent) =
    match this.specific with
    | RouteAssembly (title:string, children:RouteComponent list) ->
        $"RouteAssembly {title} {{{this.refid}}}" 
    | ComponentAssembly (title:string, refconfig:string, children:RouteComponent list) ->
        $"ComponentAssembly {title}({refconfig}) {{{this.refid}}}"
    | ComponentPart (title:string, refconfig:string) ->
        $"ComponentPart {title}({refconfig}) {{{this.refid}}}"
    | Pipe (length:float, dn:float, pn:float, material:string) ->
        $"Pipe {length}mm DN {dn} PN {pn} {material}"

    | Elbow (angle:float, dn:float, pn:float, material:string) ->
        $"Elbow {angle}degree DN {dn} PN {pn} {material}"

    | Tee (dn:float, pn:float, material:string) ->
        $"Tee DN {dn} PN {pn} {material}"

    | Flange (dn:float, pn:float, material:string) ->
        $"Flange DN {dn} PN {pn} {material}"

    | Reducer (dn1:float, dn2:float, pn:float, material:string) ->
        $"Reducer DN {dn1} x {dn2} PN {pn} {material}"

    | EccentricReducer (dn1:float, dn2:float, pn:float, material:string) ->
        $"EccentricReducer DN {dn1} x {dn2} PN {pn} {material}"

    | ReducingTee (dn1:float, dn2:float, pn:float, material:string) ->
        $"ReducingTee DN {dn1} x {dn2} PN {pn} {material}"

    | BallValve (dn:float, pn:float, material:string) ->
        $"BallValve DN {dn} PN {pn} {material}"

    | WaferButterflyValve (dn:float, pn:float, material:string) ->
        $"WaferButterflyValve DN {dn} PN {pn} {material}"

    | WaferCheckValve (dn:float, pn:float, material:string) ->
        $"WaferCheckValve DN {dn} PN {pn} {material}"

    | Expansion (dn:float, pn:float, material:string) ->
        $"Expansion DN {dn} PN {pn} {material}"

    | Flowmeter (dn:float, pn:float, material:string) ->
        $"Flowmeter DN {dn} PN {pn} {material}"

    | MagneticFilter (dn:float, pn:float, material:string) ->
        $"MagneticFilter DN {dn} PN {pn} {material}"

    | SingleFlange (children:RouteComponent list) ->
        $"SingleFlange"

    | Flanges (children:RouteComponent list) ->
        $"Flanges"

    | BallValveFlanges (children:RouteComponent list) ->
        $"BallValveFlanges"

    | BallValveSolo (children:RouteComponent list) ->
        $"BallValveSolo"

    | WaferButterflyValveFlanges (children:RouteComponent list) ->
        $"WaferButterflyValveFlanges"

    | WaferButterflyValveSolo (children:RouteComponent list) ->
        $"WaferButterflyValveSolo"

    | WaferCheckValveFlanges (children:RouteComponent list) ->
        $"WaferCheckValveFlanges"

    | ExpansionFlanges (children:RouteComponent list) ->
        $"ExpansionFlanges"

    | ExpansionSolo (children:RouteComponent list) ->
        $"ExpansionSolo"

    | FlowmeterFlanges (children:RouteComponent list) ->
        $"FlowmeterFlanges"

    | MagneticFilterFlanges (children:RouteComponent list) ->
        $"MagneticFilterFlanges"

    | Gasket (dn:float, pn:float, count:int) ->
        $"Gasket DN {dn} PN {pn}"

    | Bolt (m:float, l:float, count:int) ->
        $"Bolt M {m} x {l}, {count}"

    | Nut (m:float, count:int) ->
        $"Nut M {m}, {count}"

    | Washer ( m:float, count:int) ->
        $"Washer M {m}, {count}"

    | StudBolt (m:float, l:float, count:int) ->
        $"StudBolt M {m} x {l}, {count}"

let rec tolines (level:int) (data:RouteComponent) =
    let pad = String.replicate (level*3) " "
    [
        yield $"{pad}+{toLine data}"
        match data.specific with
        | RouteAssembly (_, children:RouteComponent list)
        | ComponentAssembly (_, _, children:RouteComponent list)
        | SingleFlange (children:RouteComponent list)
        | Flanges (children:RouteComponent list)
        | BallValveFlanges (children:RouteComponent list)
        | BallValveSolo (children:RouteComponent list)
        | WaferButterflyValveFlanges (children:RouteComponent list)
        | WaferButterflyValveSolo (children:RouteComponent list)
        | WaferCheckValveFlanges (children:RouteComponent list)
        | ExpansionFlanges (children:RouteComponent list)
        | ExpansionSolo (children:RouteComponent list)
        | FlowmeterFlanges (children:RouteComponent list)
        | MagneticFilterFlanges (children:RouteComponent list) ->
            for child in children do
                yield! tolines (level+1) child
        | ComponentPart _
        | Pipe _
        | Elbow _
        | Tee _
        | Flange _
        | Reducer _
        | EccentricReducer _
        | ReducingTee _
        | BallValve _
        | WaferButterflyValve _
        | WaferCheckValve _
        | Expansion _
        | Flowmeter _
        | MagneticFilter _
        | Gasket _
        | Bolt _
        | Nut _
        | Washer _
        | StudBolt _
            -> ()
    ]

let bom (data:RouteComponent) =
    let rec loop (data:RouteComponent) =
        [
            match data.specific with
            | RouteAssembly (_, children:RouteComponent list)
            | ComponentAssembly (_, _, children:RouteComponent list)
            | SingleFlange (children:RouteComponent list)
            | Flanges (children:RouteComponent list)
            | BallValveFlanges (children:RouteComponent list)
            | BallValveSolo (children:RouteComponent list)
            | WaferButterflyValveFlanges (children:RouteComponent list)
            | WaferButterflyValveSolo (children:RouteComponent list)
            | WaferCheckValveFlanges (children:RouteComponent list)
            | ExpansionFlanges (children:RouteComponent list)
            | ExpansionSolo (children:RouteComponent list)
            | FlowmeterFlanges (children:RouteComponent list)
            | MagneticFilterFlanges (children:RouteComponent list) ->
                for child in children do
                    yield! loop child
            | ComponentPart _
            | Pipe _
            | Elbow _
            | Tee _
            | Flange _
            | Reducer _
            | EccentricReducer _
            | ReducingTee _
            | BallValve _
            | WaferButterflyValve _
            | WaferCheckValve _
            | Expansion _
            | Flowmeter _
            | MagneticFilter _
            | Gasket _
            | Bolt _
            | Nut _
            | Washer _
            | StudBolt _
                -> yield data
        ]
    data
    |> loop
    |> List.sort
