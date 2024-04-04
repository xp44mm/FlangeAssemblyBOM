module FlangeAssemblyBOM.Routing.RouteComponentApp

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions

//let rec tojson (route:RouteComponent) =
//    match route.specific with
//    | RouteAssembly (title:string, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "RouteAssembly"
//            nameof title, Json.String title
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | ComponentAssembly (title:string, refconfig:string, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "ComponentAssembly"
//            nameof title, Json.String title
//            nameof refconfig, Json.String refconfig
//            nameof children, Json.Array (List.map tojson children)
//        ]

//    | ComponentPart (title:string, refconfig:string) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "ComponentPart"
//            nameof title, Json.String title
//            nameof refconfig, Json.String refconfig
//        ]
//    | Pipe (dn:float, length:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Pipe"
//            nameof dn, Json.Number dn
//            nameof length, Json.Number length
//        ]

//    | Elbow (dn:float, angle:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Elbow"
//            nameof dn, Json.Number dn
//            nameof angle, Json.Number angle
//        ]

//    | Tee (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Tee"
//            nameof dn, Json.Number dn
//        ]

//    | Flange (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Flange"
//            nameof dn, Json.Number dn
//        ]

//    | Reducer (dn1:float, dn2:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Reducer"
//            nameof dn1, Json.Number dn1
//            nameof dn2, Json.Number dn2
//        ]

//    | EccentricReducer (dn1:float, dn2:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "EccentricReducer"
//            nameof dn1, Json.Number dn1
//            nameof dn2, Json.Number dn2
//        ]

//    | ReducingTee (dn1:float, dn2:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "ReducingTee"
//            nameof dn1, Json.Number dn1
//            nameof dn2, Json.Number dn2
//        ]

//    | BallValve (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "BallValve"
//            nameof dn, Json.Number dn
//        ]

//    | WaferButterflyValve (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "WaferButterflyValve"
//            nameof dn, Json.Number dn
//        ]
//    | WaferCheckValve (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "WaferCheckValve"
//            nameof dn, Json.Number dn
//        ]
//    | Expansion (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Expansion"
//            nameof dn, Json.Number dn
//        ]
//    | Flowmeter (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Flowmeter"
//            nameof dn, Json.Number dn
//        ]
//    | MagneticFilter (dn:float) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "MagneticFilter"
//            nameof dn, Json.Number dn
//        ]

//    | SingleFlange (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "SingleFlange"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]

//    | Flanges (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "Flanges"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]

//    | BallValveFlanges (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "BallValveFlanges"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | BallValveSolo (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "BallValveSolo"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | WaferButterflyValveFlanges (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "WaferButterflyValveFlanges"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | WaferButterflyValveSolo (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "WaferButterflyValveSolo"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | WaferCheckValveFlanges (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "WaferCheckValveFlanges"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | ExpansionFlanges (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "ExpansionFlanges"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | ExpansionSolo (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "ExpansionSolo"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | FlowmeterFlanges (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "FlowmeterFlanges"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]
//    | MagneticFilterFlanges (dn:float, children:RouteComponent list) ->
//        Json.Object [
//            nameof route.refid, Json.String route.refid
//            "kind", Json.String "MagneticFilterFlanges"
//            nameof dn, Json.Number dn
//            nameof children, Json.Array (List.map tojson children)
//        ]

