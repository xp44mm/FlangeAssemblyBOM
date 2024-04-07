module FlangeAssemblyBOM.Routing.JsonAppender
open FlangeAssemblyBOM

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System.IO

open UnquotedJson

let fields = [ "dn";"pn";"material";"fasteners"]

///属性值下沉
let capture (json:Json) =
    let rec loop (props:Json) (json:Json) =
        let newProps = json.["props"].coalesce props
        let json =
            json.assign [
                "props", newProps
            ]

        let loopAssembly (name:string) =
            let children =
                json.[name].elements
                |> List.map (loop newProps)
                |> Json.Array
            json.assign [name,children]

        if json.hasProperty "RouteAssembly" then
            loopAssembly "RouteAssembly"
        elif json.hasProperty "Assembly" then
            loopAssembly "Assembly"
        elif json.hasProperty "Part" then
            json
        else failwith "never:RouteAssembly|Assembly|Part"

    loop (Json.Object []) json

