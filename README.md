# HelloSecure Demo (Windows)

A tiny Windows desktop demo with:
- Local password login (PBKDF2-SHA256 salted hash; password is not stored in plain text)
- Optional Windows Hello verification (Windows chooses fingerprint/PIN/face based on your device and settings)
- A welcome screen after successful authentication

## Requirements
- Windows 10/11
- .NET 8 SDK
- Windows Hello configured in Windows Settings (fingerprint reader required for fingerprint specifically)

## Build
1. Install the .NET 8 SDK.
2. Open PowerShell in this folder.
3. Run `./build.ps1`.
4. The self-contained app will be created in `publish/HelloSecureDemo.exe`.

## Install / uninstall
- After building, run `./install.ps1` to copy the app to `%LOCALAPPDATA%\\Programs\\HelloSecureDemo` and create a Start Menu shortcut.
- Run `./uninstall.ps1` to remove the installed app and shortcut.

## First run
The first launch asks you to create a demo password. On subsequent launches, enter it.
After password verification, choose **Use Windows Hello** to invoke the Windows Hello prompt. If Windows Hello is unavailable, use password-only login.

## Important
This is an educational sample, not production authentication. The password verifier is stored locally in `%LOCALAPPDATA%\\HelloSecureDemo\\auth.json`. Windows Hello verification is performed by Windows; this sample does not read or store fingerprint data. Windows Hello may ask for a PIN instead of a fingerprint depending on device configuration.

## Build online without installing .NET (GitHub Actions)
This project includes `.github/workflows/build-windows.yml`. Upload the project to a GitHub repository (keep the folder structure), then open **Actions → Build HelloSecure for Windows → Run workflow**. When it finishes, open the workflow run and download the `HelloSecureDemo-Windows-x64` artifact. That ZIP contains the compiled EXE and install/uninstall scripts. GitHub Actions performs the build on a Windows runner; no local .NET SDK is needed.
