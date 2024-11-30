module FlangeAssemblyBOM.Routing.RouteComponentFromJson

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

open ValueParser

let getName2 (json:Json) = json.["name2"].stringText
let getTitle (json:Json) = json.["title"].stringText
let getRefconfig (json:Json) = json.["refconfig"].stringText
let getRefid (json:Json) = json.["refid"].stringText
let isVirtual (json:Json) = json.["isVirtual"] = Json.True
let getProps (json:Json) = json.["props"]
let getKind (json:Json) = json.["kind"].stringText
let getChildren (json:Json) = json.["children"].elements

  //"name2": "总装配",
  //"title": "总装配.SLDASM",
  //"refconfig": "",
  //"refid": "",
  //"isVirtual": false,
  //"props": {},
  //"kind": "Assembly",
  //"children": [


/// 对夹阀门装配体带一套对夹法兰紧固件。无论两片法兰，还是一片法兰。
/// 法兰阀门装配体，有几片配对法兰就带几套紧固件。
let toroute (node: Json) =
    let rec loop (parentisroute:bool) (propsls:list<Json>) (data: Json) =
        let title = data.["title"].stringText
        let refconfig = data.["refconfig"].stringText
        let refid     = data.["refid"].stringText
        let isVirtual = data.["isVirtual"] = Json.True
        let props     = data.["props"]

        let loopChildren isroute (data:Json) =
            let propsls = props :: propsls
            data.["children"].elements
            |> List.map(fun child -> loop isroute propsls child)
        
        match title with
        | IgnoreCase "elbow LR.SLDPRT" ->
            let angle,dn =
                parseElbow refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props
            {
                refid = refid
                specific = Elbow(angle,dn,pn,material)
            }

        | IgnoreCase "straight tee.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props
            {
                refid = refid
                specific = Tee(dn,pn,material)
            }

        | IgnoreCase "flange.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props
            {
                refid = refid
                specific = Flange(dn,pn,material)
            }            
        | IgnoreCase "flange with fasteners.SLDPRT" ->
            failwith "replace <flange with fasteners.SLDPRT> with <sole flange.SLDASM>"

        | IgnoreCase "reducer.SLDPRT" ->
            let dn1,dn2 = parseDNxDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props
            {
                refid = refid
                specific = Reducer(dn1,dn2,pn,material)
            }

        | IgnoreCase "eccentric reducer.SLDPRT" ->
            let dn1,dn2 =
                parseDNxDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props
            {
                refid = refid
                specific = EccentricReducer(dn1,dn2,pn,material)
            }

        | IgnoreCase "reducing outlet tee.SLDPRT" ->
            let dn1,dn2 =
                parseDNxDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props

            {
                refid = refid
                specific = ReducingTee(dn1,dn2,pn,material)
            }

        | IgnoreCase "ball valve.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props
            {
                refid = refid
                specific = BallValve(dn,pn,material)
            }

        | IgnoreCase "wafer butterfly valve.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props

            {
                refid = refid
                specific = WaferButterflyValve(dn,pn,material)
            }

        | IgnoreCase "wafer check valve.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props

            {
                refid = refid
                specific = WaferCheckValve(dn,pn,material)
            }

        | IgnoreCase "expansion joint.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props

            {
                refid = refid
                specific = Expansion(dn,pn,material)
            }

        | IgnoreCase "flowmeter.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props

            {
                refid = refid
                specific = Flowmeter(dn,pn,material)
            }

        | IgnoreCase "magnetic filter.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN propsls props
            let material = getMaterial propsls props

            {
                refid = refid
                specific = MagneticFilter(dn,pn,material)
            }

        | IgnoreCase "single flanged joint.SLDASM" ->
            {
                refid = refid
                specific = SingleFlange(loopChildren false data)
            }

        | IgnoreCase "flanges.SLDASM" ->
            {
                refid = refid
                specific = Flanges(loopChildren false data)
            }
        | IgnoreCase "ball valve flanges.SLDASM" ->
            {
                refid = refid
                specific = BallValveFlanges(loopChildren false data)
            }
        | IgnoreCase "ball valve solo.SLDASM" ->
            {
                refid = refid
                specific = BallValveSolo(loopChildren false data)
            }
        | IgnoreCase "wafer butterfly valve flanges.SLDASM" ->
            {
                refid = refid
                specific = WaferButterflyValveFlanges(loopChildren false data)
            }

        | IgnoreCase "wafer butterfly valve solo.SLDASM" ->
            {
                refid = refid
                specific = WaferButterflyValveSolo(loopChildren false data)
            }

        | IgnoreCase "wafer check valve flanges.SLDASM" ->
            {
                refid = refid
                specific = WaferCheckValveFlanges(loopChildren false data)
            }

        | IgnoreCase "expansion joint flanges.SLDASM" ->
            {
                refid = refid
                specific = ExpansionFlanges(loopChildren false data)
            }

        | IgnoreCase "expansion joint solo.SLDASM" ->
            {
                refid = refid
                specific = ExpansionSolo(loopChildren false data)
            }

        | IgnoreCase "flowmeter flanges.SLDASM" ->
            {
                refid = refid
                specific = FlowmeterFlanges(loopChildren false data)
            }

        | IgnoreCase "magnetic filter flanges.SLDASM" ->
            {
                refid = refid
                specific = MagneticFilterFlanges(loopChildren false data)
            }
        | _ ->
            match data.["kind"].stringText with
            | "Assembly" ->
                {
                    refid = refid
                    specific = ComponentAssembly(title, refconfig, loopChildren false data)
                }
            | "RouteAssembly" ->
                {
                    refid = refid
                    specific = RouteAssembly(title, loopChildren true data)
                }
            | "Part" | _ ->
                if isCompTypeOf "Pipe" props then
                    let dn =
                        props.["Pipe Identifier"].stringText
                        |> parseDN
                    let length =
                        props.["Length"].stringText
                        |> parseLength
                    let pn = getPN propsls props
                    let material = getMaterial propsls props

                    {
                        refid = refid
                        specific = Pipe(length,dn,pn,material)
                    }
                elif isCompTypeOf "Elbow" props then
                    let angle,dn =
                        parseElbow refconfig
                    let pn = getPN propsls props
                    let material = getMaterial propsls props
                    {
                        refid = refid
                        specific = Elbow(angle,dn,pn,material)
                    }
                else
                    {
                        refid = refid
                        specific = ComponentPart(title,refconfig)
                    }
    loop false [] node

