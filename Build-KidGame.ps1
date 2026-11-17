$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot

try {
    $msbuild = $null
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (Test-Path -LiteralPath $vswhere) {
        $candidates = & $vswhere -all -products '*' -find 'MSBuild\Current\Bin\MSBuild.exe'
        foreach ($candidate in $candidates) {
            $candidate = $candidate.Trim()
            $roslynTargets = Join-Path (Split-Path $candidate) 'Roslyn\Microsoft.CSharp.Core.targets'
            if ((Test-Path -LiteralPath $candidate) -and (Test-Path -LiteralPath $roslynTargets)) {
                $msbuild = $candidate
                break
            }
        }
    }
    if (-not $msbuild) {
        $command = Get-Command 'msbuild.exe' -ErrorAction SilentlyContinue
        if ($command) { $msbuild = $command.Source }
    }
    if (-not $msbuild -or -not (Test-Path -LiteralPath $msbuild)) {
        throw 'MSBuild was not found. Install Visual Studio or Visual Studio Build Tools.'
    }

    $dataProject = Join-Path $root 'KidGameData\KidGameData.csproj'
    $bizProject = Join-Path $root 'KidGameBiz\KidGameBiz.csproj'
    $solution = Join-Path $root 'KidGame.sln'
    $assets = Join-Path $root 'KidGame\Assets'
    $resources = Join-Path $assets 'Bootstrap.resources'

    Write-Host 'Building KidGameData...'
    & $msbuild $dataProject /t:Build /p:Configuration=Release /p:Platform=AnyCPU /verbosity:minimal
    if ($LASTEXITCODE -ne 0) { throw 'Building KidGameData failed.' }

    Write-Host 'Building KidGameBiz...'
    & $msbuild $bizProject /t:Build /p:Configuration=Release /p:Platform=AnyCPU /verbosity:minimal
    if ($LASTEXITCODE -ne 0) { throw 'Building KidGameBiz failed.' }

    Add-Type -TypeDefinition @'
using System.IO;
using System.Resources;
public static class KidGameBootstrapResourceBuilder {
    public static void Build(string database, string engine, string biz, string data, string output) {
        using (var writer = new ResourceWriter(output)) {
            writer.AddResource("dbGame.accdb", File.ReadAllBytes(database));
            writer.AddResource("AccessDatabaseEngine_X64.exe", File.ReadAllBytes(engine));
            writer.AddResource("KidGameBiz.dll", File.ReadAllBytes(biz));
            writer.AddResource("KidGameData.dll", File.ReadAllBytes(data));
        }
    }
}
'@

    Write-Host 'Packing database, Access engine, and project libraries...'
    [KidGameBootstrapResourceBuilder]::Build(
        (Join-Path $assets 'dbGame.accdb'),
        (Join-Path $assets 'AccessDatabaseEngine_X64.exe'),
        (Join-Path $root 'KidGameBiz\bin\Release\KidGameBiz.dll'),
        (Join-Path $root 'KidGameData\bin\Release\KidGameData.dll'),
        $resources)

    Write-Host 'Building x64 executable...'
    & $msbuild $solution /t:Build /p:Configuration=Release /p:Platform=x64 /verbosity:minimal
    if ($LASTEXITCODE -ne 0) { throw 'Building KidGame.exe failed.' }

    $dist = Join-Path $root 'dist'
    New-Item -ItemType Directory -Force -Path $dist | Out-Null
    $builtExe = Join-Path $root 'KidGame\bin\Release\KidGame.exe'
    $finalExe = Join-Path $dist 'KidGame-x64.exe'
    Copy-Item -LiteralPath $builtExe -Destination $finalExe -Force
    Write-Host "`nBuild succeeded: $finalExe"
}
catch {
    Write-Error $_
    exit 1
}
