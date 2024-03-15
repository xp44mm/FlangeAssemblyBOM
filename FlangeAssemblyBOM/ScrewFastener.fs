namespace FlangeAssemblyBOM

open System
open System.IO
open FSharp.Idioms

type ScrewFastener =
    {
        M :float
        t:float // 螺母厚度
        z:float //倒角端
        p :float //螺距
    }

    static member fromTsv() =
        let path = Path.Combine(Dir.databasePath,"screw fastener.tsv")
        let tbl = Tsv.parseTsv path
        let headRow = Tsv.getFieldTitles tbl

        tbl.[1..]
        |> Array.map(fun data -> 
            {
                M = "M"   |> Tsv.getValue headRow <| data |> Double.Parse
                t = "t" |> Tsv.getValue headRow <| data |> Double.Parse
                z = "z"   |> Tsv.getValue headRow <| data |> Double.Parse
                p = "p"   |> Tsv.getValue headRow <| data |> Double.Parse
        })

    static member toMap() =
        ScrewFastener.fromTsv()
        |> Array.map(fun x -> x.M,x)
        |> Map.ofArray
