namespace FlangeAssemblyBOM
open System.Numerics

type TextPosition = 
    {
        text:string
        origin: float*float*float
        displacement: float*float*float
    }

    member this.cadfromsw() =
        // cad <- sw
        let cad (x,y,z) =
            let x1 = x * 1e3
            let y1 = z * -1e3
            let z1 = y * 1e3
            x1,y1,z1

        {
            text = this.text
            origin = cad this.origin
            displacement = cad this.displacement
        }

    member this.topView() =
        let x0,y0,z0 = this.origin
        let x1,y1,z1 = this.displacement

        let c0 = Complex(x0,y0)
        let c1 = Complex(x1,y1)
        let cc = c0+c1
        0,this.text,cc.Real,cc.Imaginary

    member this.sideView() =
        let x0,y0,z0 = this.origin
        let x1,y1,z1 = this.displacement

        let c0 = Complex(x0,z0)
        let c1 = Complex(x1,z1)
        let cc = c0+c1
        1,this.text,cc.Real,cc.Imaginary




