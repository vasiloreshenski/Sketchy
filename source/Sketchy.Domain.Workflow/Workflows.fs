namespace Sketchy.Domain.Workflow

// Exposes the Worflow type and domain specific logic related to it
module internal Workflows = 
    open System
    open Sketchy.Domain.Workflow
    open Sketchy.Domain.Shared
    
    // The workflow is the main shape container. 2d space for related shapes
    type Workflow = 
        { Identity : CommonTypes.Identity
          Shapes : Shapes.Shape list }
          static member EmptyList: Workflow list = list.Empty
          
    
    // Create workflow with the specified identity
    let Create(identity : CommonTypes.Identity) = 
        { Identity = identity
          Shapes = [] }
