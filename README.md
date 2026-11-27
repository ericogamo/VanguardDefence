# VanguardDefence (Skyshield & SeaWraith Contractor Pack)

Contractor pack mod for **Nuclear Option** built on [`MDSFramework`](https://github.com/ericogamo/MDSFramework):
- **Unified 20ft DIN ISO 668 HX3 Stealth Camo Containers**: standardized ISO shell, corner castings, placards, and compacted 20ft **Skyshield Long SAM (Type A)**
- **Skyshield AESA FCU** & **Skyshield Leitstand (C2 Net)**
- **Skyshield HEL Laser**, **Skyshield 35mm AHEAD CIWS**, **Skyshield Short SAM (Type C)**, **Skyshield Long SAM (Type A)**
- **Skyshield VLS** (4x `ECCM-Hunter` + 4x `SkyLance LR-SAM`) + blind Passive Home-on-Jam (HoJ) response
- **SeaWraith** unified RAM stealth anti-ship cruise missile (jet air-drop glide to 10 ft sea-skim & 6-cell elevating-roof container)
- **MAN HX3 8x8 ISO Container Carrier** (drivable logistics hauler for 20ft ISO weapon modules)

## Prerequisites
- **Nuclear Option** with **BepInEx 5**
- [`MDSFramework`](https://github.com/ericogamo/MDSFramework) installed in `BepInEx\plugins\MDSFramework`

## Installation
### Via NOMM (Nuclear Option Mod Manager)
Install `VanguardDefence` directly inside NOMM (auto-updates via NOMNOM; requires `MDSFramework`).

### Manual Release ZIP
1. Download `VanguardDefence-v0.1.5.zip` from [Releases](https://github.com/ericogamo/VanguardDefence/releases/latest).
2. Extract its contents (`VanguardDefence.dll` + `Assets/`) directly into `Nuclear Option\BepInEx\plugins\VanguardDefence\`.

### From Git Clone
```powershell
git clone https://github.com/ericogamo/VanguardDefence.git
cd VanguardDefence
powershell -ExecutionPolicy Bypass -File .\install.ps1
```

## Multiplayer & Authority
All units, missiles, sensor networks, and engagement behaviors run through `MDSFramework` under host/server authority.

## Companion Mods
- [`MDSFramework`](https://github.com/ericogamo/MDSFramework)
- [`GroundControlRtsExpanded`](https://github.com/ericogamo/GroundControlRtsExpanded)

## Build from Source
```powershell
dotnet build VanguardDefence.csproj -c Release -p:GameDir="D:\SteamLibrary\steamapps\common\Nuclear Option"
```

## License, Assets & AI Disclosure
- **Code License:** [MIT License](LICENSE)
- **3D Models, Decals, Icons & Audio:** All bundled `.glb` models, `branding/` decals & logos, `icons/`, and `audio/skyshield_gun_35mm_shot.wav` are original project/procedural assets (`CC0 1.0`). No ripped game assets are redistributed.
- **AI Assistance:** Created with substantial agentic/generative AI assistance (`isAiAssisted: True`).
