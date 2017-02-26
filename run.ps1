param(
    [string] $Wordlist = "ordlista.txt",
    [string] $EventsFile = "events.txt"
)

if (!(Test-Path $Wordlist)) {
    Write-Warning "Wordlist file $Wordlist does not exist; aborting."
    return
}
if (!(Test-Path $EventsFile)) {
    Write-Host "Events file $EventsFile does not exist; it will be created when the first domain event is persisted."
}

Write-Host "Starting Niancat API with wordlist from $Wordlist, db in $EventsFile"

& build/Niancat.Api.exe $Wordlist $EventsFile
