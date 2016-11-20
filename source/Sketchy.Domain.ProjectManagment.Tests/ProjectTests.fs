namespace Sketchy.Domain.ProjectManagment.Tests.Tests

module ProjectTests = 
    open Sketchy.Common
    open Sketchy.Domain.Shared.CommonTypes
    open Sketchy.Domain.ProjectManagment
    open Sketchy.Common.ResultCore
    open Xunit
    
    let Success() = Assert.True true
    let Failed() = Assert.True false
    let ValidIdentity = { Value = 1 }
    
    let HasFailedWithError project expectedError = 
        match project with
        | Error error -> match box error with
                         | :? 'TExpectedError as c when c = expectedError -> Success()
                         | _ -> Failed()
        | _ -> Failed()
    
    let internal CreateValidProject() = Project.Create ValidIdentity "project name" [ "" ]
    
    [<Fact>]
    let ``Create project when name is valid should succeed``() = 
        let project = Project.Create ValidIdentity "project Name" [ "" ]
        match project with
        | Value _ -> Success()
        | Error _ -> Failed()
    
    [<Fact>]
    let ``Create project when has forbidden charachter should has invalid charachter error``() = 
        let project = Project.Create ValidIdentity "~!@EDS" [ "" ]
        HasFailedWithError project (Project.ProjectCreateOrRenameError.InvalidCharachterInName "~!@EDS")
    
    [<Fact>]
    let ``Create project with too short name should has invalid length error``() = 
        let project = Project.Create ValidIdentity "a" [ "" ]
        HasFailedWithError project (Project.ProjectCreateOrRenameError.TooShortName "a")
    
    [<Fact>]
    let ``Create project with too long name should has invalid length erorr``() = 
        let longName = (Seq.init 150 (fun _ -> " ") |> Seq.reduce (fun f s -> String.concat "" [ f; s ]))
        let project = Project.Create ValidIdentity longName [ "" ]
        HasFailedWithError project (Project.ProjectCreateOrRenameError.TooLongName longName)
    
    [<Fact>]
    let ``Create project with dublicate name should has dublicate name error``() = 
        let project = Project.Create ValidIdentity "dublicate" [ "some name"; "dublicate" ]
        HasFailedWithError project (Project.ProjectCreateOrRenameError.DublicateName "dublicate") 

    [<Fact>]
    let ``Create project with valid name but with empty identity should fail with empty identity``() = 
        let project = Project.Create Identity.Empty "some name" [""]
        HasFailedWithError project Project.ProjectCreateOrRenameError.EmptyIdentity
    
    [<Fact>]
    let ``Rename project with valid name should return the same project with new name``() = 
        let originalProject = CreateValidProject()
        
        let renamedProject = 
            Result { 
                let! project = originalProject
                let renamedProject = Project.Rename project "new name" [ "" ]
                return! renamedProject
            }
        match renamedProject, originalProject with
        | Value renamed, Value original -> 
            Assert.Equal(renamed.Name, "new name")
            Assert.Equal(renamed.Identity, original.Identity)
            Assert.Equal(renamed.Workflows.Length, original.Workflows.Length)
        | _ -> Failed()
