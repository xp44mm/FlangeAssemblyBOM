namespace FlangeAssemblyBOM

open Xunit
open Xunit.Abstractions
open FSharp.Idioms.Literal
open FSharp.xUnit

type DirTest(output : ITestOutputHelper) =
    let show res = 
        res 
        |> stringify
        |> output.WriteLine

    [<Fact>]
    member _.``01 - first test``() =
        output.WriteLine("testPath:")
        output.WriteLine(Dir.dinfo.FullName)

        output.WriteLine("solutionPath:")
        output.WriteLine(Dir.solutionPath)

