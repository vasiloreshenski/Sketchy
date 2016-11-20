namespace Sketchy.Domain.ProjectManagment

open Sketchy.Domain.Shared.CommonTypes
open Sketchy.Common
open Sketchy.Common.ResultCore

[<RequireQualifiedAccess>]
module ProjectRepositoryError = 
    
    type SearchFailed = 
        | NotFound of Identity 
        | Failed
        interface Project.IProjectCreateOrRenameError
        interface Project.IProjectRestoreError
        interface Project.IProjectDeleteError
    
    type IdentityGenerationFailed = IdentityGenerationFailed interface Project.IProjectCreateOrRenameError

    type PersistanceFailed = PersistanceFailed of Identity interface Project.IProjectCreateOrRenameError

    type FetchFailed = FetchFailed of Identity
                        interface Project.IProjectCreateOrRenameError
                        interface Project.IProjectDeleteError
                        interface Project.IProjectRestoreError

    type UpdateFailed = UpdateFailed of Identity 
                            interface Project.IProjectCreateOrRenameError 
                            interface Project.IProjectDeleteError
                            interface Project.IProjectRestoreError

    type DeleteFailed = DeleteFailed of Identity interface Project.IProjectDeleteError

    type RestoreFailed = RestoreFailed of Identity interface Project.IProjectRestoreError

/// Exposes interfaces for communication bewtween the project api and a persistance storage
[<RequireQualifiedAccess>]
module ProjectRepository = 

    /// Interface to generating identity object
    /// Used by the creation functions to set the identity of the newly created project
    type IGenProjectIdentityAsync = unit -> Async<Result<Identity, ProjectRepositoryError.IdentityGenerationFailed>>
    
    /// Interface for persistance of newly created project
    type IPersistProjectAsync = Project -> Async<Result<Identity, ProjectRepositoryError.PersistanceFailed>>
    
    /// Interface for providing the names of all persisted projects
    type IFetchProjectNamesAsync = unit -> Async<Result<Name list, ProjectRepositoryError.FetchFailed>>
    
    /// Interface for retreaving project by identity
    type IFetchProjectByIdentityAsync = Identity -> Async<Result<Project, ProjectRepositoryError.SearchFailed>>

    /// Interfaces for pesisting the name of a project by given identity
    type IUpdateProjectNameAsync = Identity -> Name -> Async<Result<Project, ProjectRepositoryError.UpdateFailed>>

    /// Interfaces for persisting the state of a project by identity
    type IUpdateProjectStateAsync = Identity -> ProjectState -> Async<Result<Project, ProjectRepositoryError.UpdateFailed>>
    

