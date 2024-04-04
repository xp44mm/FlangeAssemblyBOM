module FlangeAssemblyBOM.Routing.RouteComponentFromJson

open FlangeAssemblyBOM

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions


open FSharp.Idioms
open FSharp.Idioms.Literal

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions
open FSharp.Idioms.OrdinalIgnoreCase

open ValueParser

let getFlangedFasteners (flange:RouteComponentSpecific) =
    let dn,pn =
        match flange with
        | Flange(dn,pn,material) -> dn,pn
        | _ -> failwith "never"

    [
        //flange
        Gasket(dn,pn,1)
        Nut(0,0)
        Bolt(0,0,0)
    ]
    |> List.map(fun specific -> { refid = ""; specific = specific })

let getWaferFasteners (wafer:RouteComponentSpecific) =
    let dn,pn,len =
        match wafer with
        | WaferButterflyValve (dn,pn,material)
        | WaferCheckValve (dn,pn,material) 
            -> dn,pn,0.0
        | _ -> failwith "never"

    [
        Gasket(dn,pn,2)
        Nut(0,0)
        Washer(0,0)
        StudBolt(0,0,0)
    ]
    |> List.map(fun specific -> { refid = ""; specific = specific })

let getPN (props:Json) =
    if props.hasProperty "pn" then
        props.["pn"].floatValue
    else 1.0

let getMaterial (props:Json) = 
    if props.hasProperty "material" then
        props.["material"].stringText
    else ""
    
let toroute (node: Json) =
    let rec loop (data: Json) =
        let title = data.["title"].stringText
        let refconfig = data.["refconfig"].stringText
        let refid     = data.["refid"].stringText
        let isVirtual = data.["isVirtual"] = Json.True
        let props     = data.["props"]

        let loopChildren (children:Json) = 
            children.elements
            |> List.map(fun child -> loop child)
        
        match title with
        | IgnoreCase "elbow LR.SLDPRT" ->
            let angle,dn =
                parseElbow refconfig
            let pn = getPN props
            let material = getMaterial props
            {
                refid = refid
                specific = Elbow(angle,dn,pn,material)
            }

        | IgnoreCase "straight tee.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props
            {
                refid = refid
                specific = Tee(dn,pn,material)
            }

        | IgnoreCase "flange.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props
            {
                refid = refid
                specific = Flange(dn,pn,material)
            }

        | IgnoreCase "flange with fasteners.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props
            {
                refid = refid
                specific = SingleFlange([{refid = refid;specific = Flange(dn,pn,material)}])
            }

        | IgnoreCase "reducer.SLDPRT" ->
            let dn1,dn2 =
                parseDNxDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = Reducer(dn1,dn2,pn,material)
            }

        | IgnoreCase "eccentric reducer.SLDPRT" ->
            let dn1,dn2 =
                parseDNxDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = EccentricReducer(dn1,dn2,pn,material)
            }

        | IgnoreCase "reducing outlet tee.SLDPRT" ->
            let dn1,dn2 =
                parseDNxDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = ReducingTee(dn1,dn2,pn,material)
            }

        | IgnoreCase "ball valve.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props
            {
                refid = refid
                specific = BallValve(dn,pn,material)
            }

        | IgnoreCase "wafer butterfly valve.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = WaferButterflyValve(dn,pn,material)
            }

        | IgnoreCase "wafer check valve.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = WaferCheckValve(dn,pn,material)
            }

        | IgnoreCase "expansion joint.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = Expansion(dn,pn,material)
            }

        | IgnoreCase "flowmeter.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = Flowmeter(dn,pn,material)
            }

        | IgnoreCase "magnetic filter.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props

            {
                refid = refid
                specific = MagneticFilter(dn,pn,material)
            }

        | IgnoreCase "single flanged joint.SLDASM" ->
            {
                refid = refid
                specific = SingleFlange(loopChildren data.["Assembly"])
            }

        | IgnoreCase "flanges.SLDASM" ->
            {
                refid = refid
                specific = Flanges(loopChildren data.["Assembly"])
            }
        | IgnoreCase "ball valve flanges.SLDASM" ->
            {
                refid = refid
                specific = BallValveFlanges(loopChildren data.["Assembly"])
            }
        | IgnoreCase "ball valve solo.SLDASM" ->
            {
                refid = refid
                specific = BallValveSolo(loopChildren data.["Assembly"])
            }
        | IgnoreCase "wafer butterfly valve flanges.SLDASM" ->
            {
                refid = refid
                specific = WaferButterflyValveFlanges(loopChildren data.["Assembly"])
            }

        | IgnoreCase "wafer butterfly valve solo.SLDASM" ->
            {
                refid = refid
                specific = WaferButterflyValveSolo(loopChildren data.["Assembly"])
            }

        | IgnoreCase "wafer check valve flanges.SLDASM" ->
            {
                refid = refid
                specific = WaferCheckValveFlanges(loopChildren data.["Assembly"])
            }

        | IgnoreCase "expansion joint flanges.SLDASM" ->
            {
                refid = refid
                specific = ExpansionFlanges(loopChildren data.["Assembly"])
            }

        | IgnoreCase "expansion joint solo.SLDASM" ->
            {
                refid = refid
                specific = ExpansionSolo(loopChildren data.["Assembly"])
            }

        | IgnoreCase "flowmeter flanges.SLDASM" ->
            {
                refid = refid
                specific = FlowmeterFlanges(loopChildren data.["Assembly"])
            }

        | IgnoreCase "magnetic filter flanges.SLDASM" ->
            {
                refid = refid
                specific = MagneticFilterFlanges(loopChildren data.["Assembly"])
            }
        | _ ->
            if data.hasProperty "Part" then
                if isCompTypeOf "Pipe" props then
                    let dn =
                        props.["Pipe Identifier"].stringText
                        |> parseDN
                    let length =
                        props.["Length"].stringText
                        |> parseLength
                    let pn = getPN props
                    let material = getMaterial props

                    {
                        refid = refid
                        specific = Pipe(length,dn,pn,material)
                    }
                elif isCompTypeOf "Elbow" props then
                    let angle,dn =
                        parseElbow refconfig
                    let pn = getPN props
                    let material = getMaterial props
                    {
                        refid = refid
                        specific = Elbow(angle,dn,pn,material)
                    }
                else
                    {
                        refid = refid
                        specific = ComponentPart(title,refconfig)
                    }
            elif data.hasProperty "Assembly" then
                {
                    refid = refid
                    specific = ComponentAssembly(title,refconfig, loopChildren data.["Assembly"])
                }
            elif data.hasProperty "RouteAssembly" then
                {
                    refid = refid
                    specific = RouteAssembly(title, loopChildren data.["RouteAssembly"])
                }
            else failwith "never"

    loop node

let rec fasteners (rc:RouteComponent) =
    match rc.specific with
    | RouteAssembly (title:string, children:RouteComponent list) ->
        {
            refid = rc.refid
            specific = RouteAssembly (title, children |> List.map fasteners)
        }
    | ComponentAssembly (title:string, refconfig:string, children:RouteComponent list) ->
        {
            refid = rc.refid
            specific = ComponentAssembly (title, refconfig, children |> List.map fasteners)
        }
    | SingleFlange (children:RouteComponent list) ->
        let children = [
            yield! children
            yield! getFlangedFasteners children.[0].specific
        ]
        {
            refid = rc.refid
            specific = SingleFlange children
        }

    | Flanges (children:RouteComponent list) ->
        let children = [
            yield! children
            yield! getFlangedFasteners children.[0].specific
        ]
        {
            refid = rc.refid
            specific = Flanges children
        }

    | BallValveFlanges (children:RouteComponent list) ->
        let x = getFlangedFasteners children.[1].specific
        let children = [
            yield! children
            yield! x
            yield! x
        ]
        {
            refid = rc.refid
            specific = BallValveFlanges children
        }

    | BallValveSolo (children:RouteComponent list) ->
        let children = [
            yield! children
            yield! getFlangedFasteners children.[1].specific
        ]
        {
            refid = rc.refid
            specific = BallValveSolo children
        }

    | WaferButterflyValveFlanges (children:RouteComponent list) ->
        let x = getFlangedFasteners children.[1].specific
        let children = [
            yield! children
            yield! x
            yield! x
        ]
        {
            refid = rc.refid
            specific = WaferButterflyValveFlanges children
        }

    | WaferButterflyValveSolo (children:RouteComponent list) ->
        let children = [
            yield! children
            yield! getFlangedFasteners children.[1].specific
        ]
        {
            refid = rc.refid
            specific = WaferButterflyValveSolo children
        }

    | WaferCheckValveFlanges (children:RouteComponent list) ->
        let x = getFlangedFasteners children.[1].specific
        let children = [
            yield! children
            yield! x
            yield! x
        ]
        {
            refid = rc.refid
            specific = WaferCheckValveFlanges (children)
        }

    | ExpansionFlanges (children:RouteComponent list) ->
        let x = getFlangedFasteners children.[1].specific
        let children = [
            yield! children
            yield! x
            yield! x
        ]
        {
            refid = rc.refid
            specific = ExpansionFlanges (children)
        }

    | ExpansionSolo (children:RouteComponent list) ->
        let children = [
            yield! children
            yield! getFlangedFasteners children.[1].specific
        ]
        {
            refid = rc.refid
            specific = ExpansionSolo (children)
        }

    | FlowmeterFlanges (children:RouteComponent list) ->
        let x = getFlangedFasteners children.[1].specific
        let children = [
            yield! children
            yield! x
            yield! x
        ]
        {
            refid = rc.refid
            specific = FlowmeterFlanges (children)
        }

    | MagneticFilterFlanges (children:RouteComponent list) ->
        let x = getFlangedFasteners children.[1].specific
        let children = [
            yield! children
            yield! x
            yield! x
        ]

        {
            refid = rc.refid
            specific = MagneticFilterFlanges (children)
        }

    | _ -> rc

