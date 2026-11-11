param(
    [string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$version = (Get-Content -LiteralPath (Join-Path $repoRoot 'VERSION') -Raw).Trim()
if ($version -notmatch '^\d+\.\d+\.\d+$') { throw 'VERSION must be MAJOR.MINOR.PATCH.' }
if ($env:GITHUB_REF -like 'refs/tags/*' -and $env:GITHUB_REF -ne "refs/tags/v$version") {
    throw "Release tag does not match VERSION ($version)."
}
if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $OutputDirectory = Join-Path $repoRoot "artifacts\release\$version"
}
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
$project = Join-Path $repoRoot 'desktop\Duckie\Duckie.csproj'
& dotnet build $project -c Release
if ($LASTEXITCODE -ne 0) { throw 'Desktop build failed.' }

$sourceExe = Join-Path $repoRoot 'desktop\Duckie\bin\Release\net480\Duckie.exe'
$binaryVersion = [Diagnostics.FileVersionInfo]::GetVersionInfo($sourceExe).FileVersion
if ($binaryVersion -ne "$version.0") { throw "Unexpected binary version: $binaryVersion" }
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$fileName = "Duckie-v$version.exe"
$destination = Join-Path $OutputDirectory $fileName
Copy-Item -LiteralPath $sourceExe -Destination $destination -Force
$hash = (Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash.ToLowerInvariant()
[IO.File]::WriteAllText((Join-Path $OutputDirectory 'SHA256SUMS.txt'), "$hash  $fileName`n")
$metadata = [ordered]@{
    version = $version
    tag = "v$version"
    fileName = $fileName
    size = (Get-Item -LiteralPath $destination).Length
    sha256 = $hash
    publishedAt = [DateTime]::UtcNow.ToString('yyyy-MM-dd')
    platform = 'Windows 11'
    runtime = '.NET Framework 4.8'
    source = "https://github.com/cg-zhou/duckie/releases/tag/v$version"
}
[IO.File]::WriteAllText((Join-Path $OutputDirectory 'release.json'), ($metadata | ConvertTo-Json) + "`n")
Write-Host "Packaged $fileName ($($metadata.size) bytes), SHA-256 $hash"
Write-Host "Output: $OutputDirectory"
