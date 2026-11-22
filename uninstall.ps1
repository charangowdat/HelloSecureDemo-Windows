$dest = Join-Path $env:LOCALAPPDATA 'Programs\HelloSecureDemo'
$shortcut = Join-Path ([Environment]::GetFolderPath('Programs')) 'HelloSecure Demo.lnk'
if (Test-Path $shortcut) { Remove-Item $shortcut -Force }
if (Test-Path $dest) { Remove-Item $dest -Recurse -Force }
Write-Host 'HelloSecure Demo uninstalled. To reset the demo password, also delete %LOCALAPPDATA%\HelloSecureDemo.'
