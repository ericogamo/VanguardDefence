param(
    [string]$GameDir = ""
)

$ErrorActionPreference = "Stop"
$modName = "VanguardDefence"
$repoRoot = $PSScriptRoot
$distDll = Join-Path $repoRoot "dist\$modName.dll"
$assetsSrc = Join-Path $repoRoot "Assets"

function Resolve-NuclearOptionDir {
    param([string]$ExplicitDir)

    if (-not [string]::IsNullOrWhiteSpace($ExplicitDir)) {
        if (Test-Path (Join-Path $ExplicitDir "BepInEx\core\BepInEx.dll")) {
            return $ExplicitDir
        }
        throw "BepInEx not found in provided GameDir: '$ExplicitDir'"
    }

    if (-not [string]::IsNullOrWhiteSpace($env:NUCLEAR_OPTION_DIR) -and
        (Test-Path (Join-Path $env:NUCLEAR_OPTION_DIR "BepInEx\core\BepInEx.dll"))) {
        return $env:NUCLEAR_OPTION_DIR
    }

    $steamRoots = New-Object System.Collections.Generic.List[string]
    $regPath = (Get-ItemProperty "HKCU:\Software\Valve\Steam" -Name SteamPath -ErrorAction SilentlyContinue).SteamPath
    if ($regPath) { $steamRoots.Add($regPath) }
    foreach ($fallbackSteam in @("C:\Program Files (x86)\Steam", "C:\Program Files\Steam", "D:\Steam", "D:\SteamLibrary", "E:\SteamLibrary")) {
        if (Test-Path $fallbackSteam) { $steamRoots.Add($fallbackSteam) }
    }

    $candidates = New-Object System.Collections.Generic.List[string]
    foreach ($root in $steamRoots) {
        $candidates.Add((Join-Path $root "steamapps\common\Nuclear Option"))
        $vdf = Join-Path $root "steamapps\libraryfolders.vdf"
        if (Test-Path $vdf) {
            foreach ($line in Get-Content $vdf -ErrorAction SilentlyContinue) {
                if ($line -match '"path"\s+"([^"]+)"') {
                    $libPath = $matches[1] -replace '\\\\', '\'
                    $candidates.Add((Join-Path $libPath "steamapps\common\Nuclear Option"))
                }
            }
        }
    }

    foreach ($drive in @("C:", "D:", "E:", "F:")) {
        $candidates.Add("$drive\SteamLibrary\steamapps\common\Nuclear Option")
        $candidates.Add("$drive\Program Files (x86)\Steam\steamapps\common\Nuclear Option")
    }

    foreach ($cand in ($candidates | Select-Object -Unique)) {
        if (Test-Path (Join-Path $cand "BepInEx\core\BepInEx.dll")) {
            return $cand
        }
    }

    throw "Could not auto-detect Nuclear Option with BepInEx. Pass -GameDir 'C:\...\Nuclear Option' or set NUCLEAR_OPTION_DIR."
}

if (-not (Test-Path $distDll)) {
    throw "Missing prebuilt DLL at '$distDll'. Pull again or build Release first."
}

$resolvedGameDir = Resolve-NuclearOptionDir -ExplicitDir $GameDir
$mdsDll = Join-Path $resolvedGameDir "BepInEx\plugins\MDSFramework\MDSFramework.dll"
if (-not (Test-Path $mdsDll)) {
    Write-Warning "MDSFramework.dll is not installed yet at '$mdsDll'. Install MDSFramework first!"
}

$targetDir = Join-Path $resolvedGameDir "BepInEx\plugins\$modName"
New-Item -ItemType Directory -Path $targetDir -Force | Out-Null

$targetDll = Join-Path $targetDir "$modName.dll"
Copy-Item -Path $distDll -Destination $targetDll -Force

$assetCount = 0
if (Test-Path $assetsSrc) {
    $assetsDst = Join-Path $targetDir "Assets"
    foreach ($file in (Get-ChildItem $assetsSrc -Recurse -File)) {
        $rel = $file.FullName.Substring($assetsSrc.Length).TrimStart('\', '/')
        $destFile = Join-Path $assetsDst $rel
        $destParent = Split-Path $destFile -Parent
        if (-not (Test-Path $destParent)) {
            New-Item -ItemType Directory -Path $destParent -Force | Out-Null
        }
        Copy-Item -Path $file.FullName -Destination $destFile -Force
        $assetCount++
    }
}

$srcHash = (Get-FileHash $distDll -Algorithm SHA256).Hash
$dstHash = (Get-FileHash $targetDll -Algorithm SHA256).Hash
if ($srcHash -ne $dstHash) {
    throw "SHA256 mismatch after copying $modName.dll!"
}

Write-Host "OK: $modName installed to '$targetDir' (SHA256=$dstHash, Assets=$assetCount)"
