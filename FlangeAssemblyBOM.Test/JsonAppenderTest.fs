namespace FlangeAssemblyBOM

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open System.IO
open System.Text

open FSharp.Idioms
open FSharp.Idioms.Jsons
open FSharp.Idioms.Literal
open UnquotedJson

open System
open System.Collections
open System.Collections.Generic
open System.Diagnostics
open System.Drawing
open System.IO
open System.Reflection
open System.Runtime.InteropServices
open System.Text
open System.Text.RegularExpressions

open FSharp.Idioms
open FSharp.Idioms.Literal

type JsonAppenderTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``01 - props test``() =
        let text = 
            let path = Path.Combine(Dir.CommandData,"test.json")
            File.ReadAllText(path)

        let json = 
            text
            |> Json.parse

        //let outp = Json.print json
        output.WriteLine(stringify json)

    [<Fact>]
    member this.``02 - capture test``() =
        let x = Json.Object [
            "props",Json.Object ["pn",Json.Number 1.0];
            "children",Json.Array [
                Json.Object ["props",Json.Object ["dn",Json.Number 100.0]];
                Json.Object ["props",Json.Object ["pn",Json.Number 1.6;"dn",Json.Number 100.0]]
                ]]

        let y = JsonAppender.capture x
        //output.WriteLine(stringify y)
        let e = Json.Object [
            "props",Json.Object ["pn",Json.Number 1.0];
            "children",Json.Array [
                Json.Object [
                    "props",Json.Object ["dn",Json.Number 100.0;"pn",Json.Number 1.0] //append pn
                    ];
                Json.Object [
                    "props",Json.Object ["pn",Json.Number 1.6;"dn",Json.Number 100.0] // ignore pn
                    ]
                    ]]

        //let outp = Json.print json
        Should.equal e y

    [<Fact>]
    member this.``03 - capture nested delivery test``() =
        let x = Json.Object ["props",Json.Object ["pn",Json.Number 1.0];
            "children",Json.Array [Json.Object [
            "children",Json.Array [Json.Object [
            "children",Json.Array [Json.Object [
            "children",Json.Array [Json.Object []]]]]]]]]

        let y = JsonAppender.capture x
        //output.WriteLine(stringify y)
        let e = Json.Object [
            "props",Json.Object ["pn",Json.Number 1.0];
            "children",Json.Array [
                Json.Object [
                    "children",Json.Array [
                        Json.Object [
                            "children",Json.Array [
                                Json.Object [
                                    "children",Json.Array [ Json.Object [ "props",Json.Object ["pn",Json.Number 1.0]]];
                                    "props",Json.Object ["pn",Json.Number 1.0]
                                ]
                            ];
                            "props",Json.Object ["pn",Json.Number 1.0]]
                    ];
                    "props",Json.Object ["pn",Json.Number 1.0]]]]


        //let outp = Json.print json
        Should.equal e y


    [<Fact>]
    member this.``04 - archive capture test``() =
        let text = 
            let path = Path.Combine(Dir.CommandData,"component archive.json")            
            File.ReadAllText(path)

        let json = 
            text
            |> Json.parse
            |> JsonAppender.capture

        //let json = Json.from comp
        let outp = Json.print json
        let path = Path.Combine(Dir.CommandData,"capture.json")

        File.WriteAllText(path,outp,Encoding.UTF8)
        output.WriteLine(path)
