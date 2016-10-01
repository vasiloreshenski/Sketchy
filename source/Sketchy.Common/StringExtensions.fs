namespace Sketchy.Common

module StringExtensions = 
    open System.Runtime.CompilerServices
    
    /// Wrapper for strings with OrdinalIgnoreCase functionality
    [<CustomEquality; CustomComparison>]
    type CaseInsensitiveString = 
        { Value : string }
        override this.Equals other = 
            match other with
            | :? CaseInsensitiveString as cis -> (this :> CaseInsensitiveString System.IEquatable).Equals cis
            | _ -> false
        override this.GetHashCode() = System.StringComparer.OrdinalIgnoreCase.GetHashCode(this.Value)
        
        interface CaseInsensitiveString System.IEquatable with
            member this.Equals other = System.StringComparer.OrdinalIgnoreCase.Equals(this.Value, other.Value)

        interface System.IComparable with
            member this.CompareTo other = 
                match other with
                | :? CaseInsensitiveString as cis -> System.StringComparer.OrdinalIgnoreCase.Compare(this.Value, cis.Value)
                | _ -> raise (invalidArg "other" "Can't be of type diff then CaseInsensitiveString")
        
        interface CaseInsensitiveString System.IComparable with
            member this.CompareTo(other : CaseInsensitiveString) = this.Value.CompareTo other.Value
        
        static member Create(str : string) = { Value = str }
    
    /// Returns set of ignore cases aware strings, where each string in the set is a charachter from the original string
    [<Extension>]
    type StringExtensions() = 
        [<Extension>]
        static member ToIgnoreCaseStringSet(str : string) = 
            str
            |> Seq.map (fun s -> { Value = s.ToString() })
            |> Set.ofSeq
