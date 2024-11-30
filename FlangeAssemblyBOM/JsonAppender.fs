module FlangeAssemblyBOM.JsonAppender

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System.IO

open UnquotedJson

//let fields = [ "dn";"pn";"material";"fasteners"]

///属性值下沉
let capture (json:Json) =
    let rec loop (props:Json) (json:Json) =
        let newProps = 
            if json.hasProperty "props" then
                json.["props"].coalesce props
            else props

        let newJson =
            json.assign [
                "props", newProps
            ]

        if newJson.hasProperty "children" then
            let children =
                newJson.["children"].elements
                |> List.map (loop newProps)
                |> Json.Array
            newJson.assign ["children",children]
        else newJson

    loop (Json.Object []) json

