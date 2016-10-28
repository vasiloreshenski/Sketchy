namespace Sketchy.Domain.Workflow

module ProjectValidation = 
    open Sketchy.Common.StringExtensions
    
    type ProjectNameValidationResult = 
        | Valid
        | EmptyOrNull
        | InvalidCharachter
        | InvalidLength
        | Dublicate
    
    let IsNameUnique projectNames name = 
        projectNames
        |> Seq.contains name
        |> not


    let ValidateProjectName(name : string) projectNames = 
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
            | n when IsNameUnique projectNames name -> Dublicate
            | _ -> Valid
        
        result
    

module internal Projects = 
    open Sketchy.Common.Result
    open Sketchy.Domain.Shared.CommonTypes
    open Sketchy.Domain.Workflow.Workflows
    
    /// The project is main container for workflows
    /// Project is describing by identity and name
    type Project = 
        { Identity : Identity
          Name : string
          Workflows : Workflow list }
        // Creates new project record with empty identity
        static member CreateWithoutIdentity name workflows = 
            { Identity = Identity.Empty
              Name = name
              Workflows = workflows }
    
    /// Creates project by given name and workflows
    /// The result is an option type which fails if the name of the project is not valid
    let CreateWithWorkflows (listOfProjectNames : string list) name (workflows : Workflow list) = 
        let nameValidationResult = ProjectValidation.ValidateProjectName name listOfProjectNames
        match nameValidationResult with
        | ProjectValidation.Valid -> Value(Project.CreateWithoutIdentity name workflows)
        | error -> Error [ error ]
    
    /// Creates project by given name
    /// The result is an option type which fails if the name of the project is not valid
    let Create (listOfProjectNames : string list) name = MaybeResult { let! project = CreateWithWorkflows listOfProjectNames name Workflow.EmptyList
                                                                       return project }
    
    /// Renames the specified project
    /// The result is an option type which fails if the name of the project is not valid
    let Rename (project : Project) (listOfProjectNames : string list) (name : string) = MaybeResult { let! renamed = Create listOfProjectNames name
                                                                                                      return renamed }
