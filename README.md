# TermSuggestions

A .NET 8 library that ranks a list of candidate strings by their similarity to a given search term, using a sliding-window character-mismatch algorithm.

## How It Works

`TermSuggester` scores each candidate by sliding a window of the same length as the search term across the candidate and counting character mismatches. The minimum mismatch count over all windows becomes the candidate's score. Candidates shorter than the term are excluded.

Ties are broken by:
1. Closest length to the search term
2. Alphabetical order (case-insensitive)

## API

```csharp
IEnumerable<string> GetSuggestions(string term, IEnumerable<string> choices, int numberOfSuggestions);
```

| Parameter | Description |
|---|---|
| `term` | The search term to match against |
| `choices` | The pool of candidate strings |
| `numberOfSuggestions` | Maximum number of results to return |

Returns candidates ordered from best to worst match.

## Example

```csharp
var suggester = new TermSuggester();

var results = suggester.GetSuggestions(
    term: "gros",
    choices: new[] { "gros", "gras", "graisse", "agressif", "go", "ros" },
    numberOfSuggestions: 2);

// results: ["gros", "gras"]
```

## Project Structure

```
TermSuggestions/           # Library
  IAmTheTest.cs            # Interface definition
  TermSuggester.cs         # Implementation
TermSuggestions.Tests/     # xUnit test suite
  TermSuggesterTests.cs
```

## Requirements

- .NET 8 SDK

## Build & Test

```bash
dotnet build
dotnet test
```
