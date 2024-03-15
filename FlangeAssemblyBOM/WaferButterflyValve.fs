namespace FlangeAssemblyBOM

open System
open System.IO
open FSharp.Idioms

type WaferButterflyValve =
    {
        DN :float
        Length:float
    }

    static member fromTsv() =
        let path = Path.Combine(Dir.databasePath,"wafer butterfly.tsv")
        let tbl = Tsv.parseTsv path
        let headRow = Tsv.getFieldTitles tbl

        tbl.[1..]
        |> Array.map(fun row -> {
            DN = "DN"  |> Tsv.getValue headRow <| row |> Double.Parse
            Length= "Length" |> Tsv.getValue headRow <| row |> Double.Parse
        })

    static member toMap() =
        WaferButterflyValve.fromTsv()
        |> Array.map(fun x -> x.DN,x)
        |> Map.ofArray

