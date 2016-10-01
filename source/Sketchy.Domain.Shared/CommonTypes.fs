namespace Sketchy.Domain.Shared

module CommonTypes = 
    type Identity = 
        { Value : int }
        static member Empty = { Value = 0 }
