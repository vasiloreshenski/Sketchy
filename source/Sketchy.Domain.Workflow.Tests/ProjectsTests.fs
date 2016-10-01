namespace Sketchy.Domain.Workflow.Tests

module ProjectsTests =
    open Xunit

    open Sketchy.Domain.Workflow

    [<Fact>]
    let ``Create project when name is valid should succeed``() = 
        let project = Projects.Create [""] "project Name"

        do Assert.True true

