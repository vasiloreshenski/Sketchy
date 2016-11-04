namespace Sketchy.Domain.Project.Validation

module ProjectValidation = 
    open Sketchy.Common.StringExtensions
    
    let minCharachters = 2
    let maxCharachters = 120
    let allowedCharachters = "1234567890qwertyuiopasdfghjklzxcvbnm.-_ "
    
    /// Represents the result of project name validation
    type ProjectNameValidationResult = 
        | Valid            
        | EmptyOrNull        
        | InvalidCharachter
        | InvalidLength
        | Dublicate
    
    /// Checks if the name of the project is not already containd the provided sequence of project names
    let IsNameUnique projectNames name = 
        projectNames
        |> Seq.contains name
        |> not
    
    let ValidateProjectName (name : string) projectNames = 
        let hasForbiddenCharachters (original : string) (allowed : string) = 
            let originalSet = original.ToIgnoreCaseStringSet()
            let allowedSet = allowed.ToIgnoreCaseStringSet()
            let diff = Set.difference originalSet allowedSet
            diff.Count <> 0
        
        let result = 
            match name with
            | null -> EmptyOrNull
            | n when n.Length = 0 -> EmptyOrNull
            | n when n.Length < minCharachters || n.Length > maxCharachters -> InvalidLength
            | n when hasForbiddenCharachters n allowedCharachters -> InvalidCharachter
            | n when IsNameUnique projectNames name -> Dublicate
            | _ -> Valid
        
        result
