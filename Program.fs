module Program

open FParsec
open Parser
open System.IO

let stripExtension (file: string) : string = 
    Path.GetFileNameWithoutExtension (file)

[<EntryPoint>]
let main argv =
    let file = argv[0]
    let result = (run parser (File.ReadAllText file))
    
    match result with
    | Success(v, _, _) ->
        // let compiledResult = (compile v)
        // File.WriteAllText ($"{stripExtension file}__compiled.js", compiledResult)
        printf "%A" result
        0
    | Failure (msg, err, _) ->
        printf "%s" msg
        1