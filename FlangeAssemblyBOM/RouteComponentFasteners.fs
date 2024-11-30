module FlangeAssemblyBOM.Routing.RouteComponentFasteners

open FlangeAssemblyBOM

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions

open FlangeAssemblyBOM.Scaffold

open FSharp.Idioms
open FSharp.Idioms.Literal

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions
open FSharp.Idioms.OrdinalIgnoreCase

/// 分区为法兰，非法兰
let flangesAndElse (children:RouteComponent list) =
    children
    |> List.partition(fun child ->
        match child.specific with
        | Flange _ -> true
        | _ -> false
    )

//一套对法兰的紧固件，companion flange
let getFlangedFasteners (flange:RouteComponentSpecific) =
    let dn,pn =
        match flange with
        | Flange(dn,pn,material) -> dn,pn
        | _ -> failwith "Only for case<Flange>."

    use context = new FlangeDbContext()
    let flange = context.Flange.Find(pn,dn) // 复合主键(Composite Primary Key)注意顺序正确
    let screw = 
        context.ScrewFastener.Find(flange.M)
    let len = Screw.boltLength flange screw

    [
        Gasket(dn,pn,1)
        Nut(flange.M,flange.N)
        Bolt(flange.M,len,flange.N)
    ]
    |> List.map(fun specific -> { refid = ""; specific = specific })

let getWaferFasteners (wafer:RouteComponentSpecific) =
    let dn,pn =
        match wafer with
        | WaferButterflyValve (dn,pn,material)
        | WaferCheckValve (dn,pn,material) 
            -> dn,pn
        | _ -> failwith "never"

    use context = new FlangeDbContext()
    let flange = context.Flange.Find(pn,dn)
    let screw = context.ScrewFastener.Find(flange.M)
    let washer = context.Washer.Find(flange.M)

    let fn = Screw.studLength flange screw washer
    let waferlen =
        match wafer with
        | WaferButterflyValve _ ->
            context.WaferButterflyValve.Find(dn).Length
        | WaferCheckValve _ ->
            context.WaferCheckValve.Find(dn).Length
        | _ -> failwith "never"
    let len = fn waferlen
    [
        Gasket(dn,pn,2)
        Nut(screw.M,flange.N*2)
        Washer(screw.M,flange.N*2)
        StudBolt(screw.M,len,flange.N)
    ]
    |> List.map(fun specific -> { refid = ""; specific = specific })

//给法兰装配体加上紧固件
let appendFasteners (rc:RouteComponent) =
    let rec loop (rc:RouteComponent) =
        match rc.specific with
        | RouteAssembly (title:string, children:RouteComponent list) ->
            {
                refid = rc.refid
                specific = RouteAssembly (title, children |> List.map (loop))
            }
        | ComponentAssembly (title:string, refconfig:string, children:RouteComponent list) ->
            {
                refid = rc.refid
                specific = ComponentAssembly (title, refconfig, children |> List.map (loop))
            }

        | SingleFlange (children:RouteComponent list) ->
            let fl = children |> flangesAndElse |> fst |> List.head

            let children = [
                yield! children
                yield! getFlangedFasteners fl.specific
                ]
            {
                refid = rc.refid
                specific = SingleFlange children
            }

        | Flanges (children:RouteComponent list) ->
            let fl = children |> flangesAndElse |> fst |> List.head
            let children = [
                yield! children
                yield! getFlangedFasteners fl.specific
                ]
            {
                refid = rc.refid
                specific = Flanges children
            }

        | BallValveFlanges (children:RouteComponent list) ->
            let fl = children |> flangesAndElse |> fst |> List.head

            let x = getFlangedFasteners fl.specific
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
            let fl = children |> flangesAndElse |> fst |> List.head

            let children = [
                yield! children
                yield! getFlangedFasteners fl.specific
                ]
            {
                refid = rc.refid
                specific = BallValveSolo children
            }

        | WaferButterflyValveFlanges (children:RouteComponent list) ->
            let v = children |> flangesAndElse |> snd |> List.head
            let children = [
                yield! children
                yield! getWaferFasteners v.specific
            ]
            {
                refid = rc.refid
                specific = WaferButterflyValveFlanges children
            }

        | WaferButterflyValveSolo (children:RouteComponent list) ->
            let v = children |> flangesAndElse |> snd |> List.head
            let children = [
                yield! children
                yield! getWaferFasteners v.specific
            ]
            {
                refid = rc.refid
                specific = WaferButterflyValveSolo children
            }

        | WaferCheckValveFlanges (children:RouteComponent list) ->
            let v = children |> flangesAndElse |> snd |> List.head
            let children = [
                yield! children
                yield! getWaferFasteners v.specific
            ]
            {
                refid = rc.refid
                specific = WaferCheckValveFlanges (children)
            }

        | ExpansionFlanges (children:RouteComponent list) ->
            let fl = children |> flangesAndElse |> fst |> List.head
            let x = getFlangedFasteners fl.specific
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
            let fl = children |> flangesAndElse |> fst |> List.head

            let children = [
                yield! children
                yield! getFlangedFasteners fl.specific
            ]
            {
                refid = rc.refid
                specific = ExpansionSolo (children)
            }

        | FlowmeterFlanges (children:RouteComponent list) ->
            let fl = children |> flangesAndElse |> fst |> List.head

            let x = getFlangedFasteners fl.specific
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
            let fl = children |> flangesAndElse |> fst |> List.head

            let x = getFlangedFasteners fl.specific
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
    loop rc
