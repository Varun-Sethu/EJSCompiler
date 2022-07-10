module Parser

open FParsec

type EjsNode = 
| Script of string
| Output of string
| OutputNoEscape of string
| Comment of string
| Html of string

let parseBetween start stop = start >>? (manyCharsTill anyChar stop)

// End deliminators -> captures the end of an EJS tag
let plainEnding = skipString "%>"
let slurpNewLine = skipString "-%>" .>> attempt skipNewline 
let slurpWhitespace = skipString "_%>" .>> attempt spaces

let ending = 
    choice [
        plainEnding
        slurpNewLine
        slurpWhitespace
    ]

// Start deliminators -> captures the start of an EJS tag
let outputStart = skipString "<%=".>> attempt spaces
let outputStartNoEscape = skipString "<%-" .>> attempt spaces
let commentStart = skipString "<%#"
let scriptStart = skipString "<%" .>> attempt spaces

let parseTags = 
    choice [
        parseBetween outputStart ending |>> Output
        parseBetween outputStartNoEscape ending |>> OutputNoEscape
        parseBetween commentStart ending |>> Comment
        parseBetween scriptStart ending |>> Script
    ]

// Parse regular HTML blocks
let parseHtml = many1CharsTill anyChar ((followedBy parseTags) <|> (followedBy eof)) |>> Html
// Our final combined parser :D
let parser: Parser<list<EjsNode>, unit> = many (parseTags <|> parseHtml)