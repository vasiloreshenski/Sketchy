namespace Sketchy.Domain.Workflow

// Exposes domain specific logic for shapes in the context of an workflow
module internal Shapes = 
    open Sketchy.Domain.Shared.CommonTypes
    
    // represents position in 2d space
    type Position = 
        { x : float
          y : float }
    
    // represents size in 2d space
    type Size = 
        { width : float
          height : float }
    
    // Union describing possible shapes in a workflow
    type Shape = 
        | Circle of workflowIdentity : Identity * identity : Identity * radius : float * center : Position
        | Rectangle of workFlowIdentity : Identity * identity : Identity * size : Size * position : Position
