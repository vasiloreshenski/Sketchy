namespace Sketchy.Domain.ProjectManagment

open Sketchy.Domain.Shared.CommonTypes

type Workflow = 
    { Identity : Identity
      Name: Name }

[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Workflow = 
    let EmptyList = ([] : Workflow list)
    let IsContained workflow workflows = Seq.exists (fun w -> w.Identity = workflow.Identity) workflows

    let Create (name: Name) (workflowNames: Name list) = 
        
        ()
