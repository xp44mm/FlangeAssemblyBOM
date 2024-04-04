namespace FlangeAssemblyBOM.Scaffold

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal

open System.IO
open System.Text

open UnquotedJson
open FlangeAssemblyBOM

type FlangeDbContextTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``01 - select test``() =
        use context = new FlangeDbContext()
        let ls = 
            context.flange
            |> Seq.map(fun x -> x.DN,x.PN)
            |> Seq.toList
        output.WriteLine(stringify ls)
