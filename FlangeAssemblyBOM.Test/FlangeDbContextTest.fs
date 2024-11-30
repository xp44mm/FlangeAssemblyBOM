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
    member this.``01 - select Flange test``() =
        use context = new FlangeDbContext()
        let ls = 
            context.Flange
            |> Seq.map(fun x -> x.DN,x.PN)
            |> Seq.toList
        output.WriteLine(stringify ls)

    [<Fact>]
    member this.``02 - select ScrewFastener test``() =
        use context = new FlangeDbContext()
        let ls = 
            context.ScrewFastener
            |> Seq.map(fun x -> x.M)
            |> Seq.toList
        output.WriteLine(stringify ls)

    [<Fact>]
    member this.``03 - select WaferButterflyValve test``() =
        use context = new FlangeDbContext()
        let ls = 
            context.WaferButterflyValve
            |> Seq.map(fun x -> x.DN)
            |> Seq.toList
        output.WriteLine(stringify ls)

    [<Fact>]
    member this.``04 - select WaferCheckValve test``() =
        use context = new FlangeDbContext()
        let ls = 
            context.WaferCheckValve
            |> Seq.map(fun x -> x.DN)
            |> Seq.toList
        output.WriteLine(stringify ls)

    [<Fact>]
    member this.``05 - select Washer test``() =
        use context = new FlangeDbContext()
        let ls = 
            context.Washer
            |> Seq.map(fun x -> x.M)
            |> Seq.toList
        output.WriteLine(stringify ls)
