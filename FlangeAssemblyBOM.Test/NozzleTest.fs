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

type NozzleTest (output: ITestOutputHelper) =
    let path = @"D:\崔胜利\凯帝隆\湖北武穴锂宝\solidworks4\tanks\drawing\data.json"

    [<Fact>]
    member _.``group size type test``() =
        let text = File.ReadAllText(path,Encoding.UTF8)
        let json = Json.parse text

        let records = 
            json.getElements()
            |> List.groupBy(fun json -> 
                let typ = json.["typ"].stringText //: "cauldron",
                let diameter = json.["diameter"].floatValue //: 2200,
                let height = json.["height"].floatValue //: 2600,
                typ,diameter,height
            )
            |> List.map fst
            |> List.sort

        output.WriteLine($"{records.Length}")

        let outp =
            records
            |> List.map(fun (a,b,c) -> [|a;stringify(int b);stringify(int c)|])
            |> List.toArray
            |> Tsv.stringify

        //let tgt = Path.Combine(Path.GetDirectoryName path, @"tankNozzles.csv")
        //File.WriteAllText(tgt, outp, Encoding.UTF8)
        output.WriteLine(outp)

    [<Fact>]
    member _.``texts Of top or side views``() =
        let text = File.ReadAllText(path,Encoding.UTF8)
        let json = Json.parse text

        let positionTexts = 
            json.getElements()
            |> List.collect(fun json -> 
                let tankComp = TankComponent.fromJson json
                let file = 
                    {
                        text = tankComp.file
                        origin = 0.,0.,0.
                        displacement = tankComp.origin
                    }
                let nozzles =
                    tankComp.tank.nozzles
                    |> List.map(fun nz ->
                        {
                            text = nz.name
                            origin = file.displacement
                            displacement = nz.face
                        }
                    )
                file::nozzles
                )
        let dbtexts =
            positionTexts
            |> List.collect(fun swtext ->
                let cadtext = swtext.cadfromsw()
                [
                    cadtext.topView()
                    cadtext.sideView()
                ]
            )
            |> List.groupBy(fun(a,b,c,d) -> a)
            |> List.map snd
            |> List.map(List.map(fun(a,b,c,d) -> b,c,d))

        let folder = Path.GetDirectoryName(path)

        let path1 = Path.Combine(folder,"texts Of top view.tsv")
        let text1 = 
            dbtexts.[0] 
            |> List.map(fun((file,x,y))-> $"{file}\t{x}\t{y}")
            |> String.concat "\r\n"
        File.WriteAllText(path1,text1,Encoding.UTF8)
        output.WriteLine(path1)

        let path2 = Path.Combine(folder,"texts Of side view.tsv")
        let text2 = 
            dbtexts.[1] 
            |> List.map(fun((file,x,z))-> $"{file}\t{x}\t{z}")
            |> String.concat "\r\n"
        File.WriteAllText(path2,text2,Encoding.UTF8)
        output.WriteLine(path2)

    [<Fact>]
    member _.``collect nozzles test``() =
        let text = File.ReadAllText(path,Encoding.UTF8)
        let json = Json.parse text

        let records = 
            json.getElements()
            |> List.collect(fun json -> 
                let tankComp = TankComponent.fromJson json
                tankComp.getTankNozzleRecords()
            )
            |> List.sortBy(fun row -> row.file,row.name)
            |> List.map(fun row -> row.toStringArray())

        let outp =
            TankNozzleRecord.Title::records
            |> List.toArray
            |> Tsv.stringify

        let tsv = Path.Combine(Path.GetDirectoryName path, @"nozzles.tsv")
        File.WriteAllText(tsv, outp, Encoding.UTF8)
        output.WriteLine(tsv)

