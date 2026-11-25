# VanguardDefence (Skyshield & SeaWraith Contractor Pack)

Contractor pack mod for **Nuclear Option** built on [`MDSFramework`](https://github.com/ericogamo/MDSFramework):
- **Skyshield AESA FCU** & **Skyshield Leitstand (C2 Net)**
- **Skyshield HEL Laser**, **Skyshield 35mm AHEAD CIWS**, **Skyshield Short SAM (Type C)**, **Skyshield Long SAM (Type A)**
- **Skyshield VLS** (4x `ECCM-Hunter "SAME DAY DELIVERY"` + 4x `SkyLance LR-SAM`) + blind Passive Home-on-Jam (HoJ) response
- **SeaWraith NSM** Coastal Anti-Ship Container & HLT-8x8

## Prerequisites
- **Nuclear Option** with **BepInEx 5**
- [`MDSFramework`](https://github.com/ericogamo/MDSFramework) installed in `BepInEx\plugins\MDSFramework`

## Quick Install / Update (Prebuilt DLL + Assets included)
```powershell
git clone https://github.com/ericogamo/VanguardDefence.git
cd VanguardDefence
powershell -ExecutionPolicy Bypass -File .\install.ps1
```

## Companion Mods
- [`MDSFramework`](https://github.com/ericogamo/MDSFramework)
- [`GroundControlRtsExpanded`](https://github.com/ericogamo/GroundControlRtsExpanded)

## Build from Source
```powershell
dotnet build VanguardDefence.csproj -c Release -p:GameDir="D:\SteamLibrary\steamapps\common\Nuclear Option"
```
