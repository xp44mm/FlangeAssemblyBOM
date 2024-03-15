module FlangeAssemblyBOM.Dir

open System.IO

let dinfo = DirectoryInfo(__SOURCE_DIRECTORY__)

let solutionPath = dinfo.Parent.FullName

let xp44mm = dinfo.Parent.Parent.FullName

let CommandData = Path.Combine(xp44mm, @"SwCSharpAddin1\CommandData")
