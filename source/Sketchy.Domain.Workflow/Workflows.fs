namespace Sketchy.Domain.Workflow

module Workflows = 
    open Sketchy.Domain.Shared.CommonTypes
    
    type Workflow = 
        { Identity : Identity }
        static member EmptyList: Workflow list = []
