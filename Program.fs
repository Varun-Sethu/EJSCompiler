module Program

open FParsec
open Compiler
open System.IO

let stripExtension (file: string) : string = 
    Path.GetFileNameWithoutExtension (file)

let directory (file: string) : string = 
    Path.GetDirectoryName (file)

[<EntryPoint>]
let main argv =
    let file = argv[0]
    
    let compiledJs = compile (File.ReadAllText file)
    
    match compiledJs with
    | Some (result) ->
        File.WriteAllText ($"{directory file}/{stripExtension file}__compiled.js", result)
        0
    | None ->
        printf "%s" "failed to parse requested file."
        1