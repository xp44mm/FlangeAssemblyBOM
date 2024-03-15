module FlangeAssemblyBOM.Dir

open System.IO

//let thisPath = __SOURCE_DIRECTORY__
let databasePath = Path.Combine(__SOURCE_DIRECTORY__,"database")

let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName

let CommandDataPath = Path.Combine(solutionPath,"CommandData")

    //// 附件accessories
    //| Gasket of dn:float * count:int
    //| Nut of m: float * count:int
    //| Washer of m: float * count:int
    //| Bolt of m:float * l:float * count:int
    //| StudBolt of m:float * l:float * count:int
