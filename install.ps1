$ErrorActionPreference = 'Stop'
$source = Join-Path $PSScriptRoot 'publish'
$exe = Join-Path $source 'HelloSecureDemo.exe'
if (!(Test-Path $exe)) { throw 'App EXE not found. Run build.ps1 first.' }
$dest = Join-Path $env:LOCALAPPDATA 'Programs\HelloSecureDemo'
New-Item -ItemType Directory -Force -Path $dest | Out-Null
Copy-Item (Join-Path $source '*') $dest -Recurse -Force
$shortcutPath = Join-Path ([Environment]::GetFolderPath('Programs')) 'HelloSecure Demo.lnk'
$ws = New-Object -ComObject WScript.Shell
$link = $ws.CreateShortcut($shortcutPath); $link.TargetPath = Join-Path $dest 'HelloSecureDemo.exe'; $link.WorkingDirectory = $dest; $link.Save()
Write-Host "Installed to $dest"
