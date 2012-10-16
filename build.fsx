#I @"Tools\Fake"
#r "FakeLib.dll"

open Fake

// properties
let projectName = "Lingua"
let projectSummary = "A .NET-based parser generator.  It uses reflection to generate parsers and scanners using code-based scanner and grammar definitions."
let authors = ["R. Todd"; "J. Preiss"]
let homepage = "https://github.com/Slesa/Lingua"
let mail = "joerg.preiss@slesa.de"

let currentVersion =
  if not isLocalBuild then buildVersion else
  "1.0.2"

TraceEnvironmentVariables()


// Directories
let srcDir = @".\src\"
let binDir = @".\bin\"
let buildDir = binDir @@ @"build\"
let testDir = binDir @@ @"test\"
let nugetDir = binDir @@ @"nuget\"
let reportDir = binDir @@ @"report\"
let packagesDir = srcDir @@ @"packages\"


// Tools
let MSpecVersion = GetPackageVersion packagesDir "Machine.Specifications"
let mspecTool = sprintf @"%sMachine.Specifications.%s\tools\mspec-clr4.exe" packagesDir MSpecVersion 


// Files
let appReferences  = 
  !+ @"src\Lingua\Lingua.csproj" 
    ++ @"src\LinguaDemo\LinguaDemo.csproj"
        |> Scan

let testReferences = 
  !+ @"**\Lingua.Specs\Lingua.Specs.csproj" 
    |> Scan


// Targets
Target "Clean" (fun _ ->
  CleanDirs [buildDir; testDir; nugetDir; reportDir ]
)

Target "SetAssemblyInfo" (fun _ ->
  AssemblyInfo
    (fun p ->
    {p with
      CodeLanguage = CSharp;
      Guid = "";
      ComVisible = None;
      CLSCompliant = None;
      AssemblyCompany = "Richard G. Todd";
      AssemblyProduct = "Lingua";
      AssemblyCopyright = "Copyright ©  2010";
      AssemblyTrademark = "MS Public License";
      AssemblyVersion = currentVersion;
      OutputFileName = srcDir @@ @"\VersionInfo.cs"})
)

Target "BuildApp" (fun _ ->
  MSBuildRelease buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
  MSBuildDebug testDir "Build" testReferences
    |> Log "TestBuild-Output: "
)

Target "RunTest" (fun _ ->
  !! (testDir @@ "*.Specs.dll")
    |> MSpec (fun p ->
      {p with
        ToolPath = mspecTool
        HtmlOutputDir = reportDir})
)

Target "Deploy" (fun _ ->
  
  let libDir = nugetDir @@ @"lib\net40"
  CreateDir libDir
  let toolsDir = nugetDir @@ @"tools\"
  CreateDir toolsDir
  let contentDir = nugetDir @@ @"content\"
  CreateDir contentDir
  let nugetExe = @"tools\NuGet\NuGet.exe"

  !! (buildDir @@ "Lingua.dll")
    |> CopyTo libDir
  !+ (buildDir @@ "Lingua.dll")
    ++ (buildDir @@ "LinguaDemo.exe")
    ++ (buildDir @@ "LinguaDemo.exe.config")
      |> Scan
        |> CopyTo toolsDir
  !+ "Copyright.txt"
    ++ "License.txt"
    ++ "Readme.txt"
    |> Scan
      |> CopyTo contentDir

  NuGet (fun p -> 
    {p with               
      Authors = authors
      Project = projectName
      Description = projectSummary                               
      OutputPath = nugetDir
      Version = currentVersion
      AccessKey = getBuildParamOrDefault "nugetkey" ""
      Publish = hasBuildParam "nugetkey" }) "Lingua.nuspec"
)

Target "Default" DoNothing


// Dependencies
"Clean"
  ==> "SetAssemblyInfo"
  ==> "BuildApp" <=> "BuildTest"
  ==> "RunTest"
  ==> "Deploy"
  ==> "Default"


// start build
RunParameterTargetOrDefault "target" "Default"

