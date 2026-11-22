$ErrorActionPreference = 'Stop'
dotnet publish .\HelloSecureDemo\HelloSecureDemo.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o .\publish
if ($LASTEXITCODE -ne 0) { throw 'Build failed. Check the .NET SDK and restore errors.' }
Write-Host 'Build complete: publish\HelloSecureDemo.exe'
