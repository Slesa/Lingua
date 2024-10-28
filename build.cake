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
var workingPathName = "."; //EnvironmentVariable("BUILD_REPOSITORY_LOCALPATH") ?? ".";
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
//var platform = new CakePlatform();
//public bool IsWindows { get { return platform.Family==PlatformFamily.Windows; } }
public bool IsLocalBuild { get { return BuildSystem.IsLocalBuild; } }
public bool IsDevelop { get { return BranchName.ToLower()=="develop"; } }
public bool IsMaster { get { return BranchName.ToLower()=="master"; } }
public string BranchName { get { return EnvironmentVariable("APPVEYOR_REPO_BRANCH"); } } // TFBuild.Environment.Repository.Branch; } }

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
var  demoSolution = "src/Lingua.Demo.sln";

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
    var settings = new DotNetRestoreSettings {
        ArgumentCustomization = args => CreateNugetArguments(args),
    };    
    DotNetRestore(linguaSolution, settings);
});

Task("Build")
  .Does( () => {
    var platform = new CakePlatform();
    //var framework = IsWindows ? "net461" : "netcoreapp2.2";
    var coreSettings = new DotNetBuildSettings
    {
        ArgumentCustomization = args => CreateNugetArguments(args),
        //Framework = "netcoreapp2.0",
        Configuration = configuration,
        //OutputDirectory = buildPath,
        NoRestore = true,
    };
    //if (!IsWindows) coreSettings.Framework = framework;
    DotNetBuild(linguaSolution, coreSettings);

    /*var corePath = buildPath + "/core";
    var corePublish = new DotNetPublishSettings
    {
        //Framework = framework,
        //NoBuild = true,
        NoRestore = true,
        Configuration = configuration,
        OutputDirectory = corePath
    };
    DotNetPublish(linguaSolution, corePublish);*/

    /*if (IsWindows) {
      var wpfPath = buildPath + "/wpf";
      var buildSettings = new MSBuildSettings()
        .WithProperty("OutputPath", wpfPath)
        .WithProperty("Configuration", configuration);
      MSBuild(demoSolution, buildSettings);
    }*/
});

Task("CreatePackages")
  //.WithCriteria( IsWindows )
  .Does( () => {
    var settings = new DotNetPackSettings
    {
        ArgumentCustomization = args => CreateNugetArguments(args),
        Configuration = configuration,
        OutputDirectory = deployDir,
        IncludeSymbols = true,
        NoRestore = true,
        NoBuild = true,
    };
    DotNetPack(linguaProject, settings);
});

Task("PublishPackages")
  .WithCriteria( !IsLocalBuild )
  .Does( () => {
    var apikey = EnvironmentVariable("NUGET_APIKEY");
    var packages = GetFiles(deployDir+"/*.nupkg");
    NuGetPush(packages, new NuGetPushSettings {
        Source = "https://nuget.org",
        ApiKey = apikey
    });
    /* var settings = new DotNetPublishSettings
    {
        ArgumentCustomization = args => CreateNugetArguments(args),
        Configuration = configuration,
        OutputDirectory = deployDir,
        NoRestore = true,
        NoBuild = true,
    };
    DotNetPublish(linguaProject, settings); */
});

//////////////////////////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
  .IsDependentOn("Clean")
  .IsDependentOn("VersionInfo")
  .IsDependentOn("Restore-NuGet-Packages")
  .IsDependentOn("Build")
  .IsDependentOn("CreatePackages")
  .IsDependentOn("PublishPackages");

Task("Deploy")
  .IsDependentOn("PublishPackages");

RunTarget(target); 