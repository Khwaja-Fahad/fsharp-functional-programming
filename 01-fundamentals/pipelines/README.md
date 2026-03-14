# Pipelines in F#

This project demonstrates the pipeline operator `|>` in F#.

Pipelines allow chaining operations so that data flows through multiple functions in a readable way.

Example:

numbers
|> List.map
|> List.filter
|> List.sum
