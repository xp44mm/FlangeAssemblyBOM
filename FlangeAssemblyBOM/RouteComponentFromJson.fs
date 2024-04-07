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

/// 对夹阀门装配体带一套对夹法兰紧固件。无论两片法兰，还是一片法兰。
/// 法兰阀门装配体，有几片配对法兰就带几套紧固件。
let toroute (node: Json) =
    let rec loop (parentisroute:bool) (data: Json) =
        let title = data.["title"].stringText
        let refconfig = data.["refconfig"].stringText
        let refid     = data.["refid"].stringText
        let isVirtual = data.["isVirtual"] = Json.True
        let props     = data.["props"]
        let withFasteners () =
            props.hasProperty "fasteners" && props.["fasteners"].boolValue = false

        let loopChildren parentisroute (children:Json) =
            children.elements
            |> List.map(fun child -> loop parentisroute child)
        
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
            let fl =
                {
                    refid = refid
                    specific = Flange(dn,pn,material)
                }

            // routeassy直接单法兰默认转化为singleflange
            // fasteners:false 单法兰零件
            if parentisroute && 
                props.hasProperty "fasteners" && 
                props.["fasteners"].boolValue = false then
                {
                    refid = refid
                    specific = SingleFlange([fl])
                }
            else fl
            
        | IgnoreCase "flange with fasteners.SLDPRT" ->
            let dn = parseDN refconfig
            let pn = getPN props
            let material = getMaterial props
            let fl =
                {
                    refid = refid
                    specific = Flange(dn,pn,material)
                }

            {
                refid = refid
                specific = SingleFlange([fl])
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
                specific = SingleFlange(loopChildren false data.["Assembly"])
            }

        | IgnoreCase "flanges.SLDASM" ->
            {
                refid = refid
                specific = Flanges(loopChildren false data.["Assembly"])
            }
        | IgnoreCase "ball valve flanges.SLDASM" ->
            {
                refid = refid
                specific = BallValveFlanges(loopChildren false data.["Assembly"])
            }
        | IgnoreCase "ball valve solo.SLDASM" ->
            {
                refid = refid
                specific = BallValveSolo(loopChildren false data.["Assembly"])
            }
        | IgnoreCase "wafer butterfly valve flanges.SLDASM" ->
            {
                refid = refid
                specific = WaferButterflyValveFlanges(loopChildren false data.["Assembly"])
            }

        | IgnoreCase "wafer butterfly valve solo.SLDASM" ->
            {
                refid = refid
                specific = WaferButterflyValveSolo(loopChildren false data.["Assembly"])
            }

        | IgnoreCase "wafer check valve flanges.SLDASM" ->
            {
                refid = refid
                specific = WaferCheckValveFlanges(loopChildren false data.["Assembly"])
            }

        | IgnoreCase "expansion joint flanges.SLDASM" ->
            {
                refid = refid
                specific = ExpansionFlanges(loopChildren false data.["Assembly"])
            }

        | IgnoreCase "expansion joint solo.SLDASM" ->
            {
                refid = refid
                specific = ExpansionSolo(loopChildren false data.["Assembly"])
            }

        | IgnoreCase "flowmeter flanges.SLDASM" ->
            {
                refid = refid
                specific = FlowmeterFlanges(loopChildren false data.["Assembly"])
            }

        | IgnoreCase "magnetic filter flanges.SLDASM" ->
            {
                refid = refid
                specific = MagneticFilterFlanges(loopChildren false data.["Assembly"])
            }
        | _ ->
            if data.hasProperty "Assembly" then
                {
                    refid = refid
                    specific = ComponentAssembly(title,refconfig, loopChildren false data.["Assembly"])
                }
            elif data.hasProperty "RouteAssembly" then
                {
                    refid = refid
                    specific = RouteAssembly(title, loopChildren true data.["RouteAssembly"])
                }
            elif data.hasProperty "Part" then // todo: 考虑删除Part:[]输出
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
            else failwith "never"
    loop false node

