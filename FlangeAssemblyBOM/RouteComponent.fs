namespace rec FlangeAssemblyBOM.Routing

type RouteComponent =
    {
        refid: string
        specific: RouteComponentSpecific
    }

type RouteComponentSpecific =
    | RouteAssembly of title:string * children:RouteComponent list
    | ComponentAssembly of title:string * refconfig:string * children:RouteComponent list
    | ComponentPart of title:string * refconfig:string

    | Pipe  of length:float * dn:float * pn:float * material:string
    | Elbow of angle:float * dn:float * pn:float * material:string
    | Tee of dn:float * pn:float * material:string
    | Flange of dn:float * pn:float * material:string
    | Reducer          of dn1:float * dn2:float * pn:float * material:string
    | EccentricReducer of dn1:float * dn2:float * pn:float * material:string
    | ReducingTee      of dn1:float * dn2:float * pn:float * material:string

    | BallValve           of dn:float * pn:float * material:string
    | WaferButterflyValve of dn:float * pn:float * material:string
    | WaferCheckValve     of dn:float * pn:float * material:string
    | Expansion           of dn:float * pn:float * material:string
    | Flowmeter           of dn:float * pn:float * material:string
    | MagneticFilter      of dn:float * pn:float * material:string

    | SingleFlange               of children:RouteComponent list
    | Flanges                    of children:RouteComponent list
    | BallValveFlanges           of children:RouteComponent list
    | BallValveSolo              of children:RouteComponent list
    | WaferButterflyValveFlanges of children:RouteComponent list
    | WaferButterflyValveSolo    of children:RouteComponent list
    | WaferCheckValveFlanges     of children:RouteComponent list
    | ExpansionFlanges           of children:RouteComponent list
    | ExpansionSolo              of children:RouteComponent list
    | FlowmeterFlanges           of children:RouteComponent list
    | MagneticFilterFlanges      of children:RouteComponent list

    | Gasket of dn:float * pn:float * count:int
    | Bolt of m:float * l:float * count:int
    | Nut of m:float * count:int
    | StudBolt of m:float * l:float * count:int
    | Washer of  m:float * count:int

    member specific.toLine () =
        match specific with
        | RouteAssembly (title:string, children:RouteComponent list) ->
            $"RouteAssembly {title}" 
        | ComponentAssembly (title:string, refconfig:string, children:RouteComponent list) ->
            $"ComponentAssembly {title}({refconfig})"
        | ComponentPart (title:string, refconfig:string) ->
            $"ComponentPart {title}({refconfig})"
        | Pipe (length:float, dn:float, pn:float, material:string) ->
            $"Pipe {length}mm DN {dn} PN {pn} {material}"

        | Elbow (angle:float, dn:float, pn:float, material:string) ->
            $"Elbow {angle}degree DN {dn} PN {pn} {material}"

        | Tee (dn:float, pn:float, material:string) ->
            $"Tee DN {dn} PN {pn} {material}"

        | Flange (dn:float, pn:float, material:string) ->
            $"Flange DN {dn} PN {pn} {material}"

        | Reducer (dn1:float, dn2:float, pn:float, material:string) ->
            $"Reducer DN {dn1} x {dn2} PN {pn} {material}"

        | EccentricReducer (dn1:float, dn2:float, pn:float, material:string) ->
            $"EccentricReducer DN {dn1} x {dn2} PN {pn} {material}"

        | ReducingTee (dn1:float, dn2:float, pn:float, material:string) ->
            $"ReducingTee DN {dn1} x {dn2} PN {pn} {material}"

        | BallValve (dn:float, pn:float, material:string) ->
            $"BallValve DN {dn} PN {pn} {material}"

        | WaferButterflyValve (dn:float, pn:float, material:string) ->
            $"WaferButterflyValve DN {dn} PN {pn} {material}"

        | WaferCheckValve (dn:float, pn:float, material:string) ->
            $"WaferCheckValve DN {dn} PN {pn} {material}"

        | Expansion (dn:float, pn:float, material:string) ->
            $"Expansion DN {dn} PN {pn} {material}"

        | Flowmeter (dn:float, pn:float, material:string) ->
            $"Flowmeter DN {dn} PN {pn} {material}"

        | MagneticFilter (dn:float, pn:float, material:string) ->
            $"MagneticFilter DN {dn} PN {pn} {material}"

        | SingleFlange (children:RouteComponent list) ->
            $"SingleFlange"

        | Flanges (children:RouteComponent list) ->
            $"Flanges"

        | BallValveFlanges (children:RouteComponent list) ->
            $"BallValveFlanges"

        | BallValveSolo (children:RouteComponent list) ->
            $"BallValveSolo"

        | WaferButterflyValveFlanges (children:RouteComponent list) ->
            $"WaferButterflyValveFlanges"

        | WaferButterflyValveSolo (children:RouteComponent list) ->
            $"WaferButterflyValveSolo"

        | WaferCheckValveFlanges (children:RouteComponent list) ->
            $"WaferCheckValveFlanges"

        | ExpansionFlanges (children:RouteComponent list) ->
            $"ExpansionFlanges"

        | ExpansionSolo (children:RouteComponent list) ->
            $"ExpansionSolo"

        | FlowmeterFlanges (children:RouteComponent list) ->
            $"FlowmeterFlanges"

        | MagneticFilterFlanges (children:RouteComponent list) ->
            $"MagneticFilterFlanges"

        | Gasket (dn:float, pn:float, count:int) ->
            $"Gasket DN {dn} PN {pn}"

        | Bolt (m:float, l:float, count:int) ->
            $"Bolt M {m} x {l}, {count}"

        | Nut (m:float, count:int) ->
            $"Nut M {m}, {count}"

        | Washer ( m:float, count:int) ->
            $"Washer M {m}, {count}"

        | StudBolt (m:float, l:float, count:int) ->
            $"StudBolt M {m} x {l}, {count}"
