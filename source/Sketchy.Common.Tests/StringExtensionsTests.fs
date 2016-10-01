namespace Sketchy.Common.Tests

module StringExtensionsTests = 
    open Xunit
    open FsCheck
    open FsCheck.Xunit

    open Sketchy.Common.StringExtensions
    
    [<Fact>]
    let ``Equals when to ignore case strings are same should return true``() = 
        let fStr = CaseInsensitiveString.Create "hi"
        let sStr = CaseInsensitiveString.Create "hi"

        do Assert.True(fStr.Equals sStr)

    [<Fact>]
    let ``Equals when two ignore case string are same but with different casing should return true``() = 
        let fStr = CaseInsensitiveString.Create "hellO"
        let sStr = CaseInsensitiveString.Create "Hello"
        
        do Assert.True(fStr.Equals sStr)

    [<Fact>]
    let ``Equals when two completly diff strings should return false``() =
        let fStr = CaseInsensitiveString.Create "string 1"
        let sStr = CaseInsensitiveString.Create "string 2"

        do Assert.False (fStr.Equals sStr)