namespace Niancat.Utilities

module File =

    let readAllLines = System.IO.File.ReadAllLines >> List.ofArray >> List.filter (String.isEmpty >> not)

    let appendLine path line = System.IO.File.AppendAllText(path, line)