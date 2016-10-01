namespace Sketchy.Domain.Workflow

/// Exposes the public functionlity from related to projects, workflows and shapes
/// Provides capabilities to manupulate and create projects, workflows and shapes
module Api = 
    open Sketchy.Domain.Shared.CommonTypes

    /// Creates a new project
    let CreateProject(name : string) = ()

    /// Renames the project with the specified identity
    /// If no such project identity exception is raised
    /// If the project name is not valid exception is raised
    let RenameProject(projectIdentity: Identity) (newName: string) = ()

    /// Deletes the project with the specified identity
    /// If no such project identity exception is raised
    let DeleteProject(projectIdentity: Identity) = ()

    /// Restores a deleted project
    /// If the project is not in deleted state, this method has no effect
    /// If no such project identity exception is raised
    let RestoreProject(projectIdentity: Identity) = ()

// create project
// load project
// save changes for project
// attach / remove workflow to project
// save changes to workflow
// attach / remove shape to workflow
// move shape
