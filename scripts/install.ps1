#Requires -RunAsAdministrator
$ErrorActionPreference = 'Stop'

if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(
    [Security.Principal.WindowsBuiltInRole]::Administrator)) {
    throw "Installation requires administrator privileges."
}

$repo = "lennykean/pmmux"
$installDir = Join-Path $env:ProgramFiles "pmmux"
$extensionsDir = Join-Path $installDir "extensions"

function Get-LatestRelease {
    try {
        return Invoke-RestMethod -Uri "https://api.github.com/repos/$repo/releases/latest"
    }
    catch {
        throw "Failed to fetch latest release from GitHub: $_"
    }
}

function Get-InstalledVersion {
    $exe = Join-Path $installDir "pmmux.exe"
    if (-not (Test-Path $exe)) {
        return $null
    }
    $output = & $exe --version 2>&1
    if ($output[0] -match '^pmmux\s+(.+)$') {
        return $Matches[1]
    }
    return $null
}

function Add-ToMachinePath {
    param([string]$Directory)

    $regKey = [Microsoft.Win32.Registry]::LocalMachine.OpenSubKey(
        'SYSTEM\CurrentControlSet\Control\Session Manager\Environment', $true)
    $currentPath = $regKey.GetValue('Path', '', 'DoNotExpandEnvironmentNames')

    $paths = $currentPath -split ';' | Where-Object { $_ -ne '' }
    if ($paths -contains $Directory) {
        $regKey.Close()
        return
    }

    $newPath = ($paths + $Directory) -join ';'
    $regKey.SetValue('Path', $newPath, [Microsoft.Win32.RegistryValueKind]::ExpandString)
    $regKey.Close()

    $HWND_BROADCAST = [IntPtr]0xffff
    $WM_SETTINGCHANGE = 0x1a
    $result = [UIntPtr]::Zero
    if (-not ('Win32.NativeMethods' -as [Type])) {
        Add-Type -Namespace Win32 -Name NativeMethods -MemberDefinition @'
[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
public static extern IntPtr SendMessageTimeout(
    IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam,
    uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);
'@
    }
    [Win32.NativeMethods]::SendMessageTimeout(
        $HWND_BROADCAST, $WM_SETTINGCHANGE, [UIntPtr]::Zero,
        'Environment', 2, 5000, [ref]$result) | Out-Null

    Write-Host "Added $Directory to system PATH."
}

# Fetch latest release
Write-Host "Fetching latest release..."
$release = Get-LatestRelease
$version = $release.tag_name -replace '^v', ''

# Check if already up to date
$installedVersion = Get-InstalledVersion
if ($installedVersion -eq $version) {
    Write-Host "pmmux is already up to date ($version)."
    exit 0
}

# Find the platform archive asset
$assetName = "pmmux-win-x64.tar.gz"
$asset = $release.assets | Where-Object { $_.name -eq $assetName }
if (-not $asset) {
    throw "Asset '$assetName' not found in release $version."
}

$tempDir = Join-Path ([System.IO.Path]::GetTempPath()) "pmmux-install-$([Guid]::NewGuid())"
try {
    New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
    $archivePath = Join-Path $tempDir $assetName

    # Download
    Write-Host "Downloading pmmux $version..."
    Invoke-WebRequest -Uri $asset.browser_download_url -OutFile $archivePath

    # Extract
    $extractDir = Join-Path $tempDir "extracted"
    New-Item -ItemType Directory -Path $extractDir -Force | Out-Null
    tar -xzf $archivePath -C $extractDir
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to extract archive."
    }

    # Create install directory
    if (-not (Test-Path $installDir)) {
        New-Item -ItemType Directory -Path $installDir -Force | Out-Null
    }

    # Copy files to install directory
    Get-ChildItem -Path $extractDir -Recurse | ForEach-Object {
        $dest = Join-Path $installDir $_.FullName.Substring($extractDir.Length)
        if ($_.PSIsContainer) {
            New-Item -ItemType Directory -Path $dest -Force | Out-Null
        }
        else {
            Copy-Item -Path $_.FullName -Destination $dest -Force
        }
    }

    # Create extensions directory
    if (-not (Test-Path $extensionsDir)) {
        New-Item -ItemType Directory -Path $extensionsDir -Force | Out-Null
    }

    # Add to PATH
    Add-ToMachinePath -Directory $installDir

    Write-Host "pmmux $version installed successfully to $installDir."
}
finally {
    if (Test-Path $tempDir) {
        Remove-Item -Path $tempDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}
