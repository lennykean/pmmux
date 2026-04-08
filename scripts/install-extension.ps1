#Requires -RunAsAdministrator

param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$ExtensionName
)

$ErrorActionPreference = 'Stop'

if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(
    [Security.Principal.WindowsBuiltInRole]::Administrator)) {
    throw "Installation requires administrator privileges."
}

$repo = "lennykean/pmmux"
$knownExtensions = @(
    "http", "tls", "bittorrent", "management", "acme", "acme-route53", "otlp"
)
$installDir = Join-Path $env:ProgramFiles "pmmux"
$extensionsDir = Join-Path $installDir "extensions"

# Validate extension name
if ($ExtensionName -notin $knownExtensions) {
    $list = $knownExtensions -join ', '
    throw "Unknown extension '$ExtensionName'. Valid extensions: $list"
}

function Find-PmmuxExe {
    # Check the standard install location first
    $exe = Join-Path $installDir "pmmux.exe"
    if (Test-Path $exe) {
        return $exe
    }
    # Fall back to PATH
    $found = Get-Command "pmmux.exe" -ErrorAction SilentlyContinue
    if ($found) {
        return $found.Source
    }
    return $null
}

function Get-LatestRelease {
    try {
        return Invoke-RestMethod -Uri "https://api.github.com/repos/$repo/releases/latest"
    }
    catch {
        throw "Failed to fetch latest release from GitHub: $_"
    }
}

function Get-InstalledExtensionVersion {
    param([string]$PmmuxExe, [string]$Name)

    $output = & $PmmuxExe --version 2>&1

    $dllMap = @{
        "http"          = "Pmmux.Extensions.Http.dll"
        "tls"           = "Pmmux.Extensions.Tls.dll"
        "bittorrent"    = "Pmmux.Extensions.BitTorrent.dll"
        "management"    = "Pmmux.Extensions.Management.dll"
        "acme"          = "Pmmux.Extensions.Acme.dll"
        "acme-route53"  = "Pmmux.Extensions.Acme.Route53.dll"
        "otlp"          = "Pmmux.Extensions.Otlp.dll"
    }
    $dllName = $dllMap[$Name]
    if (-not $dllName) {
        return $null
    }

    foreach ($line in $output) {
        if ($line -match "$([regex]::Escape($dllName))\s+(.+)$") {
            return $Matches[1]
        }
    }
    return $null
}

# Find pmmux
$pmmuxExe = Find-PmmuxExe
if (-not $pmmuxExe) {
    throw "pmmux is not installed. Run install.ps1 first."
}

# Fetch latest release
Write-Host "Fetching latest release..."
$release = Get-LatestRelease
$version = $release.tag_name -replace '^v', ''

# Check if extension is already up to date
$installedVersion = Get-InstalledExtensionVersion -PmmuxExe $pmmuxExe -Name $ExtensionName
if ($installedVersion -eq $version) {
    Write-Host "Extension '$ExtensionName' is already up to date ($version)."
    exit 0
}

# Find the extension asset
$assetName = "pmmux-extension-$ExtensionName.tar.gz"
$asset = $release.assets | Where-Object { $_.name -eq $assetName }
if (-not $asset) {
    throw "Asset '$assetName' not found in release $version."
}

$tempDir = Join-Path ([System.IO.Path]::GetTempPath()) "pmmux-ext-$([Guid]::NewGuid())"
try {
    New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
    $archivePath = Join-Path $tempDir $assetName

    # Download
    Write-Host "Downloading extension '$ExtensionName' $version..."
    Invoke-WebRequest -Uri $asset.browser_download_url -OutFile $archivePath

    # Ensure extensions directory exists
    if (-not (Test-Path $extensionsDir)) {
        New-Item -ItemType Directory -Path $extensionsDir -Force | Out-Null
    }

    # Extract into extensions directory
    tar -xzf $archivePath -C $extensionsDir
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to extract archive."
    }

    Write-Host "Extension '$ExtensionName' $version installed successfully."
}
finally {
    # Clean up temp files
    if (Test-Path $tempDir) {
        Remove-Item -Path $tempDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}
