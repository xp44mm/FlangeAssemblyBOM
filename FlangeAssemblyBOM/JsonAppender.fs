module FlangeAssemblyBOM.Routing.JsonAppender
open FlangeAssemblyBOM

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System.IO

open UnquotedJson

/// 修改所有RouteAssembly
let addProps (json:Json) (newFields:list<string*Json>) =
    let rec loop (node:Json) =
        if node.["kind"].stringText = "RouteAssembly" then
            Json.Object [
                yield! newFields
                yield! node.entries
            ]
        else
            if node.hasProperty "children" then
                let children =
                    node.["children"].elements
                    |> List.map loop
                    |> Json.Array
                node.replaceProperty("children",children)
            else node
    loop json

let capture (names:string list) (json:Json) =
    let rec loop (props:Json) (json:Json) =
        let newProps =
            names
            |> List.choose(fun name -> 
                if json.hasProperty name then
                    Some(name,json.[name])
                elif props.hasProperty name then
                    Some(name,props.[name])
                else None
            )
            |> Json.Object

        let newJson = json.assign newProps

        if newJson.hasProperty "children" then
            let children =
                newJson.["children"].elements
                |> List.map (loop newProps)
                |> Json.Array
            newJson.replaceProperty("children",children)

        else newJson
    loop (Json.Object[]) json

/// 生成附件
