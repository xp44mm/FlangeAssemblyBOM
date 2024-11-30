namespace FlangeAssemblyBOM

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open System
open System.IO
open System.Text

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal
open UnquotedJson

open type Math
open System.Text.RegularExpressions

type TankNozzleTest (output: ITestOutputHelper) =

    [<Fact>]
    member _.``file test``() =
        let folder = @"D:\崔胜利\凯帝隆\湖北武穴锂宝\solidworks4\tanks"
        
        let tanks =
            Directory.GetFiles(folder)
            |> Array.map(fun f -> Path.GetFileName f )

        tanks
        |> Array.iter(fun f ->
            output.WriteLine (f)
        )

    [<Fact>]
    member _.``generate tanks test``() =
        let path = Path.Combine(Dir.CommandData, @"component archive.json")
        let text = File.ReadAllText(path,Encoding.UTF8)

        let json = Json.parse text
        let tanksJson = TankNozzle.collectTanks json
        let outp = Json.print tanksJson
        let tgt = Path.Combine(Path.GetDirectoryName path, @"tanks.json")
        File.WriteAllText(tgt, outp, Encoding.UTF8)
        output.WriteLine(tgt)

    [<Fact>]
    member _.``generate nozzle records test``() =
        let path = Path.Combine(Dir.CommandData, @"tanks.json")
        let text = File.ReadAllText(path,Encoding.UTF8)

        let json = Json.parse text

        let records = 
            json
            |> TankNozzle.nozzleRecords
            |> Seq.toList
            |> List.sortBy(fun (a,b,c)->
                let m = Regex.Match(b,"^([a-z]+)(\d*)$",RegexOptions.IgnoreCase)
                let b0 = m.Groups.[1].Value
                let b1 = match m.Groups.[2].Value with "" -> -1 | x -> int x
                a,b0,b1
            )
            |> List.map(fun (a,b,c) -> $"{a},{b},{c}")

        output.WriteLine($"{records.Length}")

        let outp =
            records
            |> String.concat "\r\n"
        let tgt = Path.Combine(Path.GetDirectoryName path, @"tankNozzles.csv")
        File.WriteAllText(tgt, outp, Encoding.UTF8)
        output.WriteLine(tgt)
    
