namespace Sketchy.Common

module Result = 
    // Represents union type for operations that may succseed or may not
    type Result<'TValue, 'TError> = 
        | Value of 'TValue
        | Error of 'TError list


        
    type MaybeResultBuilder() = 
        member this.Bind(result, continuation) = 
            match result with
            | Value v -> continuation v
            | Error errors -> Error errors

        member this.Return(x) = 
            Value x

    let MaybeResult = new MaybeResultBuilder();
            