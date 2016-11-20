namespace Sketchy.Domain.ProjectManagment

open Sketchy.Common
open Sketchy.Common.ResultCore
open Sketchy.Domain.ProjectManagment.Workflow
open Sketchy.Domain.Shared
open Sketchy.Domain.Shared.CommonTypes

/// The project is main container for workflows
/// Project is describing by identity and name
type Project = 
    { Identity : Identity
      Name : Name
      Workflows : Workflow.Workflow list }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module Project = 

    type IProjectCreateOrRenameError =  interface end

    type IProjectDeleteError = interface end
    
    /// Possible errors when operating with project
    /// The result of creation of project is represented by a succes with the created project or by some of the following errors
    type ProjectCreateOrRenameError = 
        /// Representing error for too short name when creating or renaming project
        | TooShortName of Name
        /// Represents error for too long name when creating or renaming project
        | TooLongName of Name
        /// Represnts error for dublicate name when tring to create or rename project with name that is already taken
        | DublicateName of Name
        /// Represents error when the name contains invalid charachter while creating or renaming project
        | InvalidCharachterInName of Name
        /// Repesents error which indicats empty identity for project when creating it
        | EmptyIdentity
        interface IProjectCreateOrRenameError
    
    /// Creates new project
    let private CreateInternal identity name = 
        { Identity = identity
          Name = name
          Workflows = Workflow.EmptyList }
    
    /// Creates Error Result for invalid name
    let private InvalidName (factory: Name -> ProjectCreateOrRenameError) name = Error((factory name) :> IProjectCreateOrRenameError)

    /// Creates project by given name
    /// The result is an option type which fails if the name of the project or the identity are not valid or unique
    let Create (identity : Identity) name (otherProjectNames : Name list) = 
        let creationResult = 
            match identity, name with
            | _, ProjectValidation.DublicateNamePattern otherProjectNames -> InvalidName DublicateName name
            | IdentityCore.IsNotEmpty, ProjectValidation.ValidNamePattern -> Value(CreateInternal identity name)
            | _, ProjectValidation.TooShortNamePattern -> InvalidName TooShortName name
            | _, ProjectValidation.TooLongNamePattern -> InvalidName TooLongName name
            | _, ProjectValidation.InvalidCharachterInNamePattern -> InvalidName InvalidCharachterInName name
            | IdentityCore.IsEmpty, ProjectValidation.ValidNamePattern -> Error (EmptyIdentity :> IProjectCreateOrRenameError)
        creationResult
    
    /// Renames the specified project
    /// The result is an option type which fails if the name of the project is not valid
    let Rename project name otherProjectNames = 
        Result { 
            let! renamed = Create project.Identity name otherProjectNames
            let renamed = { renamed with Workflows = project.Workflows }
            return renamed
        }
    
    /// Appends the workflow to the project
    /// The result is an option type whith the workflow attached to the project.
    /// It fails if the workflow is already attached to the project
    let AppendWorkflow project workflow = 
        match workflow with
        | w when Workflow.IsContained w project.Workflows -> None
        | w -> Some { project with Workflows = w :: project.Workflows }
