namespace FlangeAssemblyBOM

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms.Literal

type FlangeTest(output : ITestOutputHelper) =
    let show res = 
        res 
        |> stringify
        |> output.WriteLine

    [<Fact>]
    member this.``01 - flanges test``() =
        let rows = FlangeRow.fromTsv()
        for row in rows do
        output.WriteLine($"{stringify row}")

    [<Fact>]
    member this.``02 - Screw Fastener test``() =
        let rows = ScrewFastener.fromTsv()
        for row in rows do
        output.WriteLine($"{stringify row}")

    [<Fact>]
    member this.``03 - washers test``() =
        let rows = Washer.fromTsv()
        for row in rows do
        output.WriteLine($"{stringify row}")

    [<Fact>]
    member this.``04 - getTbl test``() =
        let pn = 10.0
        let dn = 150.0
        let flange, screw = FlangePair.getTbl pn dn
        
        Should.equal screw.M 20.0
        Should.equal flange.N 8

    [<Theory>]
    [<InlineData(15.0, 50.0)>]
    [<InlineData(20.0, 50.0)>]
    [<InlineData(25.0, 50.0)>]
    [<InlineData(32.0, 60.0)>]
    [<InlineData(40.0, 60.0)>]
    [<InlineData(50.0, 65.0)>]
    [<InlineData(65.0, 65.0)>]
    [<InlineData(80.0, 65.0)>]
    [<InlineData(100.0, 70.0)>]
    [<InlineData(125.0, 70.0)>]
    [<InlineData(150.0, 80.0)>]
    [<InlineData(200.0, 80.0)>]
    [<InlineData(250.0, 80.0)>]
    [<InlineData(300.0, 80.0)>]
    [<InlineData(350.0, 85.0)>]
    member this.``05 - boltLength test``(dn:float,e:float) =
        let pn = 10.0
        let y = FlangePair.boltLength pn dn
        output.WriteLine($"计算长度大于给定：{y-e}")
        Assert.True(y>=e)
        //Should.equal e y

    [<Fact>]
    member this.``06 - Wafer getTbl test``() =
        let pn = 10.0
        let dn = 150.0
        let flange, screw, washer = Wafer.getTbl pn dn
        
        Should.equal screw.M 20.0
        Should.equal flange.N 8
        Should.equal washer.M 20.0

    [<Fact>]
    member this.``07 - Wafer studLength test``() =
        let pn = 10.0
        let dn = 150.0
        let fn = Wafer.studLength pn dn
        
        let y = fn 19.0
        output.WriteLine($"双头螺柱长度：{y}")




