module FlangeAssemblyBOM.Routing.RouteComponentToJson

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions

open FSharp.Idioms.Jsons

open FSharp.Idioms.Literal
open FSharp.Reflection
open FSharp.Idioms.Reflection
open UnquotedJson

let specificToJson (loop) (specific:RouteComponentSpecific) =
    let loopChildren (children:RouteComponent list) =
        children
        |> List.map loop
        |> Json.Array

    match specific with
    | RouteAssembly (title:string, children:RouteComponent list) ->
        let json = Json.Object [
            nameof title, Json.String title; 
            nameof children, loopChildren children
            ]
        "RouteAssembly",json
    | ComponentAssembly (title:string, refconfig:string, children:RouteComponent list) ->
        let json = Json.Object [
            nameof title, Json.String title; 
            nameof refconfig, Json.String refconfig;
            nameof children, loopChildren children
            ]
        "ComponentAssembly",json
    | ComponentPart (title:string, refconfig:string) ->
        let json = Json.Object [
            nameof title, Json.String title; 
            nameof refconfig, Json.String refconfig;
            ]
        "ComponentPart",json
    | Pipe (length:float, dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof length, Json.Number length;
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "Pipe",json
    | Elbow (angle:float, dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof angle, Json.Number angle;
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "Elbow",json
    | Tee (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "Tee",json
    | Flange (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "Flange",json
    | Reducer (dn1:float, dn2:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn1, Json.Number dn1;
            nameof dn2, Json.Number dn2;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "Reducer",json
    | EccentricReducer (dn1:float, dn2:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn1, Json.Number dn1;
            nameof dn2, Json.Number dn2;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "EccentricReducer",json
    | ReducingTee (dn1:float, dn2:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn1, Json.Number dn1;
            nameof dn2, Json.Number dn2;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "ReducingTee",json
    | BallValve (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "BallValve",json
    | WaferButterflyValve (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "WaferButterflyValve",json
    | WaferCheckValve (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "WaferCheckValve",json
    | Expansion (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "Expansion",json
    | Flowmeter (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "Flowmeter",json
    | MagneticFilter (dn:float, pn:float, material:string) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof material, Json.String material;
            ]
        "MagneticFilter",json
    | SingleFlange (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "SingleFlange",json
    | Flanges (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "Flanges",json
    | BallValveFlanges (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "BallValveFlanges",json
    | BallValveSolo (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "BallValveSolo",json
    | WaferButterflyValveFlanges (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "WaferButterflyValveFlanges",json
    | WaferButterflyValveSolo (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "WaferButterflyValveSolo",json
    | WaferCheckValveFlanges (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "WaferCheckValveFlanges",json
    | ExpansionFlanges (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "ExpansionFlanges",json
    | ExpansionSolo (children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "ExpansionSolo",json
    | FlowmeterFlanges(children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "FlowmeterFlanges",json
    | MagneticFilterFlanges(children:RouteComponent list) ->
        let json = Json.Object [
            nameof children, loopChildren children
            ]
        "MagneticFilterFlanges",json
    | Gasket(dn:float, pn:float, count:int) ->
        let json = Json.Object [
            nameof dn, Json.Number dn;
            nameof pn, Json.Number pn;
            nameof count, Json.Number count;
            ]
        "Gasket",json
    | Bolt(m:float, l:float, count:int) ->
        let json = Json.Object [
            nameof m, Json.Number m;
            nameof l, Json.Number l;
            nameof count, Json.Number count;
            ]
        "Bolt",json
    | Nut(m:float, count:int) ->
        let json = Json.Object [
            nameof m, Json.Number m;
            nameof count, Json.Number count;
            ]
        "Nut",json
    | Washer(m:float, count:int) ->
        let json = Json.Object [
            nameof m, Json.Number m;
            nameof count, Json.Number count;
            ]
        "Washer",json
    | StudBolt(m:float, l:float, count:int) ->
        let json = Json.Object [
            nameof m, Json.Number m;
            nameof l, Json.Number l;
            nameof count, Json.Number count;
            ]
        "StudBolt",json

let tryRouteComponent = fun (ty:Type) ->
    if ty = typeof<RouteComponent> then
        Some( fun (loop:Type -> obj -> Json) (value:obj) ->
        let ca = value :?> RouteComponent
        Json.Object [
            "refid", Json.String ca.refid
            specificToJson (loop typeof<RouteComponent>) ca.specific
        ])
    else None

/// fromObj: obj -> Json 
let fromObj: Type -> obj -> Json = 
    JsonReaderApp.mainRead (tryRouteComponent :: JsonReaderApp.readers)

/// from: 't -> Json
let from<'t> (value:'t) = fromObj typeof<'t> value

