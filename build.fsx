#I @"Tools\Fake"
#r "FakeLib.dll"

open Fake

// properties
let projectName = "Lingua"
let projectSummary = "A .NET-based parser generator.  It uses reflection to generate parsers and scanners using code-based scanner and grammar definitions."
let authors = ["R. Todd"; "J. Preiss"]

let currentVersion =
  if not isLocalBuild then buildVersion else
  "0.9.9.1"

TraceEnvironmentVariables()


// Directories
let binDir = @".\bin\"
let buildDir = binDir @@ @"build\"
let testDir = binDir @@ @"test\"
let reportDir = binDir @@ @"report\"
let deployDir = binDir @@ @"deploy\"
let packagesDir = binDir @@ @"packages"
let mspecDir = packagesDir @@ "MSpec"


// Files
let appReferences  = 
  !+ @"**\Lingua\Lingua.csproj" 
    ++ @"**\LinguaDemo\LinguaDemo.csproj"
        |> Scan

let testReferences = 
  !+ @"**\Lingua.Specs\Lingua.Specs.csproj" 
    |> Scan


// Targets
Target "Clean" (fun _ ->
  CleanDirs [buildDir; testDir; deployDir; reportDir; packagesDir]

  CreateDir mspecDir
  !! (@"src\packages\Machine.Specifications.*\**\*.*")
    |> CopyTo mspecDir
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
      AssemblyCopyright = "Copyright Â©  2010";
      AssemblyTrademark = "MS Public License";
      AssemblyVersion = currentVersion;
      OutputFileName = @".\src\VersionInfo.cs"})
)

Target "BuildApp" (fun _ ->
  MSBuildRelease buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
  MSBuildDebug testDir "Build" testReferences
    |> Log "TestBuildOutput: "
)

Target "RunTest" (fun _ ->
  let mspecTool = mspecDir @@ "mspec-clr4.exe"
  trace mspecTool

  !! (testDir @@ "*.Specs.dll")
    |> MSpec (fun p ->
      {p with
        ToolPath = mspecTool
        HtmlOutputDir = reportDir})
)

Target "Deploy" (fun _ ->
  
  let libDir = deployDir @@ @"lib\"
  CreateDir libDir
  let toolsDir = deployDir @@ @"tools\"
  CreateDir toolsDir
  let contentDir = deployDir @@ @"content\"
  CreateDir contentDir

  !! "Lingua.nuspec"
    |> CopyTo deployDir
  !! (buildDir @@ "Lingua.dll")
    |> CopyTo libDir
  !+ (buildDir @@ "Lingua.dll")
    ++ (buildDir @@ "LinguaDemo.exe")
    ++ (buildDir @@ "LinguaDemo.exe.config")
    |> Scan
      |> CopyTo toolsDir
//  let deployMsi = deployDir @@ sprintf "%s-%s.msi" projectName currentVersion
//  !! (setupDir @@ "*.wxl")
//    |> WiX (fun p -> WiXDefaults) deployMsi 
//  MSBuildReleaseExt deployDir ["Version", currentVersion] "Build" deployReferences
//    |> Log "DeployBuildOutput: "
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

