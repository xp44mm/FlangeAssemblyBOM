module FlangeAssemblyBOM.Routing.Bill
open FSharp.Idioms

let getKeyValue (data:RouteComponent) =
    match data.specific with
    | Gasket (dn:float, pn:float, count:int) -> Gasket (dn, pn, 0), count
    | Bolt (m:float, l:float, count:int) -> Bolt (m, l, 0),count
    | Nut (m:float, count:int) -> Nut (m, 0),count
    | Washer ( m:float, count:int) -> Washer (m, 0),count
    | StudBolt (m:float, l:float, count:int) -> StudBolt (m, l, 0),count
    | Pipe (length:float, dn:float, pn:float, material:string) -> Pipe (0, dn, pn, material),int length

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
    | ComponentPart _
        as part -> part, 1

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
    | MagneticFilterFlanges (children:RouteComponent list)
        -> failwith "never assembly."

let aggregate (ls:RouteComponent list) =
    ls
    |> List.map(fun rc -> getKeyValue rc )
    |> List.groupBy fst
    |> List.map(fun (k,vls) ->
        vls
        |> List.map snd
        |> List.sum
        |> Pair.ofApp k
    )
