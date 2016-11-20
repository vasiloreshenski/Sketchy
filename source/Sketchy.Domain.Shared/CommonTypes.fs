namespace Sketchy.Domain.Shared

open System.Diagnostics

module CommonTypes = 

    [<DebuggerDisplay("{Value}")>]
    /// Represents unique number
    type Identity = 
        { Value : int }
        static member Empty = { Value = 0 }
        member this.IsEmpty() = this.Value = 0
        member this.IsNotEmpty() = not (this.IsEmpty())
    
    type Name = string

    module IdentityCore = 
        let (|IsEmpty|IsNotEmpty|) (identity : Identity) = 
            if identity.IsEmpty() then IsEmpty
            else IsNotEmpty
