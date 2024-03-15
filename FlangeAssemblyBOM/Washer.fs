namespace FlangeAssemblyBOM

open System
open System.IO
open FSharp.Idioms

type Washer =
    {
        M :float
        Di:float
        Dw:float
        t :float
    }

    static member fromTsv () =
        let path = Path.Combine(Dir.databasePath,"washer.tsv")
        let tbl = Tsv.parseTsv path
        let headRow = Tsv.getFieldTitles tbl

        tbl.[1..]
        |> Array.map(fun data -> 
            {
                M = "M"   |> Tsv.getValue headRow <| data |> Double.Parse
                Di = "Di" |> Tsv.getValue headRow <| data |> Double.Parse
                Dw = "Dw" |> Tsv.getValue headRow <| data |> Double.Parse
                t = "t"   |> Tsv.getValue headRow <| data |> Double.Parse
        })
    static member toMap() =
        Washer.fromTsv()
        |> Array.map(fun x -> x.M,x)
        |> Map.ofArray
