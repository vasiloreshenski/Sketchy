namespace Sketchy.Domain.Project.Tests

module ProjectsTests = 
    open Sketchy.Common.Result
    open Sketchy.Domain.Project.Core
    open Sketchy.Domain.Project.Validation
    open Xunit
    
    let Success() = Assert.True true
    let Failed() = Assert.True false
    let HasError expectedError errors = errors |> List.contains expectedError
    
    [<Fact>]
    let ``Create project when name is valid should succeed``() = 
        let project = Projects.Create [ "" ] "project Name"
        match project with
        | Value _ -> Success()
        | Error _ -> Failed()
    
    [<Fact>]
    let ``Create project when has forbidden charachter should has invalid charachter error``() = 
        let project = Projects.Create [ "" ] "~!@EDS"
        match project with
        | Error errors when HasError ProjectValidation.InvalidCharachter errors -> Success()
        | _ -> Failed()
    
    [<Fact>]
    let ``Create project with too short name should has invalid length error``() = 
        let project = Projects.Create [ "" ] "a"
        match project with
        | Error errors when HasError ProjectValidation.InvalidLength errors -> Success()
        | _ -> Failed()

    [<Fact>]
    let ``Create project with too long name should has invalid length erorr``() = 
        let longName = (Seq.init 150 (fun _ -> "") |> Seq.reduce (fun _ _ -> "  "))
        let project = Projects.Create [""] longName
        match project with
        | Error errors when HasError ProjectValidation.InvalidLength errors -> Success()
        | _ -> Failed()

    let ``Create project with dublicate name should has dublicate name error``() =
        let project = Projects.Create ["some name"; "dublicate"] "dublicate"
        match project with
        | Error errors when HasError ProjectValidation.Dublicate errors -> Success()
        | _ -> Failed()
