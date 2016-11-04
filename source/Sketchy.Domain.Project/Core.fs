namespace Sketchy.Domain.Project.Core

module internal Projects = 
    open Sketchy.Common.Result
    open Sketchy.Domain.Shared.CommonTypes
    open Sketchy.Domain.Project.Validation
    open Sketchy.Domain.Workflow.Workflows
    
    /// The project is main container for workflows
    /// Project is describing by identity and name
    type Project = 
        { Identity : Identity
          Name : string
          Workflows : Workflow list }

    /// Creates new project with empty identity
    let CreateWithoutIdentity name workflows = 
        { Identity = Identity.Empty
          Name = name
          Workflows = workflows }
    
    /// Creates project by given name and workflows
    /// The result is an option type which fails if the name of the project is not valid
    let CreateWithWorkflows (listOfProjectNames : string list) name (workflows : Workflow list) = 
        let nameValidationResult = ProjectValidation.ValidateProjectName name listOfProjectNames
        match nameValidationResult with
        | ProjectValidation.Valid -> Value(CreateWithoutIdentity name workflows)
        | error -> Error [ error ]
    
    /// Creates project by given name
    /// The result is an option type which fails if the name of the project is not valid
    let Create (listOfProjectNames : string list) name = MaybeResult { let! project = CreateWithWorkflows listOfProjectNames name Workflow.EmptyList
                                                                       return project }
    
    /// Renames the specified project
    /// The result is an option type which fails if the name of the project is not valid
    let Rename (project : Project) (listOfProjectNames : string list) (name : string) = MaybeResult { let! renamed = Create listOfProjectNames name
                                                                                                      return renamed }
