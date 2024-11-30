namespace FlangeAssemblyBOM
open FSharp.Idioms.Jsons

type Nozzle = 
    {
    name: string
    config: string
    root: float*float*float
    face: float*float*float
    }

    static member just( name:string, config:string, root: float*float*float, face: float*float*float ) = 
        {
            name = name
            config = config
            root = root
            face = face
        }

    static member fromJson( json:Json ) = 
        let name = json.["name"].stringText
        let config = json.["config"].stringText
        let root = 
            let arr = 
                json.["root"].elements 
                |> List.map(fun json -> json.floatValue)
            arr.[0],arr.[1],arr.[2]
        let face = 
            let arr = 
                json.["face"].elements 
                |> List.map(fun json -> json.floatValue)
            arr.[0],arr.[1],arr.[2]

        {
        name = name
        config = config
        root = root
        face = face
        }
        
type Tank =
    {
        typ: string
        diameter: float
        height: float
        ventilateNozzle: bool // 呼吸管
        agitatorNozzle: bool // 搅拌器安装口
        rectNozzle: string option // 矩形接口config
        nozzles: Nozzle list // 法兰管嘴
    }

    static member fromJson(json:Json ) = 
        let typ = json.["typ"].stringText
        let diameter = json.["diameter"].floatValue
        let height = json.["height"].floatValue
        let ventilateNozzle = json.["ventilateNozzle"].boolValue
        let agitatorNozzle = json.["agitatorNozzle"].boolValue
        let rectNozzle = 
            json.["rectNozzle"].elements 
            |> List.map(fun json -> json.stringText)
            |> List.tryHead
        let nozzles =
            json.["nozzles"].elements
            |> List.map(fun json -> 
                Nozzle.fromJson json
            )
            
        {
            typ = typ
            diameter = diameter
            height = height
            ventilateNozzle = ventilateNozzle
            agitatorNozzle = agitatorNozzle
            rectNozzle = rectNozzle
            nozzles = nozzles
        }

type TankComponent = 
    {
        file: string
        origin: float*float*float
        tank:Tank
    }

    static member fromJson(json:Json ) = 
        let file = json.["file"].stringText
        let origin = 
            let arr = 
                json.["origin"].elements 
                |> List.map(fun json -> json.floatValue)
            arr.[0],arr.[1],arr.[2]

        let tank = Tank.fromJson json
        {
            file = file
            origin = origin
            tank = tank
        }
    member tankComp.getTankNozzleRecords() =
        tankComp.tank.nozzles
        |> List.map(fun nz ->
            let axis = {
                root = nz.root
                face = nz.face
            }
            let rtx,rty,rtz = nz.root
            let fcx,fcy,fcz = nz.face

            //let el = axis.elevation * 1e3
            {
                file = tankComp.file
                name = nz.name
                config = nz.config
                rootx = rtx
                rooty = rty
                rootz = rtz
                facex = fcx
                facey = fcy
                facez = fcz
                elev = axis.elevation
                slope = axis.slope
                rootDirection = axis.direction
                axisDirection = axis.AxisDirection
            })
