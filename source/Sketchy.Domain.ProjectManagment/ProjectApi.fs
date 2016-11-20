namespace Sketchy.Domain.ProjectManagment

open Sketchy.Common
open Sketchy.Domain.Shared.CommonTypes

/// Provides capabilities to manupulate and create projects and workflows
[<RequireQualifiedAccess>]
module ProjectApi = 

    type ProjectOperationMessages = 
        /// Message indicating that project was created
        | ProjectCreated of Identity
        /// Message indicating that project was renamed
        | ProjectRenamed of Identity * Name
        /// Message indicating that creation of project has failed with error
        | ProjectNotCreated of Project.IProjectCreateOrRenameError
        /// Message indicating that renaming of project has failed
        | ProjectNotRenamed of Project.IProjectCreateOrRenameError
        /// Messsage indicating that project has been deleted
        | ProjectDeleted of Identity
        /// Message indicating that project was not deleted
        | ProjectNotDeleted of Project.IProjectDeleteError
        /// Message indicating that project was restored
        | ProjectRestored of Identity
        /// Message indicating that project was not restored
        | ProjectNotRestored of Project.IProjectRestoreError
    
    /// Describies interface for message passing
    /// Used to publish a message containing information about the success or failiure of given operation
    type IPublishMessage = ProjectOperationMessages -> unit
    
    /// Creates a new project and persist the result
    /// A succsses or failiure message is send thru the publish message interface to when the operation ended
    /// Failiure is expected if the identity gen, persist project or get project names interfaces failed to return valid value
    let Create 
            (name : Name) 
            (publishMessage : IPublishMessage) 
            (identityGen : ProjectRepository.IGenProjectIdentityAsync) 
            (projectPersist : ProjectRepository.IPersistProjectAsync) 
            (fetchProjectNames : ProjectRepository.IFetchProjectNamesAsync) = 
        
        let generalizationFunc = (fun e -> e :> Project.IProjectCreateOrRenameError)

        let identityGen = Result.generalizeError generalizationFunc identityGen
        let fetchProjectNames = Result.generalizeError generalizationFunc fetchProjectNames
        let projectPersist = Result.generalizeError1 generalizationFunc projectPersist

        let onError = (fun e -> publishMessage (ProjectNotCreated e))
        let onReturn = (fun v -> publishMessage (ProjectCreated v.Identity))

        // workflow for handling async result types with message sending mechanisms
        let projectCreation = AsyncResultCallback.AsyncResultCallbackBuilder(onReturn, onError)
        projectCreation { 
            let! identity = identityGen()
            let! projectNames = fetchProjectNames()
            let! project = AsyncResult.FromResult(Project.Create identity name projectNames)
            let! _ = projectPersist project
            return project
        }
    
    /// Renames the project by given identity and name
    let Rename 
            (identity : Identity) 
            (name : Name) 
            (publishMessage : IPublishMessage) 
            (fetchProjectByIdentity :ProjectRepository.IFetchProjectByIdentityAsync) 
            (fetchProjectNames : ProjectRepository.IFetchProjectNamesAsync) 
            (updateProjectName : ProjectRepository.IUpdateProjectNameAsync) =
         
        let generalizationFunc = (fun e -> e :> Project.IProjectCreateOrRenameError)
       
        let fetchProjectByIdentity = Result.generalizeError1 generalizationFunc fetchProjectByIdentity
        let fetchProjectNames = Result.generalizeError generalizationFunc fetchProjectNames
        let updateProjectName = Result.generalizeError2 generalizationFunc updateProjectName

        let onError = (fun e -> publishMessage (ProjectNotRenamed e))
        let onRenamed = (fun v -> publishMessage (ProjectRenamed (v.Identity, v.Name)))
        let projectRename = AsyncResultCallback.AsyncResultCallbackBuilder(onRenamed, onError)
        projectRename {
            let! project = fetchProjectByIdentity identity
            let! projectNames = fetchProjectNames()
            let! renamedProject = AsyncResult.FromResult(Project.Rename project name projectNames)
            let! renamedPersistanceResult = updateProjectName renamedProject.Identity renamedProject.Name
            return renamedPersistanceResult
        }
    
    /// Deletes the project by given identity
    let Delete
            (identity : Identity)
            (fetchProject: ProjectRepository.IFetchProjectByIdentityAsync)
            (updateProjectState: ProjectRepository.IUpdateProjectStateAsync)
            (publishMessage: IPublishMessage) = 
    
        let generalizationFunc = (fun e -> e :> Project.IProjectDeleteError)
        let updateProjectState = Result.generalizeError2 generalizationFunc updateProjectState
        let fetchProject = Result.generalizeError1 generalizationFunc fetchProject
        
        let onDelete = (fun project -> publishMessage (ProjectDeleted project.Identity))
        let onError = (fun error -> publishMessage (ProjectNotDeleted error))
        let projectDelete = AsyncResultCallback.AsyncResultCallbackBuilder(onDelete, onError)
        projectDelete {
            let! project = fetchProject identity
            let deletedProject = Project.Delete project
            let! deletionResult = updateProjectState deletedProject.Identity deletedProject.State
            return deletionResult
        }
    
    /// Restores a deleted project
    /// If the project is not in deleted state, this method has no effect
    let Restore
            (identity : Identity) 
            (fetchProject: ProjectRepository.IFetchProjectByIdentityAsync)
            (updateProjectState: ProjectRepository.IUpdateProjectStateAsync) 
            (publishMessage: IPublishMessage) = 

        let generalizationFunc = (fun e -> e :> Project.IProjectRestoreError)
        let updateProjectState = Result.generalizeError2 generalizationFunc updateProjectState
        let fetchProject = Result.generalizeError1 generalizationFunc fetchProject

        let onRestore = (fun p -> publishMessage (ProjectRestored identity))
        let onError = (fun e -> publishMessage (ProjectNotRestored e))
        let projectRestore = AsyncResultCallback.AsyncResultCallbackBuilder(onRestore, onError)
        projectRestore {
            let! project = fetchProject identity
            let restoredProject = Project.Restore project
            let! restoreResult = updateProjectState restoredProject.Identity restoredProject.State
            return restoreResult
        }
        

    let Save(identity: Identity) = ()

    let AttachWorkflow
        (identity: Identity) 
        (workflowName: Name) = 
        
        ()

    let RemoveWorkflow(workflowIdentity: Identity) = ()
