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
    let show res = 
        res 
        |> stringify
        |> output.WriteLine

    [<Fact>]
    member this.``01 - json test``() =
        let text = 
            let path = Path.Combine(Dir.CommandData,"component archive.json")            
            File.ReadAllText(path)

        let comp = 
            text
            |> Json.parse
            |> RouteComponentFromJson.toroute

        let json = Json.from comp
        let outp = Json.print json
        let path = Path.Combine(Dir.CommandData,"route component.json")

        File.WriteAllText(path,outp,Encoding.UTF8)
        output.WriteLine(path)


    [<Fact>]
    member this.``02 - JsonAppender capture test``() =
        let text = 
            let path = Path.Combine(Dir.CommandData,"component archive.json")
            File.ReadAllText(path)

        let json = 
            text
            |> Json.parse
            |> JsonAppender.capture

        let outp = Json.print json
        let path = Path.Combine(Dir.CommandData,"json capture.json")

        File.WriteAllText(path,outp,Encoding.UTF8)
        output.WriteLine(path)

    [<Fact>]
    member this.``03 - to json test``() =
        let text = 
            let path = Path.Combine(Dir.CommandData,"component archive.json")
            File.ReadAllText(path)

        let comp = 
            text
            |> Json.parse
            |> JsonAppender.capture
            |> RouteComponentFromJson.toroute
            |> RouteComponentFromJson.fasteners
            
        let json = RouteComponentToJson.from comp
        let outp = Json.print json
        let path = Path.Combine(Dir.CommandData,"RouteComponentToJson.json")

        File.WriteAllText(path,outp,Encoding.UTF8)
        output.WriteLine(path)



