namespace Sketchy.Domain.Workflow.Tests

module ProjectsTests =
    open Xunit

    open Sketchy.Domain.Workflow

    [<Fact>]
    let ``Create project when name is valid should succeed``() = 
        let existingProjectNames = [""]
        let project = Projects.Create existingProjectNames "project Name"
        ()

    [<Fact>]
    let ``Create project when has forbidden charachter should raise exception``() = 
        let _ = Assert.Throws(typedefof<Projects.InvalidCharachtersPorjectNameException>, (fun() ->  ignore <| Projects.Create [""] "~!@EDS"))
        ()
