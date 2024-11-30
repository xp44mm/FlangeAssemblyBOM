namespace FlangeAssemblyBOM.Routing

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

type RouteComponentFromJsonTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``01 - json test``() =
        let text = 
            let path = Path.Combine(Dir.CommandData,"component archive.json")            
            File.ReadAllText(path)

        let y = 
            text
            |> Json.parse
            |> JsonAppender.capture
            |> RouteComponentFromJson.toroute

        output.WriteLine(stringify y)

        let e =  
            {refid= "";specific= ComponentAssembly("V0102ab.SLDASM","",[
            {refid= "";specific= ComponentPart("罐区建筑.SLDPRT","默认")};
            {refid= "";specific= ComponentPart("V0102A.SLDPRT","Default")};
            {refid= "P0104A";specific= ComponentPart("Pump.SLDPRT","GWZ-65DM-11KW-K-SSIC-EX")};
            {refid= "";specific= ComponentPart("bag filter.SLDPRT","bag3,DN80")};
            {refid= "";specific= RouteAssembly("Pipe_10^V0102ab.SLDASM",[
            {refid= "";specific= BallValveSolo [
            {refid= "";specific= BallValve(80.0,1.0,"pph")};
            {refid= "";specific= Flange(80.0,1.0,"pph")}]};
            {refid= "";specific= Pipe(448.3,80.0,1.0,"pph")};
            {refid= "";specific= SingleFlange [
            {refid= "";specific= Flange(80.0,1.0,"pph")}]};
            {refid= "";specific= Pipe(198.3,80.0,1.0,"pph")}])};
            {refid= "P0104B";specific= ComponentPart("Pump.SLDPRT","GWZ-65DM-11KW-K-SSIC-EX")};
            {refid= "";specific= ComponentPart("bag filter.SLDPRT","bag3,DN80")};
            {refid= "";specific= ComponentPart("V0102B.SLDPRT","Default")}])}
        Should.equal e y




