#r "packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

type Paths = { build : string; tests : string; xunit : string }

let paths = {
  build = "./build"
  tests = "./tests"
  xunit = "packages/xunit.runner.console/tools/xunit.console.exe"
}

Target "Clean" <| fun _ ->
  CleanDir paths.build
  CleanDir paths.tests


Target "BuildApp" <| fun _ ->
    !! "src/**/*.fsproj"
        -- "src/**/*.Tests.fsproj"
        |> MSBuildRelease paths.build "Build"
        |> Log "AppBuild-Output: "

Target "BuildTests" <| fun _ ->
    !! "src/**/*.Tests.fsproj"
    |> MSBuildRelease paths.tests "Build"
    |> Log "BuildTests-Output: "

Target "RunUnitTests" <| fun _ ->
    !! (paths.tests + "/*.Tests.dll")
    |> xUnit (fun p ->
      { p with ToolPath = paths.xunit })

"Clean"
  ==> "BuildApp"
  ==> "BuildTests"
  ==> "RunUnitTests"

RunTargetOrDefault "RunUnitTests"
