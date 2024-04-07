namespace FlangeAssemblyBOM

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open FSharp.Idioms.Literal
open FlangeAssemblyBOM.Scaffold

type ScrewTest(output : ITestOutputHelper) =

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
        let pn = 1.0
        use context = new FlangeDbContext()
        let flange = context.Flange.Find(pn,dn)
        let screw = context.ScrewFastener.Find(flange.M)

        let y = Screw.boltLength flange screw
        output.WriteLine($"计算长度大于给定：{y-e}")
        Assert.True(y>=e)
        //Should.equal e y

    [<Fact>]
    member this.``07 - Wafer studLength test``() =
        let pn = 1.0
        let dn = 150.0
        use context = new FlangeDbContext()
        let flange = 
            context.Flange.Find(pn,dn)
        let screw = context.ScrewFastener.Find(flange.M)
        let washer = context.Washer.Find(flange.M)
        let fn = Screw.studLength flange screw washer
        let waferLength = context.WaferCheckValve.Find(dn).Length

        output.WriteLine(sprintf "%f" waferLength)

        let y = fn waferLength
        output.WriteLine($"双头螺柱长度：{y}")




