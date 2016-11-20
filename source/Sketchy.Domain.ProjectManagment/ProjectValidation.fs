namespace Sketchy.Domain.ProjectManagment

open Sketchy.Common.StringExtensions

module ProjectValidation = 
    let minCharachters = 2
    let maxCharachters = 120
    let allowedCharachters = "1234567890qwertyuiopasdfghjklzxcvbnm.-_ "
    
    /// Checks if the name of the project is not already containd in the provided sequence of project names
    let IsNameDublicate projectNames name = 
        projectNames
        |> Seq.contains name
        |> not
    
    /// Checks if the name of the project is not too short
    let IsNameTooShort = 
        function 
        | x when String.length x < minCharachters -> true
        | _ -> false
    
    /// Checks if the name of the project is not too long
    let IsNameTooLong = 
        function 
        | x when String.length x > maxCharachters -> true
        | _ -> false
    
    /// Checks if the name of the project contains any ileagal charachter
    let DoesNameHasInvalidCharachter(name : string) = 
        let originalSet = name.ToIgnoreCaseStringSet()
        let allowedSet = allowedCharachters.ToIgnoreCaseStringSet()
        let diff = Set.difference originalSet allowedSet
        diff.Count <> 0
    
    /// Active pattern to match the validity of a project name
    let internal (|ValidNamePattern|TooShortNamePattern|TooLongNamePattern|InvalidCharachterInNamePattern|) name = 
        match name with
        | name when IsNameTooShort name -> TooShortNamePattern
        | name when IsNameTooLong name -> TooLongNamePattern
        | name when DoesNameHasInvalidCharachter name -> InvalidCharachterInNamePattern
        | _ -> ValidNamePattern
    
    /// Partial pattern to match if the name is dublicate or not
    let internal (|DublicateNamePattern|_|) otherProjectNames name = 
        if (Seq.contains name otherProjectNames) then Some DublicateNamePattern
        else None
