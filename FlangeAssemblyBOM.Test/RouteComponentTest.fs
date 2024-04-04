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

type RouteComponentTest(output : ITestOutputHelper) =
    let show res = 
        res 
        |> stringify
        |> output.WriteLine

    //[<Fact>]
    //member this.``02 - json add pn test``() =
    //    let path = Path.Combine(Dir.CommandData,"route.json")
    //    let text = File.ReadAllText(path)
    //    //output.WriteLine(text)

    //    let json = Json.parse text
    //    //output.WriteLine(Json.print json)

    //    let newJson = 
    //        [
    //            "pn",Json.Number 1.0
    //            "material", Json.String "PPH"
    //        ]
    //        |> JsonAppender.addProps json

    //    let outp = Json.print newJson

    //    let opath = Path.Combine(Dir.CommandData, "routePN.json")
    //    File.WriteAllText(opath, outp)
    //    output.WriteLine(outp)
    //    output.WriteLine(opath)

    //[<Fact>]
    //member this.``02 - json capture pn test``() =
    //    let path = Path.Combine(Dir.CommandData, "routePN.json")
    //    let text = File.ReadAllText(path)

    //    let json = Json.parse text
    //    let names = ["pn";"material"]
    //    let newJson = JsonAppender.capture names json

    //    let outp = Json.print newJson

    //    let opath = Path.Combine(Dir.CommandData, "routeCaputure.json")
    //    File.WriteAllText(opath, outp)
    //    output.WriteLine(outp)
    //    output.WriteLine(opath)

    [<Fact>]
    member this.``03 - toroute test``() =
        let path = Path.Combine(Dir.CommandData, "routeCaputure.json")
        let text = File.ReadAllText(path)

        let json = Json.parse text

        let outp = RouteComponentConstructor.toroute json |> stringify

        let opath = Path.Combine(Dir.CommandData, "RouteComponent.txt")
        File.WriteAllText(opath, outp)
        output.WriteLine(outp)
        output.WriteLine(opath)

    [<Fact>]
    member this.``04 - tolines test``() =
        let path = Path.Combine(Dir.CommandData, "routeCaputure.json")
        let text = File.ReadAllText(path)

        let json = Json.parse text

        let outp = 
            json
            |> RouteComponentConstructor.toroute  
            |> RouteComponentConstructor.tolines 0
            |> String.concat "\n"

        let opath = Path.Combine(Dir.CommandData, "RouteComponent tolines.txt")
        File.WriteAllText(opath, outp)
        //output.WriteLine(outp)
        output.WriteLine(opath)

    [<Fact>]
    member this.``05 - bom test``() =
        let comps =
            let path = Path.Combine(Dir.CommandData, "routeCaputure.json")
            let text = File.ReadAllText(path)
            let json = Json.parse text
            json
            |> RouteComponentConstructor.toroute  
            |> RouteComponentConstructor.bom

        let outp = 
            comps
            |> List.map (RouteComponentConstructor.toLine)
            |> String.concat "\n"

        let path = Path.Combine(Dir.CommandData, "RouteComponent bom.txt")
        File.WriteAllText(path, outp,Encoding.UTF8)
        output.WriteLine(path)

    [<Fact>]
    member this.``06 - aggregate test``() =
        let comps =
            let path = Path.Combine(Dir.CommandData, "routeCaputure.json")
            let text = File.ReadAllText(path)
            let json = Json.parse text
            json
            |> RouteComponentConstructor.toroute  
            |> RouteComponentConstructor.bom

        let outp = 
            comps
            |> Bill.aggregate
            |> List.map (fun (sp,count) -> $"{sp.toLine()},{count}")
            |> String.concat "\n"

        let path = Path.Combine(Dir.CommandData, "RouteComponent aggregate.txt")
        File.WriteAllText(path, outp,Encoding.UTF8)
        output.WriteLine(path)










