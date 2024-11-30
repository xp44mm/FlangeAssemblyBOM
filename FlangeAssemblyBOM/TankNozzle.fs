module FlangeAssemblyBOM.TankNozzle

open FlangeAssemblyBOM
open FlangeAssemblyBOM.Scaffold

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal
open FSharp.Idioms.OrdinalIgnoreCase

open System
open System.Diagnostics
open System.IO
open System.Text.RegularExpressions

let getName2 (json:Json) = json.["name2"].stringText
let getTitle (json:Json) = json.["title"].stringText
let getRefconfig (json:Json) = json.["refconfig"].stringText
let getRefid (json:Json) = json.["refid"].stringText
let isVirtual (json:Json) = json.["isVirtual"] = Json.True
let getProps (json:Json) = json.["props"]
let getKind (json:Json) = json.["kind"].stringText
let getChildren (json:Json) = json.["children"].elements

let folder = @"D:\崔胜利\凯帝隆\湖北武穴锂宝\solidworks4\tanks"
        
let tanks =
    Directory.GetFiles(folder)
    |> Array.map(fun f -> Path.GetFileName f )
    |> Set.ofArray

let tankId = Regex("^[VRE]\d\d\d\d[ABCD]?$",RegexOptions.IgnoreCase)

let rec collectTanks (json:Json) =
    let children =
        json
        |> getChildren
        |> List.filter(fun child ->
            match getKind child with
            | "Assembly" -> true
            | "RouteAssembly" -> false
            | "Part" -> false
            | _ -> false
        )
        |> List.map(fun child ->
            if tankId.IsMatch(getRefid child) || tanks.Contains(getTitle child) then
                child
            else collectTanks child
        )
        |> Json.Array
    json.assign ["children", children]

/// 从一个罐子收集管嘴
let collectNozzleFromTank (json:Json) =
    let tankId = getRefid json

    json
    |> getChildren
    |> List.filter(fun child ->
        getTitle child == "Nozzle A 99.SLDPRT"
    )
    |> List.map(fun child ->
        tankId, getRefid child, getRefconfig child
    )

let rec nozzleRecords (json:Json) =
    seq {
        for child in getChildren json do
            if tankId.IsMatch(getRefid child) || tanks.Contains(getTitle child) then
                yield! collectNozzleFromTank child
            else 
                yield! nozzleRecords child
    }
