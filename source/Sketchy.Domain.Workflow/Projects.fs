namespace Sketchy.Domain.Workflow

module private Validation = 
    open Sketchy.Common.StringExtensions
    
    type ProjectNameValidationResult = 
        | Valid
        | EmptyOrNull
        | InvalidCharachter
        | InvalidLength
    
    let ValidateProjectName(name : string) = 
        let minCharachters = 2
        let maxCharachters = 120
        let allowedCharachters = "1234567890qwertyuiopasdfghjklzxcvbnm.-_ "
        
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
            | _ -> Valid
        
        result

module internal Projects = 
    open Sketchy.Domain.Workflow.Workflows
    open Sketchy.Domain.Shared.CommonTypes
    
    exception EmptyOrNullProjectNameException
    
    exception InvalidCharachtersPorjectNameException of string
    
    exception InvalidLengthProjectNameException of string
    
    exception DublicateProjectNameException of string
    
    /// The project is main container for workflows
    /// Project is describing by identity and name
    type Project = 
        { Identity : Identity
          Name : string
          Workflows : Workflow list }
    
    /// Creates project by given name
    /// The project names list is used as validation against dublication of already existing projects with the same name
    /// If the name is not valid, or the name already exists exception is raised
    let Create (listOfPorjectNames : string list) name = 
        let nameValidationResult = Validation.ValidateProjectName name
        let isProjectNameUnique = listOfPorjectNames |> List.contains name |> not
        match nameValidationResult with
        | Validation.Valid when isProjectNameUnique -> 
            { Identity = Identity.Empty
              Name = name
              Workflows = [] }
        | Validation.Valid -> raise (DublicateProjectNameException name)
        | Validation.EmptyOrNull -> raise (EmptyOrNullProjectNameException)
        | Validation.InvalidLength -> raise (InvalidLengthProjectNameException name)
        | Validation.InvalidCharachter -> raise (InvalidCharachtersPorjectNameException name)
    
    /// Renames the specified project
    /// If the project name is invalid or already exists exception is raised
    let Rename (project : Project) (listOfProjectNames : string list) (name : string) = 
        let renamedProject = Create listOfProjectNames name
        
        let withAttachedWorkflows = 
            { renamedProject with Workflows = project.Workflows
                                  Identity = project.Identity }
        withAttachedWorkflows
