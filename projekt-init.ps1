Remove-Item -Recurse -Force .\.projekt -ErrorAction SilentlyContinue

Invoke-WebRequest `
    -Uri "https://github.com/fsprojects/Projekt/releases/download/0.0.4/Projekt.zip" `
    -OutFile temp.zip

Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory("$(Get-Location)\temp.zip", "$(Get-Location)\.projekt")
Remove-Item temp.zip
