namespace Sketchy.Common

module SequenceExtensions = 
    open Sketchy.Common.StringExtensions
    open System.Runtime.CompilerServices

    [<Extension>]
    type SequenceExtensions = 

        // True if the string is contained in the provided sequence of string in ignore case manner
        [<Extension>]
        static member IgnoreCaseContains(sequence: string seq) (str:string) = 
            let ignoreCaseSet = sequence |> Seq.map (fun x -> CaseInsensitiveString.Create x) |> Set.ofSeq
            let checkIfExistsSet = CaseInsensitiveString.Create str |> Set.singleton
            let isContained = Set.intersect ignoreCaseSet checkIfExistsSet
            isContained

