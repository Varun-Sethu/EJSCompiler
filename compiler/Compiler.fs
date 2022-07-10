module Compiler

open Parser
open System
open FParsec

let output = "__output"
let newline = Environment.NewLine 

let compileEjsNode (node: EjsNode) : string = 
    match node with
    | Comment _ -> ""
    | Script x -> $"{x} {newline}"
    | Html x -> $"{output} += `{x}`; {newline}"
    | OutputNoEscape x -> $"{output} += $`{{{x}}}`; {newline}"
    | Output x -> $"{output} += $`{{escape({x})}}`; {newline}"

let compileNodes (nodes: list<EjsNode>) : string = 
    let compiledBody = nodes |> List.fold (fun soFar node -> soFar + compileEjsNode node) ""
    sprintf @"
    const compiled = () => {
        let __output = '';
        %s
        return %s;
    }" compiledBody output

let compile (x: string) : string option = 
    let parsedData = run parser x
    match parsedData with
    | Success (v, _, _) -> Some (compileNodes v)
    | Failure (msg, _, _) -> 
        printf "%s" msg
        None