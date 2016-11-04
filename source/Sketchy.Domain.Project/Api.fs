namespace Sketchy.Domain.Project

/// Exposes the public functionlity from related to projects, workflows and shapes
/// Provides capabilities to manupulate and create projects, workflows and shapes
module Api = 
    open Sketchy.Domain.Shared.CommonTypes

    /// Creates a new project
    let CreateProject(name : string) = ()

    /// Renames the project by given identity and name
    /// If no such project identity exception is raised
    let RenameProject(projectIdentity: Identity) (newName: string) = ()

    /// Deletes the project by given identity
    let DeleteProject(projectIdentity: Identity) = ()

    /// Restores a deleted project
    /// If the project is not in deleted state, this method has no effect
    let RestoreProject(projectIdentity: Identity) = ()
