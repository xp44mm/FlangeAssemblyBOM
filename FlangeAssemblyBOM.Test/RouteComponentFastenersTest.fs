namespace FlangeAssemblyBOM.Routing

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms.Literal

type RouteComponentFastenersTest(output: ITestOutputHelper) =
    let comp spec = {refid= "";specific= spec}
    [<Fact>]
    member _.``01 - getFlangedFasteners test``() =
        let flange = Flange(100,1.0,"")
        let y = RouteComponentFasteners.getFlangedFasteners flange
        let e = [
            {refid= "";specific= Gasket(100,1.0,1)};
            {refid= "";specific= Nut(16,8)};
            {refid= "";specific= Bolt(16,70,8)}
            ]
        output.WriteLine(stringify y)
        Should.equal e y

    [<Fact>]
    member _.``02 - getWaferFasteners test``() =
        let wafer = WaferCheckValve(100,1.0,"")
        let y = RouteComponentFasteners.getWaferFasteners wafer
        output.WriteLine(stringify y)
        let e = [
            {refid= "";specific= Gasket(100.0,1.0,2)};
            {refid= "";specific= Nut(16.0,16)};
            {refid= "";specific= Washer(16.0,16)};
            {refid= "";specific= StudBolt(16.0,125.0,8)}
            ]

        Should.equal e y

    [<Fact>]
    member _.``03-01 - RouteAssembly test``() =
        let ra = {refid= "";specific= RouteAssembly("",[])}
        let y = RouteComponentFasteners.appendFasteners ra
        Should.equal ra y

    [<Fact>]
    member _.``03-02 - ComponentAssembly test``() =
        let ra = {refid= "";specific= ComponentAssembly("","",[])}
        let y = RouteComponentFasteners.appendFasteners ra
        Should.equal ra y

    [<Fact>]
    member _.``03-03 - SingleFlange test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let sf = {refid= "";specific= SingleFlange([flange])}
        let y = RouteComponentFasteners.appendFasteners sf
        output.WriteLine(stringify y)
        let e = {refid= "";specific= SingleFlange [
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-04 - Flanges test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let fs = {refid= "";specific= Flanges([flange;flange])}
        let y = RouteComponentFasteners.appendFasteners fs
        output.WriteLine(stringify y)
        let e = {refid= "";specific= Flanges [
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-05 - BallValveFlanges test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(BallValve(100,1.0,""))
        let assy = {refid= "";specific= BallValveFlanges([valve;flange;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= BallValveFlanges [
            {refid= "";specific= BallValve(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-06 - BallValveSolo test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(BallValve(100,1.0,""))
        let assy = {refid= "";specific= BallValveSolo([valve;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= BallValveSolo [
            {refid= "";specific= BallValve(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)};
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-07 - WaferButterflyValveFlanges test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(WaferButterflyValve(100,1.0,""))
        let assy = {refid= "";specific= WaferButterflyValveFlanges([valve;flange;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= WaferButterflyValveFlanges [
            {refid= "";specific= WaferButterflyValve(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,2)};
            {refid= "";specific= Nut(16.0,16)};
            {refid= "";specific= Washer(16.0,16)};
            {refid= "";specific= StudBolt(16.0,155.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-08 - WaferButterflyValveSolo test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(WaferButterflyValve(100,1.0,""))
        let assy = {refid= "";specific= WaferButterflyValveSolo([valve;flange])}

        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= WaferButterflyValveSolo [
            {refid= "";specific= WaferButterflyValve(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,2)};
            {refid= "";specific= Nut(16.0,16)};
            {refid= "";specific= Washer(16.0,16)};
            {refid= "";specific= StudBolt(16.0,155.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-09 - WaferCheckValveFlanges test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(WaferCheckValve(100,1.0,""))
        let assy = {refid= "";specific= WaferCheckValveFlanges([valve;flange;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= WaferCheckValveFlanges [
            {refid= "";specific= WaferCheckValve(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,2)};
            {refid= "";specific= Nut(16.0,16)};
            {refid= "";specific= Washer(16.0,16)};
            {refid= "";specific= StudBolt(16.0,125.0,8)}]}

        Should.equal e y


    [<Fact>]
    member _.``03-10 - ExpansionFlanges test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(Expansion(100,1.0,""))
        let assy = {refid= "";specific= ExpansionFlanges([valve;flange;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= ExpansionFlanges [
            {refid= "";specific= Expansion(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-11 - ExpansionSolo test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(Expansion(100,1.0,""))
        let assy = {refid= "";specific= ExpansionSolo([valve;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= ExpansionSolo [
            {refid= "";specific= Expansion(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)};
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-12 - FlowmeterFlanges test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(Flowmeter(100,1.0,""))
        let assy = {refid= "";specific= FlowmeterFlanges([valve;flange;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= FlowmeterFlanges [
            {refid= "";specific= Flowmeter(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``03-13 - MagneticFilterFlanges test``() =
        let flange = {refid= "";specific= Flange(100,1.0,"")}
        let valve = comp(MagneticFilter(100,1.0,""))
        let assy = {refid= "";specific= MagneticFilterFlanges([valve;flange;flange])}
        let y = RouteComponentFasteners.appendFasteners assy
        output.WriteLine(stringify y)
        let e = {refid= "";specific= MagneticFilterFlanges [
            {refid= "";specific= MagneticFilter(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Flange(100.0,1.0,"")};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)};
            {refid= "";specific= Gasket(100.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}
            ]}

        Should.equal e y

    [<Fact>]
    member _.``04 - real test``() =
        let assy =
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
        let y = RouteComponentFasteners.appendFasteners assy
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
            {refid= "";specific= Flange(80.0,1.0,"pph")};
            {refid= "";specific= Gasket(80.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}]};
            {refid= "";specific= Pipe(448.3,80.0,1.0,"pph")};
            {refid= "";specific= SingleFlange [
            {refid= "";specific= Flange(80.0,1.0,"pph")};
            {refid= "";specific= Gasket(80.0,1.0,1)};
            {refid= "";specific= Nut(16.0,8)};
            {refid= "";specific= Bolt(16.0,70.0,8)}]};
            {refid= "";specific= Pipe(198.3,80.0,1.0,"pph")}])};
            {refid= "P0104B";specific= ComponentPart("Pump.SLDPRT","GWZ-65DM-11KW-K-SSIC-EX")};
            {refid= "";specific= ComponentPart("bag filter.SLDPRT","bag3,DN80")};
            {refid= "";specific= ComponentPart("V0102B.SLDPRT","Default")}])}

        Should.equal e y




