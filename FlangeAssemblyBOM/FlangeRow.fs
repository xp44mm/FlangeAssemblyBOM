namespace FlangeAssemblyBOM

open System
open System.IO
open FSharp.Idioms

type FlangeRow =
    {
        PN :float
        DN :float
        Di1:float
        Di2:float
        Dw :float
        DK :float
        C  :float
        DS :float
        HS :float
        H  :float
        M  :float
        N  :int
    }

    static member fromTsv() =
        let path = Path.Combine(Dir.databasePath,"flange.tsv")
        let tbl = Tsv.parseTsv path
        let headRow = Tsv.getFieldTitles tbl

        tbl.[1..]
        |> Array.map(fun row -> {
            PN = "PN"  |> Tsv.getValue headRow <| row |> Double.Parse
            DN = "DN"  |> Tsv.getValue headRow <| row |> Double.Parse
            Di1= "Di1" |> Tsv.getValue headRow <| row |> Double.Parse
            Di2= "Di2" |> Tsv.getValue headRow <| row |> Double.Parse
            Dw = "Dw"  |> Tsv.getValue headRow <| row |> Double.Parse
            DK = "DK"  |> Tsv.getValue headRow <| row |> Double.Parse
            C  = "C"   |> Tsv.getValue headRow <| row |> Double.Parse
            DS = "DS"  |> Tsv.getValue headRow <| row |> Double.Parse
            HS = "HS"  |> Tsv.getValue headRow <| row |> Double.Parse
            H  = "H"   |> Tsv.getValue headRow <| row |> Double.Parse
            M  = "M"   |> Tsv.getValue headRow <| row |> Double.Parse
            N  = "N"   |> Tsv.getValue headRow <| row |> Int32.Parse
        })

    static member toMap() =
        FlangeRow.fromTsv()
        |> Array.map(fun x -> (x.PN,x.DN),x)
        |> Map.ofArray

