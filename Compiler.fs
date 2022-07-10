module Compiler

open FParsec

type EjsNode = 
| ScriptTag of string
| OutputTag of string
| Html of string

module Grammar = 
    let everythingBetween delimStart delimEnd = delimStart >>? (manyCharsTill anyChar delimEnd)  
    let delim a = skipString a

    let parseScriptTag  = (everythingBetween (delim "<%") (delim "%>")) |>> ScriptTag
    let parseOutputTag = (everythingBetween (delim "<%=") (delim "%>")) |>> OutputTag

    let parseTags = parseOutputTag <|> parseScriptTag
    let parseHtml = manyCharsTill anyChar (lookAhead parseTags) |>> Html

    let grammar : Parser<EjsNode, unit> = parseTags <|> parseHtml

let parser = (many Grammar.grammar)

let compileNode (node: EjsNode) : string =
    match node with
    | ScriptTag (x) -> $"\t{x}"
    | OutputTag (x) -> $"__output += `\t${{{x}}}`;"
    | Html (x) -> $"\t__output += `{x}`;"

let compile (nodes: list<EjsNode>) : string = 
    let newline = System.Environment.NewLine
    let body = nodes |> List.fold  (fun soFar node -> $"{soFar}\n{compileNode node}") "let __output = '';"
    $"const f = () => {{{newline}{body}{newline}\treturn __output;{newline}}}"