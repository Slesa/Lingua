//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var configuration = Argument("configuration", "Release");
Information("Configuration is "+configuration);
var target = Argument("target", "Default");
Information("Target is "+target);


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////
var workingPathName = EnvironmentVariable("BUILD_REPOSITORY_LOCALPATH") ?? ".";
var workingDir = MakeAbsolute(new DirectoryPath( workingPathName ));
var releaseNotes = ParseReleaseNotes( workingDir + "/ReleaseNotes.md");
var version = releaseNotes.Version.ToString(); //string.Format("{0}.{1}.{2}", releaseNotes.Version.Major, releaseNotes.Version.Minor, releaseNotes.Version.Patch);
Information("Version is "+version);

var srcDir = new DirectoryPath( workingDir.ToString() + "/src" );
var binDir = new DirectoryPath( workingDir.CombineWithFilePath("bin").FullPath );
var buildDir = new DirectoryPath( binDir.CombineWithFilePath("build").FullPath );
var buildPath = buildDir.FullPath;
var deployDir = new DirectoryPath( binDir.CombineWithFilePath("deploy").FullPath );
//var testDir = new DirectoryPath( binDir.CombineWithFilePath("test").FullPath );
//var testPath = testDir.FullPath;
//var reportDir = new DirectoryPath( binDir.CombineWithFilePath("report").FullPath );

//////////////////////////////////////////////////////////////////////
// PROPERTIES
//////////////////////////////////////////////////////////////////////
public bool IsLocalBuild { get { return BuildSystem.IsLocalBuild; } }
public bool IsDevelop { get { return BranchName.ToLower()=="develop"; } }
public bool IsMaster { get { return BranchName.ToLower()=="master"; } }
public string BranchName { get { return TFBuild.Environment.Repository.Branch; } }

public string CurrentVersion() {
    var isAppVeyorBuild = AppVeyor.IsRunningOnAppVeyor;
    if (isAppVeyorBuild) {
        var buildno = EnvironmentVariable("APPVEYOR_BUILD_NUMBER") ?? "0";
        var result = version + string.Format(".{0}", buildno);
        //AppVeyor.UpdateBuildVersion(result);
        return result;
    }
    return version + ".0";
}

var  linguaSolution = "src/Lingua.sln";
var  linguaProject = "src/Lingua/Lingua.csproj";
var  demoSolution = "src/LinguaDemo.sln";

ProcessArgumentBuilder CreateNugetArguments(ProcessArgumentBuilder args) {
    var relNotes = string.Join("\n", releaseNotes.Notes)
        .Replace(",", " and");
//        .Replace(",", "[MSBuild]::Escape(',')");
    Information("Release notes: \n"+relNotes);
    return args
        .Append("/p:Version="+version)
        .Append("/p:PackageReleaseNotes=\""+relNotes+"\"");
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
  .Does(() =>
{
    if( !IsLocalBuild || !DirectoryExists(buildDir) || !DirectoryExists(deployDir) /*|| !DirectoryExists(testDir) || !DirectoryExists(reportDir)*/ )
    {
      CleanDirectory(buildDir);
      CleanDirectory(deployDir);
      //CleanDirectory(testDir);
      //CleanDirectory(reportDir);
    }
});

Task("VersionInfo")
  .Does( () => {
    var file = "src/VersionInfo.cs";
    CreateAssemblyInfo(file, new AssemblyInfoSettings {
        Product = "Lingua",
        Version = version,
        FileVersion = version,
        InformationalVersion = CurrentVersion(),
        Trademark = "MS Public License",
        Copyright = string.Format("Copyright (c) Richard G. Todd 2010 - {0}", DateTime.Now.Year)
    });      
});

Task("Restore-NuGet-Packages")
  .Does( () => {
    var settings = new DotNetCoreRestoreSettings {
        ArgumentCustomization = args => CreateNugetArguments(args),
    };    
    DotNetCoreRestore(linguaSolution, settings);
});

Task("Build")
  .Does( () => {
    var coreSettings = new DotNetCoreBuildSettings
    {
        ArgumentCustomization = args => CreateNugetArguments(args),
        //Framework = "netcoreapp2.0",
        Configuration = configuration,
        //OutputDirectory = buildPath,
        NoRestore = true,
    };
    DotNetCoreBuild(linguaSolution, coreSettings);
    var outputPath = buildPath;
    var buildSettings = new MSBuildSettings()
        .WithProperty("OutputPath", outputPath)
        .WithProperty("Configuration", configuration);
    MSBuild(demoSolution, buildSettings);
});

Task("CreatePackages")
  .Does( () => {
    var settings = new DotNetCorePackSettings
    {
        ArgumentCustomization = args => CreateNugetArguments(args),
        Configuration = configuration,
        OutputDirectory = deployDir,
        IncludeSymbols = true,
        NoRestore = true,
        NoBuild = true,
    };
    DotNetCorePack(linguaProject, settings);
});


//////////////////////////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
  .IsDependentOn("Clean")
  .IsDependentOn("VersionInfo")
  .IsDependentOn("Restore-NuGet-Packages")
  .IsDependentOn("Build")
  .IsDependentOn("CreatePackages");

//Task("Deploy")
//  .IsDependentOn("PublishPackages")
//  .IsDependentOn("DeployTools");

RunTarget(target); 