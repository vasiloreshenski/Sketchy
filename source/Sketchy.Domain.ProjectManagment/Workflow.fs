namespace Sketchy.Domain.ProjectManagment.Workflow

open Sketchy.Domain.Shared.CommonTypes

module Workflow = 
    type Workflow = 
        { Identity : Identity }

    let EmptyList = ([]: Workflow list)

    let IsContained workflow workflows = 
        Seq.exists (fun w -> w.Identity = workflow.Identity) workflows