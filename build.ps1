Param(
    [string] $Configuration = "Debug",
    [string] $PackageOutputDirectory = "Build",
    [string] $Version,
    [string[]] $Package = @()
)


function Invoke-ExpressionExitCodeCheck([string] $command)
{
    Invoke-Expression $command

    if ($LASTEXITCODE -and $LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}

# Print out dotnet cli info

Invoke-Expression "dotnet --info"

# Build

Write-Host "Change to src..."
cd src
Write-Host "Restoring packages..."
Invoke-ExpressionExitCodeCheck "dotnet restore Lingua.sln" -ErrorAction Stop

Write-Host "Building in ${Configuration}..."
Invoke-ExpressionExitCodeCheck "dotnet build -c ${Configuration} Lingua.sln" -ErrorAction Stop


# Test

Write-Host "Running tests..."

$tests = Get-ChildItem -File -Recurse -Filter "*.csproj" | Where-Object { $_.FullName -imatch "^.*\.(?:Specs|Tests|Test).csproj$" }

[bool] $testsFailed = $false
$tests | ForEach-Object {
    Invoke-Expression "dotnet test $($_.FullName) -c ${Configuration}"

    if (!$testsFailed) {
        $testsFailed = $LASTEXITCODE -and $LASTEXITCODE -ne 0
    }
}

if ($testsFailed) {
    Write-Host -BackgroundColor Red -ForegroundColor Yellow "Tests failed!"
    exit -1
} else {
    Write-Host -BackgroundColor Green -ForegroundColor White "All good!"
}


# NuGet packaging

Write-Host "Creating a nuget package in ${PackageOutputDirectory} for ${Package}"

$Package | ForEach-Object {
    Invoke-ExpressionExitCodeCheck "dotnet pack $_ --include-symbols -c ${Configuration} -o ${PackageOutputDirectory} /p:Version=${Version}"
}

Write-Host "Restoring path"
cd ..
